using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Attacks/Sine Wave Burst")]
public class SineWaveBurstAttack : Attack
{
    [Header("Burst Settings")]
    public int bulletCount = 8;
    public float bulletSpeed = 4f;
    public float frequency = 6f;
    public float amplitude = 0.5f;

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
        Vector2 center = part.position;
        var dirs = PatternMath.CircleDirections(bulletCount);

        foreach (var dir in dirs)
        {
            var behaviors = new List<IBulletBehavior>()
            {
                new SineWaveBehavior(dir, frequency, amplitude)
            };

            BulletFactory.SpawnBullet(spawner, center, dir, bulletSpeed, BulletFaction.Enemy, behaviors);
        }
    }
}