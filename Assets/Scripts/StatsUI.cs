using UnityEngine;
using TMPro;

/// <summary>
/// Mostra as stats da run atual no menu.
/// Coloca num painel do menu e liga os campos de texto.
/// </summary>
public class StatsUI : MonoBehaviour
{
    [Header("Textos")]
    public TextMeshProUGUI floorText;
    public TextMeshProUGUI weaponText;
    public TextMeshProUGUI forgeText;
    public TextMeshProUGUI archerQuestText;
    public TextMeshProUGUI mageQuestText;
    public TextMeshProUGUI knightQuestText;

    void OnEnable() => UpdateStats();

    /// <summary>Chamado pelo MenuManager sempre que o menu é aberto.</summary>
    public void Refresh() => UpdateStats();

    void UpdateStats()
    {
        // Andar atual
        if (floorText != null && GameManager.Instance != null)
        {
            int floor = GameManager.Instance.currentFloor;
            floorText.text = floor <= 0 ? "Andar: Início" : $"Andar: {floor}";
        }

        // Arma equipada
        if (weaponText != null)
        {
            var shoot = PlayerHealth.Instance?.GetComponent<PlayerShoot>();
            if (shoot?.equippedWeapon != null)
            {
                float cooldown = shoot.equippedWeapon.shootCooldown
                                 * shoot.shootCooldownMultiplier
                                 * shoot.forgeCooldownMultiplier;
                float rate = cooldown > 0f ? 1f / cooldown : 0f;
                weaponText.text = $"{shoot.equippedWeapon.weaponName}  |  Dano: {shoot.equippedWeapon.damage}  |  {rate:F1}/s";
            }
            else
            {
                weaponText.text = "Sem arma";
            }
        }

        // Nível de forja
        if (forgeText != null)
        {
            int level = InventoryManager.Instance?.weaponForgeLevel ?? 0;
            forgeText.text = level > 0 ? $"Forja: ▲{level}" : "Forja: —";
        }

        // Progresso das quests
        UpdateQuestText(knightQuestText, "Knight");
        UpdateQuestText(archerQuestText, "Archer");
        UpdateQuestText(mageQuestText,   "Mage");
    }

    void UpdateQuestText(TextMeshProUGUI label, string npcId)
    {
        if (label == null || QuestManager.Instance == null) return;

        var quest = QuestManager.Instance.activeQuests.Find(q => q.npcId == npcId);
        if (quest != null)
            label.text = $"{npcId}: {quest.currentProgress}/{quest.requiredProgress}";
        else
            label.text = $"{npcId}: —";
    }
}
