using UnityEngine;
using TMPro;

public class ShopActivateItemButton : MonoBehaviour
{
    [Header("Purchase Settings")]
    public int price;
    public GameObject itemToActivate;

    [Header("UI Display")]
    public TMP_Text priceText;

    private void Start()
    {
        if (priceText != null)
            priceText.text = price.ToString();
    }

    public void PurchaseItem()
    {
        if (MoneyManager.instance.SpendCoins(price))
        {
            if (itemToActivate != null)
            {
                itemToActivate.SetActive(true);
                ShopUIController shopUIController = GetComponentInParent<ShopUIController>();
                if (shopUIController != null && shopUIController.currentTerminal != null)
                {
                    shopUIController.currentTerminal.CloseAndRemoveTerminal();
                }
            }

        }
    }
}