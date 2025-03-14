using UnityEngine;
public class ShopUIController : MonoBehaviour
{
    public ShopTerminal currentTerminal;

    public void PurchaseItem(GameObject itemPrefab, int cost)
    {
        if (MoneyManager.instance.coinCount >= cost)
        {
            MoneyManager.instance.SpendCoins(cost);
            Debug.Log("Item purchased for " + cost + " coins.");

            if (currentTerminal != null && currentTerminal.spawnPoint != null)
            {
                Instantiate(itemPrefab, currentTerminal.spawnPoint.position, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning("No spawn point set for this terminal!");
            }

            currentTerminal.CloseAndRemoveTerminal();
        }
        else
        {
            Debug.Log("Not enough coins!");
        }
    }
}