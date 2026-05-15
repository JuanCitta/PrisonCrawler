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

    /// <summary>
    /// Evento disparado ao completar quest.
    /// Parâmetros: npcId, mensagem de recompensa para exibir na UI.
    /// </summary>
    public static event System.Action<string, string> OnQuestCompleted;

    public void StartOrProgressQuest(string npcId)
    {
        QuestData quest = activeQuests.Find(q => q.npcId == npcId);

        if (quest == null)
        {
            quest = new QuestData
            {
                npcId            = npcId,
                currentProgress  = 0,
                requiredProgress = Random.Range(2, 6)
            };
            activeQuests.Add(quest);
        }

        quest.currentProgress++;

        if (quest.IsComplete())
            CompleteQuest(quest);
    }

    void CompleteQuest(QuestData quest)
    {
        string rewardMsg = GiveReward(quest.npcId);
        OnQuestCompleted?.Invoke(quest.npcId, rewardMsg);
        activeQuests.Remove(quest);
    }

    /// <summary>Aplica a recompensa temática e retorna a descrição para a UI.</summary>
    string GiveReward(string npcId)
    {
        switch (npcId)
        {
            // ── Knight: cura 1 coração ────────────────────────────────────────
            case "Knight":
                PlayerHealth.Instance?.Heal(1);
                return "+1 ❤  Coração recuperado!";

            // ── Archer: +20% velocidade de projétil (acumulável) ──────────────
            case "Archer":
                var shoot = PlayerHealth.Instance?.GetComponent<PlayerShoot>();
                if (shoot != null)
                {
                    shoot.projectileSpeedMultiplier *= 1.2f;
                    int pct = Mathf.RoundToInt((shoot.projectileSpeedMultiplier - 1f) * 100f);
                    return $"+20% velocidade de tiro  (total +{pct}%)";
                }
                return "+20% velocidade de tiro";

            // ── Mage: -20% cooldown de habilidade (acumulável) ───────────────
            case "Mage":
                var ability = PlayerHealth.Instance?.GetComponent<PlayerAbility>();
                if (ability != null)
                {
                    ability.cooldownMultiplier *= 0.8f;
                    int redPct = Mathf.RoundToInt((1f - ability.cooldownMultiplier) * 100f);
                    return $"-20% cooldown de habilidade  (total -{redPct}%)";
                }
                return "-20% cooldown de habilidade";

            default:
                return "Recompensa misteriosa...";
        }
    }

    public void Reset()
    {
        activeQuests.Clear();
    }
}
