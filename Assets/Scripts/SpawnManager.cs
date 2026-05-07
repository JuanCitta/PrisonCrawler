using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public Transform caveSpawn;
    public Transform corridorSpawn;
    public Transform castleSpawn;

    void Start()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        var biome = GameManager.Instance.currentBiome;

        Transform spawn = caveSpawn;

        if (biome == BiomeType.Corridors)
            spawn = corridorSpawn;
        else if (biome == BiomeType.Castle)
            spawn = castleSpawn;

        player.transform.position = spawn.position;
    }
}