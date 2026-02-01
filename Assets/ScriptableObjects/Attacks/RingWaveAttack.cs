using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Ring Wave")]
public class RingWaveAttack : Attack
{
    [Header("Ring Settings")]
    public int bulletsPerRing = 12;
    public float bulletSpeed = 3f;
    public float ringRadius = 2f;
    public int ringCount = 3;
    public float ringDelay = 0.25f;

    private float ringTimer;
    private int ringsFired;

    public override void BeginAttack(Transform part, BulletSpawner spawner, BulletFaction faction)
    {
        base.BeginAttack(part, spawner, faction);
        ringTimer = 0f;
        ringsFired = 0;
    }

    public override void UpdateAttack(float dt)
    {
        Debug.Log("UpdateAttack");
        base.UpdateAttack(dt);
        Debug.Log("UpdatedAttack");

        // Fire rings at intervals
        ringTimer -= dt;
        if (ringTimer <= 0f && ringsFired < ringCount)
        {
            ringTimer = ringDelay;
            FireRing();
            ringsFired++;
        }
    }

    void FireRing()
    {
        spawner.SpawnRing(
            part.position,
            bulletsPerRing,
            ringRadius,
            bulletSpeed
        );
    }
}