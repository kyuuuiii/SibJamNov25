using UnityEngine;
using UnityEngine.InputSystem;

public class GuaranteedSingleDrag : MonoBehaviour
{
    private Camera mainCamera;
    private bool isDragging = false;
    private Vector3 offset;

    [Header("Collision Settings")]
    public float minDistance = 1.0f; // Минимальное расстояние между объектами
    public LayerMask collisionLayers = -1; // С какими слоями сталкиваться

    void Start()
    {
        mainCamera = Camera.main;

        // Если minDistance не установлен, используем размер спрайта
        if (minDistance <= 0)
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                minDistance = Mathf.Max(sr.bounds.size.x, sr.bounds.size.y) * 0.8f;
            }
            else
            {
                minDistance = 1.0f;
            }
        }
    }

    void Update()
    {
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, 10f));

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            float distance = Vector2.Distance(transform.position, mouseWorldPos);
            float pickDistance = 1.0f;

            if (distance < pickDistance)
            {
                Collider2D hit = Physics2D.OverlapPoint(mouseWorldPos);
                if (hit != null && hit.gameObject == this.gameObject)
                {
                    isDragging = true;
                    offset = transform.position - mouseWorldPos;
                    Debug.Log($"Захватили: {gameObject.name}, расстояние: {distance:F2}");
                }
            }
        }

        if (isDragging)
        {
            // Получаем желаемую позицию
            Vector3 desiredPosition = new Vector3(mouseWorldPos.x, mouseWorldPos.y, transform.position.z) + offset;

            // Проверяем столкновения и корректируем позицию
            Vector3 correctedPosition = GetCorrectedPosition(desiredPosition);
            transform.position = correctedPosition;

            if (Keyboard.current.qKey.wasPressedThisFrame)
            {
                transform.Rotate(0, 0, 90);
                //Debug.Log($"Повернули {gameObject.name} на +90°");
            }

            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                transform.Rotate(0, 0, -90);
                //Debug.Log($"Повернули {gameObject.name} на -90°");
            }

            if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                isDragging = false;
                //Debug.Log($"Отпустили: {gameObject.name}");
            }
        }
    }

    private Vector3 GetCorrectedPosition(Vector3 desiredPosition)
    {
        Vector3 correctedPosition = desiredPosition;

        // Ищем все коллайдеры в радиусе
        Collider2D[] nearbyColliders = Physics2D.OverlapCircleAll(desiredPosition, minDistance * 2f, collisionLayers);

        foreach (Collider2D collider in nearbyColliders)
        {
            if (collider.gameObject != this.gameObject) // Игнорируем себя
            {
                // Получаем ближайшую точку на коллайдере
                Vector2 closestPoint = collider.ClosestPoint(desiredPosition);
                float distance = Vector2.Distance(desiredPosition, closestPoint);

                // Если расстояние меньше минимального, отталкиваем
                if (distance < minDistance)
                {
                    Vector2 direction = ((Vector2)desiredPosition - closestPoint).normalized;

                    // Если объекты уже пересекаются, direction может быть нулевым
                    if (direction.magnitude < 0.01f)
                    {
                        direction = (desiredPosition - collider.transform.position).normalized;
                        if (direction.magnitude < 0.01f)
                            direction = Vector2.up; // Направление по умолчанию
                    }

                    correctedPosition = closestPoint + direction * minDistance;
                }
            }
        }

        return correctedPosition;
    }

    // Визуализация зоны отталкивания в редакторе
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, minDistance);

        if (isDragging)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, minDistance);
        }
    }
}