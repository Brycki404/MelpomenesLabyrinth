using UnityEngine;

[CreateAssetMenu(menuName = "Boss/MovementPath")]
public class MovementPath : ScriptableObject
{
    public float speed = 1f;
    public float amplitude = 1f;

    public Vector2 Evaluate(float t)
    {
        // Infinity symbol (lemniscate)
        float x = Mathf.Sin(t);
        float y = Mathf.Sin(t) * Mathf.Cos(t);
        return new Vector2(x, y) * amplitude;
    }
}