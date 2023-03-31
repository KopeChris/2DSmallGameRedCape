using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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

    public Slider slider;

    void Start()
    {
        health = maxHealth;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        SetMax(maxHealth);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.B))
        {
            TakeDamage(1,0);
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

    public void TakeDamage(float damage, float force)
    {
        Invincible();
        Invoke("NotInvincible", hurtIseconds);
        StartCoroutine(Flash());

        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);
        Set(health);

        rb.AddForce(force * Vector2.right, ForceMode2D.Impulse);

        if (health <= 0)
        {
            Die();
            Debug.Log("Death");
        }
        else
        {
            animator.Play("Hurt");
            Debug.Log("Hurt");
        }
    }

    void Die()
    {
        animator.Play("Death");
        // disable player input and other necessary components
        // play death animation and other effects
        // reload the scene or show a game over screen
    }

    public void Push(float force)
    {
        rb.AddForce(force * Vector2.right, ForceMode2D.Impulse);
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

    public void SetMax(float max)
    {
        slider.maxValue = max;
        slider.value = max;
    }

    public void Set(float value)
    {
        slider.value = value;
    }
}