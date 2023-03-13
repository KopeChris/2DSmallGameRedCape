using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float attackDamage;
    public float pushForce;
    int playerDirectionX=1;

    void Start()
    {

    }


    void Update()
    {
        if (PlayerMovement.posX - transform.position.x > 0)
            playerDirectionX = 1;
        else
            playerDirectionX = -1;
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerCombat>().TakeDamage(attackDamage, pushForce * playerDirectionX);
        }
    }
}
