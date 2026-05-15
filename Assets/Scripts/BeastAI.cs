using System.Collections;
using UnityEngine;

/// <summary>
/// Beast — inimigo do Castelo.
/// Anda lentamente em direção ao player.
/// Periodicamente faz um dash rápido causando dano alto por contato.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class BeastAI : MonoBehaviour
{
    [Header("Movimento")]
    public float moveSpeed       = 1.2f;
    public float detectionRadius = 7f;

    [Header("Dash")]
    public float dashSpeed     = 9f;
    public float dashDuration  = 0.2f;
    public float dashCooldown  = 2.5f;

    [Header("Dano")]
    public int contactDamage = 1;
    public int dashDamage    = 2;

    private Transform     player;
    private Rigidbody2D   rb;
    private CombatManager combatManager;
    private bool          isActive   = false;
    private bool          isDashing  = false;
    private bool          canDamage  = true;
    private float         dashTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale   = 0f;
        rb.freezeRotation = true;

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;

        combatManager = CombatManager.Instance;
        dashTimer     = dashCooldown;
    }

    void Update()
    {
        if (player == null || isDashing) return;

        if (!isActive)
        {
            if (Vector2.Distance(transform.position, player.position) <= detectionRadius)
                isActive = true;
            else { rb.linearVelocity = Vector2.zero; return; }
        }

        // Movimento lento normal
        Vector2 dir = ((Vector2)player.position - (Vector2)transform.position).normalized;
        rb.linearVelocity = dir * moveSpeed;

        // Conta o cooldown do dash
        dashTimer -= Time.deltaTime;
        if (dashTimer <= 0f)
        {
            StartCoroutine(DashRoutine());
            dashTimer = dashCooldown;
        }
    }

    IEnumerator DashRoutine()
    {
        isDashing = true;
        canDamage = true;

        Vector2 dir = ((Vector2)player.position - (Vector2)transform.position).normalized;
        rb.linearVelocity = dir * dashSpeed;

        yield return new WaitForSeconds(dashDuration);

        rb.linearVelocity = Vector2.zero;
        isDashing         = false;

        // Cooldown para não dar dano múltiplo imediato
        yield return new WaitForSeconds(0.5f);
        canDamage = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || !canDamage || !isActive) return;

        int dmg = isDashing ? dashDamage : contactDamage;
        PlayerHealth.Instance?.TakeDamage(dmg);
        canDamage = false;
    }

    void OnDestroy()
    {
        if (combatManager != null && gameObject.scene.isLoaded)
            combatManager.OnEnemyKilled();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.2f, 0.2f, 0.3f);
        Gizmos.DrawSphere(transform.position, detectionRadius);
    }
}
