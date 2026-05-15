using UnityEngine;
using TMPro;

public class Door : MonoBehaviour
{
    public RoomType nextRoom;
    public string   npcId;

    private bool     isLocked = true;
    private TMP_Text label;

    void Awake()
    {
        label = GetComponentInChildren<TMP_Text>();
        if (label != null) label.text = "";
    }

    public void Unlock()
    {
        isLocked = false;
    }

    public void SetBossRoom()
    {
        nextRoom = RoomType.Combat;
        npcId    = null;

        if (label != null)
        {
            label.text  = "BOSS";
            label.color = new Color(1f, 0.1f, 0.1f); // vermelho
        }
    }

    public void SetRoom(RoomType type, string npc = null)
    {
        nextRoom = type;
        npcId    = npc;

        if (label != null)
        {
            label.text  = GetRoomLabel(type, npc);
            label.color = GetRoomColor(type, npc);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isLocked && other.CompareTag("Player"))
        {
            if (nextRoom == RoomType.Combat && !string.IsNullOrEmpty(npcId))
                QuestManager.Instance?.StartOrProgressQuest(npcId);

            GameManager.Instance.LoadRoom(nextRoom, npcId);
        }
    }

    static string GetRoomLabel(RoomType type, string npc) => type switch
    {
        RoomType.Forge => "Forja",
        RoomType.Camp  => "Descanso",
        _              => npc ?? ""
    };

    static Color GetRoomColor(RoomType type, string npc) => type switch
    {
        RoomType.Forge => new Color(1f,   0.85f, 0.1f),   // dourado
        RoomType.Camp  => new Color(0.2f, 0.85f, 0.8f),   // verde-água
        _              => npc switch
        {
            "Knight"   => new Color(0.2f, 0.5f,  1f),     // azul
            "Archer"   => new Color(0.3f, 0.9f,  0.3f),   // verde
            "Mage"     => new Color(0.65f,0.3f,  1f),     // roxo
            _          => Color.white
        }
    };
}
