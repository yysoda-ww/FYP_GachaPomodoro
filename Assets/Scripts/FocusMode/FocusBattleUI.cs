using TMPro;
using UnityEngine;

public class FocusBattleUI : MonoBehaviour
{
    [SerializeField] private TMP_Text defeatedCountText;
    [SerializeField] private TMP_Text expText;
    [SerializeField] private TMP_Text battleLogText;

    private void Start()
    {
        Refresh();
    }

    private void Update()
    {
        Refresh();
    }

    private void Refresh()
    {
        if (PlayerLevelSystem.Instance == null)
        {
            if (defeatedCountText != null)
                defeatedCountText.text = "EXP Rate: 10 / min";

            if (expText != null)
                expText.text = "Level: 1 | EXP: 0/300";

            if (battleLogText != null)
                battleLogText.text = "Complete a focus session to earn EXP.";

            return;
        }

        if (defeatedCountText != null)
            defeatedCountText.text = $"EXP Rate: {PlayerLevelSystem.Instance.ExpGainPerMinute} / min";

        if (expText != null)
            expText.text = $"Level: {PlayerLevelSystem.Instance.CurrentLevel} | EXP: {PlayerLevelSystem.Instance.CurrentExp}/{PlayerLevelSystem.Instance.GetExpToNextLevel()}";

        if (battleLogText != null)
            battleLogText.text = $"Study Time: {PlayerLevelSystem.Instance.GetFormattedStudyTime()}";
    }
}