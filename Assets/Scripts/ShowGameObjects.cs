using UnityEngine;

public class ShowGameObjects : MonoBehaviour
{
    public GameObject gameObject;
    public bool isSolved;

    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Prozrenie(isSolved);
    }

    void Prozrenie(bool isSolved)
    {
        if (isSolved) gameObject.SetActive(true);
    }
}