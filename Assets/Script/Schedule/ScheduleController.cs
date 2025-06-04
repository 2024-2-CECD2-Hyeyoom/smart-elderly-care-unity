using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScheduleController : MonoBehaviour
{
    public TextAsset scheduleCSV;
    public float preMoveLeadTimeMinutes = 5f;

    private ScheduleLoader loader;
    private List<PlayerSchedule> schedules;
    private int currentIndex = 0;
    private bool hasPreparedForNextState = false;

    void Start()
    {
        loader = gameObject.AddComponent<ScheduleLoader>();
        loader.LoadCSV(scheduleCSV);

        schedules = loader.schedules;

        TimeManager.Instance.SetStartTime(loader.GetStartTime());
        TimeManager.Instance.OnTimeChanged += OnTimeChanged;
    }

    void OnDestroy()
    {
        if (TimeManager.Instance != null)
            TimeManager.Instance.OnTimeChanged -= OnTimeChanged;
    }

    void OnTimeChanged(DateTime currentTime)
    {
        while (currentIndex < schedules.Count)
        {
            var s = schedules[currentIndex];
            var preMoveTime = s.startDateTime.AddMinutes(-preMoveLeadTimeMinutes);

            // 1. 상태 시작 전 준비 이동
            if (!hasPreparedForNextState && currentTime >= preMoveTime && currentTime < s.startDateTime)
            {
                PlayerMovement.Instance.MoveToPreparationPosition(s.state);
                hasPreparedForNextState = true;
                return;
            }

            // 2. 실제 상태 전환
            if (currentTime >= s.startDateTime && currentTime < s.endDateTime)
            {
                if (PlayerStateManager.Instance.currentState != s.state)
                    PlayerStateManager.Instance.SetState(s.state);
                return;
            }

            // 3. 상태 종료 후 다음 스케줄로
            if (currentTime >= s.endDateTime)
            {
                currentIndex++;
                hasPreparedForNextState = false;
            }
            else
            {
                PlayerStateManager.Instance.SetState(PlayerState.Active);
                return;
            }
        }

        PlayerStateManager.Instance.SetState(PlayerState.Active);
    }
}
