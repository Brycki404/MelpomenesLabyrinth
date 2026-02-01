using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public enum BulletFaction
{
    Player,
    Enemy,
    Neutral
}

public class Bullet : MonoBehaviour
{
    private Vector2 direction;
    private float speed;
    private BulletSpawner spawner;

    public BulletFaction Faction { get; private set; }

    private readonly List<IBulletBehavior> behaviors = new List<IBulletBehavior>();

    public void SetSpawner(BulletSpawner s) => spawner = s;

    public void Initialize(Vector2 dir, float spd, BulletFaction faction, List<IBulletBehavior> newBehaviors = null)
    {
        Faction = faction;
        direction = dir.normalized;
        speed = spd;

        behaviors.Clear();

        if (newBehaviors != null)
            behaviors.AddRange(newBehaviors);
    }
    
    public void SetBehaviors(List<IBulletBehavior> list)
    {
        behaviors.Clear();
        behaviors.AddRange(list);
    }

    public List<IBulletBehavior> GetBehaviors() => behaviors;

    public void AddBehavior(IBulletBehavior b) => behaviors.Add(b);

    public Vector2 GetDirection() => direction;
    public void SetDirection(Vector2 d) => direction = d.normalized;

    public float GetSpeed() => speed;
    public void SetSpeed(float s) => speed = s;

    private void Update()
    {
        float dt = Time.deltaTime;

        if (behaviors.Count > 0)
        {
            foreach (var b in behaviors)
                b.Tick(this, dt);
        }
        else
        {
            transform.position += (Vector3)(direction * speed * dt);
        }
    }

    private void OnBecameInvisible()
    {
        if (spawner != null)
            spawner.ReturnBullet(this);
        else
            gameObject.SetActive(false);
    }

    public string DebugBehaviorList
    {
        get
        {
            if (behaviors.Count == 0) return "None";
            return string.Join(", ", behaviors.Select(b => b.GetType().Name));
        }
    }
}