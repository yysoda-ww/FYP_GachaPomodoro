using UnityEngine;

public static class CoinSystem
{
    private const string CoinsKey = "PLAYER_COINS";

    public static int GetCoins()
    {
        return PlayerPrefs.GetInt(CoinsKey, 0);
    }

    public static void SetCoins(int amount)
    {
        PlayerPrefs.SetInt(CoinsKey, Mathf.Max(0, amount));
        PlayerPrefs.Save();
    }

    public static void AddCoins(int amount)
    {
        if (amount <= 0) return;

        int current = GetCoins();
        SetCoins(current + amount);
    }

    public static bool SpendCoins(int amount)
    {
        if (amount <= 0) return true;

        int current = GetCoins();
        if (current < amount)
            return false;

        SetCoins(current - amount);
        return true;
    }

    public static void ResetCoins()
    {
        SetCoins(0);
    }
}