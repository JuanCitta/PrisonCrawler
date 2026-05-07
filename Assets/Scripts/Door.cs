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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isLocked && other.CompareTag("Player"))
        {
            if (nextRoom == RoomType.Combat)
            {
                GameManager.Instance.currentNPC = npcId;
            }
            Debug.Log("PLAYER ENTROU NA PORTA");

            GameManager.Instance.LoadRoom(nextRoom);
        }
    }
}