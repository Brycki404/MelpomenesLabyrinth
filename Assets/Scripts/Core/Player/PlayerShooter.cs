using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    public BulletSpawner spawner;
    public float fireRate = 0.15f;
    public float bulletSpeed = 12f;

    float cooldown;

    void Update()
    {
        cooldown -= Time.deltaTime;

        if (Input.GetKey(KeyCode.Mouse0) && cooldown <= 0f)
        {
            cooldown = fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        spawner.SpawnBullet(
            transform.position,
            Vector2.up,
            bulletSpeed
        );
    }
}