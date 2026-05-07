using UnityEngine;

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
        else if (!other.CompareTag("Enemy") && !other.CompareTag("Projectile"))
        {
            // Destrói ao colidir com paredes ou qualquer coisa que não seja inimigo/projétil
            Destroy(gameObject);
        }
    }
}
