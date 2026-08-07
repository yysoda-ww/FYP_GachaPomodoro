using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Saves and loads the players team using PlayerPrefs which stores character ID

public class TeamManager : MonoBehaviour
{
    public static TeamManager Instance { get; private set; }

    private const string Slot1Key = "Team_Slot_1";
    private const string Slot2Key = "Team_Slot_2";
    private const string Slot3Key = "Team_Slot_3";

    private List<CharacterData> selectedTeam = new List<CharacterData>();

    public IReadOnlyList<CharacterData> SelectedTeam => selectedTeam;

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

    private void Start()
    {
        StartCoroutine(LoadTeamWhenReady());
    }

    public void SetTeam(List<CharacterData> team)
    {
        selectedTeam = new List<CharacterData>();

        foreach (CharacterData character in team)
        {
            if (character != null)
                selectedTeam.Add(character);
        }

        SaveTeam();
    }

    public int GetTotalDamage()
    {
        int total = 0;

        foreach (CharacterData c in selectedTeam)
        {
            if (c != null)
                total += c.damage;
        }

        return total;
    }

    public void SaveTeam()
    {
        SaveSlot(Slot1Key, selectedTeam.Count > 0 ? selectedTeam[0] : null);
        SaveSlot(Slot2Key, selectedTeam.Count > 1 ? selectedTeam[1] : null);
        SaveSlot(Slot3Key, selectedTeam.Count > 2 ? selectedTeam[2] : null);

        PlayerPrefs.Save();
        Debug.Log("Team saved.");
    }

    public void LoadTeam()
    {
        selectedTeam.Clear();

        if (CharacterInventory.Instance == null)
        {
            Debug.LogWarning("CharacterInventory.Instance is missing.");
            return;
        }

        LoadSlot(Slot1Key);
        LoadSlot(Slot2Key);
        LoadSlot(Slot3Key);

        Debug.Log("Team loaded. Count: " + selectedTeam.Count);
    }

    public void ClearTeam()
    {
        selectedTeam.Clear();

        PlayerPrefs.DeleteKey(Slot1Key);
        PlayerPrefs.DeleteKey(Slot2Key);
        PlayerPrefs.DeleteKey(Slot3Key);
        PlayerPrefs.Save();
    }

    private IEnumerator LoadTeamWhenReady()
    {
        float timer = 0f;
        float timeout = 5f;

        while (CharacterInventory.Instance == null && timer < timeout)
        {
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        LoadTeam();
    }

    private void SaveSlot(string key, CharacterData character)
    {
        string id = character != null ? character.characterId : "";
        PlayerPrefs.SetString(key, id);
    }

    private void LoadSlot(string key)
    {
        string id = PlayerPrefs.GetString(key, "");

        if (string.IsNullOrEmpty(id))
            return;

        CharacterData character = CharacterInventory.Instance.GetCharacterById(id);

        if (character != null)
            selectedTeam.Add(character);
    }
}