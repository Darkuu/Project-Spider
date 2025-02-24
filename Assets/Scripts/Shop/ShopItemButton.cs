using UnityEngine;
using TMPro;

public class ShopItemButton : MonoBehaviour
{
    [Header("Purchase Settings")]
    public int price = 10;                // The coin cost for this item
    public GameObject itemPrefab;         // The prefab to spawn when purchased

    [Header("UI Display")]
    public TMP_Text priceText;            // UI text to show the price

    private void Start()
    {
        if (priceText != null)
            priceText.text = price.ToString();
    }

    public void PurchaseItem()
    {
        if (MoneyManager.instance.SpendCoins(price))
        {
            ShopUIController shopUIController = GetComponentInParent<ShopUIController>();
            if (shopUIController != null && shopUIController.currentTerminal != null)
            {
                Transform spawnTransform = shopUIController.currentTerminal.spawnPoint;
                Instantiate(itemPrefab, spawnTransform.position, spawnTransform.rotation);
                Debug.Log("Item purchased for " + price + " coins.");
                shopUIController.currentTerminal.CloseAndRemoveTerminal();
            }
        }
        else
        {
            Debug.Log("Not enough coins to purchase item.");
        }
    }
}