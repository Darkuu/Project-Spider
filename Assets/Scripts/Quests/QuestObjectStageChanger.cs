using UnityEngine;

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

        foreach (var entry in stages)
        {
            bool isCurrent = entry.stage == currentStage;

            if (entry.activate != null)
            {
                foreach (GameObject obj in entry.activate)
                {
                    if (obj != null)
                        obj.SetActive(isCurrent);
                }
            }

            if (entry.deactivate != null && isCurrent)
            {
                foreach (GameObject obj in entry.deactivate)
                {
                    if (obj != null)
                        obj.SetActive(false);
                }
            }
        }
    }
}
