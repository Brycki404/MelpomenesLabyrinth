using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Attacks/Arc Sweep")]
public class ArcSweepAttack : AttackSO
{
    public int BulletCount = 12;
    public float BulletSpeed = 5f;
    public float StartAngle = -60f;
    public float EndAngle = 60f;

    public override IEnumerator Run(BossController boss)
    {
        boss.Spawner.SpawnArc(boss.transform.position, BulletCount, StartAngle, EndAngle, BulletSpeed);
        yield return new WaitForSeconds(Duration);
    }
}