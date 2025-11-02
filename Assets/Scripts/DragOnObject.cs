using UnityEngine;
using UnityEngine.InputSystem;

public class DragOnObject : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            TryStartDrag();
        }

        if (isDragging)
        {
            if (Mouse.current.leftButton.isPressed)
            {
                ContinueDrag();
            }
            else
            {
                EndDrag();
            }
        }
    }

    void TryStartDrag()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        RaycastHit2D hit = Physics2D.Raycast(GetMouseWorldPosition(mousePos), Vector2.zero);

        if (hit.collider != null && hit.collider.gameObject == this.gameObject)
        {
            isDragging = true;
            offset = transform.position - GetMouseWorldPosition(mousePos);
            Debug.Log($"Начали перемещение: {gameObject.name}");
        }
    }

    void ContinueDrag()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        transform.position = GetMouseWorldPosition(mousePos) + offset;
    }

    void EndDrag()
    {
        isDragging = false;
        Debug.Log($"Закончили перемещение: {gameObject.name}");
    }

    Vector3 GetMouseWorldPosition(Vector2 mousePosition)
    {
        Vector3 mousePos = new Vector3(mousePosition.x, mousePosition.y, -mainCamera.transform.position.z);
        return mainCamera.ScreenToWorldPoint(mousePos);
    }
}