using UnityEngine;

public interface IBulletBehavior
{
    void Tick(Bullet bullet, float dt);
}