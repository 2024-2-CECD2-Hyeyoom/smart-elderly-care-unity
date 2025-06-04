using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Active,
    Sleep,
    Outing
}

public class PlayerStateManager : MonoBehaviour
{
    public static PlayerStateManager Instance;

    public PlayerState currentState = PlayerState.Active;
    public string currentRoom = "None";

    public event Action<PlayerState> OnStateChanged;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    public void SetState(PlayerState newState)
    {
        if (currentState != newState)
        {
            currentState = newState;
            Debug.Log($"[Player State] {currentState}");
            OnStateChanged?.Invoke(currentState);
        }
    }

    public bool IsSleeping => currentState == PlayerState.Sleep;
    public bool IsOuting => currentState == PlayerState.Outing;
    public bool IsActive => currentState == PlayerState.Active;
    
}
