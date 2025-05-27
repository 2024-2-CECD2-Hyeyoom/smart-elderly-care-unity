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
        virtualTime = new DateTime(2025, 5, 25, 0, 0, 0); // 시작 시간
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
}
