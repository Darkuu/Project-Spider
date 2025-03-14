using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopUnlockButton : MonoBehaviour
{
    [Header("Unlock Settings")]
    public int price = 10;
    public GameObject[] objectsToDelete; // Objects to remove
    public GameObject objectToModify; // Object whose visuals will change
    public Sprite newSprite; // New sprite for visual change
    public bool disableAfterPurchase = true;

    [Header("UI Display")]
    public TMP_Text priceText;

    private void Start()
    {
        if (priceText != null)
            priceText.text = price.ToString();
    }

    public void UnlockItem()
    {
        if (!MoneyManager.instance.SpendCoins(price))
        {
            Debug.Log("Not enough coins to unlock item.");
            return;
        }

        if (objectsToDelete != null)
        {
            foreach (GameObject obj in objectsToDelete)
            {
                if (obj != null)
                    Destroy(obj);
            }
        }

        if (objectToModify != null && newSprite != null)
        {
            SpriteRenderer spriteRenderer = objectToModify.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = newSprite;
            }
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
