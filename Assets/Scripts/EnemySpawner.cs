using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;

    void Start()
    {
        SpawnEnemies();
    }

    void SpawnEnemies()
    {
        string roomType = GameManager.Instance.currentNPC;

        int count;
        float moveSpeed;
        float stopDistance;
        float shootInterval;
        float projectileSpeed;

        switch (roomType)
        {
            // Sala Knight: muitos inimigos, rápidos, corpo a corpo — se aproximam mais
            case "Knight":
                count         = 8;
                moveSpeed     = 4.0f;
                stopDistance  = 0.8f;
                shootInterval = 3.0f;
                projectileSpeed = 5.0f;
                break;

            // Sala Archer: menos inimigos, ficam à distância e disparam rápido
            case "Archer":
                count         = 3;
                moveSpeed     = 1.5f;
                stopDistance  = 3.5f;
                shootInterval = 1.0f;
                projectileSpeed = 8.0f;
                break;

            // Sala Mage: poucos inimigos, projéteis mais rápidos e frequentes
            case "Mage":
                count         = 2;
                moveSpeed     = 1.5f;
                stopDistance  = 3.0f;
                shootInterval = 1.5f;
                projectileSpeed = 6.5f;
                break;

            default:
                count         = 3;
                moveSpeed     = 2.0f;
                stopDistance  = 2.0f;
                shootInterval = 2.0f;
                projectileSpeed = 5.0f;
                break;
        }

        for (int i = 0; i < count; i++)
        {
            Vector2 pos = new Vector2(
                Random.Range(-4f, 4f),
                Random.Range(-3f, 3f)
            );

            GameObject enemy = Instantiate(enemyPrefab, pos, Quaternion.identity);

            EnemyAI ai = enemy.GetComponent<EnemyAI>();
            if (ai != null)
            {
                ai.moveSpeed      = moveSpeed;
                ai.stopDistance   = stopDistance;
                ai.shootInterval  = shootInterval;
                ai.projectileSpeed = projectileSpeed;
            }
        }
    }
}
