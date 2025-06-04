using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    public float openDelay = 1f;
    public bool doorOpen = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopAllCoroutines();
            StartCoroutine(OpenAndClose());
        }
    }

    IEnumerator OpenAndClose()
    {
        transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        doorOpen = true;

        DateTime openTime = TimeManager.Instance.virtualTime;
        Debug.Log($"[{openTime:HH:mm:ss}] ¹® ¿­¸²");
        SensorDataSender.Instance.Send("¹®¿­¸²", new List<float> { 1 }, openTime);
        // SensorDataSender.Instance.SaveToCSV("¹®¿­¸²", new List<float> { 1 }, openTime);

        yield return new WaitForSeconds(openDelay);

        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        doorOpen = false;

        DateTime closeTime = TimeManager.Instance.virtualTime;
        Debug.Log($"[{closeTime:HH:mm:ss}] ¹® ´ÝÈû");
        SensorDataSender.Instance.Send("¹®´ÝÈû", new List<float> { 0 }, closeTime);
        // SensorDataSender.Instance.SaveToCSV("¹®´ÝÈû", new List<float> { 0 }, closeTime);
    }
}

