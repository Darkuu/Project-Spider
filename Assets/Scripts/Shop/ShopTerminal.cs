using UnityEngine;

public class ShopTerminal : MonoBehaviour
{
    public GameObject shopUI;         // Reference to the shop UI panel
    public KeyCode openKey = KeyCode.E; // Key to open/close the shop UI
    private bool playerInRange = false;

    private void Start()
    {
        if (shopUI != null)
            shopUI.SetActive(false);
    }

    private void Update()
    {
        // If player is in range and presses E, toggle the shop UI.
        if (playerInRange && Input.GetKeyDown(openKey))
        {
            ToggleShop();
        }
    }

    void ToggleShop()
    {
        if (shopUI != null)
        {
            bool isActive = shopUI.activeSelf;
            shopUI.SetActive(!isActive);
            // Optionally, lock player movement when the shop is open.
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Make sure your player GameObject is tagged "Player"
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            // Optionally, display a prompt like "Press E to open shop"
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (shopUI != null)
                shopUI.SetActive(false);
        }
    }
}