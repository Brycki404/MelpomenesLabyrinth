using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Arc Sweep")]
public class ArcSweepAttack : Attack
{
    [Header("Arc Settings")]
    public int bulletCount = 12;
    public float bulletSpeed = 5f;
    public float startAngle = -60f;
    public float endAngle = 60f;

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
            base.spawner.SpawnArc(
                base.part.position,
                bulletCount,
                startAngle,
                endAngle,
                bulletSpeed
            );
        }
    }
}