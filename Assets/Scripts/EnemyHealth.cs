using System.Collections;
using UnityEngine;

// Minimal stand-in so Fireball has something to damage right away.
// Swap in your real enemy script later — keep the TakeDamage and
// ApplyBurn method names the same (or update Fireball.cs to match).
public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 30;
    private int currentHealth;
    private Coroutine burnRoutine;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void ApplyBurn(float damagePerTick, float duration, float tickInterval)
    {
        if (burnRoutine != null) StopCoroutine(burnRoutine);
        burnRoutine = StartCoroutine(BurnTick(damagePerTick, duration, tickInterval));
    }

    private IEnumerator BurnTick(float damagePerTick, float duration, float tickInterval)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            yield return new WaitForSeconds(tickInterval);
            TakeDamage(Mathf.RoundToInt(damagePerTick));
            elapsed += tickInterval;
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
