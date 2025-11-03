using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LaserReflector : MonoBehaviour, ILaserSource
{
    [Header("Reflection Settings")]
    public bool reflectUp = true;

    [Header("Visual Settings")]
    public Color reflectionColor = Color.blue;
    public float lineWidth = 0.05f;
    public Material lineMaterial;

    public LineRenderer lineRenderer;
    public bool isActive = false;
    public Vector2 reflectionPoint;
    public Vector2 incomingDirection;
    [SerializeField] public ILaserSource source;
    public List<LaserReflector> currentChildReflectors = new List<LaserReflector>();



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
            DeactivateReflector();
            return;
        }

        UpdateReflectionBeam();
    }

    bool IsReceivingBeamFromSource()
    {
        if (source == null) return false;

        // Проверяем луч от источника до этого рефлектора
        Vector2 directionToReflector = ((Vector2)transform.position - (Vector2)source.GetSourcePosition()).normalized;
        float distance = Vector2.Distance(source.GetSourcePosition(), transform.position);

        RaycastHit2D hit = Physics2D.Raycast(source.GetSourcePosition(), directionToReflector, distance);

        // Если луч прерван другим объектом до достижения этого рефлектора
        if (hit.collider != null && hit.collider.gameObject != gameObject)
        {
            return false;
        }

        return true;
    }

    public void ActivateReflector(ILaserSource newSource, Vector2 hitPoint, Vector2 incomingDir, Vector2 surfaceNormal)
    {
        source = newSource;
        reflectionPoint = hitPoint;
        incomingDirection = incomingDir;
        isActive = true;

        if (lineRenderer != null)
        {
            lineRenderer.enabled = true;
        }

        UpdateReflectionBeam();
    }

    public void DeactivateReflector()
    {
        if (!isActive) return;

        isActive = false;
        source = null;

        // Деактивируем все дочерние рефлекторы
        foreach (var reflector in currentChildReflectors)
        {
            if (reflector != null)
            {
                reflector.DeactivateReflector();
            }
        }
        currentChildReflectors.Clear();

        if (lineRenderer != null)
        {
            lineRenderer.enabled = false;
        }
    }

    void UpdateReflectionBeam()
    {
        if (!isActive || lineRenderer == null) return;

        Vector2 reflectionDirection = CalculateReflectionDirection();

        RaycastHit2D hit = Physics2D.Raycast(reflectionPoint + reflectionDirection * 0.1f, reflectionDirection, 250f);

        Vector2 endPoint;
        bool hitReflector = false;

        if (hit.collider != null)
        {
            endPoint = hit.point;

            if (hit.collider.CompareTag("Reflector"))
            {
                LaserReflector otherReflector = hit.collider.GetComponent<LaserReflector>();
                if (otherReflector != null && otherReflector != this)
                {
                    otherReflector.ActivateReflector(this, hit.point, reflectionDirection, hit.normal);

                    if (!currentChildReflectors.Contains(otherReflector))
                    {
                        currentChildReflectors.Add(otherReflector);
                    }
                    hitReflector = true;
                }
            }
            else
            {
                DirectionalLaserReceiver receiver = hit.collider.GetComponent<DirectionalLaserReceiver>();
                if (receiver != null)
                {
                    receiver.OnLaserHit(hit.point, reflectionDirection, this);
                }
            }
        }
        else
        {
            endPoint = reflectionPoint + reflectionDirection * 250f;
        }

        if (!hitReflector)
        {
            foreach (var reflector in currentChildReflectors)
            {
                if (reflector != null)
                {
                    reflector.DeactivateReflector();
                }
            }
            currentChildReflectors.Clear();
        }

        lineRenderer.SetPosition(0, reflectionPoint);
        lineRenderer.SetPosition(1, endPoint);
    }

    Vector2 CalculateReflectionDirection()
    {
        // Преобразуем входящее направление в локальные координаты объекта
        Vector2 localIncoming = transform.InverseTransformDirection(incomingDirection);

        // Определяем с какой стороны приходит луч в локальных координатах
        float absX = Mathf.Abs(localIncoming.x);
        float absY = Mathf.Abs(localIncoming.y);

        Vector2 localReflectionDirection = Vector2.zero;

        if (absX > absY)
        {
            // Луч приходит слева/справа - отражаем вверх/вниз
            localReflectionDirection = reflectUp ? Vector2.up : Vector2.down;
        }
        else
        {
            // Луч приходит сверху/снизу - отражаем влево/вправо
            localReflectionDirection = reflectUp ? Vector2.right : Vector2.left;
        }

        // Преобразуем направление отражения обратно в мировые координаты
        Vector2 worldReflectionDirection = transform.TransformDirection(localReflectionDirection);

        return worldReflectionDirection.normalized;
    }

    void InitializeLineRenderer()
    {
        lineRenderer = GetComponent<LineRenderer>();

        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        if (lineMaterial != null)
        {
            lineRenderer.material = lineMaterial;
        }
        else
        {
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        }

        lineRenderer.startColor = reflectionColor;
        lineRenderer.endColor = reflectionColor;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.positionCount = 2;
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

            Vector2 reflectionDir = CalculateReflectionDirection();
            Gizmos.color = reflectionColor;
            Gizmos.DrawRay(reflectionPoint, reflectionDir * 2f);
        }
    }

}