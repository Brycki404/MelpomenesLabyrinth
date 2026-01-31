using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Attacks/Circle Burst")]
public class CircleBurstAttack : AttackSO
{
    public int BulletCount = 16;
    public float BulletSpeed = 6f;

    public override IEnumerator Run(BossController boss)
    {
        boss.Spawner.SpawnCircle(boss.transform.position, BulletCount, BulletSpeed);
        yield return new WaitForSeconds(Duration);
    }
}