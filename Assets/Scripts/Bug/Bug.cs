using UnityEngine;

public class Bug : MonoBehaviour
{
    // Assign the corresponding ItemScript for this bug in the inspector.
    public ItemScript bugItem;

    [Header("Food Interaction")]
    public string allowedFoodTag = "SpecificFood"; 
    public GameObject poopPrefab;

    /// <summary>
    /// Automatically eats food when the bug collides with it.
    /// </summary>
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(allowedFoodTag))
        {
            Debug.Log("Bug has eaten the allowed food!");
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