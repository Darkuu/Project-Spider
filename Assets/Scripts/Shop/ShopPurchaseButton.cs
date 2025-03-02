using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopPurchaseButton : MonoBehaviour
{
    [Header("Purchase Settings")]
    public int price = 10;
    public GameObject itemPrefab; // For spawning new items
    public GameObject itemToActivate; // For activating an existing item
    public bool disableAfterPurchase = true;

    [Header("UI Display")]
    public TMP_Text priceText;

    private void Start()
    {
        if (priceText != null)
            priceText.text = price.ToString();
    }

    public void PurchaseItem()
    {
        if (!MoneyManager.instance.SpendCoins(price))
        {
            Debug.Log("Not enough coins to purchase item.");
            return;
        }

        ShopUIController shopUIController = GetComponentInParent<ShopUIController>();
        ShopTerminal terminal = shopUIController?.currentTerminal;

        if (itemToActivate != null)
        {
            itemToActivate.SetActive(true);
        }
        else if (itemPrefab != null && terminal != null)
        {
            Transform spawnTransform = terminal.spawnPoint;
            GameObject spawnedItem = Instantiate(itemPrefab, spawnTransform.position, spawnTransform.rotation);

            // Find DeleteObject script dynamically in case it's on a child
            DeleteObject deleteObject = spawnedItem.GetComponent<DeleteObject>() ?? spawnedItem.GetComponentInChildren<DeleteObject>();
            if (deleteObject != null)
            {
                deleteObject.spawnedFromTerminal = terminal;
            }
            else
            {
            }
        }

        if (terminal != null)
        {
            terminal.gameObject.SetActive(false); // Hides the terminal instead of destroying it
        }

        if (disableAfterPurchase)
        {
            DisableButton();
        }
    }

    private void DisableButton()
    {
        Button btn = GetComponent<Button>();
        if (btn != null)
            btn.interactable = false;
        else
            gameObject.SetActive(false);
    }
}
