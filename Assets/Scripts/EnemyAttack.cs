using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private int damage = 0;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private float attackRange = 10f;
    [SerializeField] private float shootCooldown = 2f;

    private Transform player;
    private float shootTimer;
    private bool isFacingRight = true;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Update()
    {
        if (player == null) return;

        // Check distance between player and enemy
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            shootTimer -= Time.deltaTime;

            // Only shoot if the player is in front of the enemy
            if (shootTimer <= 0f && IsPlayerInFront())
            {
                Shoot();
                shootTimer = shootCooldown;
            }
        }

        // Handle enemy flip (if moving towards left or right)
        if (player.position.x > transform.position.x && !isFacingRight)
        {
            Flip();
        }
        else if (player.position.x < transform.position.x && isFacingRight)
        {
            Flip();
        }
    }

    private bool IsPlayerInFront()
    {
        // Determine if the player is in front of the enemy (based on the enemy's facing direction)
        if (isFacingRight)
        {
            return player.position.x > transform.position.x;  // Player is in front (right side)
        }
        else
        {
            return player.position.x < transform.position.x;  // Player is in front (left side)
        }
    }

    private void Shoot()
    {
        if (projectilePrefab == null || player == null || firePoint == null) return;

        // Update the fire point position based on the enemy's facing direction
        Vector2 direction = (player.position.x > transform.position.x) ? Vector2.right : Vector2.left;

        // Instantiate the projectile at the fire point (fixed)
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            // Set the velocity to move only along the X axis (ignoring Y)
            rb.linearVelocity = direction * projectileSpeed;
        }

        // Optionally destroy the projectile after a set amount of time
        Destroy(projectile, 5f);  // Destroy after 5 seconds
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;

        // Flip the enemy
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;

        // Adjust fire point based on flip (if needed)
        Vector3 firePointOffset = firePoint.localPosition;
        firePointOffset.x *= -1;  // Flip the firePoint horizontally
        firePoint.localPosition = firePointOffset;
    }
}
