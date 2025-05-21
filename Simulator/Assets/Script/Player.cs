using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    public float moveRadius = 20f;  // 이동 반경 ( 제자리만 맴돌거나 이동 범위가 작아지는 것을 방지)

    private NavMeshAgent agent;

    private float timer;
    public float waitTime = 2f;  // 목적지 도착 후 대기 시간

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        timer = waitTime;

        MoveToRandomPosition();  // 이동할 목적지 랜덤 설정
    }

    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)  // 경로 탐색완료 및 목적지 도착
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                MoveToRandomPosition();  // 이동할 목적지 랜덤 재설정
                timer = waitTime;  // 타이머 초기화
            }
        }
    }

    void MoveToRandomPosition()
    {
        // 목적지 생성
        Vector3 randomDirection = Random.insideUnitSphere * moveRadius;
        randomDirection += transform.position;

        NavMeshHit hit; // 지점의 정보 저장
        if (NavMesh.SamplePosition(randomDirection, out hit, moveRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position); // 이동
        }
    }
}