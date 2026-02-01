using UnityEngine;

public static class BulletFactory
{
    public static Bullet SpawnBullet(
        BulletSpawner spawner,
        Vector2 position,
        Vector2 direction,
        float speed,
        BulletFaction faction = BulletFaction.Enemy,
        BehaviorPreset preset = null)
    {
        Bullet b = spawner.SpawnBullet(position, direction, speed, faction);

        if (preset != null)
        {
            var behaviors = BehaviorFactory.Build(preset.Behaviors);
            b.SetBehaviors(behaviors);
        }

        return b;
    }
}