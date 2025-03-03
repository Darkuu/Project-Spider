using UnityEngine;

public class Bug : MonoBehaviour
{
    [Header("Bug Stats Interaction")]
    [Tooltip("Item the player receives upon pickup")]
    public ItemScript bugItem;

    [Header("Food Interaction")]
    [Tooltip("What kind of food can it eat")]
    public string allowedFoodTag;
    [Tooltip("What it poops out upon eating")]
    public GameObject poopPrefab;

    /// <summary>
    /// Automatically eats food when the bug collides with it.
    /// </summary>
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(allowedFoodTag))
        {
            DropPoop();
            Destroy(other.gameObject);
        }

    }

    /// <summary>
    /// Drops a poop at the bug's current position.
    /// </summary>
    void DropPoop()
    {
            Instantiate(poopPrefab, transform.position, Quaternion.identity);
    }
}