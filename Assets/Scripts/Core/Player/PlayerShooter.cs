using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    public BulletSpawner spawner;
    public float fireRate = 0.35f;
    public float bulletSpeed = 12f;

    private PlayerHealth health;

    float cooldown;

    void Awake()
    {
        health = GetComponent<PlayerHealth>();
    }

    void Update()
    {
        if (health.CurrentHP <= 0) return;
        cooldown -= Time.deltaTime;

        if (Input.GetKey(KeyCode.Mouse0) && cooldown <= 0f)
        {
            cooldown = fireRate;
            NormalShoot();
        }
    }

    void NormalShoot()
    {
        // Convert mouse position to world space
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Compute direction from player to mouse
        Vector2 dir = (mouseWorld - transform.position);
        dir.Normalize();

        BulletFactory.SpawnBullet(
            spawner,
            transform.position,
            dir,               // ← use the real direction
            bulletSpeed,
            BulletFaction.Player,
            null
        );
    }
}