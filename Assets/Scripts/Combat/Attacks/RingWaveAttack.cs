using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Attacks/Ring Wave")]
public class RingWaveAttack : AttackSO
{
    public int BulletsPerRing = 12;
    public float BulletSpeed = 3f;
    public float RingRadius = 2f;
    public int RingCount = 3;
    public float RingDelay = 0.25f;

    public override IEnumerator Run(BossController boss)
    {
        for (int r = 0; r < RingCount; r++)
        {
            boss.Spawner.SpawnRing(boss.transform.position, BulletsPerRing, RingRadius, BulletSpeed);
            yield return new WaitForSeconds(RingDelay);
        }

        yield return new WaitForSeconds(Duration);
    }
}