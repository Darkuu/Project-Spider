using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FarmField : MonoBehaviour
{
    [Header("Planting Spots")]
    public Transform[] plantSpots;

    [Header("Growth Settings")]
    public float growthTime = 10f;
    public float initialScale = 0.3f;
    private Dictionary<Transform, GameObject> currentPlants = new Dictionary<Transform, GameObject>();

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Food"))
        {
            Food foodComponent = collision.gameObject.GetComponent<Food>();
            if (foodComponent != null && foodComponent.foodItem != null)
            {
                GameObject foodPrefab = foodComponent.foodItem.foodPlacementPrefab;
                if (foodPrefab != null)
                {
                    Destroy(collision.gameObject);
                    PlantFood(foodPrefab);
                }
            }
        }
    }

    public void PlantFood(GameObject foodPrefab)
    {
        List<Transform> availableSpots = new List<Transform>();
        foreach (Transform spot in plantSpots)
        {
            if (!currentPlants.ContainsKey(spot) || currentPlants[spot] == null)
                availableSpots.Add(spot);
        }

        int spotsToUse = Mathf.CeilToInt(availableSpots.Count * 0.5f);
        for (int i = 0; i < spotsToUse; i++)
        {
            if (availableSpots.Count == 0) break;
            Transform chosenSpot = availableSpots[Random.Range(0, availableSpots.Count)];
            availableSpots.Remove(chosenSpot);
            GrowPlant(chosenSpot, foodPrefab);
        }
    }

    private void GrowPlant(Transform spot, GameObject foodPrefab)
    {
        GameObject plant = Instantiate(foodPrefab, spot.position, Quaternion.identity, transform);
        plant.transform.localScale = Vector3.one * initialScale;

        // Disable the collider during growth
        if (plant.TryGetComponent<Collider2D>(out Collider2D collider))
        {
            collider.enabled = false;
        }

        FoodPlantInfo info = plant.AddComponent<FoodPlantInfo>();
        info.originalPrefab = foodPrefab;
        currentPlants[spot] = plant;

        StartCoroutine(ScalePlantOverTime(plant, Vector3.one, growthTime));
    }

    IEnumerator ScalePlantOverTime(GameObject plant, Vector3 targetScale, float duration)
    {
        Vector3 startScale = plant.transform.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            plant.transform.localScale = Vector3.Lerp(startScale, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        plant.transform.localScale = targetScale;

        // Enable the collider once fully grown
        if (plant.TryGetComponent<Collider2D>(out Collider2D collider))
        {
            collider.enabled = true;
        }
    }

    public void HarvestPlant(Transform spot)
    {
        if (currentPlants.ContainsKey(spot) && currentPlants[spot] != null)
        {
            GameObject plant = currentPlants[spot];
            FoodPlantInfo info = plant.GetComponent<FoodPlantInfo>();
            GameObject prefabToRegrow = (info != null) ? info.originalPrefab : null;

            Destroy(plant);
            currentPlants[spot] = null;

            if (prefabToRegrow != null)
            {
                GrowPlant(spot, prefabToRegrow);
            }
        }
    }
}
