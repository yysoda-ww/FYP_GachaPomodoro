using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Handle team selection screen

public class TeamUI : MonoBehaviour
{
    [Header("Team Slots")]
    [SerializeField] private Image slot1Image;
    [SerializeField] private Image slot2Image;
    [SerializeField] private Image slot3Image;

    [Header("Inventory UI")]
    [SerializeField] private Transform inventoryContent;
    [SerializeField] private CharacterCardUI characterCardPrefab;

    private List<CharacterData> selectedTeam = new List<CharacterData>();

    private void Start()
    {
        BuildInventory();
        LoadSavedTeamIntoUI();
        RefreshTeamSlots(false);
    }

    private void BuildInventory()
    {
        if (inventoryContent == null || characterCardPrefab == null)
            return;

        for (int i = inventoryContent.childCount - 1; i >= 0; i--)
            Destroy(inventoryContent.GetChild(i).gameObject);

        if (CharacterInventory.Instance == null)
        {
            Debug.LogWarning("CharacterInventory.Instance is missing.");
            return;
        }

        foreach (CharacterData character in CharacterInventory.Instance.OwnedCharacters)
        {
            CharacterCardUI card = Instantiate(characterCardPrefab, inventoryContent);
            card.Bind(character, this);
        }
    }

    private void LoadSavedTeamIntoUI()
    {
        selectedTeam.Clear();

        if (TeamManager.Instance == null)
            return;

        foreach (CharacterData character in TeamManager.Instance.SelectedTeam)
        {
            if (character != null)
                selectedTeam.Add(character);
        }
    }

    public void TryAddCharacterToNextSlot(CharacterData character)
    {
        if (character == null) return;
        if (selectedTeam.Contains(character)) return;
        if (selectedTeam.Count >= 3) return;

        selectedTeam.Add(character);
        RefreshTeamSlots(true);
    }

    public void RemoveSlot1() => RemoveFromSlot(0);
    public void RemoveSlot2() => RemoveFromSlot(1);
    public void RemoveSlot3() => RemoveFromSlot(2);

    private void RemoveFromSlot(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= selectedTeam.Count)
            return;

        selectedTeam.RemoveAt(slotIndex);
        RefreshTeamSlots(true);
    }

    private void RefreshTeamSlots(bool saveTeam)
    {
        UpdateSlot(slot1Image, selectedTeam.Count > 0 ? selectedTeam[0] : null);
        UpdateSlot(slot2Image, selectedTeam.Count > 1 ? selectedTeam[1] : null);
        UpdateSlot(slot3Image, selectedTeam.Count > 2 ? selectedTeam[2] : null);

        if (saveTeam && TeamManager.Instance != null)
            TeamManager.Instance.SetTeam(selectedTeam);
    }

    private void UpdateSlot(Image slotImage, CharacterData character)
    {
        if (slotImage == null) return;

        if (character != null)
        {
            slotImage.sprite = character.splashArt;
            slotImage.color = Color.white;
        }
        else
        {
            slotImage.sprite = null;
            slotImage.color = new Color(1f, 1f, 1f, 0f);
        }
    }
}