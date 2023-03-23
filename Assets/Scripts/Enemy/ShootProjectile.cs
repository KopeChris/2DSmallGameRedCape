using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootProjectile : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float forceX;
    public float forceXMultiplier;
    public float forceY;
    public Transform position;

    void Shoot()
    {
        forceXMultiplier = forceX * Mathf.Abs(PlayerMovement.posX - transform.position.x);

        GameObject projectile1 = Instantiate(projectilePrefab, position.transform.position, Quaternion.identity);
        projectile1.GetComponent<Rigidbody2D>().AddForce(new Vector2(forceXMultiplier, forceY), ForceMode2D.Impulse);

        GameObject projectile2 = Instantiate(projectilePrefab, position.transform.position, Quaternion.identity);
        projectile2.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 1.5f * forceY), ForceMode2D.Impulse);

        GameObject projectile3 = Instantiate(projectilePrefab, position.transform.position, Quaternion.identity);
        projectile3.GetComponent<Rigidbody2D>().AddForce(new Vector2(-forceXMultiplier, forceY), ForceMode2D.Impulse);
    }
}
