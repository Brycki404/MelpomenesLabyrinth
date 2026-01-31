using UnityEngine;

[System.Serializable]
public struct BehaviorRequest
{
    public BehaviorType Type;

    // Generic numeric fields
    public float FloatA;
    public float FloatB;
    public float FloatC;

    // Optional references
    public Transform Target;
    public SpriteRenderer Renderer;
    public BulletSpawner Spawner;
}