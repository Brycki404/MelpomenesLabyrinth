using UnityEngine;

public class ExplodeBehavior : IBulletBehavior
{
    private float lifetime;
    private float t;
    private BulletSpawner spawner;
    private int count;
    private float speed;

    public ExplodeBehavior(BulletSpawner spawner, float lifetime, int count, float speed)
    {
        this.spawner = spawner;
        this.lifetime = lifetime;
        this.count = count;
        this.speed = speed;
    }

    public void Tick(Bullet bullet, float dt)
    {
        t += dt;

        if (t >= lifetime)
        {
            BulletFactory.SpawnCircle(spawner, bullet.transform.position, count, speed, bullet.Faction, null);
            bullet.gameObject.SetActive(false);
        }
    }
}