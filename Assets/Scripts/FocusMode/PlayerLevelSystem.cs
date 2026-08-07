using UnityEngine;

//Tracks player progression and syncs across the game so that the dashboard can display stats.

public class PlayerLevelSystem : MonoBehaviour
{
    public static PlayerLevelSystem Instance { get; private set; }

    private const string LevelKey = "PLAYER_LEVEL";
    private const string ExpKey = "PLAYER_EXP";
    private const string StudyTimeKey = "PLAYER_STUDY_TIME_SECONDS";
    private const string TasksCompletedKey = "PLAYER_TASKS_COMPLETED";

    private const int ExpPerMinute = 10;
    private const int ExpPerLevel = 300;

    public int CurrentLevel => PlayerPrefs.GetInt(LevelKey, 1);
    public int CurrentExp => PlayerPrefs.GetInt(ExpKey, 0);
    public int CurrentStudyTimeSeconds => PlayerPrefs.GetInt(StudyTimeKey, 0);
    public int TasksCompleted => PlayerPrefs.GetInt(TasksCompletedKey, 0);

    public int ExpRequiredPerLevel => ExpPerLevel;
    public int ExpGainPerMinute => ExpPerMinute;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (!PlayerPrefs.HasKey(LevelKey))
            PlayerPrefs.SetInt(LevelKey, 1);

        if (!PlayerPrefs.HasKey(ExpKey))
            PlayerPrefs.SetInt(ExpKey, 0);

        if (!PlayerPrefs.HasKey(StudyTimeKey))
            PlayerPrefs.SetInt(StudyTimeKey, 0);

        if (!PlayerPrefs.HasKey(TasksCompletedKey))
            PlayerPrefs.SetInt(TasksCompletedKey, 0);

        PlayerPrefs.Save();
        SyncToAccountProfile();
    }

    public void AddExp(int amount)
    {
        if (amount <= 0) return;

        int exp = CurrentExp + amount;
        int level = CurrentLevel;

        while (exp >= ExpPerLevel)
        {
            exp -= ExpPerLevel;
            level++;
        }

        PlayerPrefs.SetInt(LevelKey, Mathf.Max(1, level));
        PlayerPrefs.SetInt(ExpKey, Mathf.Max(0, exp));
        PlayerPrefs.Save();

        SyncToAccountProfile();

        Debug.Log($"Added {amount} EXP | Level: {CurrentLevel} | EXP: {CurrentExp}/{ExpPerLevel}");
    }

    public void AddStudyMinutes(int minutes)
    {
        if (minutes <= 0) return;

        int addedSeconds = minutes * 60;
        int newTotal = CurrentStudyTimeSeconds + addedSeconds;

        PlayerPrefs.SetInt(StudyTimeKey, newTotal);
        PlayerPrefs.Save();

        SyncToAccountProfile();
    }

    public void AddCompletedTask(int amount = 1)
    {
        if (amount <= 0) return;

        int newTotal = TasksCompleted + amount;
        PlayerPrefs.SetInt(TasksCompletedKey, Mathf.Max(0, newTotal));
        PlayerPrefs.Save();

        SyncToAccountProfile();
    }

    public int GetExpToNextLevel()
    {
        return ExpPerLevel;
    }

    public int CalculateExpFromMinutes(int minutes)
    {
        if (minutes <= 0) return 0;
        return minutes * ExpPerMinute;
    }

    public string GetFormattedStudyTime()
    {
        int totalSeconds = CurrentStudyTimeSeconds;
        int hours = totalSeconds / 3600;
        int minutes = (totalSeconds % 3600) / 60;
        return $"{hours}h {minutes}m";
    }

    public void SyncToAccountProfile()
    {
        if (AccountManager.Instance == null || AccountManager.Instance.CurrentProfile == null)
            return;

        AccountManager.Instance.SyncFromLocalProgress(
            CurrentLevel,
            CurrentExp,
            CurrentStudyTimeSeconds,
            TasksCompleted,
            CoinSystem.GetCoins()
        );
    }
}