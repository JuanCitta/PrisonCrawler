using UnityEngine;
using UnityEngine.Tilemaps;

public class Projectile : MonoBehaviour
{
    public float speed = 5f;
    public int damage = 1;
    public float lifetime = 5f;
    public bool isPlayerProjectile = false;

    private Vector2 direction;

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isPlayerProjectile)
        {
            EnemyHealth enemy = other.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Destroy(gameObject);
                return;
            }
        }
        else
        {
            if (other.CompareTag("Player"))
            {
                PlayerHealth.Instance?.TakeDamage(damage);
                Destroy(gameObject);
                return;
            }
        }

        if (other.GetComponent<TilemapCollider2D>() != null
         || other.GetComponent<CompositeCollider2D>() != null)
        {
            Destroy(gameObject);
        }
    }
}