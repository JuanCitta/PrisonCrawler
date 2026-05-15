using UnityEngine;

/// <summary>
/// Spirit — inimigo do Castelo.
/// Flutua suavemente em direção ao player.
/// Quando leva dano, teleporta para uma posição aleatória da sala.
/// Dano por contato.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class SpiritAI : MonoBehaviour
{
    [Header("Movimento")]
    public float moveSpeed       = 2.5f;
    public float detectionRadius = 10f;

    [Header("Dano por contato")]
    public int contactDamage = 1;

    [Header("Teleporte ao levar dano")]
    public float teleportRangeX = 5f;
    public float teleportRangeY = 3f;

    private Transform     player;
    private Rigidbody2D   rb;
    private CombatManager combatManager;
    private bool          isActive  = false;
    private bool          canDamage = true;
    private EnemyHealth   health;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale   = 0f;
        rb.freezeRotation = true;

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;

        combatManager = CombatManager.Instance;

        // Subscreeve ao evento de dano para fazer o teleporte
        health = GetComponent<EnemyHealth>();
        if (health != null) health.onDeath += () => { }; // placeholder
        // Usamos OnTriggerEnter2D para dano e um componente de flash para detetar hit
    }

    void Update()
    {
        if (player == null) return;

        if (!isActive)
        {
            if (Vector2.Distance(transform.position, player.position) <= detectionRadius)
                isActive = true;
            else { rb.linearVelocity = Vector2.zero; return; }
        }

        // Movimento suave em direção ao player
        Vector2 dir = ((Vector2)player.position - (Vector2)transform.position).normalized;
        rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, dir * moveSpeed, Time.deltaTime * 4f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || !canDamage || !isActive) return;

        PlayerHealth.Instance?.TakeDamage(contactDamage);
        StartCoroutine(DamageCooldown());
    }

    System.Collections.IEnumerator DamageCooldown()
    {
        canDamage = false;
        yield return new WaitForSeconds(0.8f);
        canDamage = true;
    }

    /// <summary>Chamado pelo DamageFlash ou por quem aplicar dano — teleporta o spirit.</summary>
    public void OnHit()
    {
        float x = Random.Range(-teleportRangeX, teleportRangeX);
        float y = Random.Range(-teleportRangeY, teleportRangeY);
        transform.position = new Vector2(x, y);
        rb.linearVelocity  = Vector2.zero;
    }

    void OnDestroy()
    {
        if (combatManager != null && gameObject.scene.isLoaded)
            combatManager.OnEnemyKilled();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.8f, 0.8f, 1f, 0.3f);
        Gizmos.DrawSphere(transform.position, detectionRadius);
    }
}
