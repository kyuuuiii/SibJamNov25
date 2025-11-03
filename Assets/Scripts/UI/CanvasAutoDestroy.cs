using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CanvasAutoDestroy : MonoBehaviour
{
    public float lifetime = 3f;
    public float fadeSpeed = 1f;
    private CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        canvasGroup.alpha = 0f;
        StartCoroutine(FadeInAndDestroy());
    }

    IEnumerator FadeInAndDestroy()
    {
        float timer = 0f;

        while (timer < 1f)
        {
            timer += Time.deltaTime * fadeSpeed;
            canvasGroup.alpha = Mathf.Clamp01(timer);
            yield return null;
        }

        canvasGroup.alpha = 1f;

        yield return new WaitForSeconds(lifetime);

        timer = 0f;
        while (timer < 1f)
        {
            timer += Time.deltaTime * fadeSpeed;
            canvasGroup.alpha = 1f - Mathf.Clamp01(timer);
            yield return null;
        }

        canvasGroup.alpha = 0f;
        Destroy(gameObject);
    }
}