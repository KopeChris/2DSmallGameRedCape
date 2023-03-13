using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float acceleration = 30f;
    public float deceleration = 30f;
    public float airDeceleration = 10f;
    public float groundCheckRadius = 0.2f;

    public float coyoteDuration = 0.2f;
    public float apexHeight = 2f;
    public float fallMultiplier = 2.5f;
    [Space(5)]
    public float dashSpeed = 10f;  // dash speed
    public float dashCooldown = 1f; // cooldown time for dash
    public float dashDeceleration = 10f;
    [Space(5)]

    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform groundCheck;
    [SerializeField] Animator animator;

    Rigidbody2D rb;
    public bool stunned;
    public bool dashing = false;
    bool grounded;
    bool facingRight = true;
    int facingDirection;
    public static float posX;
    public static float posY;
    float horizontalInput;
    [HideInInspector]
    public float velocityX;
    float coyoteTimer = 0f;
    float dashTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        animator.SetFloat("Speed", Mathf.Abs(horizontalInput));
        animator.SetFloat("SpeedY", rb.velocity.y);
        animator.SetBool("Grounded", grounded);

        posX = transform.position.x;
        posY = transform.position.y;

        facingDirection = facingRight ? 1 : -1;


        if (Input.GetKeyDown(KeyCode.Space) && (grounded || coyoteTimer > 0) && !stunned)
        {
            animator.SetTrigger("Jump");
        }

        if (Input.GetButtonDown("Attack") && grounded && !stunned)
        {
            animator.SetTrigger("Attack");
        }

        if (Input.GetButtonDown("Dash") && dashTimer <= 0 && !stunned && grounded)     //Dash button
        {
            animator.SetTrigger("Dash");
            dashTimer = dashCooldown;
            velocityX = dashSpeed * facingDirection;        //maybe lerp and a few frames like the acc would be dashSpeed/number of frames to start
        }


        if (!grounded)    //gravity changes
        {
            if (rb.velocity.y < 1f && rb.velocity.y > -10f)
            {
                rb.velocity += Vector2.up * -rb.gravityScale * 9.8f * (fallMultiplier - 1f) * Time.deltaTime;
            }
            else if (rb.velocity.y > 1f && !Input.GetButton("Jump"))
            {   // smaller jump if not holding jump one would have no difference, less would make you jump more i
                rb.velocity += Vector2.up * -rb.gravityScale * 9.8f * (apexHeight - 1) * Time.deltaTime;
            }
        }
    }

    void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (horizontalInput < 0f && facingRight && rb.velocity.x < 0 && grounded && !stunned)
        {
            Flip();
        }
        else if (horizontalInput > 0f && !facingRight && rb.velocity.x > 0 && grounded && !stunned)
        {
            Flip();
        }

        //if input and not stunned
        if (horizontalInput != 0f && !stunned && !dashing && Mathf.Abs(rb.velocity.x) <= speed)
        {
            velocityX = Mathf.MoveTowards(rb.velocity.x, speed * horizontalInput, Time.fixedDeltaTime * acceleration);
        }


        else if (grounded)     //if no input ground  or is stunned friction
        {
            if (!dashing)
                velocityX = Mathf.MoveTowards(rb.velocity.x, 0f, Time.fixedDeltaTime * deceleration);
            else
                velocityX = Mathf.MoveTowards(velocityX, 0f, Time.fixedDeltaTime * dashDeceleration);
        }

        else if (!grounded)   //air friction
        {
            velocityX = Mathf.MoveTowards(rb.velocity.x, 0f, Time.fixedDeltaTime * airDeceleration);

        }



        //sets the speed
        rb.velocity = new Vector2(velocityX, rb.velocity.y);

        if (coyoteTimer > 0f && !grounded)        //timer gets to duration when it hits the ground but doesnt get lower until off the ground
        {
            coyoteTimer -= Time.fixedDeltaTime;
        }
        if (dashTimer > 0f)
            dashTimer -= Time.fixedDeltaTime;

    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        // If the collision is with an object on the ground layer
        if (groundLayer == (groundLayer | (1 << other.gameObject.layer)))
        {
            grounded = true;
            coyoteTimer = coyoteDuration;
        }
    }


    private void OnDrawGizmos()
    {
        // Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }


}
