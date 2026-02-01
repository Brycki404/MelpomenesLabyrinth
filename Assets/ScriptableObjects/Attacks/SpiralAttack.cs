using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Spiral")]
public class SpiralAttack : Attack
{
    [Header("Spiral Settings")]
    public int bulletCount = 60;
    public float bulletSpeed = 4f;
    public float startAngle = 0f;
    public float angleStep = 12f;
    public float fireInterval = 0.05f;

    private float fireTimer;
    private float angle;
    private int firedBullets;

    public override void BeginAttack(Transform part, BulletSpawner spawner, BulletFaction faction)
    {
        base.BeginAttack(part, spawner, faction);
        fireTimer = 0f;
        angle = startAngle;
        firedBullets = 0;
    }

    public override void UpdateAttack(float dt)
    {
        Debug.Log("UpdateAttack");
        base.UpdateAttack(dt);
        Debug.Log("UpdatedAttack");

        // Fire bullets at intervals
        fireTimer -= dt;
        if (fireTimer <= 0f && firedBullets < bulletCount)
        {
            fireTimer = fireInterval;
            FireOneBullet();
        }
    }

    void FireOneBullet()
    {
        Vector2 dir = PatternMath.AngleToDirection(angle);

        spawner.SpawnBullet(
            part.position,
            dir,
            bulletSpeed
        );

        angle += angleStep;
        firedBullets++;
    }
}