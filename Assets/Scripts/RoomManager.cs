using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public Door doorLeft;
    public Door doorRight;

    void Start()
    {
        GenerateDoors();
    }

    void GenerateDoors()
    {
        // Sempre duas opções de sala de combate com NPCs diferentes
        string npc1 = GetRandomNPC();
        string npc2;

        do { npc2 = GetRandomNPC(); }
        while (npc2 == npc1);

        // Randomiza qual NPC fica em qual lado
        if (Random.value < 0.5f)
        {
            doorLeft.SetRoom(RoomType.Combat, npc1);
            doorRight.SetRoom(RoomType.Combat, npc2);
        }
        else
        {
            doorLeft.SetRoom(RoomType.Combat, npc2);
            doorRight.SetRoom(RoomType.Combat, npc1);
        }
    }

    string GetRandomNPC()
    {
        string[] npcs = { "Knight", "Archer", "Mage" };
        return npcs[Random.Range(0, npcs.Length)];
    }
}