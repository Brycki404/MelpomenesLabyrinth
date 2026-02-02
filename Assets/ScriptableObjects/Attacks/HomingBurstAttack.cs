using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Attacks/Homing Burst")]
public class HomingBurstAttack : Attack
{
    [Header("Homing Burst Settings")]
    public int bulletCount = 6;
    public float bulletSpeed = 3f;
    public float turnSpeed = 5f;

    private bool fired;

    private Transform player;

    public override void BeginAttack(Transform part, BulletSpawner spawner, BulletFaction faction)
    {
        base.BeginAttack(part, spawner, faction);
        fired = false;
        // Cache player reference once
        player = GameObject.FindWithTag("Player")?.transform;
    }

    public override void UpdateAttack(float dt)
    {
        // If no player, end early
        if (player == null)
        {
            finished = true;
            return;
        }

        Debug.Log("UpdateAttack");
        base.UpdateAttack(dt);
        Debug.Log("UpdatedAttack");

        // Fire once
        if (!fired)
        {
            fired = true;
            FireBurst();
        }
    }

    void FireBurst()
    {
        Vector2 center = part.position;
        var dirs = PatternMath.CircleDirections(bulletCount);

        foreach (var dir in dirs)
        {
            var behaviors = new List<IBulletBehavior>()
            {
                new HomingBehavior(player, turnSpeed)
            };

            BulletFactory.SpawnBullet(
                spawner,
                center,
                dir,
                bulletSpeed,
                BulletFaction.Enemy,
                behaviors
            );
        }
    }
}