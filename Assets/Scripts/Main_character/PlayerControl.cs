using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;
    public Transform groundCheck;
    public LayerMask groundLayer;

    public float speed = 8f;
    public float jumpingPower = 16f;
    private bool isFacingRight = true;
    private bool isGravityFlipped = false;

    private Vector2 movementInput;
    private bool _IsRunning = false;
    private bool _IsJumping = false;
    private bool _IsDashing = false;
    private bool _IsAttacking = false;

    [Header("Dashing Parameters")]
    [SerializeField] private float dashingPower = 24f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingCooldown = 1f;
    private bool canDash = true;
    private bool isDashing;

    [Header("Attack Parameters")]
    public int attackDamage = 50;
    private GameObject currentEnemy;
    private bool canAttack = true;
    private float attackCooldown = 1f;

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
    }

    public void Move(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
        IsRunning = movementInput.x != 0;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && IsGrounded() && !isDashing && !_IsAttacking) // Prevent jump during attack
        {
            float jumpDirection = isGravityFlipped ? -1f : 1f;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower * jumpDirection);
            IsJumping = true;
        }
    }

    public void SwitchGravity(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isGravityFlipped = !isGravityFlipped;
            rb.gravityScale *= -1;

            Vector3 localScale = transform.localScale;
            localScale.y *= -1f;
            transform.localScale = localScale;
        }
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (context.performed && canDash && !isDashing && !_IsAttacking) // Prevent dash during attack
        {
            StartCoroutine(DashCoroutine());
        }
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed && canAttack)
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
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(attackDamage);
                Debug.Log($"Attacked {currentEnemy?.name}, health remaining: {enemyHealth?.CurrentHealth}");
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
        canDash = false;
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

        yield return new WaitForSeconds(dashingCooldown);
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
