using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.EventSystems;

public class Tutorial : MonoBehaviour, IPointerClickHandler
{
    public GameObject tutorialPaper;
    public float slideDistance = 300f;
    public float slideDuration = 0.5f;

    private bool isOut = false;
    private Vector3 originalPosition;
    private Vector3 targetPosition;
    private Coroutine slideCoroutine;

    void Start()
    {
        originalPosition = tutorialPaper.transform.position;
        targetPosition = originalPosition + Vector3.up * slideDistance;
    }

    // Этот метод вызывается автоматически при клике на объект с компонентом UI
    public void OnPointerClick(PointerEventData eventData)
    {
        ToggleTutorial();
    }

    private void ToggleTutorial()
    {
        if (slideCoroutine != null)
            StopCoroutine(slideCoroutine);

        if (!isOut)
        {
            slideCoroutine = StartCoroutine(SlideToPosition(targetPosition, slideDuration));
            isOut = true;
        }
        else
        {
            slideCoroutine = StartCoroutine(SlideToPosition(originalPosition, slideDuration));
            isOut = false;
        }
    }

    private IEnumerator SlideToPosition(Vector3 targetPos, float duration)
    {
        Vector3 startPos = tutorialPaper.transform.position;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float progress = timer / duration;
            tutorialPaper.transform.position = Vector3.Lerp(startPos, targetPos, progress);
            yield return null;
        }

        tutorialPaper.transform.position = targetPos;
    }
}