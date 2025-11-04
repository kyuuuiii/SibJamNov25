using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    public GameObject settingsPanel;
    public GameObject menuPanel;
    //public int sceneNumber;
    public GameObject canvas;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Settings()
    {
        settingsPanel.SetActive(true);
        menuPanel.SetActive(false);

    }
    public void Menu()
    {
        menuPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void MenuFromGame()
    {
        SceneManager.LoadScene(0);
    }
    public void Restart()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
    public void AnySceneLoader(int sceneNumber)
    {
        canvas.SetActive(false);
        SceneManager.LoadScene(sceneNumber);
    }
}
