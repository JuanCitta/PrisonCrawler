using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    public GameObject cave;
    public GameObject corridor;
    public GameObject castle;

    void Start()
    {
        ApplyBiome();
    }

    void ApplyBiome()
    {
        cave.SetActive(false);
        corridor.SetActive(false);
        castle.SetActive(false);

        var biome = GameManager.Instance.currentBiome;

        if (biome == BiomeType.Cave)
            cave.SetActive(true);
        else if (biome == BiomeType.Corridors){
            corridor.SetActive(true);
            }
        else{
            castle.SetActive(true);
        }
    }
}