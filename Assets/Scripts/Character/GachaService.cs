using System;
using System.Collections.Generic;
using UnityEngine;

//This controls the gacha logic. 

public class GachaService : MonoBehaviour
{
    [SerializeField] private CharacterDatabase characterDatabase;
    [SerializeField] private int pullCost = 50;

    [Range(0, 100)][SerializeField] private float fourStarRate = 75f;
    [Range(0, 100)][SerializeField] private float fiveStarRate = 20f;
    [Range(0, 100)][SerializeField] private float sixStarRate = 5f;

    public event Action<GachaResult> OnPullCompleted;
    public event Action<string> OnPullFailed;

    public void Pull()
    {
        if (characterDatabase == null)
        {
            OnPullFailed?.Invoke(" no character database");
            return;
        }
        // prevents pulling when insufficient coins
        if (!CoinSystem.SpendCoins(pullCost))
        {
            OnPullFailed?.Invoke("Not enough coins");
            return;
        }

        Rarity rarity = RollRarity();
        List<CharacterData> pool = characterDatabase.GetCharactersByRarity(rarity);

        if (pool == null || pool.Count == 0)
        {
            OnPullFailed?.Invoke(" nothing in the rarity");
            return;
        }

        CharacterData selectedCharacter = pool[UnityEngine.Random.Range(0, pool.Count)];

        if (CharacterInventory.Instance != null)
        {
            CharacterInventory.Instance.AddCharacter(selectedCharacter);
        }
        else
        {
            Debug.LogWarning("CharacterInventory.Instance is missing.");
        }

        GachaResult result = new GachaResult(selectedCharacter);
        OnPullCompleted?.Invoke(result);
    }

    //handles probability of rarity
    private Rarity RollRarity()
    {
        float roll = UnityEngine.Random.Range(0f, 100f);

        if (roll < sixStarRate)
            return Rarity.SixStar;

        if (roll < sixStarRate + fiveStarRate)
            return Rarity.FiveStar;

        return Rarity.FourStar;
    }
}