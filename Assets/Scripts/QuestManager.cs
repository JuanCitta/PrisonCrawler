using UnityEngine;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    public List<QuestData> activeQuests = new List<QuestData>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartOrProgressQuest(string npcId)
    {
        QuestData quest = activeQuests.Find(q => q.npcId == npcId);

        if (quest == null)
        {
            quest = new QuestData
            {
                npcId = npcId,
                currentProgress = 0,
                requiredProgress = Random.Range(2, 6) 
            };

            activeQuests.Add(quest);
        }

        quest.currentProgress++;

        if (quest.IsComplete())
        {
            CompleteQuest(quest);
        }
    }

    void CompleteQuest(QuestData quest)
    {
        Debug.Log("Quest completa: " + quest.npcId);

        GiveReward(quest.npcId);

        activeQuests.Remove(quest);
    }

    void GiveReward(string npcId)
    {
        Debug.Log("Recompensa do NPC: " + npcId);
    }
}