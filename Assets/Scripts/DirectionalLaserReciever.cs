using UnityEngine;

public class SimpleLaserReceiver : MonoBehaviour
{
    [Header("Receiver Settings")]
    public Transform detectionPoint; // Дочерний объект для обнаружения луча

    [Header("Status")]
    public bool isActivated = false;

    [Header("Visual Feedback")]
    public Color inactiveColor = Color.gray;
    public Color activeColor = Color.green;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        SetupReceiver();
        UpdateVisuals();
    }

    void SetupReceiver()
    {
        // Автоматически находим дочерний объект если не назначен
        if (detectionPoint == null)
        {
            detectionPoint = transform.Find("DetectionPoint");
        }

        // Если дочерний объект не найден, используем текущий объект
        if (detectionPoint == null)
        {
            detectionPoint = transform;
        }

        // Убеждаемся, что есть коллайдер на основном объекте
        if (GetComponent<Collider2D>() == null)
        {
            Debug.LogWarning($"На объекте {gameObject.name} нет коллайдера! Добавьте Collider2D для работы с лазером.");
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Сбрасываем статус каждый кадр, будет установлен при попадании луча
        bool wasActivated = isActivated;
        isActivated = false;

        // Обновляем визуал если статус изменился
        if (wasActivated && !isActivated)
        {
            UpdateVisuals();
            OnDeactivated();
        }
    }

    // Вызывается когда луч попадает в коллайдер
    public void OnLaserHit(Vector2 hitPoint, Vector2 laserDirection, ILaserSource source)
    {
        isActivated = true;
        UpdateVisuals();
        OnActivated();
    }

    private void UpdateVisuals()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = isActivated ? activeColor : inactiveColor;
        }
    }

    private void OnActivated()
    {
        // Действия при активации
        // Debug.Log($"{gameObject.name} activated by laser!");
    }

    private void OnDeactivated()
    {
        // Действия при деактивации
        // Debug.Log($"{gameObject.name} deactivated");
    }

    // Визуализация в редакторе
    void OnDrawGizmosSelected()
    {
        if (detectionPoint != null)
        {
            Gizmos.color = isActivated ? Color.green : Color.yellow;
            Gizmos.DrawWireSphere(detectionPoint.position, 0.1f);

            // Рисуем значок прицела
            Gizmos.color = Color.white;
            Vector3 center = detectionPoint.position;
            float size = 0.15f;
            Gizmos.DrawLine(center + Vector3.left * size, center + Vector3.right * size);
            Gizmos.DrawLine(center + Vector3.up * size, center + Vector3.down * size);
        }
    }
}