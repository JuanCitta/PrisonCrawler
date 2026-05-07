using UnityEngine;
using UnityEngine.Tilemaps;

public class Projectile : MonoBehaviour
{
    public float speed = 5f;
    public int damage = 1;
    public float lifetime = 5f;

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
        if (other.CompareTag("Player"))
        {
            PlayerHealth.Instance?.TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (other.GetComponent<TilemapCollider2D>() != null
              || other.GetComponent<CompositeCollider2D>() != null)
        {
            // Destrói apenas ao colidir com paredes (tilemaps)
            // Inimigos são ignorados independente de tag
            Destroy(gameObject);
        }
    }
}
