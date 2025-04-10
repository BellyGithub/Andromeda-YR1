using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public Image dashCooldown;
    UIManager uiManager;

    public float speed = 8f;
    //public float jumpingPower = 16f;
    private bool isFacingRight = true;
    private bool isGravityFlipped = false;

    private Vector2 movementInput;
    private bool _IsRunning = false;
    private bool _IsJumping = false;
    private bool _IsDashing = false;
    private bool _IsAttacking = false;

    [Header("Jump Parameters")]
    [SerializeField] private float minJumpPower = 10f;
    [SerializeField] private float maxJumpPower = 20f;
    [SerializeField] private float jumpHoldTimeMax = 0.35f;
    private bool jumpHeld = false;
    private float jumpTimeCounter = 0f;

    [Header("SOUNDS")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip jumpSoundClip;
    [SerializeField] AudioClip hitSoundClip;
    [SerializeField] AudioClip gravSwitchSoundClip;
    public float volume = 0.5f;

    [Header("Dashing Parameters")]
    [SerializeField] private float dashingPower = 24f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingCooldown = 1f;
    private float dashCooldownTimer = 0f;
    private bool canDash = true;
    private bool isDashing;
    private bool canFlipInAir = true;

    [Header("Attack Parameters")]
    public int attackDamage = 50;
    private HealthManagerScript healthManager;
    private GameObject currentEnemy;
    private bool canAttack = true;
    private float attackCooldown = 1f;

    private void Start()
    {
        dashCooldown.fillAmount = Mathf.Clamp(0f, 0f, dashingCooldown);
        audioSource = GetComponent<AudioSource>();
        healthManager = FindAnyObjectByType<HealthManagerScript>();
        uiManager = FindAnyObjectByType<UIManager>();
    }

    public bool IsRunning
    {
        get { return _IsRunning; }
        private set
        {
            _IsRunning = value;
            animator.SetBool("_IsRunning", value);
        }
    }

    public bool IsJumping
    {
        get { return _IsJumping; }
        private set
        {
            _IsJumping = value;
            animator.SetBool("_IsJumping", value);
        }
    }

    public bool IsDashing
    {
        get { return _IsDashing; }
        private set
        {
            _IsDashing = value;
            animator.SetBool("_IsDashing", value);
        }
    }

    public bool IsAttacking
    {
        get { return _IsAttacking; }
        private set
        {
            _IsAttacking = value;
            animator.SetBool("_IsAttacking", value);
        }
    }

    void Update()
    {
        if (isDashing || _IsAttacking) return; // Prevent movement when dashing or attacking

        rb.linearVelocity = new Vector2(movementInput.x * speed, rb.linearVelocity.y);

        bool grounded = IsGrounded();

        if (grounded) canFlipInAir = true;

        if (!grounded && !_IsJumping)
        {
            IsJumping = true;
        }
        else if (grounded && _IsJumping)
        {
            IsJumping = false;
        }

        if (movementInput.x != 0 && grounded)
        {
            IsRunning = true;
            animator.SetFloat("Speed", 1f);
        }
        else if (grounded)
        {
            IsRunning = false;
            animator.SetFloat("Speed", 0f);
            animator.Play("Idle", 0, 0f);
        }

        if (!isFacingRight && movementInput.x > 0f) Flip();
        else if (isFacingRight && movementInput.x < 0f) Flip();

        if (jumpHeld && jumpTimeCounter > 0)
        {
            float jumpDirection = isGravityFlipped ? -1f : 1f;
            float extraJumpVelocity = (maxJumpPower - minJumpPower) / jumpHoldTimeMax * Time.deltaTime;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y + extraJumpVelocity * jumpDirection);
            jumpTimeCounter -= Time.deltaTime;
        }
    }

    public void Pause(InputAction.CallbackContext context) 
    {
        uiManager.PauseMenuFunction();
    }

    public void Move(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
        IsRunning = movementInput.x != 0;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && IsGrounded() && !isDashing && !_IsAttacking && UIManager.GamePaused == false) // Prevent jump during attack
        {
            jumpHeld = true;
            jumpTimeCounter = jumpHoldTimeMax;
            float jumpDirection = isGravityFlipped ? -1f : 1f;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, minJumpPower * jumpDirection);
            IsJumping = true;
            audioSource.clip = jumpSoundClip;
            audioSource.Play();
        }

        if (context.canceled)
        {
            jumpHeld = false;
        }
    }

    public void SwitchGravity(InputAction.CallbackContext context)
    {
        if (context.performed && !isDashing && !UIManager.GamePaused)
        {
            if (IsGrounded())
            {
                FlipGravity();
                canFlipInAir = true;
            }

            else if (canFlipInAir)
            {
                FlipGravity();
                canFlipInAir = false;
            }
        }
    }
    private void FlipGravity()
    {
        audioSource.clip = gravSwitchSoundClip;
        audioSource.Play();
        isGravityFlipped = !isGravityFlipped;
        rb.gravityScale *= -1;

        Vector3 localScale = transform.localScale;
        localScale.y *= -1f;
        transform.localScale = localScale;
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (context.performed && canDash && !isDashing && !_IsAttacking && UIManager.GamePaused == false) // Prevent dash during attack
        {
            StartCoroutine(DashCoroutine());
        }
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed && canAttack && UIManager.GamePaused == false)
        {
            StartCoroutine(AttackCoroutine());
        }
    }

    private IEnumerator AttackCoroutine()
    {
        canAttack = false;
        IsAttacking = true;

        yield return new WaitForSeconds(0.1f); // Allow attack animation to start

        if (currentEnemy != null)
        {
            EnemyHealth enemyHealth = currentEnemy.GetComponent<EnemyHealth>();
            Boss boss = currentEnemy.GetComponent<Boss>();
            if (enemyHealth != null)
            {
                audioSource.clip = hitSoundClip;
                audioSource.Play();
                healthManager.SetInvincibility(attackCooldown);
                enemyHealth.TakeDamage(attackDamage, new Vector2(transform.localScale.x, 0f));
                Debug.Log($"Attacked {currentEnemy?.name}, health remaining: {enemyHealth?.CurrentHealth}");
            }
            else if (boss != null)
            {
                audioSource.clip = hitSoundClip;
                audioSource.Play();
                healthManager.SetInvincibility(attackCooldown);
                boss.TakeDamage(attackDamage);
            }
            else
            {
                Debug.LogError($"Enemy {currentEnemy?.name} has no EnemyHealth component.");
            }
        }
        else
        {
            Debug.LogWarning("No enemy to attack!");
        }

        yield return new WaitForSeconds(0.3f); // Allow attack animation to finish

        // Ensure animation has finished before resetting
        while (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            yield return null;
        }

        IsAttacking = false;
        canAttack = true;
    }

    private bool IsGrounded()
    {
        float checkOffset = isGravityFlipped ? -0.2f : 0.2f;
        Vector3 checkPosition = groundCheck.position + new Vector3(0, checkOffset, 0);
        return Physics2D.OverlapCircle(checkPosition, 0.2f, groundLayer);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    private IEnumerator DashCoroutine()
    {
        dashCooldown.fillAmount = dashingCooldown;
        
        isDashing = true;
        IsDashing = true;

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        float dashDirection = isFacingRight ? 1f : -1f;
        rb.linearVelocity = new Vector2(dashDirection * dashingPower, 0f);

        yield return new WaitForSeconds(dashingTime);

        rb.gravityScale = originalGravity;
        isDashing = false;
        IsDashing = false;
        canDash = false;
        dashCooldownTimer = dashingCooldown;

        while (dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime;
            dashCooldown.fillAmount = dashCooldownTimer / dashingCooldown;
            yield return null;
        }

        dashCooldown.fillAmount = 0f;
        canDash = true;
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
        if (other.CompareTag("Enemy") && currentEnemy == other.gameObject)
        {
            Debug.Log($"Enemy {currentEnemy.name} exited the trigger.");
            currentEnemy = null;
        }
    }
}
