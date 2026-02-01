using UnityEngine;

public class HeadMovementController : MonoBehaviour
{
    public MovementPath infinityPath;
    public float driftAmplitude = 0.5f;
    public float driftSpeed = 0.5f;

    public Vector2 swoopTarget;
    public float swoopSpeed = 6f;

    float t;
    Vector2 basePos;

    enum Mode { Infinity, Drift, Swoop }
    Mode mode = Mode.Infinity;

    void Start()
    {
        basePos = transform.localPosition;
    }

    void Update()
    {
        switch (mode)
        {
            case Mode.Infinity:
                InfinityMotion();
                break;

            case Mode.Drift:
                DriftMotion();
                break;

            case Mode.Swoop:
                SwoopMotion();
                break;
        }
    }

    // -------------------------
    // INFINITY PATH
    // -------------------------
    void InfinityMotion()
    {
        t += Time.deltaTime * infinityPath.speed;
        Vector2 offset = infinityPath.Evaluate(t);
        transform.localPosition = basePos + offset;
    }

    // -------------------------
    // DRIFT
    // -------------------------
    void DriftMotion()
    {
        t += Time.deltaTime * driftSpeed;
        Vector2 offset = new Vector2(
            Mathf.Sin(t) * driftAmplitude,
            Mathf.Cos(t * 0.5f) * driftAmplitude
        );

        transform.localPosition = basePos + offset;
    }

    // -------------------------
    // SWOOP
    // -------------------------
    public void StartSwoop(Vector2 target)
    {
        swoopTarget = target;
        mode = Mode.Swoop;
    }

    void SwoopMotion()
    {
        transform.localPosition = Vector2.MoveTowards(
            transform.localPosition,
            swoopTarget,
            swoopSpeed * Time.deltaTime
        );

        if ((Vector2)transform.localPosition == swoopTarget)
        {
            // Return to infinity path after swoop
            basePos = transform.localPosition;
            mode = Mode.Infinity;
        }
    }

    // -------------------------
    // MODE SWITCHING
    // -------------------------
    public void SetInfinityMode() => mode = Mode.Infinity;
    public void SetDriftMode() => mode = Mode.Drift;
}