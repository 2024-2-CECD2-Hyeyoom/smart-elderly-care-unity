using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class PlayerSchedule
{
    public DateTime startDateTime;
    public DateTime endDateTime;
    public PlayerState state;
}

public class ScheduleLoader : MonoBehaviour
{
    public List<PlayerSchedule> schedules = new List<PlayerSchedule>();

    public void LoadCSV(TextAsset csvFile)
    {
        schedules.Clear();
        using (StringReader reader = new StringReader(csvFile.text))
        {
            string line;
            bool isFirstLine = true;
            while ((line = reader.ReadLine()) != null)
            {
                if (isFirstLine) { isFirstLine = false; continue; }

                string[] parts = line.Split(',');
                if (parts.Length < 4) continue;

                DateTime start = DateTime.Parse($"{parts[0]} {parts[1]}");
                DateTime end = DateTime.Parse($"{parts[0]} {parts[2]}");
                if (end < start) end = end.AddDays(1);

                if (Enum.TryParse(parts[3], true, out PlayerState state))
                {
                    schedules.Add(new PlayerSchedule
                    {
                        startDateTime = start,
                        endDateTime = end,
                        state = state
                    });
                }
            }
        }

        schedules.Sort((a, b) => a.startDateTime.CompareTo(b.startDateTime));
    }

    public DateTime GetStartTime()
    {
        if (schedules.Count > 0)
        {
            var date = schedules[0].startDateTime.Date;
            return date.AddHours(6); // 06:00
        }
        return DateTime.Now.Date;
    }
}
