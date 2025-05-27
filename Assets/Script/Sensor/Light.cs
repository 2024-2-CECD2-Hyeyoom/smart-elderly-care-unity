using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light : MonoBehaviour
{
    public float naturalLight = 0f; // ÀÚ¿¬±¤
    public float artificialLight = 0f; // Á¶¸í
    public float totalLight => naturalLight + artificialLight;

    public float maxNaturalLight = 100f;
    public float maxArtificialLight = 80f;

    private float timer = 0f;
    public float recordInterval = 60f; // 60ÃÊ¸¶´Ù ¼¾¼­ ±â·Ï ÀúÀå
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
                naturalLight = Mathf.Lerp(0f, maxNaturalLight, (hour - 6f) / 6f);  // 6 > 12½Ã ¹à¾ÆÁü
            else
                naturalLight = Mathf.Lerp(maxNaturalLight, 0f, (hour - 12f) / 6f); // 12 > 20½Ã ¾îµÎ¿öÁü
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

        // 18½Ã ÀÌÈÄ Á¶¸í ÄÑÁü
        if (now.Hour >= 18 || now.Hour < 8)
        {
            if (stateManager.IsSleeping)
            {
                artificialLight = 0f; // ¼ö¸é Áß¿£ Á¶¸í ²¨Áü
            }
            else
            {
                artificialLight = maxArtificialLight; // Á¶¸í ÄÑÁü
            }
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
        Debug.Log($"[{currentTime:HH:mm:ss}] ÃÑ Á¶µµ: {total:F1} (ÀÚ¿¬±¤: {naturalLight:F1}, Á¶¸í: {artificialLight:F1})");
    }
}
