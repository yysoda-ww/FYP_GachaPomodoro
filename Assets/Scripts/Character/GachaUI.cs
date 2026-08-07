using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GachaUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GachaService gachaService;

    [Header("Buttons")]
    [SerializeField] private Button pullButton;

    [Header("Result UI")]
    [SerializeField] private Image splashArtImage;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text rarityText;
    [SerializeField] private TMP_Text damageText;
    [SerializeField] private TMP_Text feedbackText;

    private void OnEnable()
    {
        if (gachaService != null)
        {
            gachaService.OnPullCompleted += ShowResult;
            gachaService.OnPullFailed += ShowError;
        }
    }

    private void OnDisable()
    {
        if (gachaService != null)
        {
            gachaService.OnPullCompleted -= ShowResult;
            gachaService.OnPullFailed -= ShowError;
        }
    }

    private void Start()
    {
        if (pullButton != null)
            pullButton.onClick.AddListener(HandlePullButton);

        ClearResult();
    }

    private void HandlePullButton()
    {
        if (feedbackText != null)
            feedbackText.text = "";

        gachaService.Pull();
    }

    private void ShowResult(GachaResult result)
    {
        if (result == null || result.character == null)
        {
            ShowError("Pull result was empty.");
            return;
        }

        CharacterData character = result.character;

        if (splashArtImage != null)
        {
            splashArtImage.sprite = character.splashArt;
            splashArtImage.enabled = character.splashArt != null;
        }

        if (nameText != null)
            nameText.text = character.characterName;

        if (rarityText != null)
            rarityText.text = GetRarityText(character.rarity);

        if (damageText != null)
            damageText.text = "Damage: " + character.damage;

        if (feedbackText != null)
            feedbackText.text = "Pulled successfully!";
    }

    private void ShowError(string message)
    {
        if (feedbackText != null)
            feedbackText.text = message;
    }

    private void ClearResult()
    {
        if (splashArtImage != null)
        {
            splashArtImage.sprite = null;
            splashArtImage.enabled = false;
        }

        if (nameText != null)
            nameText.text = "";

        if (rarityText != null)
            rarityText.text = "";

        if (damageText != null)
            damageText.text = "";

        if (feedbackText != null)
            feedbackText.text = "";
    }

    private string GetRarityText(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.FourStar:
                return "4 Star";
            case Rarity.FiveStar:
                return "5 Star";
            case Rarity.SixStar:
                return "6 Star";
            default:
                return rarity.ToString();
        }
    }
}