using UnityEngine;

public class GravityBehavior : IBulletBehavior
{
    private readonly Vector2 gravity;

    public GravityBehavior(Vector2 gravity)
    {
        this.gravity = gravity;
    }

    public void Tick(Bullet bullet, float dt)
    {
        // Convert direction + speed into a velocity vector
        Vector2 velocity = bullet.GetDirection() * bullet.GetSpeed();

        // Apply gravity
        velocity += gravity * dt;

        // Recompute direction + speed from the new velocity
        float newSpeed = velocity.magnitude;
        if (newSpeed > 0.0001f)
        {
            bullet.SetDirection(velocity.normalized);
            bullet.SetSpeed(newSpeed);
        }

        // Move bullet
        bullet.transform.position += (Vector3)(velocity * dt);

    }
}