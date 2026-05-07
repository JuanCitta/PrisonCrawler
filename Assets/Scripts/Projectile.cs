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
    }

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isPlayerProjectile)
        {
            // Projétil do jogador: acerta inimigos (detectado pelo componente EnemyHealth)
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
            // Projétil do inimigo: acerta o jogador
            if (other.CompareTag("Player"))
            {
                PlayerHealth.Instance?.TakeDamage(damage);
                Destroy(gameObject);
                return;
            }
        }

        // Ambos destroem ao bater em parede (tilemap)
        if (other.GetComponent<TilemapCollider2D>() != null
         || other.GetComponent<CompositeCollider2D>() != null)
        {
            Destroy(gameObject);
        }
    }
}
