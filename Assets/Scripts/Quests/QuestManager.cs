using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    private Dictionary<string, int> questStages = new Dictionary<string, int>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void ProgressQuest(string questID, int newStage)
    {
        if (questStages.TryGetValue(questID, out int currentStage))
        {
            if (newStage > currentStage)
            {
                questStages[questID] = newStage;
            }
        }
        else
        {
            questStages.Add(questID, newStage);
        }
    }

    public int GetQuestStage(string questID)
    {
        return questStages.TryGetValue(questID, out int stage) ? stage : 0;
    }
}
