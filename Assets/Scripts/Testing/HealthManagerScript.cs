using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class HealthManagerScript : MonoBehaviour
{
    public float maxHealth = 100.0f;
    public float currentHealth;
    public float invincibilityDuration = 0.5f;
    public Image healthbar;
    public bool isInvincible = false;
    private bool damaged = false;
    private float ratio = 1.0f;
    private Vector3 initialPosition;

    [SerializeField] SpriteRenderer sr;

    [Header("SOUNDS")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip hurtSoundClip;

    private void VibrateController()
    {
        if (Gamepad.current != null)
        {
            Gamepad.current.SetMotorSpeeds(0.5f, 0.5f);
            StartCoroutine(StopVibrationAfterDelay(0.3f));
        }
    }

    private IEnumerator StopVibrationAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (Gamepad.current != null)
        {
            Gamepad.current.SetMotorSpeeds(0f, 0f);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible)
        {
            return;
        }
        damaged = true;
        audioSource.clip = hurtSoundClip;
        audioSource.Play();
        currentHealth -= damage;
        RefreshHealthbar();
        Debug.Log("health:" + currentHealth);

        VibrateController();

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
        if (SceneManager.GetActiveScene().name == "BossLevel")
        {
            Debug.Log("Player died in BossLevel - restarting scene");
            SceneManager.LoadScene("BossLevel");
        }
        else
        {
            Debug.Log("You die");
            transform.position = initialPosition;
            currentHealth = 100.0f;
            Debug.Log("Character has respawned at the initial position!");
            isInvincible = false;
            RefreshHealthbar();
        }
    }

    public void heal(float healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth >= 100)
        {
            currentHealth = maxHealth;
        }
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
        if (damaged) { sr.color = Color.red; }
        yield return new WaitForSeconds(duration);
        sr.color = Color.white;
        damaged = false;
        isInvincible = false;
    }

    void Start()
    {
        currentHealth = maxHealth;
        initialPosition = transform.position;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {

    }
}
