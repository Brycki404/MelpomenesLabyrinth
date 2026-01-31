using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Attacks/Sine Wave Burst")]
public class SineWaveBurstAttack : AttackSO
{
    public int BulletCount = 8;
    public float BulletSpeed = 4f;
    public float Frequency = 6f;
    public float Amplitude = 0.5f;

    public override IEnumerator Run(BossController boss)
    {
        Vector2 center = boss.transform.position;
        var dirs = PatternMath.CircleDirections(BulletCount);

        foreach (var dir in dirs)
        {
            // Build behavior list for this bullet
            var behaviors = new List<IBulletBehavior>()
            {
                new SineWaveBehavior(dir, Frequency, Amplitude)
            };

            // Spawn bullet through the factory
            Bullet b = BulletFactory.SpawnBullet(
                boss.Spawner,
                center,
                dir,
                BulletSpeed
            );

            // Apply behaviors
            b.SetBehaviors(behaviors);
        }

        yield return new WaitForSeconds(Duration);
    }
}