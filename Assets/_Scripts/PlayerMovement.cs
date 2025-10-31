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
    


    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpingPower);
        }

        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }

        Flip();
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
