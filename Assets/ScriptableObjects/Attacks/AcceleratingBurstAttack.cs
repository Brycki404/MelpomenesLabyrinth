using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Attacks/Accelerating Burst")]
public class AcceleratingBurstAttack : Attack
{
    [Header("Burst Settings")]
    public int bulletCount = 12;
    public float initialSpeed = 2f;
    public float acceleration = 3f;

    private bool fired;

    public override void BeginAttack(Transform part, BulletSpawner spawner, BulletFaction faction)
    {
        base.BeginAttack(part, spawner, faction);
        fired = false;
    }

    public override void UpdateAttack(float dt)
    {
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
        Vector2 center = base.part.position;
        var dirs = PatternMath.CircleDirections(bulletCount);

        foreach (var dir in dirs)
        {
            var behaviors = new List<IBulletBehavior>()
            {
                new AcceleratingBehavior(acceleration)
            };

            BulletFactory
                .SpawnBullet(base.spawner, center, dir, initialSpeed, preset: null)
                .SetBehaviors(behaviors);
        }
    }
}