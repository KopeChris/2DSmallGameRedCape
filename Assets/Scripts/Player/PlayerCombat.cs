using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth;
    public LayerMask enemyLayers;
    public bool invincible;
    Animator animator;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    void Update()
    {

    }



    public void TakeDamage(float damage, float force)
    {
        if (!invincible)
        {
            currentHealth -= damage;
            GetComponent<PlayerMovement>().velocityX = force;
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