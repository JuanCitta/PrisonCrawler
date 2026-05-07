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
        RoomType option1;
        RoomType option2;

        if (GameManager.Instance.currentFloor == 0)
        {
            option1 = RoomType.Combat;

            do
            {
                option2 = GetRandomRoom();
            }
            while (option2 == RoomType.Combat);

            AssignDoors(option1, option2);
            return;
        }

        option1 = RoomType.Combat;

        do
        {
            option2 = GetRandomRoom();
        }
        while (option2 == RoomType.Combat);

        AssignDoors(option1, option2);
    }

    void AssignDoors(RoomType option1, RoomType option2)
    {
        // opcional: randomizar lado das portas
        if (Random.value < 0.5f)
        {
            SetDoor(doorLeft, option1);
            SetDoor(doorRight, option2);
        }
        else
        {
            SetDoor(doorLeft, option2);
            SetDoor(doorRight, option1);
        }
    }

    void SetDoor(Door door, RoomType type)
    {
        door.nextRoom = type;

        if (type == RoomType.Combat)
        {
            door.npcId = GetRandomNPCExcludingOther(door);
        }
        else
        {
            door.npcId = null;
        }
    }

    RoomType GetRandomRoom()
    {
        RoomType[] types =
        {
            RoomType.Combat,
            RoomType.Heal,
            RoomType.Forge
        };

        return types[Random.Range(0, types.Length)];
    }

    string GetRandomNPC()
    {
        string[] npcs = { "Knight", "Archer", "Mage" };
        return npcs[Random.Range(0, npcs.Length)];
    }

    string GetRandomNPCExcludingOther(Door currentDoor)
    {
        string npc;

        Door otherDoor = (currentDoor == doorLeft) ? doorRight : doorLeft;
        string otherNPC = otherDoor.npcId;

        do
        {
            npc = GetRandomNPC();
        }
        while (!string.IsNullOrEmpty(otherNPC) && npc == otherNPC);

        return npc;
    }
}