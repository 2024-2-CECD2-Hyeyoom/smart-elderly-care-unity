using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Active,
    Sleeping,
    Outing
}

public class PlayerStateManager : MonoBehaviour
{
    public static PlayerStateManager Instance;

    public PlayerState currentState = PlayerState.Active;
    public string currentRoom = "None";

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    public void SetState(PlayerState newState)
    {
        currentState = newState;
        Debug.Log($"[Player State] {currentState}");
    }

    public bool IsSleeping => currentState == PlayerState.Sleeping;
    public bool IsOuting => currentState == PlayerState.Outing;
    public bool IsIdle => currentState == PlayerState.Active;
    
}
