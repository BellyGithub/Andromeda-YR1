using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] ParticleSystem damageParticles;
    public int maxHealth = 100;
    public int enemyScore = 100;
    private int currentHealth;
    Score scoreScript;
    public int CurrentHealth => currentHealth;
    private ParticleSystem damageParticlesInstance;

    private void Start()
    {
        currentHealth = maxHealth;
        scoreScript = FindAnyObjectByType<Score>(); // Find Score script in the scene
    }

    public void TakeDamage(int damage, Vector2 attackDirection)
    {
        currentHealth -= damage;
        SpawnDamageParticles(attackDirection);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} has been defeated!");
        scoreScript.AddScore(enemyScore);
        Destroy(gameObject); // Destroy the enemy
    }

    private void SpawnDamageParticles(Vector2 attackDirection)
    {
        Quaternion spawnRotation = Quaternion.FromToRotation(Vector2.right, attackDirection);
        damageParticlesInstance = Instantiate(damageParticles, transform.position, spawnRotation);
    }
}
