using System;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class TimeUIController : MonoBehaviour
{
    public TMP_Text timeText;
    public Button pauseButton;
    public Button speed1xButton;
    public Button speed5xButton;

    private void Start()
    {
        TimeManager.Instance.OnTimeChanged += UpdateTimeText;

        pauseButton.onClick.AddListener(TogglePause);
        speed1xButton.onClick.AddListener(() => SetSpeed(60f));
        speed5xButton.onClick.AddListener(() => SetSpeed(300f));

        UpdateTimeText(TimeManager.Instance.virtualTime);
    }

    void UpdateTimeText(DateTime time)
    {
        timeText.text = time.ToString("HH:mm");
    }

    void TogglePause()
    {
        TimeManager.Instance.isPaused = !TimeManager.Instance.isPaused;
    }

    void SetSpeed(float scale)
    {
        TimeManager.Instance.timeScale = scale;
    }
}
