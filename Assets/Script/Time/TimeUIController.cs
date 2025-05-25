using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeUIController : MonoBehaviour
{
    public TMP_Text timeText;
    public Button pauseButton;
    public Button speed1xButton;
    public Button speed2xButton;
    public Button speed5xButton;

    void Start()
    {
        TimeManager.Instance.OnTimeChanged += UpdateTimeText;

        pauseButton.onClick.AddListener(TogglePause);
        speed1xButton.onClick.AddListener(() => SetSpeed(1f));
        speed2xButton.onClick.AddListener(() => SetSpeed(2f));
        speed5xButton.onClick.AddListener(() => SetSpeed(5f));

        UpdateTimeText(TimeManager.Instance.virtualTime);
    }

    void UpdateTimeText(DateTime time)
    {
        timeText.text = time.ToString("HH:mm");
    }

    void TogglePause()
    {
        if (Time.timeScale == 0f)
            Time.timeScale = 1f;
        else
            Time.timeScale = 0f;
    }

    void SetSpeed(float scale)
    {
        Time.timeScale = scale;
    }
}
