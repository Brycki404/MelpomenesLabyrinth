using UnityEngine;

public class TestShooter : MonoBehaviour
{
    public BulletSpawner Spawner;
    public BehaviorPreset Preset;

    [Header("Pattern Settings")]
    public PatternType Pattern = PatternType.Circle;
    public int Count = 12;
    public float Speed = 4f;

    [Header("Timing")]
    public float FireRate = 1f;

    private float t;
    private float spiralAngle = 0f;

    private void Update()
    {
        t += Time.deltaTime;
        if (t >= FireRate)
        {
            t = 0f;
            FirePattern();
        }
    }

    private void FirePattern()
    {
        Vector2 pos = transform.position;

        switch (Pattern)
        {
            case PatternType.Circle:
                FireCircle(pos);
                break;

            case PatternType.Arc:
                FireArc(pos);
                break;

            case PatternType.Spiral:
                FireSpiral(pos);
                break;

            case PatternType.Wave:
                FireWave(pos);
                break;

            case PatternType.Ring:
                FireRing(pos);
                break;
        }
    }

    private void FireCircle(Vector2 pos)
    {
        var dirs = PatternMath.CircleDirections(Count);

        foreach (var dir in dirs)
            BulletFactory.SpawnBullet(Spawner, pos, dir, Speed, BulletFaction.Enemy, Preset);
    }

    private void FireArc(Vector2 pos)
    {
        var dirs = PatternMath.ArcDirections(Count, -60f, 60f);

        foreach (var dir in dirs)
            BulletFactory.SpawnBullet(Spawner, pos, dir, Speed, BulletFaction.Enemy, Preset);
    }

    private void FireSpiral(Vector2 pos)
    {
        var dirs = PatternMath.SpiralDirections(Count, spiralAngle, 10f);
        spiralAngle += 10f;

        foreach (var dir in dirs)
            BulletFactory.SpawnBullet(Spawner, pos, dir, Speed, BulletFaction.Enemy, Preset);
    }

    private void FireWave(Vector2 pos)
    {
        var dirs = PatternMath.WaveDirections(Count, 90f, 45f);

        foreach (var dir in dirs)
            BulletFactory.SpawnBullet(Spawner, pos, dir, Speed, BulletFaction.Enemy, Preset);
    }

    private void FireRing(Vector2 pos)
    {
        var offsets = PatternMath.RingOffsets(Count, 1.5f);

        foreach (var offset in offsets)
        {
            Vector2 spawnPos = pos + offset;
            Vector2 dir = offset.normalized;

            BulletFactory.SpawnBullet(Spawner, spawnPos, dir, Speed, BulletFaction.Enemy, Preset);
        }
    }
}

public enum PatternType
{
    Circle,
    Arc,
    Spiral,
    Wave,
    Ring
}