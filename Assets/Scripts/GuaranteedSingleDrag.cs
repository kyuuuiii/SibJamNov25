using UnityEngine;
using UnityEngine.InputSystem;

public class GuaranteedSingleDrag : MonoBehaviour
{
    private Camera mainCamera;
    private bool isDragging = false;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, 10f));

        // Только когда нажимаем кнопку - проверяем попадание
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            // Самый надежный способ - проверять расстояние до объекта
            float distance = Vector2.Distance(transform.position, mouseWorldPos);
            float pickDistance = 1.0f; // Расстояние для захвата

            // Если объект достаточно близко к курсору - захватываем его
            if (distance < pickDistance)
            {
                // Дополнительная проверка через коллайдер
                Collider2D hit = Physics2D.OverlapPoint(mouseWorldPos);
                if (hit != null && hit.gameObject == this.gameObject)
                {
                    isDragging = true;
                    Debug.Log($"Захватили: {gameObject.name}, расстояние: {distance:F2}");
                }
            }
        }

        if (isDragging)
        {
            // Перемещаем объект
            transform.position = new Vector3(mouseWorldPos.x, mouseWorldPos.y, transform.position.z);

            if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                isDragging = false;
                Debug.Log($"Отпустили: {gameObject.name}");
            }
        }
    }
}