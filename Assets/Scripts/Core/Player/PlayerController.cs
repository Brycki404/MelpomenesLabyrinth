using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Speed = 6f;
    public float FocusSpeed = 3f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector2 move = new Vector2(h, v).normalized;

        float s = Input.GetKey(KeyCode.LeftShift) ? FocusSpeed : Speed;

        rb.linearVelocity = move * s;
    }
}