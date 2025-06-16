using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class new_p_m : MonoBehaviour
{
    private float Horizontal;
    public float speed = 8f;
    public float Jumpingpower = 16f;
    private bool IsFacingRight = true;

    private Rigidbody2D rb;
    private Animator animator;

    [SerializeField] private Transform groundcheck;
    [SerializeField] private LayerMask groundlayer;

    public float dashSpeed = 20f;
    public float dashTime = 0.2f;
    public float dashCooldown = 1f;

    private bool isDashing;
    private float dashTimeLeft;
    private float lastDashTime;

    [SerializeField] private GameObject deathUI;
    private bool isDead = false;
    private bool wasGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (deathUI != null)
        {
            deathUI.SetActive(false);
        }
    }

    void Update()
    {
        if (isDead || isDashing)
            return;

        Horizontal = Input.GetAxisRaw("Horizontal");
        animator.SetFloat("Speed", Mathf.Abs(Horizontal));

        bool isGrounded = IsGrounded();

        if (isGrounded != wasGrounded)
        {
            animator.SetBool("IsJumping", !isGrounded);
            wasGrounded = isGrounded;
        }

        Flip();

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, Jumpingpower);
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        if (Input.GetKeyDown(KeyCode.E) && Time.time >= lastDashTime + dashCooldown)
        {
            isDashing = true;
            dashTimeLeft = dashTime;
            lastDashTime = Time.time;

            float dashDirection = IsFacingRight ? 1f : -1f;
            rb.velocity = new Vector2(dashDirection * dashSpeed, 0f);
        }
    }

    void FixedUpdate()
    {
        if (isDead)
            return;

        if (isDashing)
        {
            PushObjectsOnDash();
            dashTimeLeft -= Time.fixedDeltaTime;
            if (dashTimeLeft <= 0)
            {
                isDashing = false;
            }
            return;
        }

        rb.velocity = new Vector2(Horizontal * speed, rb.velocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundcheck.position, 0.2f, groundlayer);
    }

    private void Flip()
    {
        if ((IsFacingRight && Horizontal < 0f) || (!IsFacingRight && Horizontal > 0f))
        {
            IsFacingRight = !IsFacingRight;
            Vector3 localscale = transform.localScale;
            localscale.x *= -1f;
            transform.localScale = localscale;
        }
    }

    private void PushObjectsOnDash()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.6f, groundlayer);

        foreach (Collider2D hit in hits)
        {
            if (hit != null && hit.gameObject != gameObject)
            {
                PushableObject po = hit.GetComponent<PushableObject>();
                if (po != null)
                {
                    float direction = IsFacingRight ? 1f : -1f;
                    po.ApplyPush(direction);
                }
            }
        }
    }
}
