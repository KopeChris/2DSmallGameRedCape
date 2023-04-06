using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public bool canGetStunned = true;
    public float health;
    public float maxHealth;
    public Animator animator;
    Rigidbody2D rb;

    public SpriteRenderer sprite;

    public Slider slider;

    void Start()
    {
        if (GetComponent<Animator>() != null)
            animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        SetMax(maxHealth);
    }



    public void TakeDamage(float damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);
        StartCoroutine(FlashWhite());
        //HitEffect();
        if (health <= 0)
        {
            animator.Play("Death");
        }
        else if(canGetStunned)
        {
            animator.Play("Hurt");
        }

        Set(health);
    }

    public IEnumerator FlashWhite()
    {
        sprite.color = new Color(1, 0.5f, 0.5f, 1);
        yield return new WaitForSeconds(0.1f);
        sprite.color = new Color(1, 0.8f, 0.8f, 1);
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;
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
