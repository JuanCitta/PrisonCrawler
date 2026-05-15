using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAbility : MonoBehaviour
{
    // Habilidade atualmente equipada (null = sem habilidade)
    public AbilityData equippedAbility;

    /// <summary>Acumulado pelas recompensas de quest Mage (reduz cooldown 20% por quest).</summary>
    [HideInInspector] public float cooldownMultiplier = 1f;

    private float      cooldownTimer;
    private PlayerShoot playerShoot;
    private Camera     cam;

    void Awake()
    {
        playerShoot = GetComponent<PlayerShoot>();
    }

    void OnEnable()
    {
        cam = Camera.main;
    }

    void Update()
    {
        cooldownTimer -= Time.deltaTime;

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (equippedAbility == null) return;
            if (cooldownTimer > 0f)
            {
                Debug.Log($"[PlayerAbility] Cooldown restante: {cooldownTimer:F1}s");
                return;
            }
            ActivateAbility();
            cooldownTimer = equippedAbility.abilityCooldown * cooldownMultiplier;
        }
    }

    /// <summary>Equipa uma habilidade (chamado pelo AbilityPickup).</summary>
    public void Equip(AbilityData ability)
    {
        equippedAbility = ability;
        cooldownTimer   = 0f;
        Debug.Log($"[PlayerAbility] Habilidade equipada: {ability.abilityName}");
    }

    /// <summary>Reseta os multiplicadores da run ao morrer.</summary>
    public void ResetRunStats()
    {
        cooldownMultiplier = 1f;
        equippedAbility    = null;
        cooldownTimer      = 0f;
    }

    void ActivateAbility()
    {
        switch (equippedAbility.abilityType)
        {
            case AbilityType.RapidFire:
                StartCoroutine(RapidFireRoutine());
                break;

            case AbilityType.SpellBomb:
                SpawnSpellBomb();
                break;
        }
    }

    // ── RapidFire ──────────────────────────────────────────────────────────────
    IEnumerator RapidFireRoutine()
    {
        playerShoot.shootCooldownMultiplier = equippedAbility.rapidFireCooldownMultiplier;
        Debug.Log($"[PlayerAbility] RapidFire ativo por {equippedAbility.rapidFireDuration}s");

        yield return new WaitForSeconds(equippedAbility.rapidFireDuration);

        playerShoot.shootCooldownMultiplier = 1f;
        Debug.Log("[PlayerAbility] RapidFire encerrado");
    }

    // ── SpellBomb ──────────────────────────────────────────────────────────────
    void SpawnSpellBomb()
    {
        if (cam == null) cam = Camera.main;
        if (cam == null) return;
        if (equippedAbility.spellBombProjectilePrefab == null)
        {
            Debug.LogWarning("[PlayerAbility] SpellBomb: projectilePrefab não atribuído!");
            return;
        }

        // Centro da explosão = posição do mouse no mundo
        Vector2 center = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        int count = equippedAbility.spellBombProjectileCount;

        for (int i = 0; i < count; i++)
        {
            float   angle = i * 360f / count;
            Vector2 dir   = new Vector2(
                Mathf.Cos(angle * Mathf.Deg2Rad),
                Mathf.Sin(angle * Mathf.Deg2Rad)
            );

            float   projAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            GameObject proj   = Instantiate(
                equippedAbility.spellBombProjectilePrefab,
                center,
                Quaternion.Euler(0, 0, projAngle)
            );

            Projectile p = proj.GetComponent<Projectile>();
            if (p != null)
            {
                p.isPlayerProjectile = true;
                p.damage             = equippedAbility.spellBombDamage;
                p.speed              = equippedAbility.spellBombProjectileSpeed;
                p.lifetime           = equippedAbility.spellBombProjectileLifetime;
                p.SetDirection(dir);
            }
        }
    }
}
