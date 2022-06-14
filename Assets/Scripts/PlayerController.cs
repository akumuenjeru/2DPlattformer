using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    private Animator _anim;
    public Transform groundCheck;

    public int amountOfJumps = 1;
    private int _amountOfJumpsLeft;

    private float _movementInputDirection;
    public float movementSpeed = 10.0f;
    public float jumpForce = 16.0f;

    private bool _isFacingRight;
    private bool _isSwimming;
    private bool _isGrounded;
    private bool _canJump;

    public float groundCheckRadius;
    public LayerMask whatIsGround;

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
    }



    private void ApplyMovement()
    {
        rb.velocity = new Vector2(movementSpeed * _movementInputDirection, rb.velocity.y);
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
        _isFacingRight = !_isFacingRight;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    private void UpdateAnimations()
    {
        _anim.SetBool("isSwimming", _isSwimming);
        _anim.SetBool("isGrounded", _isGrounded);
        _anim.SetFloat("yVelocity", rb.velocity.y);
        // Debug.Log(_anim.GetParameter(0).name);
    }

    private void CheckSurroundings()
    {
        _isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        //  Debug.Log(_isGrounded);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

  
    private void Jump()
    {
        if (_canJump)
        {
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
        
        Debug.Log(_amountOfJumpsLeft);
    }
}