using System.Collections.Generic;
using UnityEngine;
//database to choose which pool of characters to pull from depending on rarity being picked
[CreateAssetMenu(fileName = "CharacterDatabase", menuName = "Game/Character Database")]
public class CharacterDatabase : ScriptableObject
{
    public List<CharacterData> fourStarCharacters = new List<CharacterData>();
    public List<CharacterData> fiveStarCharacters = new List<CharacterData>();
    public List<CharacterData> sixStarCharacters = new List<CharacterData>();

    public List<CharacterData> GetCharactersByRarity(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.FourStar:
                return fourStarCharacters;
            case Rarity.FiveStar:
                return fiveStarCharacters;
            case Rarity.SixStar:
                return sixStarCharacters;
            default:
                return null;
        }
    }
}