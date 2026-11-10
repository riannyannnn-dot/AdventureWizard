using UnityEngine;

// Attach this to the Fireball prefab.
// The fireball travels in a straight line and deals damage + a short burn
// effect to whatever it hits, then destroys itself.
public class Fireball : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 12f;

    [Header("Damage")]
    public int damage = 10;
    public float burnDamagePerTick = 2f;
    public float burnDuration = 2f;
    public float burnTickInterval = 0.5f;

    [Header("Lifetime")]
    public float lifeSeconds = 3f; // auto-destroy if it never hits anything

    private Vector2 direction = Vector2.right;

    // Called right after Instantiate to set which way the fireball should fly.
    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;

        // Rotate the sprite to face the direction it's travelling.
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    void Start()
    {
        Destroy(gameObject, lifeSeconds);
    }

    void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Ignore the player's own collider so the fireball doesn't hit its caster.
        if (other.CompareTag("Player")) return;

        EnemyHealth enemy = other.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            enemy.ApplyBurn(burnDamagePerTick, burnDuration, burnTickInterval);
        }

        // Fireball is consumed on any hit that isn't the player
        // (walls, enemies, etc.) — remove this check if you want it to
        // pass through walls too.
        if (!other.isTrigger || enemy != null)
        {
            Destroy(gameObject);
        }
    }
}
