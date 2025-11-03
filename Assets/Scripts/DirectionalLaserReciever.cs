using UnityEngine;

public class DirectionalLaserReceiver : MonoBehaviour
{
    [Header("Receiver Settings")]
    public Transform detectionPoint; // Дочерний объект для обнаружения луча

    [Header("Status")]
    public bool isActivated = false;

    [Header("Visual Feedback")]
    public Color inactiveColor = Color.gray;
    public Color activeColor = Color.green;

    [Header("Puzzle Connection")]
    public ShowGameObjects absoluteSolver; // Ссылка на скрипт управления объектами

    private SpriteRenderer spriteRenderer;
    private bool wasActivatedLastFrame = false;

    void Start()
    {
        SetupReceiver();
        UpdateVisuals();

        // Автоматически находим ShowGameObjects если не назначен
        if (absoluteSolver == null)
        {
            absoluteSolver = GetComponent<ShowGameObjects>();
            if (absoluteSolver == null)
            {
                absoluteSolver = FindObjectOfType<ShowGameObjects>();
            }
        }
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
        // Проверяем изменение статуса и передаем в absoluteSolver
        if (isActivated && !wasActivatedLastFrame)
        {
            if (absoluteSolver != null)
            {
                // Увеличиваем прогресс ТОЛЬКО ЗДЕСЬ
                absoluteSolver.IncreaseProgress();
                Debug.Log($"Прогресс увеличен через приемник. Текущий прогресс: {absoluteSolver.currentProgress}");
            }

            UpdateVisuals();
            OnActivated();
        }

        wasActivatedLastFrame = isActivated;

        isActivated = false;
    }

    public void OnLaserHit(Vector2 hitPoint, Vector2 laserDirection, ILaserSource source)
    {
        isActivated = true;
        Debug.Log("Луч попал в приемник!");
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
        Debug.Log($"{gameObject.name} activated by laser! Прогресс увеличен");
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