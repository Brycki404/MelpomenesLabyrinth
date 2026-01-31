using UnityEngine;

public class BulletDeathAnimation : MonoBehaviour
{
    public float Duration = 0.2f;
    public AnimationCurve ScaleCurve;

    private float t;

    private void OnEnable()
    {
        t = 0f;
    }

    private void Update()
    {
        t += Time.deltaTime;
        float s = ScaleCurve.Evaluate(t / Duration);
        transform.localScale = Vector3.one * s;

        if (t >= Duration)
            gameObject.SetActive(false);
    }
}