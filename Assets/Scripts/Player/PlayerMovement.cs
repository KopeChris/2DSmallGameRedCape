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
    public float fallMultiplier;
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

    public GameObject platformsOnly;

    public float dashIseconds;
    public CapsuleCollider2D PlayerHurtBox;
    //public CapsuleCollider2D CollisionBlocker;

    public float jumpHeight;
    public float jumpForce;

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

        //charges jump
        if (Input.GetKey(KeyCode.Space) && (grounded || coyoteTimer > 0) && Input.GetAxisRaw("Vertical") >= 0)
        {
            jumpHeight += 0.1f;
            if (jumpHeight >= 1.0f)
                jumpHeight = 1.0f;
            if (jumpHeight <= 0.8f)
                jumpForce = 0.8f;
            else
                jumpForce = 1;
        }

        //actually jumps
        if (Input.GetKeyDown(KeyCode.Space) && (grounded || coyoteTimer > 0) && !stunned && Input.GetAxisRaw("Vertical") >= 0)
        {
            animator.SetTrigger("Jump");
        }


        if (Input.GetKeyDown(KeyCode.Space) && !stunned && Input.GetAxisRaw("Vertical") < 0)
        {
            platformsOnly.SetActive(false);
            Invoke("ActivatePlatformsOnly", 0.15f);

        }

        if (Input.GetButtonDown("Attack") && grounded && !stunned)
        {
            animator.SetTrigger("Attack");
        }

        if (Input.GetButtonDown("Dash") && dashTimer <= 0 && !stunned && grounded)     //Dash button
        {
            Dash();
        }

        if (rb.velocity.y < -12f)
            animator.SetBool("FullLand", true);


        if (!grounded)    //gravity changes
        {
            if (rb.velocity.y < 1f && rb.velocity.y > -12f)
            {
                rb.velocity += (fallMultiplier) * 9.8f * -rb.gravityScale * Time.deltaTime * Vector2.up;
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


        else if (grounded)     //friction no input ground or stunned or dash friction
        {
            if (!dashing)
                velocityX = Mathf.MoveTowards(rb.velocity.x, 0f, Time.fixedDeltaTime * deceleration);
            else
                velocityX = Mathf.MoveTowards(rb.velocity.x, 0f, Time.fixedDeltaTime * dashDeceleration);

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

    private void Dash()
    {
        animator.SetTrigger("Dash");
        dashing = true;
        dashTimer = dashCooldown;
        if (horizontalInput == 0)
            rb.velocity = Vector2.right * dashSpeed * facingDirection;        //maybe lerp and a few frames like the acc would be dashSpeed/number of frames to start
        else
            rb.velocity = Vector2.right * dashSpeed * horizontalInput;

        Invincible();
        Invoke(nameof(NotInvincible), dashIseconds);
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

    void ActivatePlatformsOnly()
    {
        platformsOnly.SetActive(true);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }


    void Invincible()
    {
        PlayerHurtBox.enabled = false;
        //CollisionBlocker.enabled = false;
    }
    void NotInvincible()
    {
        PlayerHurtBox.enabled = true;
        //CollisionBlocker.enabled = true;
    }

}
