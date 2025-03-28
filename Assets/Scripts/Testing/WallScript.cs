using UnityEngine;
using UnityEngine.Audio;

public class WallScript : MonoBehaviour
{
    [SerializeField] private GameObject BossWalls;
    [SerializeField] Boss bossScript;
    AudioSource audioSource;
    [SerializeField] private AudioClip bossMusic;
    public float volume = 0.5f;
    UIManager uiManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        uiManager = FindAnyObjectByType<UIManager>();
        bossScript = FindAnyObjectByType <Boss>();
        audioSource = FindAnyObjectByType<AudioSource>();
        if (BossWalls != null)
            BossWalls.SetActive(false);
        else Debug.Log("can't find boss walls");
    }

    // OnTriggerEnter is called when another collider enters this object's trigger collider
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (BossWalls != null)
            {
                audioSource.clip = bossMusic;
                audioSource.Play();
                BossWalls.SetActive(true);
                bossScript.bossAwake();
            }
            else Debug.Log("can't find boss walls");
        }
    }
}
