using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    private Animator _anim;

    public Transform groundCheck;
    public Transform wallCheck;

    private MineController _mineController;
    private GameController _gameController;
    
    public LayerMask whatIsGround;

    // adjust angle of jump direction
    public Vector2 wallHopDirection;
    public Vector2 wallJumpDirection;

    public int amountOfJumps = 1;
    private int _amountOfJumpsLeft;
    private int _facingDirection = 1;
    private int _lastWallJumpDirection;

    private float _movementInputDirection;
    private float _jumpTimer;
    private float _turnTimer;
    private float _wallJumpTimer;

    public float movementSpeed = 10.0f;
    public float jumpForce = 16.0f;

    private bool _isFacingRight;

    private bool _isSwimming;
    private bool _isGrounded;
    private bool _canNormalJump;
    private bool _canWallJump;
    private bool _isTouchingWall;
    private bool _isWallSliding;
    private bool _isAttemptingToJump;
    private bool _checkJumpMultiplier;
    private bool _canMove;
    private bool _canFlip;
    private bool _hasWallJumped;

    public float groundCheckRadius;
    public float wallCheckDistance;
    public float wallSlideSpeed;
    public float airDragMultiplier = 0.95f;
    public float variableJumpHeightMultiplier = 0.5f;
    public float wallJumpForce;
    public float jumpTimerSet = 0.15f;
    public float turnTimerSet = 0.1f;
    public float wallJumpTimerSet = 0.5f;


    // Start is called before the first frame update
    void Start()
    {
        rb.GetComponent<Rigidbody2D>();

        _anim = GetComponent<Animator>();

        _mineController = GameObject.Find("mine").GetComponent<MineController>();
        _gameController = GameObject.Find("GameManager").GetComponent<GameController>();
        
        _amountOfJumpsLeft = amountOfJumps;

        wallHopDirection.Normalize();
        wallJumpDirection.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        CheckMovementDirection();
        UpdateAnimations();
        CheckIfCanJump();
        CheckIfWallSliding();
        CheckJump();
    }

    private void CheckIfWallSliding()
    {
        if (_isTouchingWall && Math.Abs(_movementInputDirection - _facingDirection) < 0.0000001 && rb.velocity.y < 0)
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
            if (_isGrounded || (_amountOfJumpsLeft > 0 && _isTouchingWall))
            {
                NormalJump();
            }
            else
            {
                _jumpTimer = jumpTimerSet;
                _isAttemptingToJump = true;
            }
        }

        if (Input.GetButtonDown("Horizontal") && _isTouchingWall)
        {
            if (!_isGrounded && Math.Abs(_movementInputDirection - _facingDirection) > 0.0000001)
            {
                _canMove = false;
                _canFlip = false;

                _turnTimer = turnTimerSet;
            }
        }

        if (!_canMove)
        {
            _turnTimer -= Time.deltaTime;
            if (_turnTimer <= 0)
            {
                _canMove = true;
                _canFlip = true;
            }
        }

        if (_checkJumpMultiplier && !Input.GetButton("Jump"))
        {
            _checkJumpMultiplier = false;
            var velocity = rb.velocity;
            velocity = new Vector2(velocity.x, velocity.y * variableJumpHeightMultiplier);
            rb.velocity = velocity;
        }
    }


    private void ApplyMovement()
    {
        if (!_isGrounded && !_isWallSliding && _movementInputDirection == 0)
        {
            var velocity = rb.velocity;
            velocity = new Vector2(velocity.x * airDragMultiplier, velocity.y);
            rb.velocity = velocity;
        }

        else if (_canMove)
        {
            rb.velocity = new Vector2(movementSpeed * _movementInputDirection, rb.velocity.y);
        }

        if (_isWallSliding)
        {
            if (rb.velocity.y < -wallSlideSpeed)
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
        if (!_isWallSliding && _canFlip)
        {
            _facingDirection *= -1;
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

        //Debug.Log(_isTouchingWall);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        var position = wallCheck.position;
        Gizmos.DrawLine(position, new Vector3(position.x + wallCheckDistance, position.y, position.z));
    }


    private void CheckJump()
    {
        if (_jumpTimer > 0)
        {
            // Wall Jump
            if (!_isGrounded && _isTouchingWall && _movementInputDirection != 0 &&
                Math.Abs(_movementInputDirection - _facingDirection) > 0.0000001)
            {
                WallJump();
            }
            else if (_isGrounded)
            {
                NormalJump();
            }
        }

        if (_isAttemptingToJump)
        {
            _jumpTimer -= Time.deltaTime;
        }

        if (_wallJumpTimer > 0)
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (_hasWallJumped && _movementInputDirection == -_lastWallJumpDirection)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0.0f);
                _hasWallJumped = false;
            }
            else if (_wallJumpTimer <= 0)
            {
                _hasWallJumped = false;
            }
            else
            {
                _wallJumpTimer -= Time.deltaTime;
            }
        }
    }

    private void NormalJump()
    {
        if (_canNormalJump && !_isWallSliding)
        {
            //  rb.AddForce(Physics.gravity * (jumpForce - 1) * rb.mass);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            _amountOfJumpsLeft--;
            _jumpTimer = 0;
            _isAttemptingToJump = false;
            _checkJumpMultiplier = true;
        }
    }

    private void WallJump()
    {
        if (_canWallJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0.0f);
            _isWallSliding = false;
            _amountOfJumpsLeft = amountOfJumps;
            _amountOfJumpsLeft--;

            Vector2 forceToAdd = new Vector2(wallJumpForce * wallJumpDirection.x * _movementInputDirection,
                wallJumpForce * wallJumpDirection.y);

            rb.AddForce(forceToAdd, ForceMode2D.Impulse);

            _jumpTimer = 0;
            _isAttemptingToJump = false;
            _checkJumpMultiplier = true;
            _turnTimer = 0;
            _canMove = true;
            _canFlip = true;
            _hasWallJumped = true;
            _wallJumpTimer = wallJumpTimerSet;
            _lastWallJumpDirection = -_facingDirection;
        }
    }

    private void CheckIfCanJump()
    {
        if (_isGrounded && rb.velocity.y <= 0.01f)
        {
            _amountOfJumpsLeft = amountOfJumps;
        }

        if (_isTouchingWall)
        {
            _canWallJump = true;
        }

        _canNormalJump = _amountOfJumpsLeft > 0;
        // Debug.Log(_amountOfJumpsLeft);
    }
    
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Mine"))
        {
            StartCoroutine(StartTimer(col.gameObject));
        }
    }

    private IEnumerator StartTimer(GameObject mine)
    {
        movementSpeed = 0;
        yield return new WaitForSeconds(1.5f);
        _mineController.Explode(mine);
        gameObject.GetComponent<Renderer>().enabled = false;
        yield return new WaitForSeconds(1f);
        if(_gameController!=null) _gameController.GameOver();
        else
        { 
            Debug.Log("Collided with Mine. Universal GameOver() method cannot be called. Reason: GameController Script not found.");
        }
    }
}