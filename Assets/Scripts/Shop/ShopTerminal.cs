using UnityEngine;

public class ShopTerminal : MonoBehaviour
{
    public GameObject shopUI;
    public KeyCode openKey = KeyCode.E;
    private bool playerInRange = false;

    [Header("Spawn Settings")]
    public Transform spawnPoint; 

    private void Start()
    {
        if (shopUI != null)
            shopUI.SetActive(false);
    }

    private void Update()
    {
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

            if (shopUI.activeSelf)
            {
                ShopUIController controller = shopUI.GetComponent<ShopUIController>();
                if (controller != null)
                {
                    controller.currentTerminal = this;
                }
            }
        }
    }

    public void CloseAndRemoveTerminal()
    {
        if (shopUI != null)
            shopUI.SetActive(false);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
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