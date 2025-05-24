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

    private float logTimer = 0f;
    public float logInterval = 30f; // 30초마다 센서 기록 저장

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

        logTimer += Time.deltaTime;
        if (logTimer >= logInterval)
        {
            DateTime currentTime = TimeManager.Instance.virtualTime;
            Debug.Log($"Time: {currentTime:yyyy-MM-ddTHH:mm:ssZ}");
            Debug.Log($"PIR (거실): {roomDistance["LivingRoom"]:F2} m");
            Debug.Log($"Radar (침실): {roomDistance["BedRoom"]:F2} m");

            // 리셋
            roomDistance["LivingRoom"] = 0f;
            roomDistance["BedRoom"] = 0f;
            logTimer = 0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        RoomZone zone = other.GetComponent<RoomZone>();
        if (zone != null)
        {
            currentRoom = zone.roomName;
            Debug.Log($"[Enter] Current Room: {currentRoom}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        RoomZone zone = other.GetComponent<RoomZone>();
        if (zone != null && zone.roomName == currentRoom)
        {
            currentRoom = "None";
        }
    }
}
