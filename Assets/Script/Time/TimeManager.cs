using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    public float timeScale = 60f; // 1초 = 1분

    public DateTime virtualTime;
    public event Action<DateTime> OnTimeChanged;
    private float timer = 0f;

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    void Start()
    {
        // virtualTime = DateTime.Now;
    }

    void Update()
    {
        float delta = Time.deltaTime;

        timer += delta * timeScale;

        if (timer >= 60f)
        {
            int minutes = Mathf.FloorToInt(timer / 60f);
            virtualTime = virtualTime.AddMinutes(minutes);
            timer -= minutes * 60f;

            OnTimeChanged?.Invoke(virtualTime);
        }
    }
    public void SetStartTime(DateTime startTime)
    {
        virtualTime = startTime;
        Debug.Log($"시간 재설정: {virtualTime:yy:MM:dd HH:mm:ss}");
    }
}
