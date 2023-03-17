using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    public float speed = 2f;
    private Rigidbody2D rb;
    private bool facingRight = false;
    Animator animator;
    public bool stunned;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!stunned)
        {

            if (facingRight)
            {
                rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, speed, 0.15f), rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, -speed, 0.15f), rb.velocity.y);
            }
        }
        else
            rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, 0.15f), rb.velocity.y);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") && !other.CompareTag("Enemy"))
            Flip();
    }

    private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }
}
