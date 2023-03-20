using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health;
    public float maxHealth;
    //int playerDirectionX;
    public Animator animator;
    Rigidbody2D rb;

    void Start()
    {
        if (GetComponent<Animator>() != null)
            animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        /*
        if (PlayerMovement.posX - transform.position.x > 0)
            playerDirectionX = 1;
        else
            playerDirectionX = -1;*/
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            animator.Play("Death");
        }
        else
        {
            animator.Play("Hurt");
        }
    }
}
