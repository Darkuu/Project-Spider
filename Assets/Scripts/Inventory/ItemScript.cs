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
    // The prefab to instantiate when the item is used (for bug placement).
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
        if (type == ItemType.Bug && bugPlacementPrefab != null)
        {
            Instantiate(bugPlacementPrefab.gameObject, spawnPosition, Quaternion.identity);
        }
        else if (isFoodItem && foodPlacementPrefab != null)
        {
            Instantiate(foodPlacementPrefab, spawnPosition, Quaternion.identity); 
        }
        else if (isEggItem && eggPlacementPrefab != null)
        {
            Instantiate(eggPlacementPrefab, spawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("No valid prefab assigned for " + name);
        }
    }

}

public enum ItemType
{
    Bug,
    Food,
    Egg
    
}

