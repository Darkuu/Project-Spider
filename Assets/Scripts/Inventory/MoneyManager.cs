using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager instance;
    public float coinCount = 0;       
    public TMP_Text coinUIText;        

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

    public void AddCoins(float  amount)
    {
        coinCount += amount;
        UpdateUI();
    }

    public bool SpendCoins(float amount)
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