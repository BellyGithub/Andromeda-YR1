using UnityEngine;

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
            audioSource.volume = uiManager.volume;
            audioSource.Play();
            Debug.Log("Player healed!");
            // Disables use of healing station (Only heals player once)
            canUse = false;
        }
    }
}
