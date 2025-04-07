using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private int damage = 10;  // Adjust damage as needed
    [SerializeField] private float lifetime = 5f;  // Time before projectile destroys itself

    private void Start()
    {
        // Destroy the projectile after a certain time if it doesn't hit anything
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Get the HealthManagerScript from the player (adjust to your actual health script)
            HealthManagerScript playerHealth = collision.gameObject.GetComponent<HealthManagerScript>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
            
            // Destroy the projectile after hitting the player
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            // Destroy projectile if it hits an obstacle
            Destroy(gameObject);
        }
    }
}
