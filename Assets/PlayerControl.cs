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

    private Vector2 movementInput; // Store full movement input (Vector2)
    private bool _IsRunning = false;

    [Header("Dashing Parameters")]
    [SerializeField] private float dashingPower = 24f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingCooldown = 1f;
    private bool canDash = true;
    private bool isDashing;

    public bool IsRunning
    {
        get { return _IsRunning; }
        private set
        {
            _IsRunning = value;
            animator.SetBool("_IsRunning", value);
        }
    }

    void Update()
    {
        if (isDashing)
        {
            return;
        }

        // Apply horizontal movement
        rb.linearVelocity = new Vector2(movementInput.x * speed, rb.linearVelocity.y);

        // Flip character sprite based on movement direction
        if (!isFacingRight && movementInput.x > 0f)
        {
            Flip();
        }
        else if (isFacingRight && movementInput.x < 0f)
        {
            Flip();
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        // Read full movement input (Vector2)
        movementInput = context.ReadValue<Vector2>();

        // Check if running (horizontal input is non-zero)
        IsRunning = movementInput != Vector2.zero;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && IsGrounded())
        {
            // Jump in the direction opposite to gravity
            float jumpDirection = isGravityFlipped ? -1f : 1f;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower * jumpDirection);
        }

        if (context.canceled && Mathf.Abs(rb.linearVelocity.y) > 0f)
        {
            // Reduce jump height when jump is canceled
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }
    }

    public void SwitchGravity(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // Toggle gravity
            isGravityFlipped = !isGravityFlipped;
            rb.gravityScale *= -1;

            // Flip the character vertically to align with gravity direction
            Vector3 localScale = transform.localScale;
            localScale.y *= -1f;
            transform.localScale = localScale;
        }
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (context.performed && canDash && !isDashing)
        {
            StartCoroutine(Dash());
        }
    }

    private bool IsGrounded()
    {
        // Check for ground regardless of gravity direction
        float checkOffset = isGravityFlipped ? -0.2f : 0.2f;
        Vector3 checkPosition = groundCheck.position + new Vector3(0, checkOffset, 0);
        return Physics2D.OverlapCircle(checkPosition, 0.2f, groundLayer);
    }

    private void Flip()
    {
        // Flip the player's sprite direction horizontally
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        // Set the dash velocity based on the direction the player is facing
        float dashDirection = isFacingRight ? 1f : -1f;
        rb.linearVelocity = new Vector2(dashDirection * dashingPower, 0f);

        yield return new WaitForSeconds(dashingTime);

        rb.gravityScale = originalGravity;
        isDashing = false;

        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
}