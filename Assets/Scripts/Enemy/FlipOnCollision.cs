using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipOnCollision : MonoBehaviour
{
    public float speed = 2f;
    private Rigidbody2D rb;
    private bool facingRight = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        if (facingRight)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Flip();
    }

    private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }
}
