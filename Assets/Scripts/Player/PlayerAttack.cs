using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float damage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<ObjectHealth>()!=null)
        collision.GetComponent<ObjectHealth>().TakeDamage(damage);

        if(collision.GetComponent<EnemyHealth>()!=null)
        collision.GetComponent<EnemyHealth>().TakeDamage(damage);
    }
}
