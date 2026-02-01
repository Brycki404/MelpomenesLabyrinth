using UnityEngine;

public static class EasingLibrary
{
    // -------------------------
    // Linear
    // -------------------------
    public static float Linear(float x) => x;

    // -------------------------
    // Quadratic
    // -------------------------
    public static float EaseInQuad(float x) => x * x;
    public static float EaseOutQuad(float x) => 1 - (1 - x) * (1 - x);
    public static float EaseInOutQuad(float x)
    {
        return x < 0.5f
            ? 2 * x * x
            : 1 - Mathf.Pow(-2 * x + 2, 2) / 2;
    }

    // -------------------------
    // Cubic
    // -------------------------
    public static float EaseInCubic(float x) => x * x * x;
    public static float EaseOutCubic(float x) => 1 - Mathf.Pow(1 - x, 3);
    public static float EaseInOutCubic(float x)
    {
        return x < 0.5f
            ? 4 * x * x * x
            : 1 - Mathf.Pow(-2 * x + 2, 3) / 2;
    }

    // -------------------------
    // Quartic
    // -------------------------
    public static float EaseInQuart(float x) => x * x * x * x;
    public static float EaseOutQuart(float x) => 1 - Mathf.Pow(1 - x, 4);
    public static float EaseInOutQuart(float x)
    {
        return x < 0.5f
            ? 8 * x * x * x * x
            : 1 - Mathf.Pow(-2 * x + 2, 4) / 2;
    }

    // -------------------------
    // Quintic
    // -------------------------
    public static float EaseInQuint(float x) => x * x * x * x * x;
    public static float EaseOutQuint(float x) => 1 - Mathf.Pow(1 - x, 5);
    public static float EaseInOutQuint(float x)
    {
        return x < 0.5f
            ? 16 * x * x * x * x * x
            : 1 - Mathf.Pow(-2 * x + 2, 5) / 2;
    }

    // -------------------------
    // Sine
    // -------------------------
    public static float EaseInSine(float x) => 1 - Mathf.Cos((x * Mathf.PI) / 2);
    public static float EaseOutSine(float x) => Mathf.Sin((x * Mathf.PI) / 2);
    public static float EaseInOutSine(float x) => -(Mathf.Cos(Mathf.PI * x) - 1) / 2;

    // -------------------------
    // Exponential
    // -------------------------
    public static float EaseInExpo(float x)
    {
        return x == 0 ? 0 : Mathf.Pow(2, 10 * x - 10);
    }

    public static float EaseOutExpo(float x)
    {
        return x == 1 ? 1 : 1 - Mathf.Pow(2, -10 * x);
    }

    public static float EaseInOutExpo(float x)
    {
        if (x == 0) return 0;
        if (x == 1) return 1;

        return x < 0.5f
            ? Mathf.Pow(2, 20 * x - 10) / 2
            : (2 - Mathf.Pow(2, -20 * x + 10)) / 2;
    }

    // -------------------------
    // Circular
    // -------------------------
    public static float EaseInCirc(float x) => 1 - Mathf.Sqrt(1 - x * x);
    public static float EaseOutCirc(float x) => Mathf.Sqrt(1 - Mathf.Pow(x - 1, 2));
    public static float EaseInOutCirc(float x)
    {
        return x < 0.5f
            ? (1 - Mathf.Sqrt(1 - Mathf.Pow(2 * x, 2))) / 2
            : (Mathf.Sqrt(1 - Mathf.Pow(-2 * x + 2, 2)) + 1) / 2;
    }

    // -------------------------
    // Back
    // -------------------------
    public static float EaseInBack(float x, float c = 1.70158f)
    {
        return (c + 1) * x * x * x - c * x * x;
    }

    public static float EaseOutBack(float x, float c = 1.70158f)
    {
        float t = x - 1;
        return 1 + (c + 1) * t * t * t + c * t * t;
    }

    public static float EaseInOutBack(float x, float c = 1.70158f)
    {
        float k = c * 1.525f;
        return x < 0.5f
            ? (Mathf.Pow(2 * x, 2) * ((k + 1) * 2 * x - k)) / 2
            : (Mathf.Pow(2 * x - 2, 2) * ((k + 1) * (x * 2 - 2) + k) + 2) / 2;
    }

    // -------------------------
    // Elastic
    // -------------------------
    public static float EaseInElastic(float x)
    {
        if (x == 0 || x == 1) return x;
        return -Mathf.Pow(2, 10 * x - 10) * Mathf.Sin((x * 10 - 10.75f) * (2 * Mathf.PI / 3));
    }

    public static float EaseOutElastic(float x)
    {
        if (x == 0 || x == 1) return x;
        return Mathf.Pow(2, -10 * x) * Mathf.Sin((x * 10 - 0.75f) * (2 * Mathf.PI / 3)) + 1;
    }

    public static float EaseInOutElastic(float x)
    {
        if (x == 0 || x == 1) return x;

        float s = (2 * Mathf.PI) / 4.5f;

        return x < 0.5f
            ? -(Mathf.Pow(2, 20 * x - 10) * Mathf.Sin((20 * x - 11.125f) * s)) / 2
            : (Mathf.Pow(2, -20 * x + 10) * Mathf.Sin((20 * x - 11.125f) * s)) / 2 + 1;
    }

    // -------------------------
    // Bounce
    // -------------------------
    public static float EaseOutBounce(float x)
    {
        const float n1 = 7.5625f;
        const float d1 = 2.75f;

        if (x < 1 / d1)
            return n1 * x * x;
        else if (x < 2 / d1)
        {
            x -= 1.5f / d1;
            return n1 * x * x + 0.75f;
        }
        else if (x < 2.5f / d1)
        {
            x -= 2.25f / d1;
            return n1 * x * x + 0.9375f;
        }
        else
        {
            x -= 2.625f / d1;
            return n1 * x * x + 0.984375f;
        }
    }

    public static float EaseInBounce(float x)
    {
        return 1 - EaseOutBounce(1 - x);
    }

    public static float EaseInOutBounce(float x)
    {
        return x < 0.5f
            ? (1 - EaseOutBounce(1 - 2 * x)) / 2
            : (1 + EaseOutBounce(2 * x - 1)) / 2;
    }
}