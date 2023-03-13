using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grassling : MonoBehaviour
{
    public int damage = 1;

    public float pushForce = 1;
    int playerDirectionX = 1;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        if (PlayerMovement.posX - transform.position.x > 0)
            playerDirectionX = 1;
        else
            playerDirectionX = -1;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerCombat player = other.GetComponent<PlayerCombat>();
            if (player != null)
            {
                player.TakeDamage(damage, pushForce * playerDirectionX);
            }
        }
    }
}
