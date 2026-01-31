using UnityEngine;

public static class PatternMath
{
    public static Vector2[] CircleDirections(int count)
    {
        Vector2[] dirs = new Vector2[count];
        float step = 360f / count;

        for (int i = 0; i < count; i++)
        {
            float angle = step * i;
            dirs[i] = AngleToDirection(angle);
        }

        return dirs;
    }

    public static Vector2[] ArcDirections(int count, float startAngle, float endAngle)
    {
        Vector2[] dirs = new Vector2[count];
        float step = (endAngle - startAngle) / Mathf.Max(1, count - 1);

        for (int i = 0; i < count; i++)
        {
            float angle = startAngle + step * i;
            dirs[i] = AngleToDirection(angle);
        }

        return dirs;
    }

    public static Vector2[] SpiralDirections(int count, float startAngle, float angleStep)
    {
        Vector2[] dirs = new Vector2[count];

        for (int i = 0; i < count; i++)
        {
            float angle = startAngle + angleStep * i;
            dirs[i] = AngleToDirection(angle);
        }

        return dirs;
    }

    public static Vector2[] WaveDirections(int count, float centerAngle, float spread)
    {
        Vector2[] dirs = new Vector2[count];
        float half = spread * 0.5f;

        if (count == 1)
        {
            dirs[0] = AngleToDirection(centerAngle);
            return dirs;
        }

        float step = spread / (count - 1);

        for (int i = 0; i < count; i++)
        {
            float angle = centerAngle - half + step * i;
            dirs[i] = AngleToDirection(angle);
        }

        return dirs;
    }

    public static Vector2[] RingOffsets(int count, float radius)
    {
        Vector2[] offsets = new Vector2[count];
        float step = 360f / count;

        for (int i = 0; i < count; i++)
        {
            float angle = step * i;
            offsets[i] = AngleToDirection(angle) * radius;
        }

        return offsets;
    }

    public static Vector2 AngleToDirection(float angleDeg)
    {
        float rad = angleDeg * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
    }
}