using UnityEngine;

/// <summary>
/// Adicionado automaticamente ao projétil do Cyclops.
/// Corrige gradualmente a direção em direção ao target.
/// </summary>
public class HomingProjectile : MonoBehaviour
{
    public Transform target;
    public float turnSpeed = 80f;   // graus por segundo

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (target == null || rb == null) return;

        float speed       = rb.linearVelocity.magnitude;
        float currentAngle  = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;

        Vector2 toTarget  = (Vector2)target.position - (Vector2)transform.position;
        float   targetAngle = Mathf.Atan2(toTarget.y, toTarget.x) * Mathf.Rad2Deg;

        float newAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, turnSpeed * Time.deltaTime);
        float rad      = newAngle * Mathf.Deg2Rad;

        rb.linearVelocity = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * speed;
    }
}
