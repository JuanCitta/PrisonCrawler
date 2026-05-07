using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyAI : MonoBehaviour
{
    [Header("Movimento")]
    public float moveSpeed = 2f;
    public float stopDistance = 2f;

    [Header("Disparo")]
    public GameObject projectilePrefab;
    public float shootInterval = 2f;
    public float projectileSpeed = 5f;

    private Transform player;
    private Rigidbody2D rb;
    private float shootTimer;
    private CombatManager combatManager;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;

        combatManager = FindObjectOfType<CombatManager>();

        // Stagger inicial para os inimigos não dispararem todos ao mesmo tempo
        shootTimer = Random.Range(0f, shootInterval);
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > stopDistance)
        {
            Vector2 dir = ((Vector2)player.position - (Vector2)transform.position).normalized;
            rb.linearVelocity = dir * moveSpeed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }

        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f)
        {
            Shoot();
            shootTimer = shootInterval;
        }
    }

    void Shoot()
    {
        if (projectilePrefab == null || player == null) return;

        Vector2 dir = ((Vector2)player.position - (Vector2)transform.position).normalized;
        GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        Projectile projectile = proj.GetComponent<Projectile>();
        if (projectile != null)
        {
            projectile.speed = projectileSpeed;
            projectile.SetDirection(dir);
        }
    }

    void OnDestroy()
    {
        // Notifica o CombatManager apenas se o jogo ainda está rodando (não ao reiniciar)
        if (combatManager != null && gameObject.scene.isLoaded)
        {
            combatManager.OnEnemyKilled();
        }
    }
}
