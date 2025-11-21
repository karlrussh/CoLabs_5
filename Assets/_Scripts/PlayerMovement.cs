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

    [SerializeField] private MovementState movementState;
    
    [SerializeField] GameObject sr;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    public static Action OnPlayerStopSliding;

    private bool _canMove;
    //private bool _sliding;
    private float slideBoost = 2f;
    private IEnumerator SlideCoroutine;

    //bool _lunging = false;
    
    private bool _flipped = false; // has the player flipped in the last 0.3 seconds
    private bool _BfRunning = false; // Backflipwindow coroutine is running
    private float backflipWindowTimer = 0.2f;
    private IEnumerator BFCoroutine;
    private float BackflipRotationSpeed = 200f;


    //private Coroutine test;

    private void Awake() => Instance = this;

    private void OnEnable()
    {
        ControlsManager.OnPlayerJump += PlayerJump;
        ControlsManager.OnPlayerSlide += PlayerSlide;
        //ControlsManager.OnShootRequested += playerAimAndShoot.ShootNormal;
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
        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    rb.AddForce(Vector3.right*horizontal * jumpingPower, ForceMode.VelocityChange);
        //}

        if (_canMove)
        {
            
            horizontal = Input.GetAxisRaw("Horizontal");
        }

        
        Flip();

        if (_flipped)
        {
            //BFCoroutine = BackflipWindow();
            if (_BfRunning) { /*Debug.Log("Stopping Coroutine"); */StopCoroutine(BFCoroutine); }
            BFCoroutine = BackflipWindow();
            StartCoroutine(BFCoroutine);
        }
    }

    private void PlayerJump()
    {
        if (!IsGrounded()) return;

        if (movementState == MovementState.Sliding)
        {
            StopCoroutine(SlideCoroutine);
            slideBoost = 2f;
            //_sliding = false;

            OnPlayerStopSliding?.Invoke();
            Debug.Log("Lunging");
            PlayerLunge();
        }
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
        if (_BfRunning)
        {
            //Debug.Log("Backflip");
            StartCoroutine(BackflipRotation());
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpingPower * 2);
            return;
        }
        if (!IsGrounded() || movementState == MovementState.Sliding) return;

        SlideCoroutine = SlidingMomentum();
        StartCoroutine(SlideCoroutine);
    }

    private void PlayerLunge()
    {
        
        rb.AddForce(Vector3.right * horizontal * jumpingPower, ForceMode.VelocityChange);
        StartCoroutine(Lunging());
    }

    private IEnumerator Lunging()
    {
        // _lunging = true;
        movementState = MovementState.Lunging;
        yield return new WaitForSeconds(0.2f);
        while (!IsGrounded())
        {
            yield return null;
        }
        movementState = MovementState.Default;
        //_lunging = false;
    }

    private IEnumerator SlidingMomentum()
    {
        //_sliding = true;
        movementState = MovementState.Sliding;
        slidingHorizontal = (isFacingRight) ? 1f : -1f;
        slideBoost = 2f;
        //Debug.Log("Start slide");

        yield return new WaitForSeconds(0.5f);
        // Debug.Log("Slowing down");

        while ((isFacingRight) ? rb.linearVelocity.x > 0f : rb.linearVelocity.x < 0f)
        {
            //Debug.Log(slideBoost);
            //Debug.Log("Hori velocity: " + rb.linearVelocity.x);

            yield return null;
            //slideBoost -= 0.005f;
            CalcSlideBoost();

        }
        //Debug.Log(rb.linearVelocity.x);
        //Debug.Log("End slide");
        slideBoost = 2f;
        //_sliding = false;
        movementState = MovementState.Default;

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
                //Debug.Log("downward terrain");
                //slideBoost += 0.005f;
                slideBoost = (slideBoost >= 2f) ? 2f : slideBoost += 0.005f;
                break;
            case 0f:
                //Debug.Log("neutral terrain");
                slideBoost -= 0.005f;
                break;
            case > 0f:
                //Debug.Log("Upward terrain");
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
        switch (movementState)
        {
            case MovementState.Default:
                rb.linearVelocity = new Vector3(horizontal * speed, rb.linearVelocity.y);
                break;
            case MovementState.Sliding:
                rb.linearVelocity = new Vector3((slidingHorizontal * speed) * slideBoost, rb.linearVelocity.y);
                break;
            case MovementState.Lunging:
                Debug.Log("loonge");
                break;
        }
        
            if (movementState != MovementState.Sliding) rb.linearVelocity = new Vector3(horizontal * speed, rb.linearVelocity.y);
            else rb.linearVelocity = new Vector3((slidingHorizontal * speed) * slideBoost, rb.linearVelocity.y);
        
    }

    private void Flip()
    {
        Vector3 playerScreenPos = Camera.main.WorldToScreenPoint(transform.position);
        float flipDir = (Input.mousePosition.x > playerScreenPos.x) ? 1f : -1f;

        if ((isFacingRight && flipDir < 0f || !isFacingRight && flipDir > 0f))
        {
            


            isFacingRight = !isFacingRight;

            //playerAimAndShoot.facingRight = isFacingRight;
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
        _flipped = false;

        //Debug.Log("Backflip on");
        _BfRunning = true;
        yield return new WaitForSeconds(backflipWindowTimer);
        //Debug.Log("Backflip off");
        _BfRunning = false;
        //_flipped = false;
    }

    private IEnumerator BackflipRotation()
    {
        float flippedRotationSpeed = isFacingRight ? BackflipRotationSpeed : BackflipRotationSpeed*-1;
        yield return new WaitForSeconds(0.2f);
        Debug.Log("Start rotation");

        while (!IsGrounded())
        {
            sr.transform.Rotate(Vector3.forward * flippedRotationSpeed * Time.deltaTime);
            yield return null;
        }
        
        OnPlayerStopSliding?.Invoke();
        sr.transform.rotation = Quaternion.identity;
        Debug.Log("End rotation");
    }

    
}

public enum MovementState
{
    Default,
    Sliding,
    Lunging
}