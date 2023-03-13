using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    public float aheadAmount, aheadSpeed;
    public Vector3 offset;

    void Start()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
    }

    void Update()
    {
        transform.position = new Vector3
            (Mathf.Lerp(transform.position.x, player.transform.position.x + aheadAmount * Input.GetAxisRaw("Horizontal")+ offset.x, aheadSpeed * Time.deltaTime),
            Mathf.Lerp(transform.position.y, player.transform.position.y + aheadAmount * Input.GetAxisRaw("Vertical")+ offset.y, aheadSpeed / 5 * Time.deltaTime), transform.position.z);
    }
}
