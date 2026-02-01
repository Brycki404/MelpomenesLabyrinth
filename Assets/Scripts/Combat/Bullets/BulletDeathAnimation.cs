using UnityEngine;

public class BulletDeathAnimation : MonoBehaviour
{
    public float Duration = 0.2f;
    public AnimationCurve ScaleCurve;

    private float t;

    private MaterialFX mat;

    private void Awake()
    {
        mat = GetComponent<MaterialFX>();
    }

    private void OnEnable()
    {
        t = 0f;
    }

    private void Update()
    {
        t += Time.deltaTime;
        float s = ScaleCurve.Evaluate(t / Duration);
        transform.localScale = Vector3.one * s;
        mat.SetDissolve(s);

        if (t >= Duration)
            gameObject.SetActive(false);
    }

    public void Enable()
    {
        OnEnable();
    }
}