using Unity.VisualScripting;
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

    public static Vector2[] LineCenterXOffsets(int count, float padding)
    {
        Vector2[] offsets = new Vector2[count];
        for (int i = 0; i < count; i++)
        {
            Vector2 v2 = new Vector2(Mathf.Pow(-1f, (float)i) * padding * i, 0f);
            offsets[i] = v2;
        }
        return offsets;
    }

    public static Vector2[] LineCenterYOffsets(int count, float padding)
    {
        Vector2[] offsets = new Vector2[count];
        for (int i = 0; i < count; i++)
        {
            Vector2 v2 = new Vector2(0f, Mathf.Pow(-1f, (float)i) * padding * i);
            offsets[i] = v2;
        }
        return offsets;
    }

    public static Vector2[] LineOffsets(int count, Vector2 p1, Vector2 p2)
    {
        Vector2[] offsets = new Vector2[count];
        for (int i = 0; i < count; i++)
        {  
            float alpha = i / count;
            Vector2 v2 = Vector2.Lerp(p1, p2, alpha);
            offsets[i] = v2;
        }
        return offsets;
    }

    public static Vector2 AngleToDirection(float angleDeg)
    {
        float rad = angleDeg * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
    }

    public static float DirectionToAngle(Vector2 dir)
    {
        // atan2 returns radians, convert to degrees
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        // Normalize to 0–360 (optional but recommended)
        if (angle < 0f)
            angle += 360f;

        return angle;
    }
}