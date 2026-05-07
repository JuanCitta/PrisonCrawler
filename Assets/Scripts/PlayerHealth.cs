using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance;

    // Disparado sempre que as vidas mudam — a HealthUI escuta este evento
    public static event System.Action<int> OnHealthChanged;

    public int maxLives = 5;
    public int currentLives;

    [Header("Iframes")]
    public float invincibilityDuration = 0.5f;
    private bool isInvincible = false;

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
        damageFlash = GetComponent<DamageFlash>();
        currentLives = maxLives;
        OnHealthChanged?.Invoke(currentLives);
    }

    public void TakeDamage(int amount)
    {
        if (isInvincible) return;

        currentLives -= amount;
        OnHealthChanged?.Invoke(currentLives);
        damageFlash?.Flash();

        if (currentLives <= 0)
            Die();
        else
            StartCoroutine(InvincibilityRoutine());
    }

    IEnumerator InvincibilityRoutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration);
        isInvincible = false;
    }

    void Die()
    {
        currentLives = maxLives;
        OnHealthChanged?.Invoke(currentLives);
        GameManager.Instance.ResetGame();
    }
}
