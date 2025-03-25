    using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/Item")]
public class ItemScript : ScriptableObject
{
    public ItemType type;
    public Vector2Int range = new Vector2Int(5, 4);

    [Header("Only UI")] 
    public bool stackable = true;

    [Header("Both")] 
    public Sprite image;

    [Header("Placement Settings")]
    public Bug bugPlacementPrefab;

    [Header("Food Interaction")]
    public bool isFoodItem = false; 
    public GameObject foodPlacementPrefab;
    
    [Header("Egg Interaction")]
    public bool isEggItem = false; 
    public GameObject eggPlacementPrefab;
    public int sellValue;



    /// <summary>
    /// Called to “use” the item at the given spawn position.
    /// </summary>
    /// <param name="spawnPosition">The position where the item should be placed.</param>
    public virtual void Use(Vector2 spawnPosition)
    {
        GameObject placedObject = null;

        if (type == ItemType.Bug && bugPlacementPrefab != null)
        {
            placedObject = Instantiate(bugPlacementPrefab.gameObject, spawnPosition, Quaternion.identity);
        }
        else if (isFoodItem && foodPlacementPrefab != null)
        {
            placedObject = Instantiate(foodPlacementPrefab, spawnPosition, Quaternion.identity);
        }
        else if (isEggItem && eggPlacementPrefab != null)
        {
            placedObject = Instantiate(eggPlacementPrefab, spawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("No valid prefab assigned for " + name);
            return;
        }

        // Apply force if the object has a Rigidbody2D
        Rigidbody2D rb = placedObject.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 forceDirection = (mousePosition - spawnPosition).normalized;

            float forceStrength = 20f;
            rb.AddForce(forceDirection * forceStrength, ForceMode2D.Impulse);
        }
    }


}

public enum ItemType
{
    Bug,
    Food,
    Egg
    
}

