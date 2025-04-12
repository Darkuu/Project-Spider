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
    private Dictionary<Transform, string> plantTags = new Dictionary<Transform, string>();
    private bool isGrowing = false;  

    private FarmUpgradeManager upgradeManager; 


    private void Start()
    {
        if (upgradeManager == null)
        {
            upgradeManager = GetComponentInChildren<FarmUpgradeManager>();
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Food") && !isGrowing)
        {
            Food foodComponent = collision.gameObject.GetComponent<Food>();
            if (foodComponent?.foodItem?.foodPlacementPrefab != null)
            {
                Destroy(collision.gameObject);
                PlantFood(foodComponent.foodItem.foodPlacementPrefab, collision.gameObject.tag);
            }
        }
    }

    public void PlantFood(GameObject foodPrefab, string foodTag)
    {
        if (isGrowing) return;

        List<Transform> availableSpots = GetAvailableSpots();

        int spotsToPlant = upgradeManager != null
            ? upgradeManager.CalculatePlantAmount(availableSpots.Count)
            : Mathf.CeilToInt(availableSpots.Count * 0.5f);

        for (int i = 0; i < spotsToPlant && availableSpots.Count > 0; i++)
        {
            Transform chosenSpot = availableSpots[Random.Range(0, availableSpots.Count)];
            availableSpots.Remove(chosenSpot);
            GrowPlant(chosenSpot, foodPrefab, foodTag);
        }
    }


    private List<Transform> GetAvailableSpots()
    {
        List<Transform> availableSpots = new List<Transform>();
        foreach (Transform spot in plantSpots)
        {
            if (!currentPlants.ContainsKey(spot) || currentPlants[spot] == null)
                availableSpots.Add(spot);
        }
        return availableSpots;
    }

    private void GrowPlant(Transform spot, GameObject foodPrefab, string foodTag)
    {
        isGrowing = true;

        GameObject plant = Instantiate(foodPrefab, spot.position, Quaternion.identity, transform);
        plant.transform.localScale = Vector3.one * initialScale;

        if (plant.TryGetComponent(out Collider2D collider))
            collider.enabled = false;

        currentPlants[spot] = plant;
        plantTags[spot] = foodTag;

        float growTime = growthTime;
        if (upgradeManager != null)
        {
            growTime = upgradeManager.GetGrowthTime(growthTime);
        }

        StartCoroutine(ScalePlantOverTime(plant, Vector3.one, growTime));

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

        if (plant.TryGetComponent(out Collider2D collider))
        {
            collider.enabled = true;
        }

        isGrowing = false;  // Plant finished growing, now allow new planting
    }

    public void HarvestPlant(Transform spot)
    {
        if (currentPlants.TryGetValue(spot, out GameObject plant) && plant != null)
        {
            Destroy(plant);
            currentPlants.Remove(spot);
            plantTags.Remove(spot);
        }
    }
}
