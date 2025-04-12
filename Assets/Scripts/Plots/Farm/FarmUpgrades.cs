using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FarmUpgradeManager : MonoBehaviour
{
    [Header("Upgrade Settings")]
    [SerializeField] private float growthTimeMultiplier = 0.75f;
    [SerializeField] private float plantingChanceMultiplier = 0.75f;
    public int growthTimeUpgradeCost = 100;  // Cost for growth time upgrade
    public int plantingChanceUpgradeCost = 150;  // Cost for planting chance upgrade

    [Header("UI")]
    [SerializeField] private Button growthTimeUpgradeButton;
    [SerializeField] private Button plantingChanceUpgradeButton;
    [SerializeField] private TMP_Text growthTimeCostText;
    [SerializeField] private TMP_Text plantingChanceCostText;

    private bool growthTimeUpgradePurchased = false;
    private bool plantingChanceUpgradePurchased = false;

    private void Start()
    {
        UpdateUI();

        if (growthTimeUpgradeButton != null)
            growthTimeUpgradeButton.onClick.AddListener(TryGrowthTimeUpgrade);

        if (plantingChanceUpgradeButton != null)
            plantingChanceUpgradeButton.onClick.AddListener(TryPlantingChanceUpgrade);
    }

    // Handle the growth time upgrade purchase
    public void TryGrowthTimeUpgrade()
    {
        if (growthTimeUpgradePurchased) return;

        if (MoneyManager.instance.SpendCoins(growthTimeUpgradeCost))
        {
            growthTimeUpgradePurchased = true;
            UpdateUI();
        }
    }

    // Handle the planting chance upgrade purchase
    public void TryPlantingChanceUpgrade()
    {
        if (plantingChanceUpgradePurchased) return;

        if (MoneyManager.instance.SpendCoins(plantingChanceUpgradeCost))
        {
            plantingChanceUpgradePurchased = true;
            UpdateUI();
        }
    }

    // Return the modified growth time based on the upgrade
    public float GetGrowthTime(float baseTime)
    {
        return growthTimeUpgradePurchased ? baseTime * growthTimeMultiplier : baseTime;
    }

    // Return the modified number of planted spots based on the upgrade
    public int CalculatePlantAmount(int availableSpots)
    {
        float multiplier = plantingChanceUpgradePurchased ? plantingChanceMultiplier : 0.5f;
        return Mathf.CeilToInt(availableSpots * multiplier);
    }

    // Update the UI based on the purchase status and dynamically display the cost
    private void UpdateUI()
    {
        // Disable the buttons if the upgrade is already purchased
        if (growthTimeUpgradeButton != null)
        {
            growthTimeUpgradeButton.interactable = !growthTimeUpgradePurchased;
        }

        if (plantingChanceUpgradeButton != null)
        {
            plantingChanceUpgradeButton.interactable = !plantingChanceUpgradePurchased;
        }

        // Update cost text
        if (growthTimeCostText != null)
        {
            growthTimeCostText.text = growthTimeUpgradePurchased ? "" : growthTimeUpgradeCost.ToString(); 
        }

        if (plantingChanceCostText != null)
        {
            plantingChanceCostText.text = plantingChanceUpgradePurchased ? "" : plantingChanceUpgradeCost.ToString(); 
        }
    }
}
