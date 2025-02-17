using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class Feeder : MonoBehaviour
{
    [Header("Collector Settings")]
    public Collider2D collectionCollider; // Collider where food is collected
    public int maxFoodCapacity = 20;

    [Header("Feeding Settings")]
    public Transform dropPoint;
    public float slowInterval = 10f;
    public float mediumInterval = 5f;
    public float fastInterval = 2f;
    private float currentInterval;

    [Header("UI")]
    public GameObject speedSelectionUI;
    public TMP_Text foodCountText;

    // Store foods by type
    private Dictionary<string, Queue<GameObject>> foodStorage = new Dictionary<string, Queue<GameObject>>();
    private Coroutine feedingRoutine;
    private bool isPlayerInRange = false;

    private void Start()
    {
        SetFeedingSpeed(2);
        UpdateFoodCountUI();
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            ToggleSpeedUI();
        }
    }

    // Automatically collects food when it enters the collider
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Entered Trigger with: {other.gameObject.name}");

        if (other.CompareTag("Food"))
        {
            Debug.Log("Food detected in trigger!");
        }
        
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }

        if (other.TryGetComponent(out Food foodItem) && collectionCollider != null)
        {
            // Collect only if it entered the collection zone
            if (collectionCollider.bounds.Contains(other.transform.position))
            {
                CollectFood(other.gameObject, foodItem.foodType);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            ToggleSpeedUI(false);
        }
    }

    private void CollectFood(GameObject food, string type)
    {
        if (!foodStorage.ContainsKey(type))
        {
            foodStorage[type] = new Queue<GameObject>();
        }

        if (GetTotalFoodCount() < maxFoodCapacity)
        {
            food.SetActive(false); 
            foodStorage[type].Enqueue(food);
            UpdateFoodCountUI();
        }
    }

    // Dispenses food based on the collected types
    private IEnumerator FeedRoutine()
    {
        while (true)
        {
            foreach (var kvp in foodStorage)
            {
                string foodType = kvp.Key;
                Queue<GameObject> queue = kvp.Value;

                if (queue.Count > 0)
                {
                    GameObject food = queue.Dequeue();
                    food.transform.position = dropPoint.position;
                    food.SetActive(true);
                    UpdateFoodCountUI();
                }
            }
            yield return new WaitForSeconds(currentInterval);
        }
    }

    public void SetFeedingSpeed(int speed)
    {
        if (feedingRoutine != null)
        {
            StopCoroutine(feedingRoutine);
        }

        switch (speed)
        {
            case 1: currentInterval = slowInterval; break;
            case 2: currentInterval = mediumInterval; break;
            case 3: currentInterval = fastInterval; break;
        }

        feedingRoutine = StartCoroutine(FeedRoutine());
        ToggleSpeedUI(false);
    }

    private void ToggleSpeedUI(bool show = true)
    {
        if (speedSelectionUI != null)
        {
            speedSelectionUI.SetActive(show);
        }
    }

    private void UpdateFoodCountUI()
    {
        if (foodCountText != null)
        {
            foodCountText.text = $"{GetTotalFoodCount()}/{maxFoodCapacity}";
        }
    }

    private int GetTotalFoodCount()
    {
        int total = 0;
        foreach (var kvp in foodStorage)
        {
            total += kvp.Value.Count;
        }
        return total;
    }
    
}
