using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Line Burst (Player Aimed)")]
public class PlayerAimedLineBurstAttack : Attack
{
    [Header("Line Settings")]
    public int bulletCount = 12;
    public float bulletSpeed = 6f;
    public float fireInterval = 0.05f;

    private float fireTimer;

    private float angle;
    private int firedBullets;

    private Transform player;

    public override void BeginAttack(Transform part, BulletSpawner spawner, BulletFaction faction = BulletFaction.Enemy)
    {
        //This has to be an enemy attack
        faction = BulletFaction.Enemy;
        base.BeginAttack(part, spawner, faction);
        fireTimer = 0f;
        firedBullets = 0;
        player = GameObject.FindWithTag("Player")?.transform;
        if (player != null)
        {
            Vector2 toPlayer = (player.position - part.position).normalized;
            angle = PatternMath.DirectionToAngle(toPlayer);
        }
        else
        {
            base.EndAttack();            
        }
    }

    public override void UpdateAttack(float dt)
    {
        Debug.Log("UpdateAttack");
        base.UpdateAttack(dt);
        Debug.Log("UpdatedAttack");

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
        
        firedBullets++;
    }
}