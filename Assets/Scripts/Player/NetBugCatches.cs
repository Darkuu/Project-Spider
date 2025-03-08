using UnityEngine;

public class NetBugCatcher : MonoBehaviour
{
    [Header("Net Settings")]
    public Vector2 boxSize = new Vector2(3f, 3f);
    public Vector2 offset = Vector2.zero;

    [Header("Capture Settings")]
    public LayerMask captureLayer;

    public Transform player;

    private void Start()
    {
        if (player == null) player = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        RotateAroundPlayer();

        if (Input.GetMouseButtonDown(1)) Capture();
        if (Input.GetMouseButtonDown(0)) PlaceItem();
    }

    private void Capture()
    {
        Vector2 center = (Vector2)transform.position + offset;
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(center, boxSize, 0f, captureLayer);

        if (hitColliders.Length == 0)
        {
            Debug.Log("No bugs, food, or eggs found in the capture area.");
            return;
        }

        foreach (Collider2D collider in hitColliders)
        {
            // Only process colliders that are not triggers
            if (!collider.isTrigger)
            {
                ItemScript item = TryCaptureItem(collider);
                if (item != null) Destroy(collider.gameObject);
            }
        }
    }


    private ItemScript TryCaptureItem(Collider2D collider)
    {
        // Check if item is valid and add it to inventory
        ItemScript item = null;
        
        if (collider.TryGetComponent(out Bug bugComp) && bugComp.bugItem != null)
            item = bugComp.bugItem;
        else if (collider.TryGetComponent(out Food foodComp) && foodComp.foodItem != null)
            item = foodComp.foodItem;
        else if (collider.TryGetComponent(out EggItem eggComp) && eggComp.eggItem != null)
            item = eggComp.eggItem;

        if (item != null && InventoryManager.instance.AddItem(item))
        {
            Debug.Log($"{item.type} captured and added to inventory.");
            return item;
        }
        else
        {
            Debug.Log("Inventory full or unable to add the item.");
            return null;
        }
    }

    private void PlaceItem()
    {
        ItemScript selectedItem = InventoryManager.instance.GetSelectedItem(false);
        if (selectedItem != null)
        {
            Vector2 spawnPosition = (Vector2)transform.position + offset;
            if (selectedItem.type == ItemType.Bug || selectedItem.type == ItemType.Food || selectedItem.type == ItemType.Egg)
            {
                InventoryManager.instance.GetSelectedItem(true); // Remove from inventory
                selectedItem.Use(spawnPosition);
            }
        }
    }

    private void RotateAroundPlayer()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        Vector2 direction = (mousePosition - player.position).normalized;
        transform.position = (Vector2)player.position + direction * 1f;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector2 center = (Vector2)transform.position + offset;
        Gizmos.DrawWireCube(center, boxSize);
    }
}
