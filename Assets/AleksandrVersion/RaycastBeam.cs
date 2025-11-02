using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class RaycastBeam2D : MonoBehaviour, ILaserSource
{
  [Header("Raycast Settings")]
  public float rayDistance = 250f;
  public string targetTag = "part";
  public string reflectorTag = "Reflector";
  public LayerMask layerMask = -1;

  [Header("Line Renderer Settings")]
  public Material lineMaterial;
  public float lineWidth = 0.1f;
  public Color lineColor = Color.red;
  public Color hitColor = Color.green;

  private LineRenderer lineRenderer;
  private Vector3 hitPoint;
  private bool hasHit;
  private List<LaserReflector> currentFrameReflectors = new List<LaserReflector>();

  void Reset()
  {
    SetupLineRendererInEditor();
  }

  void Start()
  {
    lineRenderer = GetComponent<LineRenderer>();
    if (lineRenderer != null)
    {
      lineRenderer.enabled = true;
    }
  }

  void Update()
  {
    // Сначала деактивируем все рефлекторы, которые были активны в предыдущем кадре
    DeactivateAllReflectors();

    // Затем выполняем новый raycast
    ShootRaycast2D();
    UpdateLineRenderer();
  }

  void ShootRaycast2D()
  {
    Vector2 direction = transform.right;
    RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, rayDistance, layerMask);

    currentFrameReflectors.Clear();

    if (hit.collider != null)
    {
      hitPoint = hit.point;

      if (hit.collider.CompareTag(reflectorTag))
      {
        LaserReflector reflector = hit.collider.GetComponent<LaserReflector>();
        if (reflector != null)
        {
          reflector.ActivateReflector(this, hit.point, direction, hit.normal);
          currentFrameReflectors.Add(reflector);
          hasHit = true;
        }
      }
      else if (hit.collider.CompareTag(targetTag))
      {
        hasHit = true;
      }
      else
      {
        hasHit = false;
      }
    }
    else
    {
      hitPoint = transform.position + (Vector3)direction * rayDistance;
      hasHit = false;
    }
  }

  void DeactivateAllReflectors()
  {
    // Деактивируем все рефлекторы, которые не попали под луч в этом кадре
    foreach (var reflector in FindObjectsOfType<LaserReflector>())
    {
      if (!currentFrameReflectors.Contains(reflector))
      {
        reflector.DeactivateReflector();
      }
    }
  }

  void UpdateLineRenderer()
  {
    if (lineRenderer == null) return;

    lineRenderer.enabled = true;
    lineRenderer.SetPosition(0, transform.position);
    lineRenderer.SetPosition(1, hitPoint);

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

  [ContextMenu("Setup Line Renderer")]
  void SetupLineRendererInEditor()
  {
    lineRenderer = GetComponent<LineRenderer>();

    if (lineRenderer == null)
    {
      lineRenderer = gameObject.AddComponent<LineRenderer>();
    }

    lineRenderer.material = lineMaterial;
    lineRenderer.startColor = lineColor;
    lineRenderer.endColor = lineColor;
    lineRenderer.startWidth = lineWidth;
    lineRenderer.endWidth = lineWidth;
    lineRenderer.positionCount = 2;
    lineRenderer.useWorldSpace = true;
    lineRenderer.enabled = true;

    Vector3 endPoint = transform.position + transform.right * rayDistance;
    lineRenderer.SetPosition(0, transform.position);
    lineRenderer.SetPosition(1, endPoint);

#if UNITY_EDITOR
    UnityEditor.EditorUtility.SetDirty(this);
    UnityEditor.EditorUtility.SetDirty(lineRenderer);
#endif
  }

  // Реализация интерфейса ILaserSource
  public bool IsSourceActive()
  {
    return isActiveAndEnabled;
  }

  public Vector3 GetSourcePosition()
  {
    return transform.position;
  }

  void OnDrawGizmosSelected()
  {
    Vector2 direction = transform.right;
    Gizmos.color = Color.blue;
    Gizmos.DrawRay(transform.position, direction * rayDistance);

    if (Application.isPlaying)
    {
      Gizmos.color = hasHit ? Color.green : Color.red;
      Gizmos.DrawWireSphere(hitPoint, 0.2f);
    }
  }
}

// Интерфейс для всех источников лазера
public interface ILaserSource
{
  bool IsSourceActive();
  Vector3 GetSourcePosition();
}