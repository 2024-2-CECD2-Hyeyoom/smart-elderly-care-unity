using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartAndBreath : MonoBehaviour
{
    private float timer = 0f;
    public float recordInterval = 1f; // 1초마다 센서 기록 저장

    private List<float> heartRates = new List<float>();
    private List<float> breathRates = new List<float>();

    public float sendInterval = 60f; // 60초마다 기록 전송
    private float sendTimer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        sendTimer += Time.deltaTime;

        if (timer >= recordInterval)
        {
            timer = 0f;
            RecordVitals();
        }
        if (sendTimer >= sendInterval)
        {
            sendTimer = 0f;
            DateTime currentTime = TimeManager.Instance.virtualTime;

            SensorDataSender.Instance.Send("심박", heartRates, currentTime);
            SensorDataSender.Instance.Send("호흡", breathRates, currentTime);
            
            // SensorDataSender.Instance.SaveToCSV("심박", heartRates, currentTime);
            // SensorDataSender.Instance.SaveToCSV("호흡", breathRates, currentTime);
            
            heartRates.Clear();
            breathRates.Clear();
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
                    heartRate = UnityEngine.Random.Range(40, 80);
                    breathRate = UnityEngine.Random.Range(12, 25);
                    break;
                case PlayerState.Sleep:
                    heartRate = UnityEngine.Random.Range(0, 10);
                    breathRate = UnityEngine.Random.Range(0, 5);
                    break;
            }
        }

        heartRates.Add(heartRate);
        breathRates.Add(breathRate);

        DateTime currentTime = TimeManager.Instance.virtualTime;
        Debug.Log($"[{currentTime:HH:mm:ss}] 심박: {heartRate} bpm / 호흡: {breathRate} rpm");
    }
}
