using TMPro;
using UnityEngine;

public class CoinsUI : MonoBehaviour
{
    [SerializeField] private TMP_Text coinsText;

    private int lastShownCoins = -1;

    private void Start()
    {
        Refresh();
    }

    private void Update()
    {
        int currentCoins = CoinSystem.GetCoins();

        if (currentCoins != lastShownCoins)
        {
            Refresh();
        }
    }

    public void Refresh()
    {
        int currentCoins = CoinSystem.GetCoins();
        lastShownCoins = currentCoins;

        if (coinsText != null)
            coinsText.text = "Coin: " + currentCoins;
    }
}