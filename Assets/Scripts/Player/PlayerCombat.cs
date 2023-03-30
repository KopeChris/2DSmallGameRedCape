using UnityEngine;
using System.Collections;


public class PlayerCombat : MonoBehaviour
{
    public float maxHealth = 100;
    public float health;
    public float hurtIseconds;
    Animator animator;

    public SpriteRenderer sprite;
    public CapsuleCollider2D PlayerHurtBox;
    //public CapsuleCollider2D CollisionBlocker;
    Rigidbody2D rb;

    void Start()
    {
        health = maxHealth;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.B))
        {
            TakeDamage(1);
        }
    }


    public IEnumerator Flash()
    {
        for (float i = 0; i < hurtIseconds; i += 0.2f)
        {
            sprite.color = new Color(1, 0.5f, 0.5f, 0.5f);
            yield return new WaitForSeconds(0.1f);
            sprite.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void TakeDamage(float damage)
    {

        Invincible();
        Invoke("NotInvincible", hurtIseconds);
        StartCoroutine(Flash());

        health -= damage;


        if (health <= 0)
        {
            Die();
        }
        else
        {
            animator.SetTrigger("Hurt");
        }
    }

    public void Push(float force)
    {
        rb.AddForce(force * Vector2.right, ForceMode2D.Impulse);
        animator.SetTrigger("Hurt");
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
    void Die()
    {
        animator.Play("Death");
        // disable player input and other necessary components
        // play death animation and other effects
        // reload the scene or show a game over screen
    }


}