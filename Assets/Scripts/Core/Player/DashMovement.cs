using UnityEngine;

public class DashMovement : MonoBehaviour
{
    [Header("Dash Settings")]
    public float dashDuration = 0.25f;
    public AnimationCurve dashSpeedCurve; // 0–1 normalized time
    public float baseDashSpeed = 20f;

    [Header("Cancel Rules")]
    public bool allowDashCancel = false; // can dash cancel into another dash?
    public bool allowAttackCancel = false;

    [Header("Buffering")]
    public float dashBufferTime = 0.12f;

    [Header("Invulnerability")]
    public bool useIFrames = true;
    public float iframeStart = 0.05f;
    public float iframeEnd = 0.20f;

    private Rigidbody2D rb;
    private PlayerController controller;
    private PlayerAnimationController anim;

    private float dashTimer;
    private float dashBufferTimer;
    private Vector2 dashDir;

    public bool IsDashing => dashTimer > 0f;
    public bool IsInvulnerable { get; private set; }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        controller = GetComponent<PlayerController>();
        anim = GetComponentInChildren<PlayerAnimationController>();
    }

    public void TryStartDash()
    {
        dashBufferTimer = dashBufferTime;
    }

    public void TickDash()
    {
        // BUFFER COUNTDOWN
        if (dashBufferTimer > 0)
            dashBufferTimer -= Time.deltaTime;

        // START DASH
        if (!IsDashing && dashBufferTimer > 0)
        {
            if (!allowDashCancel)
                return;

            StartDash();
            dashBufferTimer = 0;
        }

        // UPDATE DASH
        if (IsDashing)
        {
            dashTimer -= Time.deltaTime;

            float t = 1f - (dashTimer / dashDuration);
            float speedMult = dashSpeedCurve.Evaluate(t);

            rb.linearVelocity = dashDir * baseDashSpeed * speedMult;

            // I-FRAMES
            if (useIFrames)
                IsInvulnerable = t >= iframeStart && t <= iframeEnd;

            // END DASH
            if (dashTimer <= 0)
            {
                IsInvulnerable = false;
            }
        }
    }

    private void StartDash()
    {
        Vector2 move = controller.GetMoveInput();
        if (move.sqrMagnitude < 0.01f)
            move = anim.GetFacingDir();

        dashDir = move.normalized;
        dashTimer = dashDuration;
    }

    public void EndDash()
    {
        dashTimer = 0f;
        IsInvulnerable = false;
    }
}