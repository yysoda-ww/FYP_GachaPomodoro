using TMPro;
using UnityEngine;

public class LevelUI : MonoBehaviour
{
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text expText;

    private void Update()
    {
        if (PlayerLevelSystem.Instance == null)
            return;

        if (levelText != null)
            levelText.text = "Level: " + PlayerLevelSystem.Instance.CurrentLevel;

        if (expText != null)
            expText.text = "EXP: " + PlayerLevelSystem.Instance.CurrentExp + "/" + PlayerLevelSystem.Instance.GetExpToNextLevel();
    }
}