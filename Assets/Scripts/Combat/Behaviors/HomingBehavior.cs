using UnityEngine;

public class HomingBehavior : IBulletBehavior
{
    public Transform target;
    public float turnSpeed;

    public HomingBehavior(Transform target, float turnSpeed)
    {
        this.target = target;
        this.turnSpeed = turnSpeed;
    }

    public void Tick(Bullet bullet, float deltaTime)
    {
        if (target == null) return;

        Vector2 toTarget = ((Vector2)target.position - (Vector2)bullet.transform.position).normalized;
        Vector2 current = bullet.GetDirection();

        Vector2 newDir = Vector2.Lerp(current, toTarget, turnSpeed * deltaTime).normalized;
        bullet.SetDirection(newDir);

        bullet.transform.position += (Vector3)(newDir * bullet.GetSpeed() * deltaTime);
    }
}