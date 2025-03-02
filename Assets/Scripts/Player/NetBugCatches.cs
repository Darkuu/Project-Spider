using UnityEngine;

public class NetBugCatcher : MonoBehaviour
{
    [Header("Net Settings")]
    public Vector2 boxSize = new Vector2(3f, 3f);
    public Vector2 offset = Vector2.zero;

    [Header("Capture Settings")]
    public LayerMask bugLayer;
    public LayerMask foodLayer;
    public LayerMask eggLayer;

    public Transform player;  // Reference to the player object

    private void Start()
    {
        // Ensure we have a reference to the player
        if (player == null)
        {
            player = GameObject.FindWithTag("Player").transform;  // Assume your player object is tagged "Player"
        }
    }

    private void Update()
    {
        // Rotate and move the net around the player to face the mouse
        RotateAroundPlayer();

        // --- CAPTURE (RIGHT MOUSE BUTTON) ---
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Right mouse button clicked for capture.");

            Vector2 center = (Vector2)transform.position + offset;
            Collider2D[] hitColliders = Physics2D.OverlapBoxAll(center, boxSize, 0f, bugLayer | foodLayer | eggLayer);

            if (hitColliders.Length == 0)
            {
                Debug.Log("No bugs, food, or eggs found in the capture area.");
            }

            foreach (Collider2D collider in hitColliders)
            {
                // Handle Bugs:
                Bug bugComp = collider.GetComponent<Bug>();
                if (bugComp != null && bugComp.bugItem != null)
                {
                    if (InventoryManager.instance.AddItem(bugComp.bugItem))
                    {
                        Debug.Log("Bug captured and added to the inventory.");
                        Destroy(collider.gameObject);
                    }
                    else Debug.Log("Inventory full or unable to add the bug item.");

                    continue;
                }

                // Handle Food:
                Food foodComp = collider.GetComponent<Food>();
                if (foodComp != null && foodComp.foodItem != null)
                {
                    if (InventoryManager.instance.AddItem(foodComp.foodItem))
                    {
                        Debug.Log("Food captured and added to the inventory.");
                        Destroy(collider.gameObject);
                    }
                    else Debug.Log("Inventory full or unable to add the food item.");

                    continue;
                }

                // Handle Eggs:
                EggItem eggComp = collider.GetComponent<EggItem>();
                if (eggComp != null)
                {
                    if (eggComp.eggItem != null)
                    {
                        bool added = InventoryManager.instance.AddItem(eggComp.eggItem);
                        if (added)
                        {
                            Debug.Log("Egg captured and added to the inventory.");
                            // Remove the egg from the world.
                            Destroy(collider.gameObject);
                        }
                        else
                        {
                            Debug.Log("Inventory full or unable to add the egg item.");
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Egg component found but no ItemScript assigned!");
                    }
                    // You can continue to the next collider if needed.
                }
            }
        }

        // --- PLACEMENT (LEFT MOUSE BUTTON) ---
        if (Input.GetMouseButtonDown(0))
        {
            ItemScript selectedItem = InventoryManager.instance.GetSelectedItem(false);
            if (selectedItem != null)
            {
                Vector2 spawnPosition = (Vector2)transform.position + offset;

                if (selectedItem.type == ItemType.Bug || selectedItem.type == ItemType.Food || selectedItem.type == ItemType.Egg)
                {
                    InventoryManager.instance.GetSelectedItem(true); // Remove from inventory
                    selectedItem.Use(spawnPosition);
                   // Debug.Log($"{selectedItem.type} placed back into the world at {spawnPosition}");
                }
            }
        }
    }

    private void RotateAroundPlayer()
    {
        // Get the mouse position in world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // We're in 2D, so ignore the Z-axis

        // Calculate the direction from the player to the mouse
        Vector2 direction = (mousePosition - player.position).normalized;

        // Calculate the angle from the player to the mouse
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Set the position of the net around the player (this keeps it in orbit)
        float radius = 1f;  // Set a fixed distance from the player (adjustable if needed)
        Vector2 netPosition = (Vector2)player.position + direction * radius;
        transform.position = netPosition;

        // Apply the rotation to the net, so it faces the mouse
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    // Optional: Visualize the capture area in Scene view.
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector2 center = (Vector2)transform.position + offset;
        Gizmos.DrawWireCube(center, boxSize);
    }
}
