using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityComponent : MonoBehaviour
{
    public float gravity = 100f;

    private float editGravity;
    private float gravityApplied;

    private Rigidbody2D rb;
    void Start()
    {
        editGravity = gravity;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        editGravity = gravity;
        gravityApplied = editGravity * 100 * -1;
        rb.AddForce(new Vector2(0, gravityApplied) * Time.deltaTime);
    }
    public void setGravity(float multiplier)
    {
        editGravity *= gravity * multiplier;
    }
}
