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

    public void LoadRoom(RoomType type, string npcId = null)
    {
        currentFloor++;
        UpdateBiome();

        switch (type)
        {
            case RoomType.Combat:
                currentNPC = npcId;
                SceneManager.LoadScene("CombatRoom");
                break;

            // Reservado para implementações futuras
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

    public void ResetGame()
    {
        currentFloor = 0;
        currentNPC   = null;
        currentBiome = BiomeType.Cave;
        QuestManager.Instance?.Reset();

        // Reseta posição do player para o centro antes de carregar a cena
        if (PlayerHealth.Instance != null)
            PlayerHealth.Instance.transform.position = Vector3.zero;

        SceneManager.LoadScene("StartRoom");
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