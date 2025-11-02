using UnityEngine;

public class LineSplitter : MonoBehaviour
{
    [Header("Split Settings")]
    public float splitAngle = 45f; // Угол расщепления
    public bool hasSplit = false;

    [Header("Prefab References")]
    public GameObject linePrefab; // Префаб линии

    public void SplitLine(Vector2 hitPoint, Vector2 incomingDirection, float originalLength)
    {
        if (hasSplit) return;

        hasSplit = true;

        // Вычисляем направление отраженной линии
        Vector2 splitDirection = CalculateSplitDirection(incomingDirection);

        // Создаем новую линию
        CreateNewLine(hitPoint, splitDirection, originalLength);

        // Визуальный эффект (опционально)
        OnSplit();
    }

    Vector2 CalculateSplitDirection(Vector2 incomingDirection)
    {
        // Можно реализовать разные варианты:

        // 1. Случайное направление
        // return Random.insideUnitCircle.normalized;

        // 2. Отражение от нормали поверхности
        // Vector2 surfaceNormal = transform.up;
        // return Vector2.Reflect(incomingDirection, surfaceNormal);

        // 3. Фиксированный угол относительно входящего направления
        float angle = Vector2.SignedAngle(Vector2.right, incomingDirection);
        float newAngle = angle + splitAngle; 
        return new Vector2(Mathf.Cos(newAngle * Mathf.Deg2Rad), Mathf.Sin(newAngle * Mathf.Deg2Rad));
    }

    void CreateNewLine(Vector2 startPoint, Vector2 direction, float length)
    {
        if (linePrefab == null)
        {
            Debug.LogWarning("Line prefab is not assigned!");
            return;
        }

        GameObject newLine = Instantiate(linePrefab, startPoint, Quaternion.identity);
        newLine.transform.right = direction;

        LineController newLineController = newLine.GetComponent<LineController>();
        if (newLineController != null)
        {
            newLineController.lineLength = length;
        }

        // Опционально: устанавливаем родителя
        newLine.transform.SetParent(transform);
    }

    void OnSplit()
    {
        // Визуальные/звуковые эффекты при расщеплении
        GetComponent<SpriteRenderer>().color = Color.green;

        // Можно добавить анимацию, звук и т.д.
    }

    // Сброс состояния (если нужно)
    public void ResetSplit()
    {
        hasSplit = false;
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}