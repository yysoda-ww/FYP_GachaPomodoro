using UnityEngine;
//store id name rarity damage and sprite to reuse
[CreateAssetMenu(fileName = "NewCharacter", menuName = "Game/Character Data")]
public class CharacterData : ScriptableObject
{
    [Header("ID")]
    public string characterId;

    [Header("Basic Info")]
    public string characterName;
    public Rarity rarity;

    [Header("Stats")]
    public int damage;

    [Header("Splashart")]
    public Sprite splashArt;
}