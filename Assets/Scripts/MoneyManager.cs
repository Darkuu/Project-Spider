using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MoneyManager : MonoBehaviour
{
    public static MoneyManager instance;

    [Header("Coin Settings")]
    public int coinCount = 0; 
    public TMP_Text coinUIText;   

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    /// <summary>
    /// Adds the specified number of coins and updates the UI.
    /// </summary>
    /// <param name="amount">The number of coins to add.</param>
    public void AddCoins(int amount)
    {
        coinCount += amount;
        UpdateUI();
    }

    /// <summary>
    /// Updates the UI text to display the current coin count.
    /// </summary>
    void UpdateUI()
    {
        if (coinUIText != null)
            coinUIText.text = coinCount.ToString();
    }
}