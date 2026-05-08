using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    // Arma atualmente equipada (null = sem arma, não pode atirar)
    public WeaponData equippedWeapon;

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
            cooldownTimer = equippedWeapon.shootCooldown;
        }
    }

    /// <summary>Equipa uma arma (chamado pelo WeaponPickup ao coletar).</summary>
    public void Equip(WeaponData weapon)
    {
        equippedWeapon = weapon;
        Debug.Log($"[PlayerShoot] Arma equipada: {weapon.weaponName}");
    }

    void Shoot()
    {
        if (cam == null) cam = Camera.main;
        if (cam == null) return;

        Vector2 mouseWorld = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 dir        = (mouseWorld - (Vector2)transform.position).normalized;
        Vector2 spawnPos   = (Vector2)transform.position + dir * 0.5f;

        GameObject proj = Instantiate(equippedWeapon.projectilePrefab, spawnPos, Quaternion.identity);

        Projectile p = proj.GetComponent<Projectile>();
        if (p != null)
        {
            p.isPlayerProjectile = true;
            p.damage             = equippedWeapon.damage;
            p.speed              = equippedWeapon.projectileSpeed;
            p.lifetime           = equippedWeapon.projectileLifetime;
            p.SetDirection(dir);
        }
    }
}
