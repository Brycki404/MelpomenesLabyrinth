using UnityEngine;

public class AcceleratingBehavior : IBulletBehavior
{
    private float acceleration;

    public AcceleratingBehavior(float acceleration)
    {
        this.acceleration = acceleration;
    }

    public void Tick(Bullet bullet, float deltaTime)
    {
        float newSpeed = bullet.GetSpeed() + acceleration * deltaTime;
        bullet.SetSpeed(newSpeed);

        Vector2 dir = bullet.GetDirection();
        bullet.transform.position += (Vector3)(dir * newSpeed * deltaTime);
    }
}