using UnityEngine;
using System.Collections;

public class BossOutro : MonoBehaviour
{
    public float Duration = 1f;

    public IEnumerator PlayOutro()
    {
        float t = 0f;
        Vector3 start = transform.localScale;

        while (t < Duration)
        {
            t += Time.deltaTime;
            transform.localScale = Vector3.Lerp(start, Vector3.zero, t / Duration);
            yield return null;
        }
    }
}