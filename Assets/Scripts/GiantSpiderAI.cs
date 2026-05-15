using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GiantSpiderAI : MonoBehaviour
{
    [Header("Movimento")]
    public float moveSpeed    = 1.2f;
    public float stopDistance = 2.5f;

    [Header("Campo de Visão")]
    [Tooltip("Boss detecta o player de longe — quase sem aviso")]
    public float detectionRadius = 10f;

    [Header("Disparo em leque")]
    public GameObject projectilePrefab;
    public float shootInterval      = 1.8f;
    public float projectileSpeed    = 4.5f;
    public int   projectileDamage   = 1;
    public float projectileLifetime = 3.3f;
    [Tooltip("Ângulo entre cada projétil do leque")]
    public float spreadAngle = 20f;

    [Header("Filhotes ao morrer")]
    public GameObject spiderPrefab;
    public int        spawnCount = 3;

    private Transform     player;
    private Rigidbody2D   rb;
    private float         shootTimer;
    private CombatManager combatManager;
    private bool          isActive = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale   = 0f;
        rb.freezeRotation = true;

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;

        combatManager = CombatManager.Instance;
        shootTimer    = Random.Range(0f, shootInterval);

        EnemyHealth health = GetComponent<EnemyHealth>();
        if (health != null) health.onDeath += SpawnBabies;
    }

    void Update()
    {
        if (player == null) return;

        // ── Campo de visão ───────────────────────────────────────────────────
        if (!isActive)
        {
            if (Vector2.Distance(transform.position, player.position) <= detectionRadius)
                isActive = true;
            else
            {
                rb.linearVelocity = Vector2.zero;
                return;
            }
        }

        // ── Comportamento ativo ──────────────────────────────────────────────
        float   distance = Vector2.Distance(transform.position, player.position);
        Vector2 dir      = ((Vector2)player.position - (Vector2)transform.position).normalized;

        rb.linearVelocity = distance > stopDistance ? dir * moveSpeed : Vector2.zero;

        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f)
        {
            ShootSpread(dir);
            shootTimer = shootInterval;
        }
    }

    void ShootSpread(Vector2 baseDir)
    {
        if (projectilePrefab == null) return;

        float[] angles = { 0f, spreadAngle, -spreadAngle };
        foreach (float offset in angles)
        {
            Vector2 dir      = Rotate(baseDir, offset);
            Vector2 spawnPos = (Vector2)transform.position + dir * 0.9f;
            float   angle    = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            GameObject proj = Instantiate(projectilePrefab, spawnPos, Quaternion.Euler(0, 0, angle));
            Projectile p    = proj.GetComponent<Projectile>();
            if (p != null)
            {
                p.speed    = projectileSpeed;
                p.damage   = projectileDamage;
                p.lifetime = projectileLifetime;
                p.SetDirection(dir);
            }
        }
    }

    void SpawnBabies()
    {
        if (spiderPrefab == null)
        {
            Debug.LogWarning("[GiantSpiderAI] Spider Prefab não atribuído no Inspector! Filhotes não vão spawnar.", this);
            return;
        }

        for (int i = 0; i < spawnCount; i++)
        {
            combatManager?.AddEnemy();
            Vector2 offset = Random.insideUnitCircle * 0.6f;
            Instantiate(spiderPrefab, (Vector2)transform.position + offset, Quaternion.identity);
        }

        Debug.Log($"[GiantSpiderAI] Spawnados {spawnCount} filhotes.");
    }

    void OnDestroy()
    {
        if (combatManager != null && gameObject.scene.isLoaded)
            combatManager.OnEnemyKilled();
    }

    static Vector2 Rotate(Vector2 v, float degrees)
    {
        float rad = degrees * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad), sin = Mathf.Sin(rad);
        return new Vector2(v.x * cos - v.y * sin, v.x * sin + v.y * cos);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.2f, 0.1f, 0.2f);
        Gizmos.DrawSphere(transform.position, detectionRadius);
        Gizmos.color = new Color(1f, 0.2f, 0.1f, 0.9f);
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
