using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartAndBreath : MonoBehaviour
{
    private float timer = 0f;
    public float recordInterval = 1f; // 1초마다 센서 기록 저장

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= recordInterval)
        {
            timer = 0f;
            RecordVitals();
        }
    }

    void RecordVitals()
    {
        var state = PlayerStateManager.Instance;

        int heartRate = 0;
        int breathRate = 0;

        if (state.currentRoom == "BedRoom")
        {
            switch (state.currentState)
            {
                case PlayerState.Active:
                    heartRate = UnityEngine.Random.Range(50, 100);
                    breathRate = UnityEngine.Random.Range(12, 25);
                    break;
                case PlayerState.Sleeping:
                    heartRate = UnityEngine.Random.Range(0, 20);
                    breathRate = UnityEngine.Random.Range(0, 10);
                    break;
            }
        }

        DateTime currentTime = TimeManager.Instance.virtualTime;
        Debug.Log($"[{currentTime:HH:mm:ss}] 심박: {heartRate} bpm / 호흡: {breathRate} rpm");
    }
}
