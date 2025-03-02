using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class Feeder : MonoBehaviour
{
    [Header("Collector Settings")]
    public Collider2D collectionCollider;  
    public int maxFoodCapacity = 20;
    private string foodType = null;

    [Header("Feeding Settings")]
    public Transform dropPoint;
    public float feedInterval = 2f;  

    [Header("UI")]
    public TMP_Text foodCountText;

    private List<GameObject> storedFood = new List<GameObject>();
    private int foodCount = 0;

    private void Start()
    {
        UpdateFoodCountUI();
        StartCoroutine(FeedRoutine());  
    }

    // Collect food when it enters the collection collider
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (foodCount >= maxFoodCapacity) return;  

        if (other.CompareTag("Food"))
        {
            Food food = other.GetComponent<Food>();
            if (food == null) return;

            // Lock onto food type if not set
            if (foodType == null)
            {
                foodType = food.foodType;
            }

            // Only collect matching food type
            if (food.foodType == foodType && foodCount < maxFoodCapacity)
            {
                other.gameObject.SetActive(false);  
                storedFood.Add(other.gameObject);  
                foodCount++;
                UpdateFoodCountUI();
            }
        }
    }

    // Feed food from storage at regular intervals
    private IEnumerator FeedRoutine()
    {
        while (true)
        {
            if (storedFood.Count > 0)
            {
                GameObject food = storedFood[0];
                storedFood.RemoveAt(0);
                food.transform.position = dropPoint.position;  
                food.SetActive(true); 
                foodCount--;  
                UpdateFoodCountUI();
            }
            yield return new WaitForSeconds(feedInterval);  
        }
    }

    private void UpdateFoodCountUI()
    {
        string foodTypeDisplay = foodType != null ? foodType : "Any";
        if (foodCountText != null)
        {
            foodCountText.text = $"Food: {foodCount}/{maxFoodCapacity} ({foodTypeDisplay})";
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (collectionCollider != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(collectionCollider.bounds.center, collectionCollider.bounds.size);
        }
    }
}
