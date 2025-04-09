using System.Collections.Generic;
using UnityEngine;

public class PayWithItemShop : MonoBehaviour
{
    // Define a list for required items and their quantities
    [System.Serializable]
    public class PurchaseCost
    {
        public ItemScript item;
        public int count;
    }

    // Set your purchase costs in the inspector
    public List<PurchaseCost> requiredItems;

    // Optional game objects to activate, deactivate, or spawn
    public GameObject objectToActivate;
    public GameObject objectToDeactivate;
    public GameObject objectToSpawn;  // A prefab to spawn upon purchase
    public Transform spawnPoint;      // Where to spawn the object, if needed

    // This is called by the UI Button OnClick event
    public void OnPurchaseButtonPressed()
    {
        if (HasRequiredItems())
        {
            RemoveRequiredItems();

            // Perform actions based on purchase
            if (objectToDeactivate != null)
            {
                objectToDeactivate.SetActive(false);
            }
            if (objectToActivate != null)
            {
                objectToActivate.SetActive(true);
            }
            if (objectToSpawn != null && spawnPoint != null)
            {
                Instantiate(objectToSpawn, spawnPoint.position, spawnPoint.rotation);
            }

            Debug.Log("Purchase successful!");
        }
        else
        {
            Debug.Log("Not enough items to complete the purchase.");
        }
    }

    // Check if the player inventory has the required items and counts
    bool HasRequiredItems()
    {
        // For each cost, verify that the sum of items in the inventory meets the required count.
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

    // Remove the required items from the inventory
    void RemoveRequiredItems()
    {
        // Loop through each required item and then remove the specified count from the inventory.
        foreach (PurchaseCost cost in requiredItems)
        {
            int remainingToRemove = cost.count;
            foreach (InventorySlot slot in InventoryManager.instance.inventorySlots)
            {
                if (remainingToRemove <= 0)
                    break;  // Move to next required item

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
                        // Subtract full slot count and remove the item from the slot
                        remainingToRemove -= invItem.count;
                        Destroy(invItem.gameObject);
                    }
                }
            }
        }
    }
}
