using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Line Burst (Oscillating)")]
public class OscillatingLineBurstAttack : Attack
{
    [Header("Oscillation Settings")]
    public float baseAngle = 0f;
    public float oscillationAmplitude = 30f; // degrees
    public float oscillationSpeed = 4f;      // Hz

    [Header("Bullet Settings")]
    public float bulletSpeed = 6f;
    public float fireInterval = 0.05f;

    private float fireTimer;

    private float t;

    public override void BeginAttack(Transform part, BulletSpawner spawner, BulletFaction faction)
    {
        base.BeginAttack(part, spawner, faction);
        fireTimer = 0f;
        t = 0f;
    }

    public override void UpdateAttack(float dt)
    {
        Debug.Log("UpdateAttack");
        base.UpdateAttack(dt);
        Debug.Log("UpdatedAttack");

        t += dt;
        fireTimer -= dt;
        if (fireTimer <= 0f)
        {
            fireTimer = fireInterval;
            FireOscillatingBullet();
        }
    }

    void FireOscillatingBullet()
    {
        float oscillatedAngle =
            baseAngle +
            Mathf.Sin(t * oscillationSpeed) * oscillationAmplitude;

        Vector2 dir = PatternMath.AngleToDirection(oscillatedAngle);

        base.spawner.SpawnBullet(
            base.part.position,
            dir,
            bulletSpeed
        );
    }
}