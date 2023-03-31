using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackProjectile : MonoBehaviour
{
    public float attackDamage;
    Animator animator;
    Rigidbody2D rb;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        animator.Play("ProjectileExplosion");
        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;

        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerCombat>().TakeDamage(attackDamage,0);
        }
    }

    private void OnDisable()
    {
        Destroy(this.gameObject);

    }
}
