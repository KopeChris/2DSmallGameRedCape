using UnityEngine;

public class TitanWood : MonoBehaviour
{
    public bool stunned;
    public bool run;

    [Header("Movement")]
    public float speed = 3;
    public float acceleration;
    public float deceleration;
    public bool facingRight = true;
    public int playerDirectionX;

    [Header("Attacks")]
    public bool playerDetected;
    public float attackRange = 1;
    public float attackRange2 = 1;

    public float cooldown;
    float timer;

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
    public Transform Attack2;

    [SerializeField]
    public bool playerSeen { get; internal set; }
    public bool playerFollowed { get; internal set; }
    public bool PlayerInRange { get; internal set; }
    public bool PlayerInRange2 { get; internal set; }

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
            animator.ResetTrigger("Attack2");
            animator.SetBool("Run", false);
        }

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

        //stop follow if out of follow range or if too close
        var follow = Physics2D.OverlapCircle(SightPositionSphere.position, followRadius, targetLayer);
        playerFollowed = follow != null;
        if (playerFollowed == false || Mathf.Abs(PlayerMovement.posX - transform.position.x) < 1.5f)
        {
            animator.SetBool("Run", false);
            playerDetected = false;

        }

        if (timer < 0)
        {
            //In attack Range Player
            var collider = Physics2D.OverlapCircle(Attack.position, attackRange, targetLayer);
            PlayerInRange = collider != null;

            if (PlayerInRange)
            {
                animator.SetTrigger("Attack");
                timer = cooldown;
            }
            var collider2 = Physics2D.OverlapCircle(Attack2.position, attackRange2, targetLayer);
            PlayerInRange2 = collider2 != null;

            if (PlayerInRange2)
            {
                animator.SetTrigger("Attack2");
                timer = cooldown;
            }
        }
        Debug.Log(timer);
        if (timer >= 0)
            timer -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        //followplayer
        if (playerDetected && !stunned && !PlayerInRange && run && Mathf.Abs(PlayerMovement.posX - transform.position.x) > 1)
            rb.velocity = new Vector2(Mathf.MoveTowards(rb.velocity.x, speed * playerDirectionX, Time.fixedDeltaTime * acceleration), rb.velocity.y);

        //friction
        else

            rb.velocity = new Vector2(Mathf.MoveTowards(rb.velocity.x, 0, Time.fixedDeltaTime * deceleration), rb.velocity.y);

        if (rb.velocity.y < 0)
            rb.gravityScale = 2;
        else
            rb.gravityScale = 1;

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
        Gizmos.DrawWireSphere(Attack2.position, attackRange2);
    }


}
