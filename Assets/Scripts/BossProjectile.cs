using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    [Header("Damage Settings")]
    [SerializeField] private int damage = 15;
    [SerializeField] private float destroyDelay = 0.1f; // Small delay before destruction for visual feedback

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if we hit the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Try to get the player's health component
            HealthManagerScript healthManager = collision.gameObject.GetComponent<HealthManagerScript>();
            if (healthManager != null)
            {
                healthManager.TakeDamage(damage);
            }
        }
        // Destroy the projectile regardless of what it hit
        Destroy(gameObject, destroyDelay);
    }
}