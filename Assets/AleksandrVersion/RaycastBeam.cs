using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RaycastBeam2D : MonoBehaviour
{
  [Header("Raycast Settings")]
  public float rayDistance = 250f;
  public string targetTag = "part";
  public LayerMask layerMask = -1; // Все слои по умолчанию

  [Header("Line Renderer Settings")]
  public Material lineMaterial;
  public float lineWidth = 0.1f;
  public Color lineColor = Color.red;
  public Color hitColor = Color.green;

  private LineRenderer lineRenderer;
  private Vector3 hitPoint;
  private bool hasHit;

  void Reset()
  {
    // Автоматическая настройка при добавлении компонента
    SetupLineRendererInEditor();
  }

  void Start()
  {
    // Получаем ссылку на Line Renderer
    lineRenderer = GetComponent<LineRenderer>();

    // Включаем Line Renderer при старте игры
    if (lineRenderer != null)
    {
      lineRenderer.enabled = true;
    }
  }

  void Update()
  {
    ShootRaycast2D();
    UpdateLineRenderer();
  }

  void ShootRaycast2D()
  {
    // Получаем направление вперед в 2D пространстве
    Vector2 direction = transform.right; // В 2D обычно используют right вместо forward

    // Выстреливаем луч в 2D
    RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, rayDistance, layerMask);

    // Проверяем попадание
    if (hit.collider != null)
    {
      Debug.Log("Попал");
      // Проверяем тег объекта
      if (hit.collider.CompareTag(targetTag))
      {
        Debug.Log("Попал по тегу");
        hitPoint = hit.point;
        hasHit = true;
      }
      else
      {
        Debug.Log("Попал не по тегу");
        // Попадание есть, но не в целевой тег - используем точку попадания
        hitPoint = hit.point;
        hasHit = false;
      }
    }
    else
    {
      Debug.Log("Не попал");
      // Попадания нет - используем максимальную дистанцию
      hitPoint = transform.position + (Vector3)direction * rayDistance;
      hasHit = false;
    }
  }

  void UpdateLineRenderer()
  {
    if (lineRenderer == null) return;

    // Всегда обновляем позиции линии
    lineRenderer.enabled = true;
    lineRenderer.SetPosition(0, transform.position);
    lineRenderer.SetPosition(1, hitPoint);

    // Меняем цвет в зависимости от попадания в целевой объект
    if (hasHit)
    {
      lineRenderer.startColor = hitColor;
      lineRenderer.endColor = hitColor;
    }
    else
    {
      lineRenderer.startColor = lineColor;
      lineRenderer.endColor = lineColor;
    }
  }

  // Метод для настройки в редакторе
  [ContextMenu("Setup Line Renderer")]
  void SetupLineRendererInEditor()
  {
    lineRenderer = GetComponent<LineRenderer>();

    if (lineRenderer == null)
    {
      lineRenderer = gameObject.AddComponent<LineRenderer>();
    }

    // Настройка Line Renderer
    lineRenderer.material = lineMaterial;
    lineRenderer.startColor = lineColor;
    lineRenderer.endColor = lineColor;
    lineRenderer.startWidth = lineWidth;
    lineRenderer.endWidth = lineWidth;
    lineRenderer.positionCount = 2;
    lineRenderer.useWorldSpace = true;
    lineRenderer.enabled = true;

    // Устанавливаем начальные позиции
    Vector3 endPoint = transform.position + transform.right * rayDistance;
    lineRenderer.SetPosition(0, transform.position);
    lineRenderer.SetPosition(1, endPoint);

#if UNITY_EDITOR
    UnityEditor.EditorUtility.SetDirty(this);
    UnityEditor.EditorUtility.SetDirty(lineRenderer);
#endif
  }

  // Визуализация луча в редакторе
  void OnDrawGizmosSelected()
  {
    Vector2 direction = transform.right;

    Gizmos.color = Color.blue;
    Gizmos.DrawRay(transform.position, direction * rayDistance);

    // Показываем точку попадания в редакторе во время игры
    if (Application.isPlaying)
    {
      Gizmos.color = hasHit ? Color.green : Color.red;
      Gizmos.DrawWireSphere(hitPoint, 0.2f);
    }
  }

  // Методы для получения информации о луче
  public Vector3 GetHitPoint()
  {
    return hitPoint;
  }

  public bool HasHitTarget()
  {
    return hasHit;
  }

  public float GetCurrentBeamLength()
  {
    return Vector3.Distance(transform.position, hitPoint);
  }

  // Дополнительный метод для получения информации о попадании
  public RaycastHit2D GetLastHitInfo()
  {
    Vector2 direction = transform.right;
    return Physics2D.Raycast(transform.position, direction, rayDistance, layerMask);
  }
}