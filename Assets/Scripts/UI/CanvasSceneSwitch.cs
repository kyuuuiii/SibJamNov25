using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasSceneSwitch : MonoBehaviour
{
    public enum SwitchMode
    {
        Instant,
        Fade
    }

    public SwitchMode switchMode = SwitchMode.Fade;
    public float fadeSpeed = 1f;
    public int sceneNumber = 0;
    private CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        canvasGroup.alpha = 0f;
        StartCoroutine(FadeAndSwitchScene());
    }

    IEnumerator FadeAndSwitchScene()
    {
        if (switchMode == SwitchMode.Instant)
        {
            canvasGroup.alpha = 1f;
            yield return null;
            SceneManager.LoadScene(sceneNumber);
            yield break; 
        }
        else
        {
            float timer = 0f;

            while (timer < 1f)
            {
                timer += Time.deltaTime * fadeSpeed;
                canvasGroup.alpha = Mathf.Clamp01(timer);
                yield return null;
            }

            canvasGroup.alpha = 1f;

            yield return new WaitForSeconds(1f);

            SceneManager.LoadScene(sceneNumber);
        }
    }
}