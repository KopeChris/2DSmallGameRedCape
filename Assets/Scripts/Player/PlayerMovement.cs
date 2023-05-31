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

    bool halfYspeed;
    public GameObject platformsOnly;

    public float dashIseconds;
    public CapsuleCollider2D PlayerHurtBox;
    //public CapsuleCollider2D CollisionBlocker;

    [SerializeField]
    public float bufferTime = 0.2f;
    private float jumpBufferCounter;
    private float dropplatformsBuffer;
    private float attackBufferCounter;
    private float dashBufferCounter;
    private float healBufferCounter;

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

        //inputs
        if (Input.GetKeyDown(KeyCode.Space) && Input.GetAxisRaw("Vertical") >= 0)
            jumpBufferCounter = bufferTime;
        else if (jumpBufferCounter >= 0)
            jumpBufferCounter -= Time.deltaTime;
        if (jumpBufferCounter >= 0 && (grounded || coyoteTimer > 0) && !stunned)
        {
            animator.SetTrigger("Jump");
            jumpBufferCounter = 0;
        }
                
        if (grounded)
            halfYspeed = true;
        if (!Input.GetKey(KeyCode.Space) && rb.velocity.y<12 && rb.velocity.y > 1 && halfYspeed)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 2);
            halfYspeed = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && Input.GetAxisRaw("Vertical") < 0)
            dropplatformsBuffer = bufferTime;
        else if (dropplatformsBuffer >= 0)
            dropplatformsBuffer -= Time.deltaTime;
        if (dropplatformsBuffer >= 0 && !stunned)
        {
            platformsOnly.SetActive(false);
            Invoke("ActivatePlatformsOnly", 0.15f);
            dropplatformsBuffer = 0;
        }

        if (Input.GetButtonDown("Attack"))
            attackBufferCounter = bufferTime;
        else if (attackBufferCounter >= 0)
            attackBufferCounter -= Time.deltaTime;
        if (attackBufferCounter > 0 && grounded && !stunned)
        {
            animator.SetTrigger("Attack");
            attackBufferCounter = 0;
        }

        if (Input.GetButtonDown("Dash"))     //Dash button
            dashBufferCounter = bufferTime;
        else if (dashBufferCounter >= 0)
            dashBufferCounter -= Time.deltaTime;
        if (dashBufferCounter >= 0 && !stunned && grounded)    //&& dashTimer <= 0
            Dash();



        //gravity changes
        if (!grounded && rb.velocity.y < 1f && rb.velocity.y > -12f)
        {
            rb.velocity += (fallMultiplier) * 9.8f * -rb.gravityScale * Time.deltaTime * Vector2.up;

        }

        if (rb.velocity.y > -16)
            rb.gravityScale = 2.3f;
        else if (rb.velocity.y < -16)
            rb.gravityScale = 0;

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
        /*
        if (dashTimer > 0f)
            dashTimer -= Time.fixedDeltaTime;   */

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

        if ((horizontalInput < 0 && facingRight) || (horizontalInput > 0 && !facingRight))
            Flip();

        Invincible();
        Invoke(nameof(NotInvincible), dashIseconds);
        dashBufferCounter = 0;
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
