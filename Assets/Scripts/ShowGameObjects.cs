using UnityEngine;

public class ShowGameObjects : MonoBehaviour
{
    [System.Serializable]
    public class GameObjectData
    {
        public GameObject gameObject;
        public bool shouldBeActive;
        public bool hasBeenActivated; // Флаг того, что объект уже был активирован
    }

    public GameObjectData[] gameObjectsData;

    void Start()
    {
        // Инициализируем объекты согласно их начальным настройкам
        UpdateGameObjects();
    }

    void Update()
    {
        // Постоянно обновляем состояние объектов
        UpdateGameObjects();
    }

    public void UpdateGameObjects()
    {
        foreach (GameObjectData data in gameObjectsData)
        {
            if (data.gameObject != null)
            {
                // Активируем/деактивируем объекты на основе shouldBeActive
                if (data.gameObject.activeSelf != data.shouldBeActive)
                {
                    data.gameObject.SetActive(data.shouldBeActive);
                }
            }
        }
    }

    // Метод для изменения shouldBeActive конкретного объекта (одноразовая активация)
    public void SetObjectActivation(int objectIndex, bool shouldActivate)
    {
        if (objectIndex >= 0 && objectIndex < gameObjectsData.Length)
        {
            // Если объект еще не был активирован, активируем его
            if (!gameObjectsData[objectIndex].hasBeenActivated && shouldActivate)
            {
                gameObjectsData[objectIndex].shouldBeActive = true;
                gameObjectsData[objectIndex].hasBeenActivated = true;
                UpdateGameObjects();
                Debug.Log($"Объект {objectIndex} активирован одноразово");
            }
            // Если нужно деактивировать (для reset), сбрасываем флаг
            else if (!shouldActivate)
            {
                gameObjectsData[objectIndex].shouldBeActive = false;
                gameObjectsData[objectIndex].hasBeenActivated = false;
                UpdateGameObjects();
            }
        }
        else
        {
            Debug.LogWarning($"Неверный индекс объекта: {objectIndex}. Допустимый диапазон: 0-{gameObjectsData.Length - 1}");
        }
    }

    // Метод для принудительной активации объекта (игнорируя одноразовость)
    public void ForceSetObjectActivation(int objectIndex, bool shouldActivate)
    {
        if (objectIndex >= 0 && objectIndex < gameObjectsData.Length)
        {
            gameObjectsData[objectIndex].shouldBeActive = shouldActivate;
            UpdateGameObjects();
        }
    }

    // Метод для сброса всех объектов
    public void ResetAllObjects()
    {
        foreach (GameObjectData data in gameObjectsData)
        {
            data.shouldBeActive = false;
            data.hasBeenActivated = false;
        }
        UpdateGameObjects();
        Debug.Log("Все объекты сброшены");
    }

    // Метод для получения текущего состояния объекта
    public bool GetObjectActivationState(int objectIndex)
    {
        if (objectIndex >= 0 && objectIndex < gameObjectsData.Length)
        {
            return gameObjectsData[objectIndex].shouldBeActive;
        }
        return false;
    }

    // Метод для проверки, был ли объект уже активирован
    public bool HasObjectBeenActivated(int objectIndex)
    {
        if (objectIndex >= 0 && objectIndex < gameObjectsData.Length)
        {
            return gameObjectsData[objectIndex].hasBeenActivated;
        }
        return false;
    }

    // Для отладки в редакторе
    void OnValidate()
    {
        if (Application.isPlaying)
        {
            UpdateGameObjects();
        }
    }
}