using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public bool canGetStunned = true;
    public float health;
    public float maxHealth;
    public Animator animator;
    Rigidbody2D rb;

    public SpriteRenderer sprite;


    void Start()
    {
        if (GetComponent<Animator>() != null)
            animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }



    public void TakeDamage(float damage)
    {
        health -= damage;
        StartCoroutine(FlashWhite());

        if (health <= 0)
        {
            animator.Play("Death");
        }
        else if(canGetStunned)
        {
            animator.Play("Hurt");
        }
    }

    public IEnumerator FlashWhite()
    {
        sprite.color = new Color(1, 0.5f, 0.5f, 1);
        yield return new WaitForSeconds(0.1f);
        sprite.color = new Color(1, 0.8f, 0.8f, 1);
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;
    }
}
