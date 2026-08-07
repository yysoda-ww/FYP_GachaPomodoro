using System.Collections.Generic;
using UnityEngine;

public class CharacterCollection : MonoBehaviour
{
    [SerializeField] private List<CharacterData> ownedCharacters = new List<CharacterData>();

    public IReadOnlyList<CharacterData> OwnedCharacters => ownedCharacters;

    public void AddCharacter(CharacterData character)
    {
        if (character == null) return;
        if (!ownedCharacters.Contains(character))
            ownedCharacters.Add(character);
    }

    public CharacterData GetCharacterById(string id)
    {
        return ownedCharacters.Find(c => c.characterId == id);
    }
}