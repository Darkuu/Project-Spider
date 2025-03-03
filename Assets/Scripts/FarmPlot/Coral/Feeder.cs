using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class Feeder : MonoBehaviour
{
    [Header("Collector Settings")]
    public Collider2D collectionCollider;
    public int maxFoodCapacity = 20;
    private string foodTag = null;

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

        // Lock onto food tag if not set
        if (foodTag == null)
        {
            foodTag = other.tag;
        }

        // Only collect matching food tag
        if (other.CompareTag(foodTag) && foodCount < maxFoodCapacity)
        {
            other.gameObject.SetActive(false);
            storedFood.Add(other.gameObject);
            foodCount++;
            UpdateFoodCountUI();
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

            // Reset foodTag when storage is empty
            if (storedFood.Count == 0)
            {
                foodTag = null;
            }

            yield return new WaitForSeconds(feedInterval);
        }
    }

    private void UpdateFoodCountUI()
    {
        string foodTagDisplay = foodTag != null ? foodTag : "Any";
        if (foodCountText != null)
        {
            foodCountText.text = $"Food: {foodCount}/{maxFoodCapacity} ({foodTagDisplay})";
        }
    }
}