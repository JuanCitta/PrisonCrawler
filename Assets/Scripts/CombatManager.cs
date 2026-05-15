using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance { get; private set; }

    private Door[] doors;
    private int enemiesAlive;

    void Awake()
    {
        Instance = this;
    }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

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
            UnlockDoors();
    }

    public void AddEnemy()
    {
        enemiesAlive++;
    }

    void UnlockDoors()
    {
        foreach (var door in doors)
            door.Unlock();
    }
}