using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int enemyCount = 3;

    void Start()
    {
        SpawnEnemies();
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            Vector2 pos = new Vector2(
                Random.Range(-4, 4),
                Random.Range(-3, 3)
            );

            Instantiate(enemyPrefab, pos, Quaternion.identity);
        }
    }
}