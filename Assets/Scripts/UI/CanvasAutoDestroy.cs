using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CanvasAutoDestroy : MonoBehaviour
{
    public enum DestroyMode
    {
        Self,
        Target,
        Both
    }

    public DestroyMode destroyMode = DestroyMode.Self;
    public GameObject targetObject;
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

        switch (destroyMode)
        {
            case DestroyMode.Self:
                Destroy(gameObject);
                break;

            case DestroyMode.Target:
                if (targetObject != null)
                {
                    Destroy(targetObject);
                }
                else
                {
                    Debug.LogWarning("Target object is not assigned in CanvasAutoDestroy script!");
                    Destroy(gameObject);
                }
                break;

            case DestroyMode.Both:
                if (targetObject != null)
                {
                    Destroy(targetObject);
                }
                Destroy(gameObject);
                break;
        }
    }

    public void SetTarget(GameObject newTarget)
    {
        targetObject = newTarget;
    }

    public void SetDestroyMode(DestroyMode newMode)
    {
        destroyMode = newMode;
    }
}