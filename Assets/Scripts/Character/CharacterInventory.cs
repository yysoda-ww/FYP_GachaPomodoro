using System.Collections.Generic;
using System.IO;
using UnityEngine;

//saves character ID into JSON and load full data again from database
public class CharacterInventory : MonoBehaviour
{
    //singleton pattern so its easy to access from other scripts
    public static CharacterInventory Instance { get; private set; }

    [Header("Database")]
    [SerializeField] private CharacterDatabase characterDatabase;

    [Header("Owned Characters")]
    [SerializeField] private List<CharacterData> ownedCharacters = new List<CharacterData>();

    public IReadOnlyList<CharacterData> OwnedCharacters => ownedCharacters;

    private string SavePath => Path.Combine(Application.persistentDataPath, "character_inventory.json");

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadInventory();
    }

    public void AddCharacter(CharacterData character)
    {
        if (character == null) return;

        if (!ownedCharacters.Contains(character))
        {
            ownedCharacters.Add(character);
            SaveInventory();
            Debug.Log("Added character to inventory: " + character.characterName);
        }
    }

    public CharacterData GetCharacterById(string id)
    {
        return ownedCharacters.Find(c => c != null && c.characterId == id);
    }

    public bool HasCharacter(string id)
    {
        return ownedCharacters.Exists(c => c != null && c.characterId == id);
    }

    public void SaveInventory()
    {
        CharacterInventorySaveData data = new CharacterInventorySaveData();

        foreach (CharacterData character in ownedCharacters)
        {
            if (character != null && !string.IsNullOrEmpty(character.characterId))
            {
                data.ownedCharacterIds.Add(character.characterId);
            }
        }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SavePath, json);
    }

    public void LoadInventory()
    {
        ownedCharacters.Clear();

        if (characterDatabase == null)
        {
            Debug.LogWarning("CharacterDatabase is missing on CharacterInventory.");
            return;
        }

        if (!File.Exists(SavePath))
        {
            Debug.Log("No saved inventory found yet.");
            return;
        }

        string json = File.ReadAllText(SavePath);
        CharacterInventorySaveData data = JsonUtility.FromJson<CharacterInventorySaveData>(json);

        if (data == null || data.ownedCharacterIds == null)
        {
            Debug.LogWarning("Inventory save file was empty or invalid.");
            return;
        }

        foreach (string id in data.ownedCharacterIds)
        {
            CharacterData character = FindCharacterInDatabase(id);

            if (character != null && !ownedCharacters.Contains(character))
            {
                ownedCharacters.Add(character);
            }
        }

        Debug.Log("Loaded inventory. Count: " + ownedCharacters.Count);
    }

    public void ClearInventory()
    {
        ownedCharacters.Clear();
        SaveInventory();
    }

    //Rebuilds inventory using saved IDs
    private CharacterData FindCharacterInDatabase(string id)
    {
        if (string.IsNullOrEmpty(id) || characterDatabase == null)
            return null;

        foreach (CharacterData c in characterDatabase.fourStarCharacters)
        {
            if (c != null && c.characterId == id)
                return c;
        }

        foreach (CharacterData c in characterDatabase.fiveStarCharacters)
        {
            if (c != null && c.characterId == id)
                return c;
        }

        foreach (CharacterData c in characterDatabase.sixStarCharacters)
        {
            if (c != null && c.characterId == id)
                return c;
        }

        return null;
    }
}