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