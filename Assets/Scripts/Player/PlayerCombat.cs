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


    void Start()
    {
        health = maxHealth;
        animator = GetComponent<Animator>();
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
        StartCoroutine(Flash());
        Invoke(nameof(NotInvincible), hurtIseconds);

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