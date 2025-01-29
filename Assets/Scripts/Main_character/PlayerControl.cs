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

    public bool IsJumping
    {
        get { return _IsJumping; }
        private set
        {
            _IsJumping = value;
            animator.SetBool("_IsJumping", value);
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
        // Apply movement
        rb.velocity = new Vector2(movementInput.x * speed, rb.velocity.y);

        // Check if player is grounded
        bool grounded = IsGrounded();

        // Update Jumping Animation (only trigger when airborne)
        if (!grounded && !_IsJumping)
        {
            IsJumping = true; // Triggers jump animation once
        }
        else if (grounded && _IsJumping)
        {
            IsJumping = false; // Reset jump when grounded
        }

        // Running Animation Logic
        if (movementInput.x != 0)
        {
            IsRunning = true;
            animator.SetFloat("Speed", 1f);
        }
        else
        {
            // Ensure animation resets to Idle when stopping
            IsRunning = false;
            animator.SetFloat("Speed", 0f);
            animator.Play("Idle", 0, 0f); // Force reset to Idle state
        }

        // Flip character sprite
        if (!isFacingRight && movementInput.x > 0f) Flip();
        else if (isFacingRight && movementInput.x < 0f) Flip();
    }

    public void Move(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();

        // Set running state (true if moving, false if stopped)
        IsRunning = movementInput.x != 0;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && IsGrounded())
        {
            float jumpDirection = isGravityFlipped ? -1f : 1f;
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower * jumpDirection);
            IsJumping = true; // Triggers jump animation
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
        if (context.performed && canDash && !isDashing)
        {
            StartCoroutine(Dash());
        }
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