using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 50;
    public int currentHealth;

    public LayerMask playerLayer;

    public Transform attackPoint;

    Animator animator;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
       
    }

   
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

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
        // disable enemy input and other necessary components
        // play death animation and other effects
        // destroy the GameObject after a delay or other conditions
    }

    
}