using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public int attackDamage = 50; // Damage dealt to the enemy
    private GameObject currentEnemy; // The enemy currently inside the collider
    private bool canAttack = true; // Whether the player is allowed to attack
    private float attackCooldown = 1f; // Cooldown duration in seconds
    HealthManagerScript healthManager;

    private void Start()
    {
        healthManager = FindAnyObjectByType<HealthManagerScript>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            currentEnemy = other.gameObject;
            Debug.Log($"Enemy {currentEnemy.name} entered the trigger.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log($"Enemy {currentEnemy.name} exited the trigger.");
            currentEnemy = null;
        }
    }

    public void AttackEnemy(InputAction.CallbackContext context)
    {
        if (context.performed && canAttack)
        {
            if (currentEnemy != null)
            {
                EnemyHealth enemyHealth = currentEnemy.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(attackDamage);
                    Debug.Log($"Attacked {currentEnemy.name}, health remaining: {enemyHealth.CurrentHealth}");

                    // Start cooldown
                    StartCoroutine(AttackCooldown());
                }
                else
                {
                    Debug.LogError($"Enemy {currentEnemy.name} has no EnemyHealth component.");
                }
            }
            else
            {
                Debug.Log("No enemy to attack!");
            }
        }
    }

    private System.Collections.IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
}
