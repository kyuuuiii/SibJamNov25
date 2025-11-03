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
                    Debug.Log($"Захватили: {gameObject.name}, расстояние: {distance:F2}");
                }
            }
        }

        if (isDragging)
        {
            transform.position = new Vector3(mouseWorldPos.x, mouseWorldPos.y, transform.position.z);

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
}