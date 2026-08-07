using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCardUI : MonoBehaviour
{
    [SerializeField] private Image portraitImage;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private Button button;

    private CharacterData characterData;
    private TeamUI teamUI;

    public void Bind(CharacterData data, TeamUI ui)
    {
        characterData = data;
        teamUI = ui;

        if (portraitImage != null)
            portraitImage.sprite = data.splashArt;

        if (nameText != null)
            nameText.text = data.characterName;

        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnClicked);
        }
    }

    private void OnClicked()
    {
        if (teamUI != null && characterData != null)
            teamUI.TryAddCharacterToNextSlot(characterData);
    }
}