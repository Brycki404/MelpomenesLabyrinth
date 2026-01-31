using UnityEngine;
using System.Collections;

public class BossIntro : MonoBehaviour
{
    public float Duration = 1.5f;
    public AnimationCurve ScaleCurve;

    private IEnumerator Start()
    {
        float t = 0f;
        Vector3 start = Vector3.zero;
        Vector3 end = Vector3.one;

        while (t < Duration)
        {
            t += Time.deltaTime;
            float s = ScaleCurve.Evaluate(t / Duration);
            transform.localScale = Vector3.Lerp(start, end, s);
            yield return null;
        }
    }
}