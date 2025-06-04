using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light : MonoBehaviour
{
    public float naturalLight = 0f; // 자연광
    public float artificialLight = 0f; // 조명
    public float totalLight => naturalLight + artificialLight;

    public float maxNaturalLight = 100f;
    public float maxArtificialLight = 80f;

    private float timer = 0f;
    public float recordInterval = 60f; // 60초마다 센서 기록 저장

    void Update()
    {
        UpdateNaturalLight();
        UpdateArtificialLight();

        timer += Time.deltaTime;
        if (timer >= recordInterval)
        {
            timer = 0f;
            RecordLight();
        }
    }

    private void UpdateNaturalLight()
    {
        DateTime now = TimeManager.Instance.virtualTime;
        float hour = now.Hour + now.Minute / 60f;

        if (hour >= 6f && hour <= 20f)
        {
            if (hour <= 12f)
                naturalLight = Mathf.Lerp(0f, maxNaturalLight, (hour - 6f) / 6f);  // 6 > 12시 밝아짐
            else
                naturalLight = Mathf.Lerp(maxNaturalLight, 0f, (hour - 12f) / 6f); // 12 > 20시 어두워짐
        }
        else
        {
            naturalLight = 0f;
        }
    }

    private void UpdateArtificialLight()
    {
        DateTime now = TimeManager.Instance.virtualTime;
        var stateManager = PlayerStateManager.Instance;

        // 18시 이후 조명 켜짐
        if (now.Hour >= 18 || now.Hour < 8)
        {
            artificialLight = stateManager.IsSleeping ? 0f : maxArtificialLight;
        }
        else
        {
            artificialLight = 0f;
        }
    }

    void RecordLight()
    {
        float total = Mathf.Clamp(naturalLight + artificialLight, 0f, maxNaturalLight + maxArtificialLight);
        DateTime currentTime = TimeManager.Instance.virtualTime;
        SensorDataSender.Instance.Send("조도", new List<float> { total }, currentTime);
        // SensorDataSender.Instance.SaveToCSV("조도", new List<float> { total }, currentTime);

        Debug.Log($"[{currentTime:HH:mm:ss}] 총 조도: {total:F1} (자연광: {naturalLight:F1}, 조명: {artificialLight:F1})");
    }
}
