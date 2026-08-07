using TMPro;
using UnityEngine;

public class DashboardProfileUI : MonoBehaviour
{
    [SerializeField] private TMP_Text usernameText;
    [SerializeField] private TMP_Text coinsText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text studyTimeText;
    [SerializeField] private TMP_Text tasksCompletedText;

    private void Start()
    {
        Refresh();
    }

    private void Update()
    {
        Refresh();
    }

    public void Refresh()
    {
        if (usernameText != null)
        {
            string username = "Guest";

            if (AccountManager.Instance != null && AccountManager.Instance.CurrentProfile != null)
                username = AccountManager.Instance.CurrentProfile.username;

            usernameText.text = $"User: {username}";
        }

        if (coinsText != null)
            coinsText.text = $"Coins: {CoinSystem.GetCoins()}";

        if (PlayerLevelSystem.Instance != null)
        {
            if (levelText != null)
                levelText.text = $"Level: {PlayerLevelSystem.Instance.CurrentLevel}";

            if (studyTimeText != null)
                studyTimeText.text = $"Study Time: {PlayerLevelSystem.Instance.GetFormattedStudyTime()}";

            if (tasksCompletedText != null)
                tasksCompletedText.text = $"Tasks Done: {PlayerLevelSystem.Instance.TasksCompleted}";
        }
        else
        {
            if (levelText != null)
                levelText.text = "Level: 1";

            if (studyTimeText != null)
                studyTimeText.text = "Study Time: 0h 0m";

            if (tasksCompletedText != null)
                tasksCompletedText.text = "Tasks Done: 0";
        }
    }
}