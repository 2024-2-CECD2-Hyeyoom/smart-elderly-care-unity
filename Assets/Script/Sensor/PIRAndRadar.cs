using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PIRAndRadar : MonoBehaviour
{
    private Vector3 lastPosition;

    private string currentRoom = "None";

    private Dictionary<string, float> roomDistance = new Dictionary<string, float>();

    private float timer = 0f;
    public float recordInterval = 30f; // 30초마다 센서 기록 저장

    void Start()
    {
        lastPosition = transform.position;
        roomDistance["LivingRoom"] = 0f;
        roomDistance["BedRoom"] = 0f;
    }

    void Update()
    {
        float moved = Vector3.Distance(transform.position, lastPosition);
        lastPosition = transform.position;

        if (roomDistance.ContainsKey(currentRoom))
        {
            roomDistance[currentRoom] += moved;
        }

        timer += Time.deltaTime;
        if (timer >= recordInterval)
        {
            timer = 0f;

            DateTime currentTime = TimeManager.Instance.virtualTime;

            float livingRoomDistance = roomDistance["LivingRoom"] * 5;
            float bedRoomDistance = roomDistance["BedRoom"] * 12;

            Debug.Log($"[{currentTime:HH:mm:ss}] PIR (거실): {livingRoomDistance:F2} / Radar (침실): {bedRoomDistance:F2}");

            SensorDataSender.Instance.Send("PIR활동", new List<float> { livingRoomDistance }, currentTime);
            SensorDataSender.Instance.Send("레이더활동", new List<float> { bedRoomDistance }, currentTime);
            // SensorDataSender.Instance.SaveToCSV("PIR활동", new List<float> { livingRoomDistance }, currentTime);
            // SensorDataSender.Instance.SaveToCSV("레이더활동", new List<float> { bedRoomDistance }, currentTime);

            roomDistance["LivingRoom"] = 0f;
            roomDistance["BedRoom"] = 0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        RoomZone zone = other.GetComponent<RoomZone>();
        if (zone != null)
        {
            currentRoom = zone.roomName;
            PlayerStateManager.Instance.currentRoom = currentRoom;
            Debug.Log($"[Enter] Current Room: {currentRoom}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        RoomZone zone = other.GetComponent<RoomZone>();
        if (zone != null && zone.roomName == currentRoom)
        {
            currentRoom = "None";
            PlayerStateManager.Instance.currentRoom = currentRoom;
        }
    }
}
