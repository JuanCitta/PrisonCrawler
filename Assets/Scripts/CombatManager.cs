using UnityEngine;

public class CombatManager : MonoBehaviour
{
    private Door[] doors;
    private int enemiesAlive;

    void Start()
    {
        enemiesAlive = GameObject.FindGameObjectsWithTag("Enemy").Length;
        doors = FindObjectsOfType<Door>();
        foreach (var door in doors)
            door.Unlock(); 
    }

    void Update()
    {
    }




    public void OnEnemyKilled()
    {
        enemiesAlive--;

        if (enemiesAlive <= 0)
        {
            UnlockDoors();
        }
    }
    void UnlockDoors()
    {
        foreach (var door in doors)
            door.Unlock();

        // pega o NPC atual
        string npc = GameManager.Instance.currentNPC;

        if (!string.IsNullOrEmpty(npc))
        {
            QuestManager.Instance.StartOrProgressQuest(npc);
        }
    }
}