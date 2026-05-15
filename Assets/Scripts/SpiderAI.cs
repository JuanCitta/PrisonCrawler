using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SpiderAI : MonoBehaviour
{
    [Header("Movimento")]
    public float moveSpeed    = 2f;
    public float stopDistance = 2f;

    [Header("Campo de Visão")]
    [Tooltip("Distância em que o inimigo detecta o player e acorda")]
    public float detectionRadius = 6f;

    [Header("Disparo")]
    public GameObject projectilePrefab;
    public float shootInterval   = 1.2f;
    public float projectileSpeed = 5f;
    public int   projectileDamage   = 1;
    public float projectileLifetime = 3f;

    [Header("Separação")]
    public float separationRadius = 1.2f;
    public float separationForce  = 3f;

    private Transform player;
    private Rigidbody2D rb;
    private float shootTimer;
    private CombatManager combatManager;
    private Animator animator;
    private Vector2 lastMoveDir;

    // Começa inativo até o player entrar no raio de detecção
    private bool isActive = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;

        combatManager = CombatManager.Instance;

        // Stagger inicial para os inimigos não dispararem todos ao mesmo tempo
        shootTimer = Random.Range(0f, shootInterval);
    }

    void Update()
    {
        if (player == null) return;

        // ── Verificação de campo de visão ────────────────────────────────────
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
        float distance   = Vector2.Distance(transform.position, player.position);
        Vector2 separation = GetSeparationForce();

        if (distance > stopDistance)
        {
            Vector2 dir = ((Vector2)player.position - (Vector2)transform.position).normalized;
            rb.linearVelocity = dir * moveSpeed + separation * separationForce;
            Vector2 velocity  = rb.linearVelocity;

            if (velocity.magnitude > 0.1f)
            {
                Vector2 moveDir = velocity.normalized;
                animator.SetBool("isWalking", true);
                animator.SetFloat("InputX",    moveDir.x);
                animator.SetFloat("InputY",    moveDir.y);
                lastMoveDir = moveDir;
                animator.SetFloat("LastInputX", lastMoveDir.x);
                animator.SetFloat("LastInputY", lastMoveDir.y);
            }
            else
            {
                animator.SetBool("isWalking", false);
                animator.SetFloat("LastInputX", lastMoveDir.x);
                animator.SetFloat("LastInputY", lastMoveDir.y);
            }
        }
        else
        {
            rb.linearVelocity = separation * separationForce;
            animator.SetBool("isWalking", false);
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

        Vector2 dir      = ((Vector2)player.position - (Vector2)transform.position).normalized;
        Vector2 spawnPos = (Vector2)transform.position + dir * 0.8f;
        GameObject proj  = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);

        Projectile projectile = proj.GetComponent<Projectile>();
        if (projectile != null)
        {
            projectile.speed    = projectileSpeed;
            projectile.damage   = projectileDamage;
            projectile.lifetime = projectileLifetime;
            projectile.SetDirection(dir);
        }
    }

    Vector2 GetSeparationForce()
    {
        Vector2 force    = Vector2.zero;
        Collider2D[] nearby = Physics2D.OverlapCircleAll(transform.position, separationRadius);

        foreach (var col in nearby)
        {
            if (col.gameObject == gameObject) continue;
            if (col.GetComponent<SpiderAI>() == null) continue;

            Vector2 diff = (Vector2)transform.position - (Vector2)col.transform.position;
            if (diff.sqrMagnitude > 0f)
                force += diff.normalized / diff.magnitude;
        }

        return force;
    }

    void OnDestroy()
    {
        if (combatManager != null && gameObject.scene.isLoaded)
            combatManager.OnEnemyKilled();
    }

    // Mostra o raio de detecção no Editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.9f, 0.1f, 0.3f);
        Gizmos.DrawSphere(transform.position, detectionRadius);
        Gizmos.color = new Color(1f, 0.9f, 0.1f, 0.9f);
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
