using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    public float maxHealth = 1000.0f;
    public float health;
    private float ratio = 1.0f;
    [SerializeField] GameObject bossBarCanvas;
    public Image bossBar;
    [SerializeField] private int damage = 25;
    public float chargeSpeed = 10f;
    public float waitTime = 10f;

    private bool isChargingLeft = true;
    private bool isWaiting = false;
    private bool idle = true;
    private float waitTimer = 0f;

    private Rigidbody2D rb;
    [SerializeField] private HealthManagerScript healthManager;

    void Start()
    {
        bossBarCanvas.SetActive(false);
        health = maxHealth;
        healthManager = FindAnyObjectByType<HealthManagerScript>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isWaiting)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitTime)
            {
                isWaiting = false;
                waitTimer = 0f;
                isChargingLeft = !isChargingLeft; // Switch direction
            }
        }
        else if(!idle)
        {
            Charge();
        }
    }

    public void bossAwake()
    {
        idle = false;
        bossBarCanvas.SetActive(true);
    }

    void Charge()
    {
        Vector2 direction = isChargingLeft ? Vector2.left : Vector2.right;
        rb.linearVelocity = direction * chargeSpeed;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Check if the boss collides with a wall

        if (other.gameObject.CompareTag("Wall"))
        {
            rb.linearVelocity = Vector2.zero;
            isWaiting = true;
        }

        // Check if the boss collides with the player
        if (other.gameObject.CompareTag("Player"))
        {
            healthManager.TakeDamage(damage);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        RefreshHealthbar();
        if (health <= 0)
        {
            Die();
        }
    }

    public void RefreshHealthbar()
    {
        ratio = health / maxHealth;
        bossBar.fillAmount = ratio;
    }

    void Die()
    {
        Destroy(gameObject);
    }
}