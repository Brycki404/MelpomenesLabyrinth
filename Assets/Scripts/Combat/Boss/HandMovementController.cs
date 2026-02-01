using UnityEngine;

public class HandMovementController : MonoBehaviour
{
    [Header("Idle Float")]
    public float floatAmplitude = 0.25f;
    public float floatSpeed = 1f;

    [Header("Slam")]
    public Vector2 slamOffset = new Vector2(0, -2f);
    public float slamSpeed = 12f;
    public float slamReturnSpeed = 6f;

    [Header("Beam Position")]
    public Vector2 beamLocalPosition;

    [Header("Recoil")]
    public float recoilDistance = 0.5f;
    public float recoilSpeed = 10f;

    Vector2 origin;
    float floatT;

    enum Mode { Idle, Moving, Slamming, Recoiling }
    Mode mode = Mode.Idle;

    Vector2 moveTarget;

    void Start()
    {
        origin = transform.localPosition;
    }

    void Update()
    {
        switch (mode)
        {
            case Mode.Idle:
                IdleFloat();
                break;

            case Mode.Moving:
                MoveToTarget();
                break;

            case Mode.Slamming:
                SlamMotion();
                break;

            case Mode.Recoiling:
                RecoilMotion();
                break;
        }
    }

    // -------------------------
    // IDLE FLOAT
    // -------------------------
    void IdleFloat()
    {
        floatT += Time.deltaTime * floatSpeed;
        Vector2 offset = new Vector2(0, Mathf.Sin(floatT) * floatAmplitude);
        transform.localPosition = origin + offset;
    }

    // -------------------------
    // GENERIC MOVE
    // -------------------------
    void MoveToTarget()
    {
        transform.localPosition = Vector2.MoveTowards(
            transform.localPosition,
            moveTarget,
            slamReturnSpeed * Time.deltaTime
        );

        if ((Vector2)transform.localPosition == moveTarget)
            mode = Mode.Idle;
    }

    // -------------------------
    // SLAM ARC
    // -------------------------
    public void StartSlam()
    {
        origin = transform.localPosition;
        moveTarget = origin + slamOffset;
        mode = Mode.Slamming;
    }

    void SlamMotion()
    {
        transform.localPosition = Vector2.MoveTowards(
            transform.localPosition,
            moveTarget,
            slamSpeed * Time.deltaTime
        );

        if ((Vector2)transform.localPosition == moveTarget)
        {
            // After impact, return to origin
            moveTarget = origin;
            mode = Mode.Moving;
        }
    }

    // -------------------------
    // BEAM POSITIONING
    // -------------------------
    public void MoveToBeamPosition()
    {
        moveTarget = beamLocalPosition;
        mode = Mode.Moving;
    }

    // -------------------------
    // RECOIL
    // -------------------------
    public void StartRecoil(Vector2 direction)
    {
        origin = transform.localPosition;
        moveTarget = origin - direction.normalized * recoilDistance;
        mode = Mode.Recoiling;
    }

    void RecoilMotion()
    {
        transform.localPosition = Vector2.MoveTowards(
            transform.localPosition,
            moveTarget,
            recoilSpeed * Time.deltaTime
        );

        if ((Vector2)transform.localPosition == moveTarget)
        {
            moveTarget = origin;
            mode = Mode.Moving;
        }
    }

    // -------------------------
    // RETURN TO ORIGIN
    // -------------------------
    public void ReturnToOrigin()
    {
        moveTarget = origin;
        mode = Mode.Moving;
    }
}