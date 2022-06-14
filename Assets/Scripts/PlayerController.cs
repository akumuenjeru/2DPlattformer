using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    private Animator _anim;

    public Transform groundCheck;
    public Transform wallCheck;

    public LayerMask whatIsGround;


    public int amountOfJumps = 1;
    private int _amountOfJumpsLeft;

    private float _movementInputDirection;
    public float movementSpeed = 10.0f;
    public float jumpForce = 16.0f;

    private bool _isFacingRight;

    private bool _isSwimming;
    private bool _isGrounded;
    private bool _canJump;
    private bool _isTouchingWall;
    private bool _isWallSliding;

    public float groundCheckRadius;
    public float wallCheckDistance;
    public float wallSlideSpeed;
    public float movementForceInAir;
    public float airDragMultiplier = 0.95f;
    public float variableJumpHeightMultiplier = 0.5f;


    // Start is called before the first frame update
    void Start()
    {
        rb.GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _amountOfJumpsLeft = amountOfJumps;
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        CheckMovementDirection();
        UpdateAnimations();
        CheckIfCanJump();
        CheckIfWallSliding();
    }

    private void CheckIfWallSliding()
    {
        if (_isTouchingWall && !_isGrounded && rb.velocity.y < 0)
        {
            _isWallSliding = true;
        }
        else
        {
            _isWallSliding = false;
        }
    }


    private void FixedUpdate()
    {
        ApplyMovement();
        CheckSurroundings();
    }

    private void CheckInput()
    {
        _movementInputDirection = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        if (Input.GetButtonUp("Jump"))
        {
            var velocity = rb.velocity;
            velocity = new Vector2(velocity.x, velocity.y * variableJumpHeightMultiplier);
            rb.velocity = velocity;
        }
    }


    private void ApplyMovement()
    {
        if (_isGrounded)
        {
            rb.velocity = new Vector2(movementSpeed * _movementInputDirection, rb.velocity.y);
        }
        else if (!_isGrounded && !_isWallSliding && _movementInputDirection != 0)
        {
            Vector2 forceToAdd = new Vector2(movementForceInAir * _movementInputDirection, 0);
            rb.AddForce(forceToAdd);
            if (Mathf.Abs(rb.velocity.x) > movementSpeed)
            {
                rb.velocity = new Vector2(movementSpeed * _movementInputDirection, rb.velocity.y);
            }
        }
        else if (!_isGrounded && !_isWallSliding && _movementInputDirection == 0)
        {
            var velocity = rb.velocity;
            velocity = new Vector2(velocity.x * airDragMultiplier, velocity.y);
            rb.velocity = velocity;
        }

        {
        }


        if (_isWallSliding)
        {
            if (rb.velocity.y < wallSlideSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
            }
        }
    }


    /* Flips character depending on his direction */
    private void CheckMovementDirection()
    {
        if (!_isFacingRight && _movementInputDirection < 0)

        {
            Flip();
        }
        else if (_isFacingRight && _movementInputDirection > 0)
        {
            Flip();
        }

        // check if character moves
        _isSwimming = rb.velocity.x != 0;
    }

    private void Flip()
    {
        if (!_isWallSliding)
        {
            _isFacingRight = !_isFacingRight;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }
    }

    private void UpdateAnimations()
    {
        _anim.SetBool("isSwimming", _isSwimming);
        _anim.SetBool("isGrounded", _isGrounded);
        _anim.SetFloat("yVelocity", rb.velocity.y);
        _anim.SetBool("isWallSliding", _isWallSliding);
        // Debug.Log(_anim.GetParameter(0).name);
    }

    private void CheckSurroundings()
    {
        _isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        _isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround);

        Debug.Log(_isTouchingWall);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        var position = wallCheck.position;
        Gizmos.DrawLine(position, new Vector3(position.x + wallCheckDistance, position.y, position.z));
    }


    private void Jump()
    {
        if (_canJump)
        {
          //  rb.AddForce(Physics.gravity * (jumpForce - 1) * rb.mass);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            _amountOfJumpsLeft--;
        }
    }

    private void CheckIfCanJump()
    {
        if (_isGrounded && rb.velocity.y <= 0)
        {
            _amountOfJumpsLeft = amountOfJumps;
        }

        _canJump = _amountOfJumpsLeft > 0;

        // Debug.Log(_amountOfJumpsLeft);
    }
}