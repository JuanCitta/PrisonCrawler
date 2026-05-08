using UnityEngine;
using TMPro;

public class Door : MonoBehaviour
{
    public RoomType nextRoom;
    public string npcId;

    private bool isLocked = true;
    private TMP_Text label;

    void Awake()
    {
        label = GetComponentInChildren<TMP_Text>();
    }

    public void Unlock()
    {
        isLocked = false;
    }

    public void SetRoom(RoomType type, string npc = null)
    {
        nextRoom = type;
        npcId    = npc;

        if (label != null)
        {
            label.text  = npc ?? "";
            label.color = GetNPCColor(npc);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isLocked && other.CompareTag("Player"))
        {
            // Progressão de quest: só quando o player entra numa sala de combate com NPC
            if (nextRoom == RoomType.Combat && !string.IsNullOrEmpty(npcId))
                QuestManager.Instance?.StartOrProgressQuest(npcId);

            GameManager.Instance.LoadRoom(nextRoom, npcId);
        }
    }

    static Color GetNPCColor(string npc) => npc switch
    {
        "Knight" => new Color(1f,  0.45f, 0.2f),   // laranja
        "Archer" => new Color(0.3f, 0.9f, 0.3f),   // verde
        "Mage"   => new Color(0.65f, 0.3f, 1f),    // roxo
        _        => Color.white
    };
}