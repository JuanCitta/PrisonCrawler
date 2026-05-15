using System.Collections;
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

    IEnumerator Start()
    {
        doors = FindObjectsOfType<Door>();

        // Aguarda 1 frame para que o EnemySpawner (Awake) e os
        // inimigos instanciados (Start) estejam todos inicializados.
        yield return null;

        // Conta por tag "Enemy" — armadilhas (Mushroom Untagged) ficam de fora
        enemiesAlive = GameObject.FindGameObjectsWithTag("Enemy").Length;

        if (enemiesAlive <= 0)
            UnlockDoors();
    }

    public void OnEnemyKilled()
    {
        enemiesAlive--;
        if (enemiesAlive <= 0)
            UnlockDoors();
    }

    /// <summary>Chamado quando um inimigo spawna outro (ex: GiantSpider → filhotes).</summary>
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