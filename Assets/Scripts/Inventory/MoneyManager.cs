using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager instance;
    public int coinCount = 0;       // Player starts with 0 coins
    public TMP_Text coinUIText;         // Assign your UI Text element in the Inspector

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        UpdateUI();
    }

    public void AddCoins(int amount)
    {
        coinCount += amount;
        UpdateUI();
    }

    /// <summary>
    /// Attempts to spend the specified amount of coins.
    /// Returns true if successful; false if insufficient coins.
    /// </summary>
    public bool SpendCoins(int amount)
    {
        if (coinCount >= amount)
        {
            coinCount -= amount;
            UpdateUI();
            return true;
        }
        return false;
    }

    void UpdateUI()
    {
        if (coinUIText != null)
            coinUIText.text = coinCount.ToString();
    }
}