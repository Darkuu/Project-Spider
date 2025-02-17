using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopActivateItemButton : MonoBehaviour
{
    [Header("Purchase Settings")]
    public int price;
    public GameObject itemToActivate;

    [Header("UI Display")]
    public TMP_Text priceText;

    [Header("Button Settings")]
    public bool disableAfterPurchase = true; // Toggle to disable button after purchase

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

                if (disableAfterPurchase)
                {
                    Button btn = GetComponent<Button>();
                    if (btn != null)
                    {
                        btn.interactable = false;
                    }
                    else
                    {
                        gameObject.SetActive(false);
                    }
                }

                ShopUIController shopUIController = GetComponentInParent<ShopUIController>();
                if (shopUIController != null && shopUIController.currentTerminal != null)
                {
                    shopUIController.currentTerminal.CloseAndRemoveTerminal();
                }
            }
        }
    }
}