using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class Splitter : MonoBehaviour, ILaserSource
{
    [Header("Reflection Settings")]
    public bool reflectUp = true;

    [Header("Visual Settings")]
    public Color reflectionColor = Color.blue;
    public float lineWidth = 0.05f;

    public LineRenderer lineRenderer;
    public bool isActive = false;
    public Vector2 reflectionPoint;
    public Vector2 incomingDirection;
    [SerializeField] public ILaserSource source;
    public List<Splitter> currentChildSplitters = new List<Splitter>();

    void Start()
    {
        InitializeLineRenderer();
        lineRenderer.enabled = false;
    }

    void Update()
    {
        if (!isActive) return;

        // Проверяем, получаем ли мы еще луч от источника
        if (!IsReceivingBeamFromSource())
        {
            DeactivateSplitter();
            return;
        }

        UpdateReflectionBeam();
    }

    bool IsReceivingBeamFromSource()
    {
        if (source == null) return false;

        // Проверяем луч от источника до этого сплиттера
        Vector2 directionToSplitter = ((Vector2)transform.position - (Vector2)source.GetSourcePosition()).normalized;
        float distance = Vector2.Distance(source.GetSourcePosition(), transform.position);

        RaycastHit2D hit = Physics2D.Raycast(source.GetSourcePosition(), directionToSplitter, distance);

        // Если луч прерван другим объектом до достижения этого сплиттера
        if (hit.collider != null && hit.collider.gameObject != gameObject)
        {
            return false;
        }

        return true;
    }

    public void ActivateSplitter(ILaserSource newSource, Vector2 hitPoint, Vector2 incomingDir, Vector2 surfaceNormal)
    {
        source = newSource;
        reflectionPoint = hitPoint;
        incomingDirection = incomingDir;
        isActive = true;

        if (lineRenderer != null)
        {
            lineRenderer.enabled = true;
        }
    }

    public void DeactivateSplitter()
    {
        if (!isActive) return;

        isActive = false;
        source = null;

        // Деактивируем все дочерние сплиттеры
        foreach (var splitter in currentChildSplitters)
        {
            if (splitter != null)
            {
                splitter.DeactivateSplitter();
            }
        }
        currentChildSplitters.Clear();

        if (lineRenderer != null)
        {
            lineRenderer.enabled = false;
        }
    }

    void UpdateReflectionBeam()
    {
        if (!isActive || lineRenderer == null) return;

        Vector2[] reflectionDirections = CalculateReflectionDirections();

        // Увеличиваем количество позиций в LineRenderer для двух лучей
        lineRenderer.positionCount = 4;

        bool hitSplitter = false;

        // Обрабатываем первый луч
        Vector2 firstDirection = reflectionDirections[0];
        RaycastHit2D firstHit = Physics2D.Raycast(reflectionPoint + firstDirection * 0.1f, firstDirection, 250f);
        Vector2 firstEndPoint;

        if (firstHit.collider != null)
        {
            firstEndPoint = firstHit.point;

            // Активируем другие сплиттеры
            if (firstHit.collider.CompareTag("Reflector"))
            {
                Splitter otherSplitter = firstHit.collider.GetComponent<Splitter>();
                if (otherSplitter != null && otherSplitter != this)
                {
                    otherSplitter.ActivateSplitter(this, firstHit.point, firstDirection, firstHit.normal);

                    // Сохраняем ссылку на активированный сплиттер
                    if (!currentChildSplitters.Contains(otherSplitter))
                    {
                        currentChildSplitters.Add(otherSplitter);
                    }
                    hitSplitter = true;
                }
            }
        }
        else
        {
            firstEndPoint = reflectionPoint + firstDirection * 250f;
        }

        // Обрабатываем второй луч
        Vector2 secondDirection = reflectionDirections[1];
        RaycastHit2D secondHit = Physics2D.Raycast(reflectionPoint + secondDirection * 0.1f, secondDirection, 250f);
        Vector2 secondEndPoint;

        if (secondHit.collider != null)
        {
            secondEndPoint = secondHit.point;

            // Активируем другие сплиттеры
            if (secondHit.collider.CompareTag("Reflector"))
            {
                Splitter otherSplitter = secondHit.collider.GetComponent<Splitter>();
                if (otherSplitter != null && otherSplitter != this)
                {
                    otherSplitter.ActivateSplitter(this, secondHit.point, secondDirection, secondHit.normal);

                    // Сохраняем ссылку на активированный сплиттер
                    if (!currentChildSplitters.Contains(otherSplitter))
                    {
                        currentChildSplitters.Add(otherSplitter);
                    }
                    hitSplitter = true;
                }
            }
        }
        else
        {
            secondEndPoint = reflectionPoint + secondDirection * 250f;
        }

        // Деактивируем сплиттеры, которые больше не попадают под луч
        if (!hitSplitter)
        {
            foreach (var splitter in currentChildSplitters)
            {
                if (splitter != null)
                {
                    splitter.DeactivateSplitter();
                }
            }
            currentChildSplitters.Clear();
        }

        // Обновляем Line Renderer для двух лучей
        lineRenderer.SetPosition(0, reflectionPoint);
        lineRenderer.SetPosition(1, firstEndPoint);
        lineRenderer.SetPosition(2, reflectionPoint);
        lineRenderer.SetPosition(3, secondEndPoint);
    }

    Vector2[] CalculateReflectionDirections()
    {
        float dotRight = Vector2.Dot(incomingDirection, Vector2.right);
        float dotLeft = Vector2.Dot(incomingDirection, Vector2.left);
        float dotUp = Vector2.Dot(incomingDirection, Vector2.up);
        float dotDown = Vector2.Dot(incomingDirection, Vector2.down);

        bool fromLeft = dotRight > 0.7f;
        bool fromRight = dotLeft > 0.7f;
        bool fromBottom = dotUp > 0.7f;
        bool fromTop = dotDown > 0.7f;

        Vector2[] directions = new Vector2[2];

        if (fromLeft || fromRight)
        {
            directions[0] = Vector2.up;
            directions[1] = Vector2.down;
        }
        else if (fromBottom || fromTop)
        {
            directions[0] = Vector2.right;
            directions[1] = Vector2.left;
        }
        else
        {
            directions[0] = Vector2.up;
            directions[1] = Vector2.down;
        }

        return directions;
    }

    void InitializeLineRenderer()
    {
        lineRenderer = GetComponent<LineRenderer>();

        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = reflectionColor;
        lineRenderer.endColor = reflectionColor;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.positionCount = 4; // Для двух лучей
        lineRenderer.useWorldSpace = true;
    }

    // Реализация интерфейса ILaserSource
    public bool IsSourceActive()
    {
        return isActive && source != null && source.IsSourceActive();
    }

    public Vector3 GetSourcePosition()
    {
        return reflectionPoint;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, Vector3.one * 0.3f);

        if (isActive && Application.isPlaying)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(reflectionPoint, 0.1f);

            Vector2[] reflectionDirs = CalculateReflectionDirections();
            Gizmos.color = reflectionColor;
            Gizmos.DrawRay(reflectionPoint, reflectionDirs[0] * 2f);
            Gizmos.DrawRay(reflectionPoint, reflectionDirs[1] * 2f);
        }
    }
}