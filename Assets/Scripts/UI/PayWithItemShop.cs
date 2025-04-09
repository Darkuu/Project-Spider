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

    public List<PurchaseCost> requiredItems;

    public List<GameObject> objectsToActivate;
    public List<GameObject> objectsToDeactivate;
    public List<GameObject> objectsToSpawn;
    public Transform spawnPoint;

    public void OnPurchaseButtonPressed()
    {
        if (HasRequiredItems())
        {
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
            Debug.Log("Not enough items to complete the purchase.");
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
