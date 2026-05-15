using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public Door doorLeft;
    public Door doorRight;

    [Header("Salas especiais")]
    [Tooltip("Desmarca no StartRoom, ForgeRoom e CampRoom")]
    public bool allowSpecialRooms = true;
    [Range(0f, 1f)] public float forgeChance = 0.25f;
    [Range(0f, 1f)] public float campChance  = 0.25f;

    void Start()
    {
        GenerateDoors();
    }

    void GenerateDoors()
    {
        // Andar 4 → próxima sala é o boss, ambas as portas levam lá
        int floor = GameManager.Instance?.currentFloor ?? 0;
        if (floor == 7 || floor == 15)
        {
            doorLeft.SetBossRoom();
            doorRight.SetBossRoom();
            doorLeft.Unlock();
            doorRight.Unlock();
            return;
        }

        // Porta esquerda: sempre Combat
        string npcLeft = GetRandomNPC();
        doorLeft.SetRoom(RoomType.Combat, npcLeft);

        // Porta direita: especial só se permitido
        if (allowSpecialRooms)
        {
            float roll = Random.value;

            if (roll < forgeChance)
            {
                doorRight.SetRoom(RoomType.Forge);
                return;
            }
            else if (roll < forgeChance + campChance)
            {
                doorRight.SetRoom(RoomType.Camp);
                return;
            }
        }

        // Combat com NPC diferente da esquerda
        string npcRight;
        do { npcRight = GetRandomNPC(); } while (npcRight == npcLeft);
        doorRight.SetRoom(RoomType.Combat, npcRight);
    }

    string GetRandomNPC()
    {
        string[] npcs = { "Knight", "Archer", "Mage" };
        return npcs[Random.Range(0, npcs.Length)];
    }
}
