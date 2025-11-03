using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [Header("Timer Settings")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private bool startOnAwake = true;

    private float elapsedTime = 0f;
    private bool isRunning = false;

    public float TotalSeconds => elapsedTime;
    public int Hours => Mathf.FloorToInt(elapsedTime / 3600f);
    public int Minutes => Mathf.FloorToInt((elapsedTime % 3600f) / 60f);
    public int Seconds => Mathf.FloorToInt(elapsedTime % 60f);

    public string FormattedTime
    {
        get
        {
            if (Hours > 0)
            {
                return $"{Hours:00}:{Minutes:00}:{Seconds:00}";
            }
            else
            {
                return $"{Minutes:00}:{Seconds:00}";
            }
        }
    }

    private void Awake()
    {
        if (startOnAwake)
        {
            StartTimer();
        }
    }

    private void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimerDisplay();
        }
    }

    public void StartTimer()
    {
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void ResetTimer()
    {
        elapsedTime = 0f;
        UpdateTimerDisplay();
    }

    public void RestartTimer()
    {
        ResetTimer();
        StartTimer();
    }
    public void SetTime(float seconds)
    {
        elapsedTime = seconds;
        UpdateTimerDisplay();
    }

    private void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            timerText.text = FormattedTime;
        }
    }

    [Header("Debug Info")]
    [SerializeField] private string currentTimeDisplay;

    private void OnValidate()
    {
        if (timerText == null)
        {
            timerText = GetComponent<TextMeshProUGUI>();
        }

        currentTimeDisplay = FormattedTime;
    }

    private void OnGUI()
    {
#if UNITY_EDITOR
        currentTimeDisplay = FormattedTime;
#endif
    }
}