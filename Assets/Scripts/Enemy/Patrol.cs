using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    public float speed;
    Rigidbody2D rb;
    bool facingRight;
    int facingDirection;

    public float acceleration;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2 (Mathf.Lerp(rb.velocity.x,speed, acceleration), rb.velocity.y);
    }
}
