using UnityEngine;
using TMPro;
using System;

public class PlayerStateUI : MonoBehaviour
{
    public TextMeshProUGUI stateText;
    public PlayerStateManager playerStateManager;

    private void OnEnable()
    {
        if (playerStateManager != null)
        {
            playerStateManager.OnStateChanged += UpdateStateUI;
            UpdateStateUI(playerStateManager.currentState);
        }
        else
        {
            Debug.LogWarning("PlayerStateManager.Instance is null");
        }
    }

    private void OnDisable()
    {
        if (playerStateManager != null)
        {
            playerStateManager.OnStateChanged -= UpdateStateUI;
        }
    }

    private void UpdateStateUI(PlayerState newState)
    {
        if (stateText != null)
        {
            stateText.text = $"Player State: {newState.ToString()}";
        }
    }
}