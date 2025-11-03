using UnityEngine;

public class ShowGameObjects : MonoBehaviour
{
    [System.Serializable]
    public class GameObjectData
    {
        public GameObject gameObject;
        public bool shouldBeActive;
        public int requiredProgressLevel; // Уровень прогресса для активации
    }

    public GameObjectData[] gameObjectsData;
    public int currentProgress = 0; // Текущий уровень прогресса

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
                // Активируем объект если достигнут нужный уровень прогресса
                bool shouldActivate = data.shouldBeActive && currentProgress >= data.requiredProgressLevel;
                if (data.gameObject.activeSelf != shouldActivate)
                {
                    data.gameObject.SetActive(shouldActivate);
                }
            }
        }
    }

    // Метод для увеличения прогресса
    public void IncreaseProgress()
    {
        currentProgress++;
        Debug.Log($"Прогресс увеличен: {currentProgress}");
        UpdateGameObjects();
    }

    // Метод для изменения shouldBeActive конкретного объекта
    public void SetObjectActivation(int objectIndex, bool shouldActivate)
    {
        if (objectIndex >= 0 && objectIndex < gameObjectsData.Length)
        {
            gameObjectsData[objectIndex].shouldBeActive = shouldActivate;
            UpdateGameObjects();
            Debug.Log($"Объект {objectIndex} установлен shouldBeActive: {shouldActivate}");
        }
        else
        {
            Debug.LogWarning($"Неверный индекс объекта: {objectIndex}. Допустимый диапазон: 0-{gameObjectsData.Length - 1}");
        }
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