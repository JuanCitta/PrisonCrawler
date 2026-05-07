using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 2;
    private int currentHealth;
    private DamageFlash damageFlash;

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
            Destroy(gameObject);
    }
}
