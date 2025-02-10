using UnityEngine;

public class Bug : MonoBehaviour
{
    // Assign the corresponding ItemScript for this bug in the inspector.
    public ItemScript bugItem;

    [Header("Food Interaction")]
    // The poop item that the bug will drop after eating
    public GameObject poopPrefab;
    

    /// <summary>
    /// Automatically eats food when the bug collides with it.
    /// </summary>
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Food"))  // Ensure the object has the "Food" tag
        {
            Debug.Log("Bug has eaten the food!");

            // Simulate the bug eating the food (could be enhanced with animations or effects)
            DropPoop();  // After eating, drop a poop
            Destroy(other.gameObject);  // Destroy the food object (it has been consumed)
        }
    }

    /// <summary>
    /// Drops a poop at the bug's current position.
    /// </summary>
    void DropPoop()
    {
        if (poopPrefab != null)
        {
            Instantiate(poopPrefab, transform.position, Quaternion.identity);
            Debug.Log("Bug has dropped poop.");
        }
        else
        {
            Debug.LogWarning("Poop prefab is not assigned!");
        }
    }
}
