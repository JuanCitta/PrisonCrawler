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

            // Garante sempre estado limpo ao iniciar
            currentFloor = 0;
            currentBiome = BiomeType.Cave;
            currentNPC   = null;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadRoom(RoomType type, string npcId = null)
    {
        switch (type)
        {
            case RoomType.Combat:
                currentFloor++;   // só salas de combate avançam o contador
                UpdateBiome();
                currentNPC = npcId;
                SceneManager.LoadScene("CombatRoom");
                break;

            case RoomType.Camp:
                currentNPC = null;
                SceneManager.LoadScene("CampRoom");
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
        QuestManager.Instance?.Reset();
        InventoryManager.Instance?.Reset();

        if (PlayerHealth.Instance != null)
        {
            PlayerHealth.Instance.transform.position = Vector3.zero;
            PlayerHealth.Instance.GetComponent<PlayerShoot>()?.ResetRunStats();
            PlayerHealth.Instance.GetComponent<PlayerAbility>()?.ResetRunStats();
        }

        Instance = null; // limpa a referência estática
        SceneManager.LoadScene("Menu");
        Destroy(gameObject); // destrói depois de carregar
    }

    void UpdateBiome()
    {
        if (currentFloor <= 8)
            currentBiome = BiomeType.Cave;
        else
            currentBiome = BiomeType.Castle;
    }
}