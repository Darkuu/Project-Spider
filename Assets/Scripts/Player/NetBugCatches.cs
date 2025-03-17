using UnityEngine;

public class NetBugCatcher : MonoBehaviour
{
    [Header("Net Settings")]
    public Vector2 boxSize = new Vector2(3f, 3f);

    [Header("Capture Settings")]
    public LayerMask captureLayer;

    public Transform player;

    private void Start()
    {
        if (player == null) player = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        if (UIManager.instance.isUIOpen) return;

        RotateAroundPlayer();
        if (Input.GetMouseButtonDown(1)) Capture();
        if (Input.GetMouseButtonDown(0)) PlaceItem();
    }

    private void Capture()
    {
        // Get the center of the box and the rotation to apply
        Vector2 center = (Vector2)transform.position;
        float rotation = transform.rotation.eulerAngles.z;

        // Use the OverlapBox method that allows rotation of the box
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(center, boxSize, rotation, captureLayer);

        if (hitColliders.Length == 0)
        {
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
            return item;
        }
        else
        {
            return null;
        }
    }

    private void PlaceItem()
    {
        ItemScript selectedItem = InventoryManager.instance.GetSelectedItem(false);
        if (selectedItem != null)
        {
            Vector2 spawnPosition = (Vector2)transform.position;
            if (selectedItem.type == ItemType.Bug || selectedItem.type == ItemType.Food || selectedItem.type == ItemType.Egg)
            {
                InventoryManager.instance.GetSelectedItem(true); // Remove from inventory
                selectedItem.Use(spawnPosition);
            }
        }
    }

    private void RotateAroundPlayer()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPos2D = (Vector2)player.position;

        Vector2 direction = (mousePosition - playerPos2D).normalized;
        transform.position = playerPos2D + direction * 1f;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        // Get the center of the box and the rotation to apply
        Vector2 center = (Vector2)transform.position;
        float rotation = transform.rotation.eulerAngles.z;

        // Draw the rotated box for Gizmos
        Gizmos.matrix = Matrix4x4.TRS(center, Quaternion.Euler(0, 0, rotation), Vector3.one);
        Gizmos.DrawWireCube(Vector2.zero, boxSize);
    }
}
