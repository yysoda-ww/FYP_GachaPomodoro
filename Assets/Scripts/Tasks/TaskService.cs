using System;
using System.Collections.Generic;
using UnityEngine;

//manages adding, deleting, completing, saving tasks and rewarding coins when tasks completed

public class TaskService : MonoBehaviour
{
    [SerializeField] private int rewardPerTask = 10;

    private TaskSaveData saveData;

    public IReadOnlyList<TaskData> Tasks => saveData.tasks;

    public event Action OnTasksChanged;

    private void Awake()
    {
        if (SaveManager.Instance != null)
            saveData = SaveManager.Instance.LoadTasks();
        else
            saveData = new TaskSaveData();
    }

    public void AddTask(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            return;

        saveData.tasks.Add(new TaskData(title.Trim()));
        Save();
        OnTasksChanged?.Invoke();
    }

    public void DeleteTask(string taskId)
    {
        TaskData task = saveData.tasks.Find(t => t.id == taskId);
        if (task == null) return;

        saveData.tasks.Remove(task);
        Save();
        OnTasksChanged?.Invoke();
    }

    public void ToggleCompleted(string taskId)
    {
        TaskData task = saveData.tasks.Find(t => t.id == taskId);
        if (task == null) return;

        task.isCompleted = !task.isCompleted;

        //cannot claim coins more than once from the same tasks that has been ticked

        if (task.isCompleted && !task.rewardClaimed)
        {
            task.rewardClaimed = true;
            CoinSystem.AddCoins(rewardPerTask);
        }

        Save();
        OnTasksChanged?.Invoke();
    }

    private void Save()
    {
        if (SaveManager.Instance != null)
            SaveManager.Instance.SaveTasks(saveData);
    }
}