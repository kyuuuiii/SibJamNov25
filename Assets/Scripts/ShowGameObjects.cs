using UnityEngine;

public class ShowGameObjects : MonoBehaviour
{
    [System.Serializable]
    public class GameObjectData
    {
        public GameObject gameObject;
        public bool shouldBeActive;
        public int requiredProgressLevel;
    }

    public GameObjectData[] gameObjectsData;
    public int currentProgress = 0;

    void Start()
    {
        ForceHideAllObjects();
        UpdateGameObjects();
    }

    void Update()
    {
        UpdateGameObjects();
    }

    public void UpdateGameObjects()
    {
        foreach (GameObjectData data in gameObjectsData)
        {
            if (data.gameObject != null)
            {
                bool shouldActivate = data.shouldBeActive && currentProgress >= data.requiredProgressLevel;
                if (data.gameObject.activeSelf != shouldActivate)
                {
                    data.gameObject.SetActive(shouldActivate);
                }
            }
        }
    }

    public void IncreaseProgress()
    {
        currentProgress++;
        Debug.Log($"Прогресс увеличен: {currentProgress}");
        UpdateGameObjects();
    }

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

    private void ForceHideAllObjects()
    {
        foreach (GameObjectData data in gameObjectsData)
        {
            if (data.gameObject != null)
            {
                data.gameObject.SetActive(false);
            }
        }
        Debug.Log("Все объекты принудительно скрыты при старте сцены");
    }

    void OnValidate()
    {
        if (Application.isPlaying)
        {
            UpdateGameObjects();
        }
    }
}