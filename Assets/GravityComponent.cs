using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityComponent : MonoBehaviour
{
    public float gravity = 10;
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.AddForce(new Vector2(0, -gravity*100) * Time.deltaTime);
    }
}
