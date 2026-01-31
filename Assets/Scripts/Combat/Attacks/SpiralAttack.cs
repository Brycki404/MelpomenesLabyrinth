using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Attacks/Spiral")]
public class SpiralAttack : AttackSO
{
    public int BulletCount = 60;
    public float BulletSpeed = 4f;
    public float StartAngle = 0f;
    public float AngleStep = 12f;
    public float FireInterval = 0.05f;

    public override IEnumerator Run(BossController boss)
    {
        float angle = StartAngle;

        for (int i = 0; i < BulletCount; i++)
        {
            Vector2 dir = PatternMath.AngleToDirection(angle);
            boss.Spawner.SpawnBullet(boss.transform.position, dir, BulletSpeed);

            angle += AngleStep;
            yield return new WaitForSeconds(FireInterval);
        }

        yield return new WaitForSeconds(Duration);
    }
}