using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    // Arma atualmente equipada (null = sem arma, não pode atirar)
    public WeaponData equippedWeapon;

    /// <summary>Modificado pelo PlayerAbility durante o RapidFire (1 = normal, 0.3 = 3x mais rápido).</summary>
    [HideInInspector] public float shootCooldownMultiplier = 1f;

    /// <summary>Acumulado pelas recompensas de quest Archer (+20% por quest completa).</summary>
    [HideInInspector] public float projectileSpeedMultiplier = 1f;

    /// <summary>Reduzido pela Forja (-20% por melhoria). 1 = normal, 0.8 = 20% mais rápido.</summary>
    [HideInInspector] public float forgeCooldownMultiplier = 1f;

    private float cooldownTimer = 0f;
    private Camera cam;

    void OnEnable()
    {
        // Re-busca a câmera ao entrar em nova cena (o Player persiste via DontDestroyOnLoad)
        cam = Camera.main;
    }

    void Update()
    {
        cooldownTimer -= Time.deltaTime;

        if (Mouse.current.leftButton.wasPressedThisFrame && cooldownTimer <= 0f)
        {
            if (equippedWeapon == null) return;   // sem arma = sem tiro
            Shoot();
            cooldownTimer = equippedWeapon.shootCooldown * shootCooldownMultiplier * forgeCooldownMultiplier;
        }
    }

    /// <summary>Equipa uma arma (chamado pelo WeaponPickup ao coletar).</summary>
    public void Equip(WeaponData weapon)
    {
        equippedWeapon = weapon;
        Debug.Log($"[PlayerShoot] Arma equipada: {weapon.weaponName}");
    }

    /// <summary>Reseta os multiplicadores da run ao morrer.</summary>
    public void ResetRunStats()
    {
        projectileSpeedMultiplier = 1f;
        shootCooldownMultiplier   = 1f;
        forgeCooldownMultiplier   = 1f;
        equippedWeapon            = null;
    }

    void Shoot()
    {
        if (cam == null) cam = Camera.main;
        if (cam == null) return;

        Vector2 mouseWorld = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 dir        = (mouseWorld - (Vector2)transform.position).normalized;
        Vector2 spawnPos   = (Vector2)transform.position + dir * 0.5f;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;


        GameObject proj = Instantiate(equippedWeapon.projectilePrefab, spawnPos, Quaternion.Euler(0, 0, angle));

        Projectile p = proj.GetComponent<Projectile>();
        if (p != null)
        {
            p.isPlayerProjectile = true;
            p.damage             = equippedWeapon.damage;
            p.speed              = equippedWeapon.projectileSpeed * projectileSpeedMultiplier;
            p.lifetime           = equippedWeapon.projectileLifetime;
            p.SetDirection(dir);
        }
    }
}
