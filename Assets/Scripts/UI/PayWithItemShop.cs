using System.Collections.Generic;
using UnityEngine;

public class PayWithItemShop : MonoBehaviour
{
    [System.Serializable]
    public class PurchaseCost
    {
        public ItemScript item;
        public int count;
    }

    [Header("Item and Coin Cost")]
    public List<PurchaseCost> requiredItems = new List<PurchaseCost>();
    public int coinPrice = 0;

    [Header("Shop Actions")]
    public List<GameObject> objectsToActivate;
    public List<GameObject> objectsToDeactivate;
    public List<GameObject> objectsToSpawn;
    public Transform spawnPoint;

    public void OnPurchaseButtonPressed()
    {
        bool hasItems = HasRequiredItems();
        bool canAffordCoins = MoneyManager.instance.SpendCoins(coinPrice);

        // If both items and coins are required, you can adjust logic below
        if ((requiredItems.Count == 0 || hasItems) && canAffordCoins)
        {
            if (requiredItems.Count > 0)
                RemoveRequiredItems();

            // Deactivate objects
            foreach (GameObject obj in objectsToDeactivate)
            {
                if (obj != null)
                    obj.SetActive(false);
            }

            // Activate objects
            foreach (GameObject obj in objectsToActivate)
            {
                if (obj != null)
                    obj.SetActive(true);
            }

            // Spawn objects
            foreach (GameObject prefab in objectsToSpawn)
            {
                if (prefab != null && spawnPoint != null)
                    Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
            }

            Debug.Log("Purchase successful!");
        }
        else
        {
            if (!hasItems && requiredItems.Count > 0)
                Debug.Log("Not enough items to complete the purchase.");
            if (!canAffordCoins)
                Debug.Log("Not enough coins to complete the purchase.");
        }
    }

    bool HasRequiredItems()
    {
        foreach (PurchaseCost cost in requiredItems)
        {
            int totalCount = 0;
            foreach (InventorySlot slot in InventoryManager.instance.inventorySlots)
            {
                InventoryItem invItem = slot.GetComponentInChildren<InventoryItem>();
                if (invItem != null && invItem.item == cost.item)
                {
                    totalCount += invItem.count;
                }
            }
            if (totalCount < cost.count)
            {
                return false;
            }
        }
        return true;
    }

    void RemoveRequiredItems()
    {
        foreach (PurchaseCost cost in requiredItems)
        {
            int remainingToRemove = cost.count;
            foreach (InventorySlot slot in InventoryManager.instance.inventorySlots)
            {
                if (remainingToRemove <= 0)
                    break;

                InventoryItem invItem = slot.GetComponentInChildren<InventoryItem>();
                if (invItem != null && invItem.item == cost.item)
                {
                    if (invItem.count > remainingToRemove)
                    {
                        invItem.count -= remainingToRemove;
                        invItem.RefreshCount();
                        remainingToRemove = 0;
                    }
                    else
                    {
                        remainingToRemove -= invItem.count;
                        Destroy(invItem.gameObject);
                    }
                }
            }
        }
    }
}
