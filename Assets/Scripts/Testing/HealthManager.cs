using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthManager : MonoBehaviour
{
    public float maxHealth = 100.0f;
    public float currentHealth;
    public float invincibilityDuration = 2.0f;
    public Image healthbar;
    private bool isInvincible = false;
    private float ratio = 1.0f;
    private Vector3 initialPosition;

    public void TakeDamage(int damage)
    {
        if (isInvincible) return;
        currentHealth -= damage;
        RefreshHealthbar();
        Debug.Log("health:" + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(InvincibilityCoroutine());
        }
    }
    public void Die()
    {
        Debug.Log("You die");
        Respawn();
    }

    public void RefreshHealthbar()
    {
        ratio = currentHealth / maxHealth;
        healthbar.fillAmount = ratio;
    }

    public void Respawn()
    {
        transform.position = initialPosition;
        currentHealth = 100.0f;
        Debug.Log("Character has respawned at the initial position!");
    }

    IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration);
        isInvincible = false;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
