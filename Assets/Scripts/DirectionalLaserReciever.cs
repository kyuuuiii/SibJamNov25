using UnityEngine;

public class DirectionalLaserReceiver : MonoBehaviour
{
    [Header("Receiver Settings")]
    public Transform detectionPoint;

    [Header("Status")]
    public bool isActivated = false;

    [Header("Visual Feedback")]
    public Color inactiveColor = Color.gray;
    public Color activeColor = Color.green;
    public Color blockedColor = Color.red;

    [Header("Puzzle Connection")]
    public ShowGameObjects puzzleController;
    public int targetObjectIndex = 0;

    private SpriteRenderer spriteRenderer;
    private bool wasActivatedLastFrame = false;

    void Start()
    {
        SetupReceiver();
        UpdateVisuals();

        if (puzzleController == null)
        {
            puzzleController = GetComponent<ShowGameObjects>();
            if (puzzleController == null)
            {
                puzzleController = FindObjectOfType<ShowGameObjects>();
            }
        }
    }

    void SetupReceiver()
    {
        if (detectionPoint == null)
        {
            detectionPoint = transform.Find("DetectionPoint");
        }

        if (detectionPoint == null)
        {
            detectionPoint = transform;
        }

        if (GetComponent<Collider2D>() == null)
        {
            Debug.LogWarning($"На объекте {gameObject.name} нет коллайдера! Добавьте Collider2D для работы с лазером.");
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (isActivated != wasActivatedLastFrame)
        {
            if (isActivated)
            {
                if (puzzleController != null)
                {
                    // Пытаемся активировать объект (проверка условий внутри ShowGameObjects)
                    puzzleController.SetObjectActivation(targetObjectIndex, true);
                }

                OnActivated();
            }
            else
            {
                OnDeactivated();
            }

            UpdateVisuals();
        }

        wasActivatedLastFrame = isActivated;
        isActivated = false;
    }

    // Вызывается когда луч попадает в коллайдер
    public void OnLaserHit(Vector2 hitPoint, Vector2 laserDirection, ILaserSource source)
    {
        isActivated = true;
    }

    private void UpdateVisuals()
    {
        if (spriteRenderer != null)
        {
            // Проверяем статус объекта для визуальной обратной связи
            if (puzzleController != null && puzzleController.HasObjectBeenActivated(targetObjectIndex))
            {
                spriteRenderer.color = activeColor; // Уже активирован
            }
            else if (puzzleController != null && puzzleController.GetObjectActivationState(targetObjectIndex))
            {
                spriteRenderer.color = activeColor; // Активен сейчас
            }
            else
            {
                spriteRenderer.color = inactiveColor; // Не активен
            }
        }
    }

    private void OnActivated()
    {
        Debug.Log($"{gameObject.name} получил луч. Целевой объект: {targetObjectIndex}");
    }

    private void OnDeactivated()
    {
        // Debug.Log($"{gameObject.name} деактивирован");
    }

    // Визуализация в редакторе
    void OnDrawGizmosSelected()
    {
        if (detectionPoint != null)
        {
            // Разный цвет в зависимости от статуса
            if (puzzleController != null && puzzleController.HasObjectBeenActivated(targetObjectIndex))
            {
                Gizmos.color = Color.green; // Уже активирован
            }
            else if (puzzleController != null && puzzleController.GetObjectActivationState(targetObjectIndex))
            {
                Gizmos.color = Color.blue; // Активен сейчас
            }
            else
            {
                Gizmos.color = Color.yellow; // Не активен
            }

            Gizmos.DrawWireSphere(detectionPoint.position, 0.1f);

            Gizmos.color = Color.white;
            Vector3 center = detectionPoint.position;
            float size = 0.15f;
            Gizmos.DrawLine(center + Vector3.left * size, center + Vector3.right * size);
            Gizmos.DrawLine(center + Vector3.up * size, center + Vector3.down * size);

#if UNITY_EDITOR
            string status = puzzleController != null && puzzleController.HasObjectBeenActivated(targetObjectIndex) ? "✓" : "";
            UnityEditor.Handles.Label(detectionPoint.position + Vector3.up * 0.3f, $"Target: {targetObjectIndex} {status}");
#endif
        }
    }
}