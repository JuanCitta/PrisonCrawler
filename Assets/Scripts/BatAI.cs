using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BatAI : MonoBehaviour
{
    [Header("Movimento")]
    public float moveSpeed    = 3.5f;
    public float fleeSpeed    = 5f;
    public float fleeDuration = 1f;

    [Header("Campo de Visão")]
    [Tooltip("Morcegos têm visão mais ampla — detectam o player de longe")]
    public float detectionRadius = 8f;

    [Header("Zigue-Zague")]
    public float zigzagFrequency = 4f;   // oscilações por segundo
    public float zigzagAmplitude = 5.5f; // intensidade lateral

    [Header("Dano por contato")]
    public int contactDamage = 1;

    private Transform   player;
    private Rigidbody2D rb;
    private CombatManager combatManager;

    private enum State { Idle, Attacking, Fleeing }
    private State state    = State.Idle;
    private bool  canDamage = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale   = 0f;
        rb.freezeRotation = true;

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;

        combatManager = CombatManager.Instance;
    }

    void Update()
    {
        if (player == null) return;

        switch (state)
        {
            case State.Idle:
                // Acorda quando o player entra no raio de detecção
                if (Vector2.Distance(transform.position, player.position) <= detectionRadius)
                    state = State.Attacking;
                else
                    rb.linearVelocity = Vector2.zero;
                break;

            case State.Attacking:
                MoveAttacking();
                break;

            case State.Fleeing:
                // Controlado pela coroutine
                break;
        }
    }

    void MoveAttacking()
    {
        Vector2 dir  = ((Vector2)player.position - (Vector2)transform.position).normalized;
        Vector2 perp = new Vector2(-dir.y, dir.x);
        float   wave = Mathf.Sin(Time.time * zigzagFrequency) * zigzagAmplitude;

        rb.linearVelocity = dir * moveSpeed + perp * wave;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (!canDamage || state == State.Fleeing || state == State.Idle) return;

        PlayerHealth.Instance?.TakeDamage(contactDamage);
        StartCoroutine(FleeRoutine());
    }

    IEnumerator FleeRoutine()
    {
        canDamage = false;
        state     = State.Fleeing;

        float timer = 0f;
        while (timer < fleeDuration)
        {
            if (player != null)
            {
                Vector2 away = ((Vector2)transform.position - (Vector2)player.position).normalized;
                rb.linearVelocity = away * fleeSpeed;
            }
            timer += Time.deltaTime;
            yield return null;
        }

        rb.linearVelocity = Vector2.zero;
        state     = State.Attacking;
        canDamage = true;
    }

    void OnDestroy()
    {
        if (combatManager != null && gameObject.scene.isLoaded)
            combatManager.OnEnemyKilled();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.6f, 0.2f, 1f, 0.25f);
        Gizmos.DrawSphere(transform.position, detectionRadius);
        Gizmos.color = new Color(0.6f, 0.2f, 1f, 0.9f);
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
