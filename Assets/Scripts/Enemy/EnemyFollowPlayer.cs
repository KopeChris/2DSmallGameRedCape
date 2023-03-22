using UnityEngine;

public class EnemyFollowPlayer : MonoBehaviour
{
    public bool stunned;
    public bool run;

    [Header("Movement")]
    public float speed = 3;
    public float acceleration;
    public float deceleration;
    public bool facingRight = true;
    public int facingDirection;
    public int playerDirectionX;

    [Header("Attacks")]
    public bool playerDetected;
    public float attackRange = 1;

    [Space(10)]
    public SpriteRenderer sprite;
    public Animator animator;
    public EnemyHealth health;
    public Rigidbody2D rb;
    public LayerMask targetLayer;

    [Header("Gizmos Parameters")]
    public float sightRadius;
    public float followRadius;
    public Transform SightPositionSphere;
    public Transform Attack;

    [SerializeField]
    public bool playerSeen { get; internal set; }
    public bool playerFollowed { get; internal set; }
    public bool PlayerInRange { get; internal set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        animator.SetBool("Run", false);
    }

    void Update()
    {
        if (stunned)
        {
            animator.ResetTrigger("Attack");
            animator.SetBool("Run", false);
        }

        if (facingRight) { facingDirection = 1; } else { facingDirection = -1; }     //facing depends on x movement direction
        if (PlayerMovement.posX > rb.transform.position.x) { playerDirectionX = 1; } else { playerDirectionX = -1; }

        //seen
        var seen = Physics2D.OverlapCircle(SightPositionSphere.position, sightRadius, targetLayer);
        playerSeen = seen != null;

        //detected = seen or enemy hurt
        if ((playerSeen || health.health < health.maxHealth) && !stunned && Mathf.Abs(PlayerMovement.posX - transform.position.x) > 1)
        {
            animator.SetBool("Run", true);
            playerDetected = true;

        }

        //stop follow if out of follow range
        var follow = Physics2D.OverlapCircle(SightPositionSphere.position, followRadius, targetLayer);
        playerFollowed = follow != null;
        if (playerFollowed == false || Mathf.Abs(PlayerMovement.posX - transform.position.x) < 1)
        {
            animator.SetBool("Run", false);
            playerDetected = false;

        }

        //In attack Range Player
        var collider = Physics2D.OverlapCircle(Attack.position, attackRange, targetLayer);
        PlayerInRange = collider != null;

        if (PlayerInRange)
        {
            animator.SetTrigger("Attack");
        }
    }

    void FixedUpdate()
    {
        //followplayer
        if (playerDetected && !stunned && !PlayerInRange && run && Mathf.Abs(PlayerMovement.posX - transform.position.x) > 1)
            rb.velocity = new Vector2(Mathf.MoveTowards(rb.velocity.x, speed * playerDirectionX, Time.fixedDeltaTime * acceleration), rb.velocity.y);

        //friction
        else

            rb.velocity = new Vector2(Mathf.MoveTowards(rb.velocity.x, 0, Time.fixedDeltaTime * deceleration), rb.velocity.y);


        if (PlayerMovement.posX < transform.position.x && facingRight && !stunned && playerDetected)
        {
            Flip();
        }
        else if (PlayerMovement.posX > transform.position.x && !facingRight && !stunned && playerDetected)
        {
            Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }


    //Gizmos
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 1, 0, 0.4f);
        Gizmos.DrawWireSphere(SightPositionSphere.position, sightRadius);
        Gizmos.color = new Color(0, 1, 0, 0.4f);
        Gizmos.DrawWireSphere(SightPositionSphere.position, followRadius);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.7f);
        Gizmos.DrawWireSphere(Attack.position, attackRange);
    }


}
