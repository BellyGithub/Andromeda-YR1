using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    public float maxHealth = 1000.0f;
    public float health;
    private float ratio = 1.0f;
    [SerializeField] GameObject bossBarCanvas;
    public GameManager gameManager;
    public Image bossBar;
    public float chargeSpeed = 10f;
    public float baseWaitTimeBetweenAttacks = 4f;
    public bool idle = true;
    public bool dead = false;

    [Header("Attack Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed = 8f;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private float attackWindupTime = 0.8f;
    [SerializeField] private int chargeDamage = 25;
    [SerializeField] private float projectileSpawnOffset = 1.44f;

    [Header("Sounds")]
    [SerializeField] private AudioClip chargeWindupSound;
    [SerializeField] private AudioClip shootWindupSound;
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip chargeAttackSound;
    private AudioSource audioSource;
    public float volume = 0.5f;

    private bool isAttacking = false;
    private bool isWindingUp = false;
    private float attackCooldownTimer = 0f;
    private Rigidbody2D rb;
    private Transform player;
    private SpriteRenderer spriteRenderer;
    private HealthManagerScript healthManager;
    Animator animator;

    void Start()
    {
        animator = FindAnyObjectByType<Animator>();
        gameManager = FindAnyObjectByType<GameManager>();
        audioSource = GetComponent<AudioSource>();
        bossBarCanvas.SetActive(false);
        health = maxHealth;
        healthManager = FindAnyObjectByType<HealthManagerScript>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (audioSource.volume != volume)
        {
            audioSource.volume = volume;
        }
        if (isAttacking && !isWindingUp)
        {
            attackCooldownTimer += Time.deltaTime;
            if (attackCooldownTimer >= baseWaitTimeBetweenAttacks)
            {
                isAttacking = false;
                attackCooldownTimer = 0f;
                ChooseNextAttack();
            }
        }

        // Flip sprite based on player position
        if (player != null)
        {
            bool shouldFaceRight = player.position.x > transform.position.x;
            spriteRenderer.flipX = shouldFaceRight;

            // Adjust projectile spawn position
            Vector3 spawnPosition = transform.position;
            spawnPosition.x += shouldFaceRight ? projectileSpawnOffset : -projectileSpawnOffset;
            projectileSpawnPoint.position = spawnPosition;
        }
    }

    public void bossAwake()
    {
        if (!bossBarCanvas.activeSelf)
        {
            idle = false;
            bossBarCanvas.SetActive(true);
            ChooseNextAttack();
        }
    }

    void ChooseNextAttack()
    {
        if (isAttacking) return;

        // Start windup phase
        isWindingUp = true;
        int attackType = Random.Range(0, 2); // 0 or 1

        switch (attackType)
        {
            case 0:
                StartCoroutine(ChargeWindup());
                break;
            case 1:
                StartCoroutine(ShootWindup());
                break;
        }
    }

    System.Collections.IEnumerator ChargeWindup()
    {
        // Play charge warning sound
        if (chargeWindupSound != null)
        {
            audioSource.PlayOneShot(chargeWindupSound);
        }

        yield return new WaitForSeconds(attackWindupTime);

        // Execute attack
        isWindingUp = false;
        isAttacking = true;
        ChargeAttack();
    }

    System.Collections.IEnumerator ShootWindup()
    {
        // Play shoot warning sound
        if (shootWindupSound != null)
        {
            audioSource.PlayOneShot(shootWindupSound);
        }

        yield return new WaitForSeconds(attackWindupTime);

        // Execute attack
        isWindingUp = false;
        isAttacking = true;
        ShootAttack();
    }

    void ChargeAttack()
    {
        Debug.Log("Charging!");
        audioSource.PlayOneShot(chargeAttackSound);
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * chargeSpeed;
        animator.SetFloat("xVelocity", Mathf.Abs(rb.linearVelocity.x));

        Invoke("StopCharge", 1f);
    }

    void StopCharge()
    {
        rb.linearVelocity = Vector2.zero;
        animator.SetFloat("xVelocity", Mathf.Abs(rb.linearVelocity.x));
    }

    void ShootAttack()
    {
        Debug.Log("Shooting!");
        if (projectilePrefab != null && projectileSpawnPoint != null)
        {
            Vector2 direction = (player.position - projectileSpawnPoint.position).normalized;
            GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
            projectile.GetComponent<Rigidbody2D>().linearVelocity = direction * projectileSpeed;

            if (shootSound != null)
            {
                audioSource.PlayOneShot(shootSound);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Charge attack collision
        if (other.gameObject.CompareTag("Player") && rb.linearVelocity.magnitude > 0.1f)
        {
            healthManager.TakeDamage(chargeDamage);
            rb.linearVelocity = Vector2.zero;
            CancelInvoke("StopCharge");
            StopCharge();
        }
        else if (other.gameObject.CompareTag("Wall") && rb.linearVelocity.magnitude > 0.1f)
        {
            rb.linearVelocity = Vector2.zero;
            CancelInvoke("StopCharge");
            StopCharge();
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
        bossBarCanvas.SetActive(false);
        gameManager.CompleteLevel();
        dead = true;
        Destroy(gameObject);
    }
}