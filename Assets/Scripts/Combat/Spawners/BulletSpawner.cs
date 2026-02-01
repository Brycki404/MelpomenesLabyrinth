using UnityEngine;
using System.Collections.Generic;

public class BulletSpawner : MonoBehaviour
{
    [Header("Bullet Setup")]
    public GameObject BulletPrefab;
    public GameObject DeathPrefab;
    public int InitialPoolSize = 64;
    public Transform BulletParent;

    private readonly Queue<Bullet> pool = new Queue<Bullet>();

    private void Awake()
    {
        if (BulletParent == null)
            BulletParent = this.transform;

        WarmPool();
    }

    private void WarmPool()
    {
        for (int i = 0; i < InitialPoolSize; i++)
            CreateBulletInstance();
    }

    private Bullet CreateBulletInstance()
    {
        GameObject go = Instantiate(BulletPrefab, BulletParent);
        go.SetActive(false);
        Bullet b = go.GetComponent<Bullet>();
        b.SetSpawner(this);
        pool.Enqueue(b);
        return b;
    }

    public Bullet GetBullet()
    {
        if (pool.Count == 0)
            CreateBulletInstance();

        Bullet b = pool.Dequeue();
        b.gameObject.SetActive(true);
        return b;
    }

    public void ReturnBullet(Bullet b)
    {
        b.gameObject.SetActive(false);
        pool.Enqueue(b);
    }

    // ---------- Low-level spawn ----------

    public Bullet SpawnBullet(Vector2 position, Vector2 direction, float speed, BulletFaction faction = BulletFaction.Enemy)
    {
        Bullet b = GetBullet();
        b.transform.position = position;
        b.Initialize(direction, speed, faction);
        return b;
    }

    // ---------- Pattern helpers ----------

    public void SpawnCircle(Vector2 center, int count, float speed, BulletFaction faction = BulletFaction.Enemy)
    {
        var dirs = PatternMath.CircleDirections(count);

        foreach (var dir in dirs)
            SpawnBullet(center, dir, speed, faction);
    }

    public void SpawnArc(Vector2 center, int count, float startAngle, float endAngle, float speed, BulletFaction faction = BulletFaction.Enemy)
    {
        var dirs = PatternMath.ArcDirections(count, startAngle, endAngle);

        foreach (var dir in dirs)
            SpawnBullet(center, dir, speed, faction);
    }

    public void SpawnSpiral(Vector2 center, int count, float startAngle, float angleStep, float speed, BulletFaction faction = BulletFaction.Enemy)
    {
        var dirs = PatternMath.SpiralDirections(count, startAngle, angleStep);

        foreach (var dir in dirs)
            SpawnBullet(center, dir, speed, faction);
    }

    public void SpawnWave(Vector2 center, int count, float centerAngle, float spread, float speed, BulletFaction faction = BulletFaction.Enemy)
    {
        var dirs = PatternMath.WaveDirections(count, centerAngle, spread);

        foreach (var dir in dirs)
            SpawnBullet(center, dir, speed, faction);
    }

    public void SpawnRing(Vector2 center, int count, float radius, float speed, BulletFaction faction = BulletFaction.Enemy)
    {
        var offsets = PatternMath.RingOffsets(count, radius);

        foreach (var offset in offsets)
        {
            Vector2 pos = center + offset;
            Vector2 dir = offset.normalized;
            SpawnBullet(pos, dir, speed, faction);
        }
    }

    public void PlayDeathAnimation(Vector2 pos)
    {
        var bulletDeathAnimation = GetComponent<BulletDeathAnimation>();
        if (bulletDeathAnimation != null)
        {
            bulletDeathAnimation.Enable();
        }
        else if (DeathPrefab)
        {
            var anim = Instantiate(DeathPrefab, pos, Quaternion.identity);
        }
    }
}