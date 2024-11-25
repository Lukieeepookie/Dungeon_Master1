using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerComponent : MonoBehaviour
{
    #region Public Variables

    public float maxSpeed = 10f;
    public float AccRate = 100f;
    public float DecelRate = 100f;
    public float airAcceleration = 50f;
    public float jumpLeniency = 0.1f;
    public float jumpForce = 16f;
    public float minJumpTime = 0.25f;
    public float airTimeLeniency = 1f;
    public float airTimeGravity = 0.5f;
    public float airTimeBoost = 2f;
    public float CoyoteTime = 0.1f;

    public Vector3 respawn;

    public Transform groundedCheck;
    public LayerMask groundLayer;

    #endregion
    #region Private Variables

    private Rigidbody2D rb;
    private GravityComponent gravity;

    private float boundaryY = -10;
    private bool isFacingRight = true;
    private float movementX = 0f;
    private float appliedVelocity = 0f;
    private float coyoteTimer = 0f;

    private float jumpTimer = 0f;
    private float jumpingTimer = 0f;

    private bool jumpTry = false;
    private bool jumping = false;

    private bool heavyGravity = false;
    private bool airTime = false;
    private bool grounded = false;

    #endregion
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gravity = GetComponent<GravityComponent>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (coyoteTimer > CoyoteTime)
        {
            grounded = false;
        }
        else
        {
            coyoteTimer += Time.deltaTime;
        }
        if (IsGrounded())
        {
            grounded = true;
            coyoteTimer = 0f;
        }
        this.transform.rotation = Quaternion.identity; //keep player rotation
        if (this.transform.position.y < boundaryY) //reset player position
        {
            this.transform.position = respawn;
            this.rb.velocity = new Vector2(0, 0);
        }
        Flip();
        jump();
        gravity.gravityMult = (heavyGravity ? 1.5f : 1f);
        if (airTime)
        {
            gravity.gravityMult = airTimeGravity;
        }
    }
    void FixedUpdate()
    {
        movement();
    }

    void Flip()
    {
        bool pastFacingRight = isFacingRight;
        if (movementX > 0f)
        {
            isFacingRight = true;
        }
        else if (movementX < 0f)
        {
            isFacingRight = false;
        }
        if (pastFacingRight != isFacingRight)
        {
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
    void jump()
    {
        print("Jumping : " + jumping);
        bool jumped = false;
        if (jumpTimer > jumpLeniency)
        {
            jumpTry = false;
        }
        else
        {
            jumpTimer += Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            jumpTimer = 0f;
            jumpTry = true;
        }
        if (jumpTry && grounded)
        {
            jumping = true;
            jumped = true;
        }
        if (jumping)
        {
            if (jumped)
            {
                jumpingTimer = 0f;
                jumped = false;
                rb.velocity = new Vector2(this.rb.velocity.x, jumpForce);
            }
            if (!Input.GetKey(KeyCode.Z) && jumpingTimer > minJumpTime || this.rb.velocity.y < 0f)
            {
                if (this.rb.velocity.y > 0.5f)
                {
                    this.rb.velocity = new Vector2(this.rb.velocity.x,0.5f);
                }
                heavyGravity = true;
            }
            if (this.rb.velocity.y > -airTimeLeniency && this.rb.velocity.y < airTimeLeniency)
            {
                airTime = true;
            }
            else
            {
                airTime = false;
            }
            if (IsGrounded() && this.rb.velocity.y <= 0f)
            {
                airTime = false;
                jumping = false;
                heavyGravity = false;
            }
            jumpingTimer += Time.deltaTime;
        }
    }
    void movement()
    {
        if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
        {
            if (movementX < 0f)
            {
                appliedVelocity = 0f;
            }
            else
            {
                if (IsGrounded())
                {
                    appliedVelocity = movementX + (AccRate * Time.deltaTime);
                }
                else
                {
                    if (airTime)
                    {
                        appliedVelocity = movementX + (airAcceleration * Time.deltaTime * airTimeBoost);
                    }
                    else
                    {
                        appliedVelocity = movementX + (airAcceleration * Time.deltaTime);
                    }
                }
            }
            if (appliedVelocity > maxSpeed)
            {
                appliedVelocity = maxSpeed;
            }
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
        {
            if (movementX > 0f)
            {
                appliedVelocity = 0f;
            }
            else
            {
                if (IsGrounded())
                {
                    appliedVelocity = movementX - (AccRate * Time.deltaTime);
                }
                else
                {
                    if (airTime)
                    {
                        appliedVelocity = movementX - (airAcceleration * Time.deltaTime * airTimeBoost);
                    }
                    else
                    {
                        appliedVelocity = movementX - (airAcceleration * Time.deltaTime);
                    }
                }
            }
            if (appliedVelocity < -maxSpeed)
            {
                appliedVelocity = -maxSpeed;
            }
        }
        else
        {
            if (movementX > 0f)
            {
                appliedVelocity = movementX - (DecelRate * Time.deltaTime);
                if (appliedVelocity < 0f)
                {
                    appliedVelocity = 0f;
                }
            }
            else if (movementX < 0f)
            {
                appliedVelocity = movementX + (DecelRate * Time.deltaTime);
                if (appliedVelocity > 0f)
                {
                    appliedVelocity = 0f;
                }
            }
        }
        this.rb.velocity = new Vector2(appliedVelocity, this.rb.velocity.y);
        movementX = this.rb.velocity.x;
    }
    bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundedCheck.position, 0.2f, groundLayer);
    }
}
