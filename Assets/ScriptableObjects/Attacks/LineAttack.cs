using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Line")]
public class LineAttack : Attack
{
    [Header("Burst Settings")]
    public int bulletCount = 16;
    public float bulletSpeed = 6f;
    public float fireInterval = 0.05f;

    [Tooltip("Angle in degrees for the first bullet")]
    public float startAngle = 0f;
    private Vector2 dir;

    private float fireTimer;
    private int firedBullets;

    public override void BeginAttack(Transform part, BulletSpawner spawner, BulletFaction faction)
    {
        base.BeginAttack(part, spawner, faction);
        fireTimer = 0f;
        firedBullets = 0;
        dir = PatternMath.AngleToDirection(startAngle);
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
        spawner.SpawnBullet(
            part.position,
            dir,
            bulletSpeed
        );
        
        firedBullets++;
    }
}