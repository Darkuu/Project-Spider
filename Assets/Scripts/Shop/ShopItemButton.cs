using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemButton : MonoBehaviour
{
    [Header("Purchase Settings")]
    public int price = 10;                  
    public GameObject itemPrefab;           
    public Transform spawnPoint;            

    [Header("UI Display")]
    public TMP_Text priceText;                  

    private void Start()
    {
        if (priceText != null)
            priceText.text = price.ToString();
    }

    // This method is called when the UI button is clicked.
    public void PurchaseItem()
    {
        // Check if the player has enough coins.
        if (MoneyManager.instance.SpendCoins(price))
        {
            // Spawn the purchased item.
            Instantiate(itemPrefab, spawnPoint.position, spawnPoint.rotation);
            Debug.Log("Item purchased for " + price + " coins.");
        }
        else
        {
            Debug.Log("Not enough coins to purchase item.");
        }
    }
}