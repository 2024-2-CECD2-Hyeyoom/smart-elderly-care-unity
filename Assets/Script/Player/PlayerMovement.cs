using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;

    private NavMeshAgent agent;

    public Transform doorPosition;
    public Transform bedPosition;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        agent = GetComponent<NavMeshAgent>();
    }

    public void MoveToPreparationPosition(PlayerState upcomingState)
    {
        UpdateMovementArea(upcomingState); // 상태에 맞게 이동 가능 영역 조정

        switch (upcomingState)
        {
            case PlayerState.Outing:
                if (doorPosition != null)
                    agent.SetDestination(doorPosition.position);
                break;
            case PlayerState.Sleep:
                if (bedPosition != null)
                    agent.SetDestination(bedPosition.position);
                break;
        }
    }

    public void StopMoving()
    {
        agent.ResetPath();
    }

    public void UpdateMovementArea(PlayerState state)
    {
        int walkable = NavMesh.GetAreaFromName("Walkable");
        int outside = NavMesh.GetAreaFromName("Outside");
        int bed = NavMesh.GetAreaFromName("Bed");

        switch (state)
        {
            case PlayerState.Active:
                // 실내만
                agent.areaMask = 1 << walkable;
                break;

            case PlayerState.Outing:
                // 실내 + 실외
                agent.areaMask = (1 << walkable) | (1 << outside);
                break;

            case PlayerState.Sleep:
                // 실내 + 침대영역
                agent.areaMask = (1 << walkable) | (1 << bed);
                break;
        }
    }
}
