using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Attacks/Accelerating Burst")]
public class AcceleratingBurstAttack : AttackSO
{
    public int BulletCount = 12;
    public float InitialSpeed = 2f;
    public float Acceleration = 3f;

    public override IEnumerator Run(BossController boss)
    {
        Vector2 center = boss.transform.position;
        var dirs = PatternMath.CircleDirections(BulletCount);

        foreach (var dir in dirs)
        {
            // Build a behavior list for this bullet
            var behaviors = new List<IBulletBehavior>()
            {
                new AcceleratingBehavior(Acceleration)
            };

            // Spawn bullet through the factory so behaviors are applied
            BulletFactory.SpawnBullet(
                boss.Spawner,
                center,
                dir,
                InitialSpeed,
                preset: null // not using BehaviorPreset here
            ).SetBehaviors(behaviors);
        }

        yield return new WaitForSeconds(Duration);
    }
}