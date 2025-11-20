using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;

    private float horizontal;
    private float slidingHorizontal;

    [SerializeField] private float speed = 4f;
    [SerializeField] private float jumpingPower = 5f;
    [SerializeField] private PlayerAimAndShoot playerAimAndShoot;

    private bool isFacingRight = true;
    
    [SerializeField] GameObject sr;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    public static Action OnPlayerStopSliding;

    private bool _canMove;
    private bool _sliding;
    private float slideBoost = 2f;
    
    private bool _flipped = false; // has the player flipped in the last 0.3 seconds
    private bool _BfRunning = false; // Backflipwindow coroutine is running
    private float backflipWindowTimer = 1f;
    //private Coroutine test;

    private void Awake() => Instance = this;

    private void OnEnable()
    {
        ControlsManager.OnPlayerJump += PlayerJump;
        ControlsManager.OnPlayerSlide += PlayerSlide;
        ControlsManager.OnShootRequested += playerAimAndShoot.ShootNormal;
        PlayerManager.OnPlayerStateChanged += HandlePlayerStateChange;
        
    }

    private void OnDisable()
    {
        ControlsManager.OnPlayerJump -= PlayerJump;
        ControlsManager.OnPlayerSlide -= PlayerSlide;

        PlayerManager.OnPlayerStateChanged -= HandlePlayerStateChange;
    }

    private void HandlePlayerStateChange(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.InThirdPerson:
                _canMove = true;
                break;
            case PlayerState.InFirstPerson:
                _canMove = false;
                horizontal = 0f;
                break;
        }
    }

    void Update()
    {
        if (_canMove)
        {
            horizontal = Input.GetAxisRaw("Horizontal");
        }
        
        Flip();

        if (_flipped)
        {
            if (!_BfRunning) StopCoroutine(BackflipWindow());
            StartCoroutine(BackflipWindow());
        }
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
    
    private void PlayerSlide()
    {
        if (!IsGrounded() || _sliding) return;

        StartCoroutine(SlidingMomentum());
    }

    private IEnumerator SlidingMomentum()
    {
        _sliding = true;
        slidingHorizontal = (isFacingRight) ? 1f : -1f;
        slideBoost = 2f;
        Debug.Log("Start slide");

        yield return new WaitForSeconds(0.5f);
        Debug.Log("Slowing down");

        while ((isFacingRight) ? rb.linearVelocity.x > 0f : rb.linearVelocity.x < 0f)
        {
            //Debug.Log(slideBoost);
            //Debug.Log("Hori velocity: " + rb.linearVelocity.x);

            yield return null;
            //slideBoost -= 0.005f;
            CalcSlideBoost();

        }
        //Debug.Log(rb.linearVelocity.x);
        Debug.Log("End slide");
        slideBoost = 2f;
        _sliding = false;

        OnPlayerStopSliding?.Invoke();
    }

    private void CalcSlideBoost()
    {
        //if (rb.linearVelocity.y < 0f) slideBoost += 0.005f;
        //else if (rb.linearVelocity.y == 0f) slideBoost -= 0.005f;
        //else slideBoost -= 0.01f;

        switch (rb.linearVelocity.y)
        {
            case < 0f:
                Debug.Log("downward terrain");
                //slideBoost += 0.005f;
                slideBoost = (slideBoost >= 2f) ? 2f : slideBoost += 0.005f;
                break;
            case 0f:
                Debug.Log("neutral terrain");
                slideBoost -= 0.005f;
                break;
            case > 0f:
                Debug.Log("Upward terrain");
                slideBoost -= 0.01f;
                break;
        }
    }

    private bool IsGrounded()
    {
        return Physics.OverlapSphere(groundCheck.position, 0.2f, groundLayer).Length > 0;
    }

    private void FixedUpdate()
    {
        if (!_sliding) rb.linearVelocity = new Vector3(horizontal * speed, rb.linearVelocity.y);
        else rb.linearVelocity = new Vector3((slidingHorizontal * speed) * slideBoost, rb.linearVelocity.y);
    }

    private void Flip()
    {
        if ((isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f))
        {
            


            isFacingRight = !isFacingRight;

            playerAimAndShoot.facingRight = isFacingRight;
            //Vector3 localScale = transform.localScale;
            //localScale.x *= -1f;
            //transform.localScale = localScale;

            Vector3 spriteScale = sr.transform.localScale;
            spriteScale.x *= -1f;
            sr.transform.localScale = spriteScale;
            //sr.flipX;

            _flipped = true;
        }
    }

    private IEnumerator BackflipWindow()
    {
        Debug.Log("Backflip on");
        _BfRunning = true;
        yield return new WaitForSeconds(backflipWindowTimer);
        Debug.Log("Backflip off");
        _BfRunning = false;
        _flipped = false;
    }
}
