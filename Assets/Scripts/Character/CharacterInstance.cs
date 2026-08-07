[System.Serializable]
public class CharacterInstance
{
    public CharacterData data;

    public CharacterInstance(CharacterData data)
    {
        this.data = data;
    }

    public string Name => data.characterName;
    public Rarity Rarity => data.rarity;
    public int Damage => data.damage;
}