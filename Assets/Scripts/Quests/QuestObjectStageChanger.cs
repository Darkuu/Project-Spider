using UnityEngine;
using System.Collections;

public class QuestObjectStageChanger : MonoBehaviour
{
    public string questID;

    [System.Serializable]
    public class StageEntry
    {
        public int stage;
        public GameObject[] activate;
        public GameObject[] deactivate;
    }

    public StageEntry[] stages;

    private int lastStage = -1;

    void Update()
    {
        if (QuestManager.Instance == null) return;

        int currentStage = QuestManager.Instance.GetQuestStage(questID);
        if (currentStage == lastStage) return;

        lastStage = currentStage;

        // Find the matching stage entry
        StageEntry currentEntry = null;
        foreach (var entry in stages)
        {
            if (entry.stage == currentStage)
            {
                currentEntry = entry;
                break;
            }
        }

        // Deactivate all previously active objects
        foreach (var entry in stages)
        {
            if (entry.activate != null)
            {
                foreach (GameObject obj in entry.activate)
                {
                    if (obj != null)
                        obj.SetActive(false);
                }
            }
        }

        // Activate relevant objects for the current stage
        if (currentEntry != null)
        {
            if (currentEntry.activate != null)
            {
                foreach (GameObject obj in currentEntry.activate)
                {
                    if (obj != null)
                        obj.SetActive(true);
                }
            }

            if (currentEntry.deactivate != null)
            {
                foreach (GameObject obj in currentEntry.deactivate)
                {
                    if (obj != null && obj != this.gameObject) 
                        StartCoroutine(DeferredDeactivate(obj));
                }
            }
        }
    }

    private IEnumerator DeferredDeactivate(GameObject obj)
    {
        yield return new WaitForEndOfFrame();
        if (obj != null)
            obj.SetActive(false);
    }
}
