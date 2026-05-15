using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 2;
    private int currentHealth;
    public float HealthRatio => maxHealth > 0 ? (float)currentHealth / maxHealth : 0f;
    private DamageFlash damageFlash;
    public System.Action onDeath;

    void Start()
    {
        currentHealth = maxHealth;
        damageFlash   = GetComponent<DamageFlash>();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        damageFlash?.Flash();

        if (currentHealth <= 0)
        {
            onDeath?.Invoke();
            Destroy(gameObject);
        }
    }
}
