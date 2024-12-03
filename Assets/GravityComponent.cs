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
        gravityApplied = -gravity * gravityMult;
        float velocityApplied = rb.velocity.y + (gravityApplied * Time.deltaTime);
        if (velocityApplied < maxFallingSpeed)
        rb.velocity += new Vector2(0, gravityApplied*Time.deltaTime);
    }
    public void setGravityMultiplier(float multiplier)
    {
        gravityMult = multiplier;
    }
}
