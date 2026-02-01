using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float Duration = 0.2f;
    public float Magnitude = 0.2f;

    private Vector3 originalPos;
    private float t;

    private void Awake()
    {
        originalPos = transform.localPosition;
    }

    public void Shake()
    {
        t = Duration;
    }

    private void Update()
    {
        if (t > 0)
        {
            t -= Time.deltaTime;
            transform.localPosition = originalPos + (Vector3)Random.insideUnitCircle * Magnitude;
        }
        else
        {
            transform.localPosition = originalPos;
        }
    }
}