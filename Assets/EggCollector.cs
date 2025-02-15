using UnityEngine;
using TMPro;
using System.Collections;

public class EggCollector : MonoBehaviour
{
    [Header("Collector Settings")]
    public float collectionRadius = 5f;  
    public LayerMask eggLayer;           
    public float collectionInterval = 5f; 
    public int maxCapacity = 100;        
    private string eggType = null;        // The egg type it collects (first collected)

    [Header("UI Settings")]
    public TMP_Text eggCountText;         

    [Header("Egg Drop Settings")]
    public GameObject eggPrefab;          
    public Transform dropPoint;           

    private int eggCount = 0;
    public int EggCount => eggCount;

    private void Start()
    {
        StartCoroutine(CollectEggsPeriodically());
        UpdateEggCountDisplay();
    }

    private IEnumerator CollectEggsPeriodically()
    {
        while (true)
        {
            CollectEggsInRadius();
            yield return new WaitForSeconds(collectionInterval);
        }
    }

    private void CollectEggsInRadius()
    {
        if (eggCount >= maxCapacity) return;

        Collider2D[] eggs = Physics2D.OverlapCircleAll(transform.position, collectionRadius, eggLayer);

        foreach (Collider2D eggCollider in eggs)
        {
            if (eggCollider.CompareTag("Egg"))
            {
                EggItem egg = eggCollider.GetComponent<EggItem>();

                // If no egg type is set, assign it to the first collected egg
                if (eggType == null)
                {
                    eggType = egg.eggType;
                }

                // Collect only eggs that match the type
                if (egg.eggType == eggType && eggCount < maxCapacity)
                {
                    Destroy(eggCollider.gameObject);
                    eggCount++;
                }
            }
        }
        UpdateEggCountDisplay();
    }

    public void DropAllEggs()
    {
        if (eggPrefab == null || dropPoint == null) return;

        for (int i = 0; i < eggCount; i++)
        {
            Instantiate(eggPrefab, dropPoint.position, Quaternion.identity);
        }

        // Reset the collector after dropping all eggs
        eggCount = 0;
        eggType = null;

        UpdateEggCountDisplay();
    }

    private void UpdateEggCountDisplay()
    {
        string eggTypeDisplay = eggType != null ? eggType : "Any";
        if (eggCountText != null)
        {
            eggCountText.text = $"Eggs: {eggCount}/{maxCapacity} ({eggTypeDisplay})";
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, collectionRadius);
    }
}
