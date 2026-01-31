using UnityEngine;

public static class BulletFactory
{
    public static Bullet SpawnBullet(
        BulletSpawner spawner,
        Vector2 position,
        Vector2 direction,
        float speed,
        BehaviorPreset preset = null)
    {
        Bullet b = spawner.SpawnBullet(position, direction, speed);

        if (preset != null)
        {
            var behaviors = BehaviorFactory.Build(preset.Behaviors);
            b.SetBehaviors(behaviors);
        }

        return b;
    }
}