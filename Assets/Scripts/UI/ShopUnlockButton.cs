using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopUnlockButton : MonoBehaviour
{
    [Header("Unlock Settings")]
    public int price = 10;
    public GameObject[] objectsToDelete; 
    public GameObject objectToModify; 
    public Sprite newSprite; 
    public bool disableAfterPurchase = true;

    [Header("Activation Settings")]
    public GameObject[] objectsToActivate; 

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

        // Delete objects
        if (objectsToDelete != null)
        {
            foreach (GameObject obj in objectsToDelete)
            {
                if (obj != null)
                    Destroy(obj);
            }
        }

        // Modify object sprite
        if (objectToModify != null && newSprite != null)
        {
            SpriteRenderer spriteRenderer = objectToModify.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = newSprite;
            }
        }

        // Activate objects
        if (objectsToActivate != null)
        {
            foreach (GameObject obj in objectsToActivate)
            {
                if (obj != null)
                    obj.SetActive(true);
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
