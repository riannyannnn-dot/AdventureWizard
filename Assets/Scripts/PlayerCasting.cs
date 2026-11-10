using UnityEngine;

// Attach this to the Wizard, alongside your existing PlayerMovement script.
// Left mouse click fires a fireball from the player toward the mouse position.
public class PlayerCasting : MonoBehaviour
{
    [Header("Setup")]
    public GameObject fireballPrefab;   // assign your Fireball prefab here
    public Transform firePoint;         // optional: empty child object marking spawn spot
    public Camera mainCamera;           // assign Main Camera, or it will auto-find it

    [Header("Cooldown")]
    public float castCooldown = 0.4f;
    private float cooldownTimer = 0f;

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    void Update()
    {
        cooldownTimer -= Time.deltaTime;

        if (Input.GetMouseButtonDown(0) && cooldownTimer <= 0f)
        {
            CastFireball();
            cooldownTimer = castCooldown;
        }
    }

    void CastFireball()
    {
        if (fireballPrefab == null)
        {
            Debug.LogWarning("PlayerCasting: no fireballPrefab assigned.");
            return;
        }

        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;
        Vector2 direction = (mouseWorldPos - spawnPos).normalized;

        GameObject fireballObj = Instantiate(fireballPrefab, spawnPos, Quaternion.identity);
        Fireball fireball = fireballObj.GetComponent<Fireball>();
        if (fireball != null)
        {
            fireball.SetDirection(direction);
        }
    }
}
