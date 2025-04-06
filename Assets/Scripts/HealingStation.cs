using UnityEngine;
using UnityEngine.InputSystem; 
using System.Collections;

public class HealingStation : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip healSoundClip;
    [SerializeField] private HealthManagerScript HealthManager;
    [SerializeField] private float healAmount = 50.0f;
    UIManager uiManager;
    private bool canUse;

    void Start()
    {
        HealthManager = FindAnyObjectByType<HealthManagerScript>();
        audioSource = GetComponent<AudioSource>();
        uiManager = FindAnyObjectByType<UIManager>();
        canUse = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Heals player
        if (other.CompareTag("Player") && canUse)
        {
            HealthManager.heal(healAmount);
            audioSource.clip = healSoundClip;
            audioSource.Play();
            Debug.Log("Player healed!");
            canUse = false;

            // Trigger vibration on heal
            if (Gamepad.current != null)
            {
                StartCoroutine(VibrateOnHeal(0.5f, 0.7f, 0.4f)); // (lowFreq, highFreq, duration)
            }
        }
    }

    private IEnumerator VibrateOnHeal(float lowFreq, float highFreq, float duration)
    {
        Gamepad.current.SetMotorSpeeds(lowFreq, highFreq);
        yield return new WaitForSeconds(duration);
        Gamepad.current.SetMotorSpeeds(0, 0);
    }
}
