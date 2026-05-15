using System.Collections;
using UnityEngine;

/// <summary>
/// Boss final — Tengu. Floor 16 (8ª sala do Castelo).
///
/// Fase 1 (HP > 50%):
///   - Move em direção ao player
///   - A cada 3s faz dash + dispara leque de 5 projéteis
///
/// Fase 2 (HP <= 50%):
///   - Mais rápido
///   - Dano por contato
///   - Dash mais frequente + leque de 8 projéteis
///   - Sprite fica avermelhado (DamageFlash ativado manualmente)
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(EnemyHealth))]
public class TenguBoss : MonoBehaviour
{
    [Header("Movimento")]
    public float moveSpeedP1 = 2.5f;
    public float moveSpeedP2 = 4f;

    [Header("Dash")]
    public float dashSpeed     = 10f;
    public float dashDuration  = 0.25f;
    public float dashCooldownP1 = 3f;
    public float dashCooldownP2 = 1.8f;

    [Header("Projéteis")]
    public GameObject projectilePrefab;
    public float projectileSpeed    = 6f;
    public int   projectileDamage   = 1;
    public float projectileLifetime = 3f;
    public float spreadAngleP1      = 20f;   // ângulo entre projéteis fase 1
    public float spreadAngleP2      = 15f;   // ângulo entre projéteis fase 2

    [Header("Contato (Fase 2)")]
    public int contactDamage = 1;

    private Transform     player;
    private Rigidbody2D   rb;
    private EnemyHealth   health;
    private CombatManager combatManager;
    private DamageFlash   damageFlash;
    private SpriteRenderer sr;

    private bool  isDashing   = false;
    private bool  inPhase2    = false;
    private bool  canContact  = true;
    private float dashTimer;

    void Start()
    {
        rb          = GetComponent<Rigidbody2D>();
        health      = GetComponent<EnemyHealth>();
        damageFlash = GetComponent<DamageFlash>();
        sr          = GetComponent<SpriteRenderer>();

        rb.gravityScale   = 0f;
        rb.freezeRotation = true;

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;

        combatManager = CombatManager.Instance;
        dashTimer     = 1.5f; // pequeno delay antes do primeiro dash
    }

    void Update()
    {
        if (player == null || isDashing) return;

        // ── Verifica transição de fase ───────────────────────────────────────
        if (!inPhase2 && health.HealthRatio <= 0.5f)
            EnterPhase2();

        // ── Movimento ────────────────────────────────────────────────────────
        float speed = inPhase2 ? moveSpeedP2 : moveSpeedP1;
        Vector2 dir = ((Vector2)player.position - (Vector2)transform.position).normalized;
        rb.linearVelocity = dir * speed;

        // ── Cooldown do dash ─────────────────────────────────────────────────
        dashTimer -= Time.deltaTime;
        float cooldown = inPhase2 ? dashCooldownP2 : dashCooldownP1;
        if (dashTimer <= 0f)
        {
            StartCoroutine(DashAndShoot());
            dashTimer = cooldown;
        }
    }

    IEnumerator DashAndShoot()
    {
        isDashing = true;

        // Dash em direção ao player
        if (player != null)
        {
            Vector2 dir = ((Vector2)player.position - (Vector2)transform.position).normalized;
            rb.linearVelocity = dir * dashSpeed;
        }

        yield return new WaitForSeconds(dashDuration);

        rb.linearVelocity = Vector2.zero;
        isDashing = false;

        // Dispara leque após o dash
        if (player != null)
        {
            Vector2 shootDir = ((Vector2)player.position - (Vector2)transform.position).normalized;
            ShootSpread(shootDir);
        }
    }

    void ShootSpread(Vector2 baseDir)
    {
        if (projectilePrefab == null) return;

        int   count       = inPhase2 ? 8 : 5;
        float spreadAngle = inPhase2 ? spreadAngleP2 : spreadAngleP1;
        float totalSpread = spreadAngle * (count - 1);
        float startAngle  = -totalSpread / 2f;

        for (int i = 0; i < count; i++)
        {
            float   angle    = startAngle + spreadAngle * i;
            Vector2 dir      = Rotate(baseDir, angle);
            Vector2 spawnPos = (Vector2)transform.position + dir * 0.7f;

            GameObject proj = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
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

    void EnterPhase2()
    {
        inPhase2 = true;

        // Tinge o sprite de vermelho escuro para sinalizar a fase 2
        if (sr != null)
            sr.color = new Color(1f, 0.3f, 0.3f);

        Debug.Log("[Tengu] Fase 2 ativada!");
    }

    // ── Dano por contato (Fase 2) ────────────────────────────────────────────

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!inPhase2 || !canContact) return;
        if (!other.CompareTag("Player")) return;

        PlayerHealth.Instance?.TakeDamage(contactDamage);
        StartCoroutine(ContactCooldown());
    }

    IEnumerator ContactCooldown()
    {
        canContact = false;
        yield return new WaitForSeconds(0.6f);
        canContact = true;
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
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1f);
    }
}
