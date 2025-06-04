using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class SensorData
{
    public string sensor_type_name;
    public string measurement_time;
    public List<float> measurement_values;
}

public class SensorDataSender : MonoBehaviour
{
    private static SensorDataSender _instance;
    public static SensorDataSender Instance
    {
        get
        {
            if (_instance == null)
            {
                var obj = new GameObject("SensorDataSender");
                _instance = obj.AddComponent<SensorDataSender>();
                DontDestroyOnLoad(obj);
            }
            return _instance;
        }
    }

    public void Send(string sensorName, List<float> values, DateTime eventTime)
    {
        StartCoroutine(SendSensorData(sensorName, values, eventTime));
    }

    private IEnumerator SendSensorData(string sensorName, List<float> values, DateTime eventTime)
    {
        SensorData data = new SensorData()
        {
            sensor_type_name = sensorName,
            measurement_time = eventTime.ToString("yyyy-MM-ddTHH:mm:ssZ"),
            measurement_values = values
        };

        string json = JsonUtility.ToJson(data);
        UnityWebRequest request = new UnityWebRequest("http://localhost:8080/api/sensor", "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log($"{sensorName} 데이터 전송 성공");
        }
        else
        {
            Debug.LogError($"{sensorName} 데이터 전송 실패: {request.error}");
        }
    }

    public void SaveToCSV(string sensorName, List<float> values, DateTime eventTime)
    {
        string path = Path.Combine(Application.persistentDataPath, "sensor_log.csv");
        string timestamp = eventTime.ToString("yyyy-MM-ddTHH:mm:ssZ");
        string line = $"{sensorName},{timestamp}";

        foreach (float value in values)
        {
            line += $",{value}";
        }

        try
        {
            File.AppendAllText(path, line + "\n");
            Debug.Log($"CSV 저장 완료: {line}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"CSV 저장 실패: {ex.Message}");
        }
    }
}
