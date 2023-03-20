using UnityEngine;

public class DetectFollowAttack : MonoBehaviour
{
    Animator animator;

    public Transform sightPositionSphere;
    public Transform Attack;

    public float speed;
    public float acceleration;

    public float sightRadius;
    public float attackRange;

    bool playerSeen;
    bool playerDetected;
    bool attack;

    public bool stunned;
    bool patrolScript;

    public EnemyHealth health;
    public Patrol patrol;

    bool facingRight;
    int playerDirection;

    Rigidbody2D rb;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        patrolScript = patrol.enabled;
    }

    private void Update()
    {
        //Detects the enemy or when get hurt. Turns off patrol and chases player

        var seen = Physics2D.OverlapCircle(sightPositionSphere.position, sightRadius, LayerMask.NameToLayer("Player"));
        playerSeen = seen != null;

        var collider = Physics2D.OverlapCircle(Attack.position, attackRange, LayerMask.NameToLayer("Player"));
        attack = collider != null;
        if (attack)
            animator.Play("Attack");


        if (PlayerMovement.posX - transform.position.x > 0) playerDirection = 1;
        else playerDirection = -1;

    }

    private void FixedUpdate()
    {
        if (playerSeen || health.health < health.maxHealth && patrolScript)
        {
            patrolScript = false;
            facingRight = patrol.facingRight;
            patrol.enabled = false;
            playerDetected = true;
        }

        if (!stunned && (playerSeen || playerDetected))
        {
            rb.velocity = Vector2.MoveTowards(rb.velocity, Vector2.right * speed * playerDirection, Time.fixedDeltaTime * acceleration);
        }

        if (!patrolScript)
        {
            if ((rb.velocity.x > 0 && !facingRight) || (rb.velocity.x < 0 && facingRight))
                Flip();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 1, 0, 0.4f);
        Gizmos.DrawWireSphere(sightPositionSphere.position, sightRadius);
        Gizmos.color = new Color(1, 0, 0, 0.4f);
        Gizmos.DrawWireSphere(Attack.position, attackRange);


    }

    private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }
}
