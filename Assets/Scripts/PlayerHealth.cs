using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance;

    // Disparado sempre que as vidas mudam — a HealthUI escuta este evento
    public static event System.Action<int> OnHealthChanged;

    public int maxLives    = 5;
    public int currentLives;

    private DamageFlash damageFlash;

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

    void Start()
    {
        damageFlash  = GetComponent<DamageFlash>();
        currentLives = maxLives;
        OnHealthChanged?.Invoke(currentLives);
    }

    /// <summary>Recupera vidas, sem ultrapassar o máximo.</summary>
    public void Heal(int amount)
    {
        currentLives = Mathf.Min(currentLives + amount, maxLives);
        OnHealthChanged?.Invoke(currentLives);
    }

    public void TakeDamage(int amount)
    {
        currentLives -= amount;
        OnHealthChanged?.Invoke(currentLives);
        damageFlash?.Flash();

        if (currentLives <= 0)
            Die();
    }

    void Die()
    {
        currentLives = maxLives;
        OnHealthChanged?.Invoke(currentLives);
        GameManager.Instance.ResetGame();
    }
}
