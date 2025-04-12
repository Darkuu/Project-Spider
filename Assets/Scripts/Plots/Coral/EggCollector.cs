using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class EggCollector : MonoBehaviour
{
    [Header("Collector Settings")]
    public float collectionRadius = 5f;  
    public float collectionInterval = 5f; 
    public int maxCapacity = 100;        
    private string eggType = null;                

    [Header("UI Settings")]
    public TMP_Text eggCountText;         

    [Header("Egg Drop Settings")]
    public Transform dropPoint;           

    [Header("Egg Storage Settings")]
    public Vector3 storagePoint = new Vector3(10, 50, 0);

    private int eggCount = 0;
    public int EggCount => eggCount;
    private List<GameObject> storedEggs = new List<GameObject>();

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

        Collider2D[] eggs = Physics2D.OverlapCircleAll(transform.position, collectionRadius);
        foreach (Collider2D eggCollider in eggs)
        {
            if (eggCollider.CompareTag("Egg"))
            {
                EggItem egg = eggCollider.GetComponent<EggItem>();
                if (egg == null) continue;

                // Lock onto the egg type if not already set
                if (eggType == null)
                {
                    eggType = egg.eggType;
                }

                // Only collect eggs matching the locked type
                if (egg.eggType == eggType && eggCount < maxCapacity)
                {
                    // Teleport the egg to the preset storage point and disable it
                    eggCollider.transform.position = storagePoint;
                    eggCollider.gameObject.SetActive(false);
                    storedEggs.Add(eggCollider.gameObject);
                    eggCount++;
                }
            }
        }
        UpdateEggCountDisplay();
    }

    public void DropAllEggs()
    {
        if (dropPoint == null)
        {
            return;
        }

        if (eggCount <= 0)
        {
            return;
        }

        foreach (GameObject egg in storedEggs)
        {
            egg.transform.position = dropPoint.position;
            egg.SetActive(true);
        }

        storedEggs.Clear();
        eggCount = 0;
        eggType = null;
        UpdateEggCountDisplay();
    }

    private void UpdateEggCountDisplay()
    {
        if (eggCountText != null)
        {
            eggCountText.text = $"{eggCount}/{maxCapacity}";
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, collectionRadius);
    }
}
