using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public float jumpForceX;
    public float jumpForceY;
    public Rigidbody2D rb;

    void OnEnable()
    {
        rb.velocity = new Vector2(jumpForceX * Input.GetAxisRaw("Horizontal"), jumpForceY);
    }
}