using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    public GameObject settingsPanel;
    public GameObject menuPanel;
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
}
