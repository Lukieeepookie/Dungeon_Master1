using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityComponent : MonoBehaviour
{
    public float gravity = 100f;
    public float maxFallingSpeed = -25f;

    private float gravityApplied;
    public float gravityMult = 1f;

    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        gravityApplied = -gravity * gravityMult * Time.deltaTime;
        float velocityApplied = rb.velocity.y + (gravityApplied);
        if (velocityApplied < maxFallingSpeed)
        {
            gravityApplied = 0f;
        }
        rb.velocity += new Vector2(0, gravityApplied);
    }
    public void setGravityMultiplier(float multiplier)
    {
        gravityMult = multiplier;
    }
}
