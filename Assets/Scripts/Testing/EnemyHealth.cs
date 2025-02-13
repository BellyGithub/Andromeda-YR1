using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int enemyScore = 100;
    private int currentHealth;
    Score scoreScript;
    public int CurrentHealth => currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
        scoreScript = FindObjectOfType<Score>(); // Find Score script in the scene
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
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
}
