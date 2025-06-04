using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    public float moveRadius = 20f;
    public float baseSpeed = 3.5f;
    public float waitTime = 2f;

    public Transform sleepPosition;
    public Transform outingPosition;

    private NavMeshAgent agent;
    private float timer;
    private bool isIdle = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = baseSpeed;
        timer = waitTime;

        PlayerStateManager.Instance.OnStateChanged += HandleStateChanged;

        HandleStateChanged(PlayerState.Active);
    }

    void OnDestroy()
    {
        if (PlayerStateManager.Instance != null)
            PlayerStateManager.Instance.OnStateChanged -= HandleStateChanged;
    }

    void Update()
    {
        if (isIdle) return;

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                MoveToRandomPosition();
                timer = waitTime;
            }
        }
    }

    void MoveToRandomPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * moveRadius;
        randomDirection += transform.position;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, moveRadius, agent.areaMask))
        {
            agent.SetDestination(hit.position);
        }
    }

    void HandleStateChanged(PlayerState newState)
    {
        UpdateMovementArea(newState);

        switch (newState)
        {
            case PlayerState.Sleep:
                isIdle = true;
                agent.SetDestination(sleepPosition.position);
                break;
            case PlayerState.Outing:
                isIdle = true;
                agent.SetDestination(outingPosition.position);
                break;
            case PlayerState.Active:
                isIdle = false;
                MoveToRandomPosition();
                break;
        }
    }

    void UpdateMovementArea(PlayerState state)
    {
        int walkable = NavMesh.GetAreaFromName("Walkable");
        int outside = NavMesh.GetAreaFromName("Outside");
        int bed = NavMesh.GetAreaFromName("Bed");

        switch (state)
        {
            case PlayerState.Active:
                agent.areaMask = 1 << walkable;
                break;
            case PlayerState.Outing:
                agent.areaMask = (1 << walkable) | (1 << outside);
                break;
            case PlayerState.Sleep:
                agent.areaMask = (1 << walkable) | (1 << bed);
                break;
        }
    }
}