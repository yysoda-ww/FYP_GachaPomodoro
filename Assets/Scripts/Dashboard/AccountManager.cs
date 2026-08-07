using System.IO;
using UnityEngine;

[DefaultExecutionOrder(-200)]
public class AccountManager : MonoBehaviour
{
    public static AccountManager Instance { get; private set; }

    public PlayerProfile CurrentProfile { get; private set; }

    private string ProfilesFolder => Path.Combine(Application.persistentDataPath, "Profiles");

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (!Directory.Exists(ProfilesFolder))
            Directory.CreateDirectory(ProfilesFolder);
    }

    public bool Login(string username)
    {
        username = CleanUsername(username);

        if (string.IsNullOrWhiteSpace(username))
            return false;

        string path = GetProfilePath(username);

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            CurrentProfile = JsonUtility.FromJson<PlayerProfile>(json);

            if (CurrentProfile == null)
                CurrentProfile = new PlayerProfile(username);
        }
        else
        {
            CurrentProfile = new PlayerProfile(username);
        }

        SyncFromLocalProgress(
            PlayerPrefs.GetInt("PLAYER_LEVEL", 1),
            PlayerPrefs.GetInt("PLAYER_EXP", 0),
            PlayerPrefs.GetInt("PLAYER_STUDY_TIME_SECONDS", 0),
            PlayerPrefs.GetInt("PLAYER_TASKS_COMPLETED", 0),
            CoinSystem.GetCoins()
        );

        SaveCurrentProfile();
        return true;
    }

    public void SaveCurrentProfile()
    {
        if (CurrentProfile == null) return;

        string path = GetProfilePath(CurrentProfile.username);
        string json = JsonUtility.ToJson(CurrentProfile, true);
        File.WriteAllText(path, json);
    }

    public void Logout()
    {
        SaveCurrentProfile();
        CurrentProfile = null;
    }

    public void AddCoins(int amount)
    {
        if (amount <= 0) return;

        CoinSystem.AddCoins(amount);

        if (CurrentProfile != null)
        {
            CurrentProfile.coins = CoinSystem.GetCoins();
            SaveCurrentProfile();
        }
    }

    public void AddStudyTime(int seconds)
    {
        if (seconds <= 0) return;

        int current = PlayerPrefs.GetInt("PLAYER_STUDY_TIME_SECONDS", 0);
        PlayerPrefs.SetInt("PLAYER_STUDY_TIME_SECONDS", current + seconds);
        PlayerPrefs.Save();

        if (CurrentProfile != null)
        {
            CurrentProfile.studyTimeSeconds = PlayerPrefs.GetInt("PLAYER_STUDY_TIME_SECONDS", 0);
            SaveCurrentProfile();
        }
    }

    public void AddCompletedTask(int amount = 1)
    {
        if (amount <= 0) return;

        int current = PlayerPrefs.GetInt("PLAYER_TASKS_COMPLETED", 0);
        PlayerPrefs.SetInt("PLAYER_TASKS_COMPLETED", current + amount);
        PlayerPrefs.Save();

        if (CurrentProfile != null)
        {
            CurrentProfile.tasksCompleted = PlayerPrefs.GetInt("PLAYER_TASKS_COMPLETED", 0);
            SaveCurrentProfile();
        }
    }

    public void SetLevel(int level)
    {
        PlayerPrefs.SetInt("PLAYER_LEVEL", Mathf.Max(1, level));
        PlayerPrefs.Save();

        if (CurrentProfile != null)
        {
            CurrentProfile.level = PlayerPrefs.GetInt("PLAYER_LEVEL", 1);
            SaveCurrentProfile();
        }
    }

    public void SyncFromLocalProgress(int level, int exp, int studyTimeSeconds, int tasksCompleted, int coins)
    {
        if (CurrentProfile == null) return;

        CurrentProfile.level = Mathf.Max(1, level);
        CurrentProfile.exp = Mathf.Max(0, exp);
        CurrentProfile.studyTimeSeconds = Mathf.Max(0, studyTimeSeconds);
        CurrentProfile.tasksCompleted = Mathf.Max(0, tasksCompleted);
        CurrentProfile.coins = Mathf.Max(0, coins);

        SaveCurrentProfile();
    }

    private string GetProfilePath(string username)
    {
        return Path.Combine(ProfilesFolder, username + ".json");
    }

    private string CleanUsername(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            return "";

        username = username.Trim().ToLower();

        foreach (char c in Path.GetInvalidFileNameChars())
            username = username.Replace(c.ToString(), "");

        return username;
    }
}