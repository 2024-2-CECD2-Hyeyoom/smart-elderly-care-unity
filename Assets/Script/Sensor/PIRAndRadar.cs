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
            DateTime currentTime = TimeManager.Instance.virtualTime;
            Debug.Log($"[{currentTime:HH:mm:ss}] PIR (거실): {roomDistance["LivingRoom"]:F2} / Radar (침실): {roomDistance["BedRoom"]:F2}");

            roomDistance["LivingRoom"] = 0f;
            roomDistance["BedRoom"] = 0f;

            timer = 0f;
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
