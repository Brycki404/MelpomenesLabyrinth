using UnityEngine;

public class DashMovement : MonoBehaviour
{
    [Header("Dash Settings")]
    public float dashDuration = 0.2f;
    public AnimationCurve dashSpeedCurve;
    public float baseDashSpeed = 20f;
    public float dashCooldown = 2.25f;

    [Header("Buffering")]
    public float dashBufferTime = 0.12f;

    [Header("Invulnerability")]
    public bool useIFrames = true;
    public float iframeStart = 0.0f;
    public float iframeEnd = 0.75f;

    private Rigidbody2D rb;
    private PlayerController controller;
    private PlayerAnimationController anim;
    private GhostTrail gt;
    private PlayerHealth health;

    private float dashTimer = 0f;
    private float cooldownTimer = 0f;
    private float dashBufferTimer = 0f;
    private Vector2 dashDir;

    public bool IsDashing => dashTimer > 0f;
    public bool IsInvulnerable { get; private set; }
    private AudioManager audioManager;

    void Awake()
    {
        audioManager = transform.root.GetComponentInChildren<AudioManager>();
        rb = GetComponent<Rigidbody2D>();
        controller = GetComponent<PlayerController>();
        health = GetComponent<PlayerHealth>();
        anim = GetComponentInChildren<PlayerAnimationController>();
        gt = GetComponent<GhostTrail>();
    }

    public void TryStartDash()
    {
        if (health.CurrentHP > 0)
            dashBufferTimer = dashBufferTime;
    }

    public void TickDash()
    {
        // COOLDOWN COUNTDOWN
        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;

        // BUFFER COUNTDOWN
        if (dashBufferTimer > 0f)
            dashBufferTimer -= Time.deltaTime;

        // START DASH (buffer + cooldown + cancel rules)
        if (!IsDashing && dashBufferTimer > 0f)
        {
            if (cooldownTimer <= 0f)
            {
                StartDash();
                dashBufferTimer = 0f;
            }
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
            if (dashTimer <= 0f)
            {
                EndDash();
            }
        }
    }

    private void StartDash()
    {
        // Reset cooldown
        cooldownTimer = dashCooldown;

        // Determine dash direction
        Vector2 move = controller.GetMoveInput();
        if (move.sqrMagnitude < 0.01f)
            move = anim.GetFacingDir();

        dashDir = move.normalized;
        dashTimer = dashDuration;

        // Start ghost trail
        gt.StartGhostTrail(dashDuration);

        // Reset i-frames
        IsInvulnerable = false;

        audioManager.PlayDodge();
    }

    public void EndDash()
    {
        dashTimer = 0f;
        IsInvulnerable = false;
        rb.linearVelocity = Vector2.zero;
        gt.StopGhostTrail();
    }
}