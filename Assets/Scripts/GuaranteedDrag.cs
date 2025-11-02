using UnityEngine;
using UnityEngine.InputSystem;

public class GuaranteedDrag : MonoBehaviour
{
    void Update()
    {
        // Самый простой способ - перемещать объект к мышке при клике
        if (Mouse.current.leftButton.isPressed)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 0));
            worldPos.z = transform.position.z; // Сохраняем Z координату

            transform.position = worldPos;
        }
    }
}