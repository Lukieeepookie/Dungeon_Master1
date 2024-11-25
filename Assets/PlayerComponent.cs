using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerComponent : MonoBehaviour
{
    #region Public Variables

    public float maxSpeed = 10f;
    public float AccRate = 10f;
    public float DecelRate = 15f;
    public float jumpLeniency = 0.1f;
    public float jumpForce = 10f;
    public int jumpCount = 2;
    public float gravityApex = 0.2f;

    public Vector3 respawn;

    public Transform groundedCheck;
    public LayerMask groundLayer;

    #endregion
    #region Private Variables

    private Rigidbody2D rb;
    private GravityComponent gravity;

    private int jumps = 0;

    private float boundaryY = -10;
    private bool isFacingRight = true;
    private float movementX;
    private float moveDirection = 0;
    private float appliedVelocity = 0f;
    private float jumpTimer = 0f;
    private bool jumping = false;

    private bool lightGravity = false;

    private float gravityTimer = 0f;

    #endregion
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gravity = GetComponent<GravityComponent>();
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.rotation = Quaternion.identity;
        if (this.transform.position.y < boundaryY)
        {
            this.transform.position = respawn;
            this.rb.velocity = Vector2.zero;
        }
        movementX = rb.velocity.x;
        #region Inputs
        if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
        {
            moveDirection = 1f;
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
        {
            moveDirection = -1f;
        }
        else
        {
            moveDirection = 0f;
        }
        if (jumpTimer > jumpLeniency)
        {
            jumping = false;
        }
        else
        {
            jumpTimer += Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.Z) && jumps > 0)
        {
            jumps -= 1;
            jumpTimer = 0;
            jumping = true;
        }

        #endregion
        Flip();
        #region Left & Right Movement
        appliedVelocity = movementX + (moveDirection * AccRate * Time.deltaTime);
        if (appliedVelocity > maxSpeed)
        {
            appliedVelocity = maxSpeed;
        }
        else if (appliedVelocity < -maxSpeed)
        {
            appliedVelocity = -maxSpeed;
        }
        if (moveDirection == 1f && movementX < 0f)
        {
            appliedVelocity = 0f;
        }
        else if (moveDirection == -1f && movementX > 0f)
        {
            appliedVelocity = 0f;
        }
        if (moveDirection == 0)
        {
            if (movementX > 0f)
            {
                appliedVelocity = movementX - (DecelRate * Time.deltaTime);
                if (appliedVelocity <= 0f)
                {
                    appliedVelocity = 0f;
                }
            }
            else if (movementX < 0f)
            {
                appliedVelocity = movementX + (DecelRate * Time.deltaTime);
                if (appliedVelocity >= 0f)
                {
                    appliedVelocity = 0f;
                }
            }
        }

        this.rb.velocity = new Vector2(appliedVelocity, this.rb.velocity.y);
        #endregion

        #region Jump Movement
        if (IsGrounded() && !jumping)
        {
            jumps = jumpCount;
        }
        if (jumping && IsGrounded())
        {
            jumping = false;
            rb.velocity = new Vector2(this.rb.velocity.x, jumpForce);
            lightGravity = true;
        }
        if (Input.GetKeyUp(KeyCode.Z) || !IsGrounded() && this.rb.velocity.y <=0)
        {
            lightGravity = false;
        }

        gravity.setGravityMultiplier(lightGravity ? 0.5f : 1f);

        #endregion
    }

    void Flip()
    {
        if (isFacingRight && moveDirection < 0f || !isFacingRight && moveDirection > 0f)
        {
            isFacingRight = !isFacingRight;
            print(isFacingRight);
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
    
    bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundedCheck.position, 0.2f, groundLayer);
    }
}
