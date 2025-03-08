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

    [Tooltip("What it poops out upon eating")]
    public int damage;

    public bool isHostile;

    // Automatically eats food when the bug collides with it.
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(allowedFoodTag))
        {
            DropPoop();
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Player") && isHostile)
        {
            PlayerStats playerHealth = other.gameObject.GetComponent<PlayerStats>();
            if (playerHealth != null)
            {
                // Apply damage and pass the bug's position for knockback
                playerHealth.TakeDamage(damage);
            }
        }
    }

    // Drops a poop at the bug's current position.
    void DropPoop()
    {
        Instantiate(poopPrefab, transform.position, Quaternion.identity);
    }
}
