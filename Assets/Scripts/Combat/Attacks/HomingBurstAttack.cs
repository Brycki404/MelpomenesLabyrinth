using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Attacks/Homing Burst")]
public class HomingBurstAttack : AttackSO
{
    public int BulletCount = 6;
    public float BulletSpeed = 3f;
    public float TurnSpeed = 5f;

    public override IEnumerator Run(BossController boss)
    {
        var player = GameObject.FindWithTag("Player")?.transform;
        if (player == null)
        {
            yield return new WaitForSeconds(Duration);
            yield break;
        }

        Vector2 center = boss.transform.position;
        var dirs = PatternMath.CircleDirections(BulletCount);

        foreach (var dir in dirs)
        {
            BulletFactory.SpawnBullet(
                boss.Spawner,
                center,
                dir,
                BulletSpeed,
                preset: new BehaviorPreset
                {
                    Behaviors = new BehaviorRequest[]
                    {
                        new BehaviorRequest
                        {
                            Type = BehaviorType.Homing,
                            Target = player,
                            FloatA = TurnSpeed
                        }
                    }
                }
            );
        }

        yield return new WaitForSeconds(Duration);
    }
}