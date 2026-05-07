using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance;

    public int maxLives = 5;
    public int currentLives;

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
        currentLives = maxLives;
    }

    public void TakeDamage(int amount)
    {
        currentLives -= amount;
        Debug.Log($"Player HP: {currentLives}/{maxLives}");

        if (currentLives <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Game Over — Reiniciando...");
        currentLives = maxLives;
        GameManager.Instance.ResetGame();
    }
}
