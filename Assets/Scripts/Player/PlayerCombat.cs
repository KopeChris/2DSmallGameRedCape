using UnityEngine;
using System.Collections;


public class PlayerCombat : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth;
    public LayerMask enemyLayers;
    public bool invincible;
    Animator animator;

    public float hurtIseconds;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    void Update()
    {

    }

    SpriteRenderer sprite;
    public IEnumerator Flash()
    {
        for (float i = 0; i < hurtIseconds; i += 0.2f)
        {
            sprite.color = new Color(1, 1, 1, 0.4f);
            yield return new WaitForSeconds(0.1f);
            sprite.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void TakeDamage(float damage, float force)
    {
        if (!invincible)
        {
            currentHealth -= damage;
            GetComponent<PlayerMovement>().velocityX = force;
            Flash();
        }

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            animator.SetTrigger("Hurt");
        }
    }

    void Die()
    {
        animator.SetBool("IsDead", true);
        // disable player input and other necessary components
        // play death animation and other effects
        // reload the scene or show a game over screen
    }


}