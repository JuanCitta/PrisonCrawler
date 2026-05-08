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

    [Header("Separação")]
    public float separationRadius = 1.2f;
    public float separationForce  = 3f;

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

        Vector2 separation = GetSeparationForce();

        if (distance > stopDistance)
        {
            Vector2 dir = ((Vector2)player.position - (Vector2)transform.position).normalized;
            rb.linearVelocity = dir * moveSpeed + separation * separationForce;
        }
        else
        {
            rb.linearVelocity = separation * separationForce;
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
        // Spawna levemente à frente do inimigo para não colidir com o próprio collider
        Vector2 spawnPos = (Vector2)transform.position + dir * 0.8f;
        GameObject proj = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);

        Projectile projectile = proj.GetComponent<Projectile>();
        if (projectile != null)
        {
            projectile.speed = projectileSpeed;
            projectile.SetDirection(dir);
        }
    }

    Vector2 GetSeparationForce()
    {
        Vector2 force = Vector2.zero;
        Collider2D[] nearby = Physics2D.OverlapCircleAll(transform.position, separationRadius);

        foreach (var col in nearby)
        {
            if (col.gameObject == gameObject) continue;
            if (col.GetComponent<EnemyAI>() == null) continue;

            Vector2 diff = (Vector2)transform.position - (Vector2)col.transform.position;
            if (diff.sqrMagnitude > 0f)
                force += diff.normalized / diff.magnitude;
        }

        return force;
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
