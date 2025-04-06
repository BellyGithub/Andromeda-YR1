using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

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

        if (Gamepad.current != null)
        {
            StartCoroutine(VibrateAndDestroy());
        }
        else
        {
            Destroy(gameObject); // fallback if no controller
        }
    }

    private IEnumerator VibrateAndDestroy()
    {
        Gamepad.current.SetMotorSpeeds(0.5f, 1.0f); // rumble
        yield return new WaitForSeconds(0.3f);      // wait 0.2s
        Gamepad.current.SetMotorSpeeds(0f, 0f);     // stop rumble
        Destroy(gameObject);                        // then destroy enemy
    }

    private void SpawnDamageParticles(Vector2 attackDirection)
    {
        Quaternion spawnRotation = Quaternion.FromToRotation(Vector2.right, attackDirection);
        damageParticlesInstance = Instantiate(damageParticles, transform.position, spawnRotation);
    }
}
