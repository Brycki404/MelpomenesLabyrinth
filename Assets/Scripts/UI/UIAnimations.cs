using UnityEngine;
using System.Collections;

public delegate float EaseFunc(float x);

public static class UIAnimations
{
    public static IEnumerator FadeScaleIn(CanvasGroup group, RectTransform panel, float duration, EaseFunc ease = null)
    {
        float t = 0f;
        group.alpha = 0f;
        panel.localScale = Vector3.one * 0.95f;

        ease ??= (x => x);

        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            float p = Mathf.Clamp01(t / duration);
            float e = ease(p);

            group.alpha = Mathf.Lerp(0f, 1f, e);
            panel.localScale = Vector3.Lerp(Vector3.one * 0.95f, Vector3.one, e);

            yield return null;
        }

        group.alpha = 1f;
        panel.localScale = Vector3.one;
    }

    public static IEnumerator FadeScaleOut(CanvasGroup group, RectTransform panel, float duration, EaseFunc ease = null)
    {
        float t = 0f;
        group.alpha = 1f;
        panel.localScale = Vector3.one;

        ease ??= (x => x);

        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            float p = Mathf.Clamp01(t / duration);
            float e = ease(p);

            group.alpha = Mathf.Lerp(1f, 0f, e);
            panel.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 0.95f, e);

            yield return null;
        }

        group.alpha = 0f;
        panel.localScale = Vector3.one * 0.95f;
    }
}