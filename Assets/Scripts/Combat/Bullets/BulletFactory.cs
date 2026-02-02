using UnityEngine;
using System.Collections.Generic;

public static class BulletFactory
{

    public static Bullet SpawnBullet(
        BulletSpawner spawner,
        Vector3 position,
        Vector2 direction,
        float speed,
        BulletFaction faction = BulletFaction.Enemy,
        List<IBulletBehavior> behaviors = null
    )
    {
        var bullet = spawner.GetBullet();
        bullet.transform.position = position;
        bullet.SetSpawner(spawner);
        bullet.Initialize(direction, speed, faction, behaviors);
        return bullet;
    }

    // ---------- Pattern helpers ----------

    public static void SpawnCircle(
        BulletSpawner spawner,
        Vector2 center,
        int count,
        float speed,
        BulletFaction faction = BulletFaction.Enemy,
        List<IBulletBehavior> behaviors = null)
    {
        var dirs = PatternMath.CircleDirections(count);

        foreach (var dir in dirs)
            SpawnBullet(spawner, center, dir, speed, faction, behaviors);
    }

    public static void SpawnArc(
        BulletSpawner spawner,
        Vector2 center,
        int count,
        float startAngle,
        float endAngle,
        float speed,
        BulletFaction faction = BulletFaction.Enemy,
        List<IBulletBehavior> behaviors = null)
    {
        var dirs = PatternMath.ArcDirections(count, startAngle, endAngle);

        foreach (var dir in dirs)
            SpawnBullet(spawner, center, dir, speed, faction, behaviors);
    }

    public static void SpawnSpiral(
        BulletSpawner spawner,
        Vector2 center,
        int count,
        float startAngle,
        float angleStep,
        float speed,
        BulletFaction faction = BulletFaction.Enemy,
        List<IBulletBehavior> behaviors = null)
    {
        var dirs = PatternMath.SpiralDirections(count, startAngle, angleStep);

        foreach (var dir in dirs)
            SpawnBullet(spawner, center, dir, speed, faction, behaviors);
    }

    public static void SpawnWave(
        BulletSpawner spawner,
        Vector2 center,
        int count,
        float centerAngle,
        float spread,
        float speed,
        BulletFaction faction = BulletFaction.Enemy,
        List<IBulletBehavior> behaviors = null)
    {
        var dirs = PatternMath.WaveDirections(count, centerAngle, spread);

        foreach (var dir in dirs)
            SpawnBullet(spawner, center, dir, speed, faction, behaviors);
    }

    public static void SpawnRing(
        BulletSpawner spawner,
        Vector2 center,
        int count,
        float radius,
        float speed,
        BulletFaction faction = BulletFaction.Enemy,
        List<IBulletBehavior> behaviors = null)
    {
        var offsets = PatternMath.RingOffsets(count, radius);

        foreach (var offset in offsets)
        {
            Vector2 pos = center + offset;
            Vector2 dir = offset.normalized;

            SpawnBullet(spawner, pos, dir, speed, faction, behaviors);
        }
    }

    public static void SpawnLineXAtCenter(
        BulletSpawner spawner,
        Vector2 center,
        int count,
        float padding,
        float speed,
        Vector2 dir,
        BulletFaction faction = BulletFaction.Enemy,
        List<IBulletBehavior> behaviors = null)
    {
        var offsets = PatternMath.LineCenterXOffsets(count, padding);

        foreach (var offset in offsets)
        {
            Vector2 pos = center + offset;

            SpawnBullet(spawner, pos, dir, speed, faction, behaviors);
        }
    }

    public static void SpawnLineYAtCenter(
        BulletSpawner spawner,
        Vector2 center,
        int count,
        float padding,
        float speed,
        Vector2 dir,
        BulletFaction faction = BulletFaction.Enemy,
        List<IBulletBehavior> behaviors = null)
    {
        var offsets = PatternMath.LineCenterYOffsets(count, padding);

        foreach (var offset in offsets)
        {
            Vector2 pos = center + offset;
            SpawnBullet(spawner, pos, dir, speed, faction, behaviors);
        }
    }

    public static void SpawnLine(
        BulletSpawner spawner,
        int count,
        Vector2 p1,
        Vector2 p2,
        float speed,
        Vector2 dir,
        BulletFaction faction = BulletFaction.Enemy,
        List<IBulletBehavior> behaviors = null)
    {
        var offsets = PatternMath.LineOffsets(count, p1, p2);

        foreach (var offset in offsets)
        {
            Vector2 pos = p1 + offset;
            SpawnBullet(spawner, pos, dir, speed, faction, behaviors);
        }
    }
}