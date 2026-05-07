using UnityEngine;

public class Door : MonoBehaviour
{
    public RoomType nextRoom;
    public string npcId;

    private bool isLocked = true;

    public void Unlock()
    {
        isLocked = false;
    }

    public void SetRoom(RoomType type, string npc = null)
    {
        nextRoom = type;
        npcId    = npc;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isLocked && other.CompareTag("Player"))
        {
            GameManager.Instance.LoadRoom(nextRoom, npcId);
        }
    }
}