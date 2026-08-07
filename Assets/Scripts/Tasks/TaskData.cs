using System;
using UnityEngine;

[Serializable]
public class TaskData
{
    public string id;
    public string title;
    public bool isCompleted;
    public bool rewardClaimed;

    public TaskData(string title)
    {
        id = Guid.NewGuid().ToString();
        this.title = title;
        isCompleted = false;
        rewardClaimed = false;
    }
}