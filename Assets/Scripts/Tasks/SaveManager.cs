using System.IO;
using UnityEngine;

//saving and loading JSON so task doesnt get erased from quiting app.

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    private string SavePath => Path.Combine(Application.persistentDataPath, "tasks.json");

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public TaskSaveData LoadTasks()
    {
        if (!File.Exists(SavePath))
            return new TaskSaveData();

        string json = File.ReadAllText(SavePath);
        TaskSaveData data = JsonUtility.FromJson<TaskSaveData>(json);

        return data ?? new TaskSaveData();
    }

    public void SaveTasks(TaskSaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SavePath, json);
    }
}