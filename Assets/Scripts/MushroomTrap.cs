using UnityEngine;

/// <summary>
/// Cogumelo estático que dispara esporos em burst circular.
/// Fica inativo até o player entrar no raio de detecção.
/// </summary>
public class MushroomTrap : MonoBehaviour
{
    [Header("Campo de Visão")]
    [Tooltip("Distância em que o cogumelo começa a disparar")]
    public float detectionRadius = 5f;

    [Header("Esporos")]
    public GameObject projectilePrefab;
    public int   sporeCount          = 6;
    public float shootInterval       = 2f;
    public float projectileSpeed     = 3.5f;
    public float projectileLifetime  = 2f;
    public int   sporeDamage         = 1;

    private float      shootTimer;
    private Transform  player;
    private bool       isActive = false;

    void Start()
    {
        shootTimer = Random.Range(0f, shootInterval);

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }

    void Update()
    {
        if (player == null) return;

        // ── Acorda quando o player se aproxima ───────────────────────────────
        if (!isActive)
        {
            if (Vector2.Distance(transform.position, player.position) <= detectionRadius)
                isActive = true;
            else
                return;
        }

        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f)
        {
            ShootSpores();
            shootTimer = shootInterval;
        }
    }

    void ShootSpores()
    {
        if (projectilePrefab == null) return;

        float angleStep = 360f / sporeCount;

        for (int i = 0; i < sporeCount; i++)
        {
            float   angle     = i * angleStep;
            Vector2 dir       = new Vector2(
                Mathf.Cos(angle * Mathf.Deg2Rad),
                Mathf.Sin(angle * Mathf.Deg2Rad)
            );
            Vector2 spawnPos  = (Vector2)transform.position + dir * 0.5f;
            float   projAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            GameObject proj = Instantiate(projectilePrefab, spawnPos, Quaternion.Euler(0, 0, projAngle));
            Projectile p    = proj.GetComponent<Projectile>();
            if (p != null)
            {
                p.isPlayerProjectile = false;
                p.damage             = sporeDamage;
                p.speed              = projectileSpeed;
                p.lifetime           = projectileLifetime;
                p.SetDirection(dir);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.2f, 0.8f, 0.2f, 0.2f);
        Gizmos.DrawSphere(transform.position, detectionRadius);
        Gizmos.color = new Color(0.2f, 0.8f, 0.2f, 0.9f);
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
