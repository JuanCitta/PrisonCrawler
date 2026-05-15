using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Inimigos — Caverna")]
    public GameObject spiderPrefab;
    public GameObject giantSpiderPrefab;
    public GameObject batPrefab;
    public GameObject mushroomPrefab;

    [Header("Inimigos — Castelo")]
    public GameObject spiritPrefab;
    public GameObject beastPrefab;
    public GameObject cyclopsPrefab;
    public GameObject tenguPrefab;

    [Header("Área de spawn")]
    public float spawnMinX = 2f;
    public float spawnMaxX = 6f;
    public float spawnMinY = 1.5f;
    public float spawnMaxY = 3.5f;

    void Awake()
    {
        if (GameManager.Instance == null) return;

        int       floor = GameManager.Instance.currentFloor;
        string    npc   = GameManager.Instance.currentNPC;
        BiomeType biome = GameManager.Instance.currentBiome;

        // Boss final — Tengu (floor 16)
        if (floor == 16)
        {
            SpawnTengu();
            return;
        }

        // Boss da caverna (floor 8)
        if (floor == 8 && biome == BiomeType.Cave)
        {
            SpawnBossRoom();
            return;
        }

        // Inimigos extras por andar (1 extra a cada sala de combate)
        int bonus = Mathf.Max(0, floor - 1);

        // Composição baseada em bioma + NPC
        if (biome == BiomeType.Castle)
            SpawnCastle(npc, bonus);
        else
            SpawnCave(npc, bonus);
    }

    // ── Caverna ──────────────────────────────────────────────────────────────

    void SpawnCave(string npc, int bonus)
    {
        switch (npc)
        {
            case "Knight":
                Spawn(batPrefab,    4 + bonus);
                Spawn(spiderPrefab, 2);
                break;
            case "Mage":
                Spawn(mushroomPrefab, 2);
                Spawn(spiderPrefab,   2 + bonus);
                Spawn(batPrefab,      1);
                break;
            default: // Archer
                Spawn(spiderPrefab,   4 + bonus);
                Spawn(mushroomPrefab, 1);
                break;
        }
    }

    void SpawnTengu()
    {
        if (tenguPrefab != null)
            Instantiate(tenguPrefab, new Vector2(0f, 2f), Quaternion.identity);
        else
            Debug.LogWarning("[EnemySpawner] Tengu Prefab não atribuído!");
    }

    void SpawnBossRoom()
    {
        if (giantSpiderPrefab != null)
            Instantiate(giantSpiderPrefab,
                new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)),
                Quaternion.identity);
        Spawn(spiderPrefab, 2);
    }

    // ── Castelo ──────────────────────────────────────────────────────────────

    void SpawnCastle(string npc, int bonus)
    {
        switch (npc)
        {
            case "Knight":
                Spawn(beastPrefab,  2 + bonus);
                Spawn(spiritPrefab, 3);
                break;
            case "Archer":
                Spawn(cyclopsPrefab, 2 + bonus);
                Spawn(spiritPrefab,  2);
                Spawn(beastPrefab,   1);
                break;
            default: // Mage
                Spawn(cyclopsPrefab, 2 + bonus);
                Spawn(beastPrefab,   2);
                Spawn(spiritPrefab,  1);
                break;
        }
    }

    // ── Helpers ──────────────────────────────────────────────────────────────

    void Spawn(GameObject prefab, int count)
    {
        if (prefab == null) return;
        for (int i = 0; i < count; i++)
            Instantiate(prefab, GetSpawnPosition(), Quaternion.identity);
    }

    Vector2 GetSpawnPosition()
    {
        float signX = Random.value > 0.5f ? 1f : -1f;
        float signY = Random.value > 0.5f ? 1f : -1f;
        return new Vector2(
            signX * Random.Range(spawnMinX, spawnMaxX),
            signY * Random.Range(spawnMinY, spawnMaxY)
        );
    }
}
