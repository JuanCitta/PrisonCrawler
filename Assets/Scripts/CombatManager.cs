using UnityEngine;

public class CombatManager : MonoBehaviour
{
    private Door[] doors;
    private int enemiesAlive;

    void Start()
    {
        doors = FindObjectsOfType<Door>();
        enemiesAlive = GameObject.FindGameObjectsWithTag("Enemy").Length;

        if (enemiesAlive <= 0)
        {
            UnlockDoors();
        }
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
    }
}