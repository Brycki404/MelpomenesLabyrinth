using UnityEngine;

public class SineWaveBehavior : IBulletBehavior
{
    private Vector2 baseDir;
    private float frequency;
    private float amplitude;
    private float time;

    public SineWaveBehavior(Vector2 baseDir, float frequency, float amplitude)
    {
        this.baseDir = baseDir.normalized;
        this.frequency = frequency;
        this.amplitude = amplitude;
    }

    public void Tick(Bullet bullet, float deltaTime)
    {
        time += deltaTime;

        Vector2 perp = new Vector2(-baseDir.y, baseDir.x);
        Vector2 offset = perp * Mathf.Sin(time * frequency) * amplitude;

        Vector2 forward = baseDir * bullet.GetSpeed() * deltaTime;
        bullet.transform.position += (Vector3)(forward + offset * deltaTime);
    }
}