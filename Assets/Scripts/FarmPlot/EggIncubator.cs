using UnityEngine;
using System.Collections;
using TMPro;

public class EggIncubator : MonoBehaviour
{
    [System.Serializable]
    public class EggType
    {
        public string eggType;
        public float incubationTime; 
    }

    public EggType[] eggTypes; 
    public LayerMask eggLayer; 
    public TMP_Text timerText;  

    private bool isIncubating = false; 
    private float timeRemaining; 
    private EggItem currentEggItem; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsEggLayer(other.gameObject))
        {
            EggItem eggItem = other.GetComponent<EggItem>();
            if (eggItem != null && !isIncubating)
            {
                Destroy(eggItem.gameObject);
                StartEggIncubation(eggItem);
            }
        }
    }

    private bool IsEggLayer(GameObject obj)
    {
        return ((1 << obj.layer) & eggLayer) != 0;
    }

    private void StartEggIncubation(EggItem eggItem)
    {
        isIncubating = true;
        currentEggItem = eggItem; 
        float incubationTime = GetIncubationTime(eggItem.eggType);
        timeRemaining = incubationTime;
        timerText.gameObject.SetActive(true); 
        StartCoroutine(IncubateEgg(eggItem, incubationTime));
    }

    private IEnumerator IncubateEgg(EggItem eggItem, float incubationTime)
    {
        while (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            timerText.text = $"Time Remaining: {Mathf.Round(timeRemaining)}s";

            yield return null;
        }
        HatchEgg(eggItem);
    }

    private void HatchEgg(EggItem eggItem)
    {
        timerText.gameObject.SetActive(false);
        Instantiate(eggItem.bugPrefab, transform.position, Quaternion.identity);
        isIncubating = false;
    }

    private float GetIncubationTime(string eggType)
    {
        // Loop through all the egg types and return the corresponding incubation time
        foreach (EggType egg in eggTypes)
        {
            if (egg.eggType == eggType)
            {
                return egg.incubationTime;
            }
        }

        Debug.LogWarning($"No incubation time set for egg type: {eggType}. Using default.");
        return 30f; // Default incubation time (can adjust as necessary)
    }
}
