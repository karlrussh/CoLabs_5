using System;
using System.Linq;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private float horizontal;
    private float speed = 4f;
    private float jumpingPower = 5f;
    private bool isFacingRight = true;
    
    [SerializeField] SpriteRenderer sr;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    private void OnEnable()
    {
        ControlsManager.OnPlayerJump += PlayerJump;
    }

    private void OnDisable()
    {
        ControlsManager.OnPlayerJump -= PlayerJump;
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        Flip();
    }

    private void PlayerJump()
    {
        if (!IsGrounded()) return;
        
        if (rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }
        else
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpingPower);
        }
    }

    private bool IsGrounded()
    {
        return Physics.OverlapSphere(groundCheck.position, 0.2f, groundLayer).Length > 0;
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector3(horizontal * speed, rb.linearVelocity.y);
    }

    private void Flip()
    {
        if ((isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f))
        {
            // Debug.Log("FLIPPING");
            isFacingRight = !isFacingRight;
            //Vector3 localScale = transform.localScale;
            //localScale.x *= -1f;
            //transform.localScale = localScale;

            Vector3 spriteScale = sr.transform.localScale;
            spriteScale.x *= -1f;
            sr.transform.localScale = spriteScale;
            //sr.flipX;
        }
    }
}
