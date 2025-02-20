using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Audio;

public class HealthManagerScript : MonoBehaviour
{
    public float maxHealth = 100.0f;
    public float currentHealth;
    public float invincibilityDuration = 2.0f;
    public Image healthbar;
    public bool isInvincible = false;
    private float ratio = 1.0f;
    private Vector3 initialPosition;

    [Header("SOUNDS")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip hurtSoundClip;
    public void TakeDamage(int damage)
    {
        if (isInvincible)
        {
            return;
        }

        audioSource.clip = hurtSoundClip;
        audioSource.volume = 0.2f;
        audioSource.Play();
        currentHealth -= damage;
        RefreshHealthbar();
        Debug.Log("health:" + currentHealth);
        
        if (currentHealth <= 0)
        {
            
            isInvincible = true;
            Respawn();
        }
        else if (!isInvincible)
        {
            StartCoroutine(InvincibilityCoroutine(invincibilityDuration));
        }
    }

    public void RefreshHealthbar()
    {
        ratio = currentHealth / maxHealth;
        healthbar.fillAmount = ratio;
    }

    public void Respawn()
    {
        Debug.Log("You die");
        transform.position = initialPosition;
        currentHealth = 100.0f;
        Debug.Log("Character has respawned at the initial position!");
        isInvincible = false;
        RefreshHealthbar();
    }

    public void SetInvincibility(float duration)
    {
        Debug.Log("Invincible for " + duration);
        StartCoroutine(InvincibilityCoroutine(duration));
    }

    private IEnumerator InvincibilityCoroutine(float duration)
    {
        isInvincible = true;
        yield return new WaitForSeconds(duration);
        isInvincible = false;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        initialPosition = transform.position;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
