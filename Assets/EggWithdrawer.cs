using UnityEngine;

public class Eggwithdrawer : MonoBehaviour
{
    public float interactionRange = 3f;  // Optional: define how close player must be to interact
    private bool isPlayerInRange = false;  // Flag to track if the player is in range
    private EggCollector eggCollector;  // Reference to the EggCollector component

    private void Start()
    {
        eggCollector = GetComponent<EggCollector>();  // Get the EggCollector component on this object
        if (eggCollector == null)
        {
            Debug.LogError("EggCollector component not found on this object.");
        }
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Player pressed E near EggCollector.");
            // Trigger the EggCollector to drop all eggs
            if (eggCollector != null)
            {
                eggCollector.DropAllEggs();
            }
            else
            {
                Debug.LogError("EggCollector not found or not assigned.");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;  // Player is in range to interact
            Debug.Log("Player entered the trigger zone.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;  // Player is no longer in range
            Debug.Log("Player exited the trigger zone.");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}