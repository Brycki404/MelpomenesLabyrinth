using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Attacks/Slam")]
public class SlamAttack : Attack
{
    [Header("Slam Settings")]
    public int bulletCount = 12;
    public float bulletSpeed = 5f;
    public float maxLeftAngle = -30f;
    public float maxRightAngle = 30f;
    public float lineLength = 1.375f;
    public float spawn_offset_y = 1.3125f / 2f;

    private float padding;
    private float DOWN_ANGLE = 270f;
    private bool fired;
    public override void BeginAttack(Transform part, BulletSpawner spawner, BulletFaction faction)
    {
        base.BeginAttack(part, spawner, faction);
        fired = false;
        padding = lineLength / bulletCount;
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

            Vector2 pos = base.part.position - new Vector3(0, spawn_offset_y, 0);
            float random_delta_angle = UnityEngine.Random.Range(maxLeftAngle, maxRightAngle);
            float random_angle = DOWN_ANGLE + random_delta_angle;
            Vector2 dir = PatternMath.AngleToDirection(random_angle);

            var behaviors = new List<IBulletBehavior>()
            {
                new GravityBehavior(new Vector2(0,-9.8f/16f))
            };

            BulletFactory.SpawnLineXAtCenter(base.spawner, pos, bulletCount, padding, bulletSpeed, dir, BulletFaction.Enemy, behaviors);
        }
    }
}