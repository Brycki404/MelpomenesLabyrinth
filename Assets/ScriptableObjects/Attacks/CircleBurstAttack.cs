using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Circle Burst")]
public class CircleBurstAttack : Attack
{
    [Header("Burst Settings")]
    public int bulletCount = 16;
    public float bulletSpeed = 6f;

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
        base.spawner.SpawnCircle(
            base.part.position,
            bulletCount,
            bulletSpeed
        );
    }
}