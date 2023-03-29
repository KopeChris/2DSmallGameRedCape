using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    public float aheadAmount, aheadSpeed;
    public Vector3 offset;

    public Transform topRight;
    public Transform bottomLeft;


    void Start()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
    }

    void LateUpdate()
    {
        transform.position = new Vector3
            (Mathf.Lerp(transform.position.x, player.transform.position.x + aheadAmount * Input.GetAxisRaw("Horizontal") + offset.x, aheadSpeed * Time.deltaTime),
            Mathf.Lerp(transform.position.y, player.transform.position.y + aheadAmount * Input.GetAxisRaw("Vertical") + offset.y, aheadSpeed / 5 * Time.deltaTime),
            transform.position.z + offset.z);

        float clampedX = Mathf.Clamp(transform.position.x, bottomLeft.position.x, topRight.position.x);
        float clampedY = Mathf.Clamp(transform.position.y, bottomLeft.position.y, topRight.position.y);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }
}
