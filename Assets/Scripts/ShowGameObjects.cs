using UnityEngine;

public class ShowGameObjects : MonoBehaviour
{
    [System.Serializable]
    public class GameObjectData
    {
        public GameObject gameObject;
        public bool shouldBeActive;
    }

    public GameObjectData[] gameObjectsData;
    public bool isSolved;

    void Start()
    {
        foreach (GameObjectData data in gameObjectsData)
        {
            if (data.gameObject != null)
                data.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        Prozrenie(isSolved);
    }

    void Prozrenie(bool isSolved)
    {
        foreach (GameObjectData data in gameObjectsData)
        {
            if (data.gameObject != null)
            {
                data.gameObject.SetActive(isSolved && data.shouldBeActive);
            }
        }
    }
}