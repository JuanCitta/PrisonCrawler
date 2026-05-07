using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 8f;
    public float shootCooldown = 0.25f;

    private float cooldownTimer = 0f;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        cooldownTimer -= Time.deltaTime;

        if (Mouse.current.leftButton.wasPressedThisFrame && cooldownTimer <= 0f)
        {
            Shoot();
            cooldownTimer = shootCooldown;
        }
    }

    void Shoot()
    {
        Vector2 mouseWorld = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 dir = (mouseWorld - (Vector2)transform.position).normalized;

        Vector2 spawnPos = (Vector2)transform.position + dir * 0.5f;
        GameObject proj = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);

        Projectile p = proj.GetComponent<Projectile>();
        if (p != null)
        {
            p.isPlayerProjectile = true;
            p.speed = projectileSpeed;
            p.SetDirection(dir);
        }
    }
}
