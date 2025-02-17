using UnityEngine;

public class EggWithdrawer : MonoBehaviour
{
    [Header("Collector Reference")]
    public EggCollector eggCollector;  // Assign the specific EggCollector from the Inspector

    private bool isPlayerInRange = false;

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (eggCollector != null)
            {
                eggCollector.DropAllEggs(); // Call DropAllEggs instead of WithdrawEggs
                Debug.Log("Eggs withdrawn from collector.");
            }
            else
            {
                Debug.LogError("No EggCollector assigned to withdrawer.");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log("Player entered the withdrawer area.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            Debug.Log("Player left the withdrawer area.");
        }
    }
}