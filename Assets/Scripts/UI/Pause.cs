using UnityEngine;
using UnityEngine.InputSystem;

public class Pause : MonoBehaviour
{
    public GameObject pauseMenu;
    private bool pauseIsActive = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
        
    }

    public void PauseGame()
    {
        if (Time.timeScale == 1)
        {
            pauseMenu.SetActive(true);
            pauseIsActive = true;
            Time.timeScale = 0;
        }
        else if (Time.timeScale == 0)
        {
            pauseMenu.SetActive(false);
            pauseIsActive = false;
            Time.timeScale = 1.0f;
        }
    }
}
