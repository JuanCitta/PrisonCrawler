using UnityEngine;

/// <summary>
/// Cyclops — inimigo do Castelo.
/// Quase estático, dispara um projétil com leve homming no player.
/// Projétil corrige a direção ao longo do tempo.
/// </summary>
public class CyclopsAI : MonoBehaviour
{
    [Header("Campo de visão")]
    public float detectionRadius = 8f;

    [Header("Disparo")]
    public GameObject projectilePrefab;
    public float shootInterval      = 2.5f;
    public float projectileSpeed    = 4f;
    public int   projectileDamage   = 2;
    public float projectileLifetime = 3.3f;

    [Header("Movimento mínimo")]
    public float moveSpeed = 0.4f;   // anda muito devagar, quase estático

    private Transform     player;
    private Rigidbody2D   rb;
    private CombatManager combatManager;
    private float         shootTimer;
    private bool          isActive = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null) { rb.gravityScale = 0f; rb.freezeRotation = true; }

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;

        combatManager = CombatManager.Instance;
        shootTimer    = Random.Range(0f, shootInterval);
    }

    void Update()
    {
        if (player == null) return;

        if (!isActive)
        {
            if (Vector2.Distance(transform.position, player.position) <= detectionRadius)
                isActive = true;
            else return;
        }

        // Movimento muito lento de recuo/avanço para não ficar completamente parado
        Vector2 dir = ((Vector2)player.position - (Vector2)transform.position).normalized;
        if (rb != null) rb.linearVelocity = dir * moveSpeed;

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

        Vector2 dir      = ((Vector2)player.position - (Vector2)transform.position).normalized;
        Vector2 spawnPos = (Vector2)transform.position + dir * 0.6f;

        GameObject proj = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
        Projectile p    = proj.GetComponent<Projectile>();
        if (p != null)
        {
            p.speed    = projectileSpeed;
            p.damage   = projectileDamage;
            p.lifetime = projectileLifetime;
            p.SetDirection(dir);
        }

        // Adiciona homming ao projétil
        proj.AddComponent<HomingProjectile>().target = player;
    }

    void OnDestroy()
    {
        if (combatManager != null && gameObject.scene.isLoaded)
            combatManager.OnEnemyKilled();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.6f, 0f, 0.3f);
        Gizmos.DrawSphere(transform.position, detectionRadius);
    }
}
