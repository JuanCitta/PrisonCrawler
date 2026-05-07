using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int currentFloor = 0;
    public BiomeType currentBiome;

    public string currentNPC; 

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadRoom(RoomType type)
    {
        currentFloor++;

        UpdateBiome();

        switch (type)
        {
            case RoomType.Combat:
                SceneManager.LoadScene("CombatRoom");
                break;

            case RoomType.Heal:
                currentNPC = null; 
                SceneManager.LoadScene("HealRoom");
                break;

            case RoomType.Forge:
                currentNPC = null;
                SceneManager.LoadScene("ForgeRoom");
                break;

            case RoomType.Risk:
                currentNPC = null;
                SceneManager.LoadScene("RiskRoom");
                break;
        }
    }

    void UpdateBiome()
    {
        if (currentFloor <= 5)
            currentBiome = BiomeType.Cave;
        else if (currentFloor <= 12)
            currentBiome = BiomeType.Corridors;
        else
            currentBiome = BiomeType.Castle;
    }
}