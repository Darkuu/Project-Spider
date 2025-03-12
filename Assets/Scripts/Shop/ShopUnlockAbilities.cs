using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopUnlockAbilities : MonoBehaviour
{
    [Header("Unlock Settings")]
    public int price = 10; // Price for unlocking the ability
    public PlayerUnlockManager.Unlockable unlockableAbility; // Ability to unlock
    public bool disableAfterPurchase = true; // Disable button after purchase

    [Header("UI Display")]
    public TMP_Text priceText; // Text to show the price

    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();

        if (priceText != null)
            priceText.text = price.ToString(); // Display price on button

        // Check if the ability is already unlocked
        if (PlayerUnlockManager.instance.IsAbilityUnlocked(unlockableAbility))
        {
            DisableButton();
        }
    }

    public void UnlockItem()
    {
        if (!MoneyManager.instance.SpendCoins(price)) // Check if player has enough coins
        {
            Debug.Log("Not enough coins to unlock " + unlockableAbility);
            return;
        }

        // Unlock ability in PlayerUnlockManager
        PlayerUnlockManager.instance.UnlockAbility(unlockableAbility);

        if (disableAfterPurchase)
        {
            DisableButton();
        }
    }

    private void DisableButton()
    {
        if (button != null)
        {
            button.interactable = false;
        }
        else
        {
            gameObject.SetActive(false);
        }

        if (priceText != null)
        {
            priceText.text = "Purchased";
        }
    }
}
