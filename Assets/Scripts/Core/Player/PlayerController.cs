using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Speed = 6f;
    public float FocusSpeed = 3f;

    private Rigidbody2D rb;
    private DashMovement dash;
    private Vector2 moveInput;
    private PlayerHealth health;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        dash = GetComponent<DashMovement>();
        health = GetComponent<PlayerHealth>();
    }

    void Update()
    {
        if (health.CurrentHP <= 0) return;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(h, v).normalized;

        if (Input.GetKeyDown(KeyCode.Q))
            dash.TryStartDash();

        dash.TickDash();

        if (!dash.IsDashing)
        {
            float s = Input.GetKey(KeyCode.LeftShift) ? FocusSpeed : Speed;
            rb.linearVelocity = moveInput * s;
        }
    }

    public Vector2 GetMoveInput() => moveInput;
}