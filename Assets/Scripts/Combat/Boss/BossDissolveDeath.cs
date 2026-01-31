using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MaterialPresetApplier))]
public class BossDissolveDeath : MonoBehaviour
{
    public float Duration = 1.5f;

    private Material mat;

    private void Awake()
    {
        mat = GetComponent<MaterialPresetApplier>().Mat;
    }

    public void PlayDissolve()
    {
        StartCoroutine(DissolveRoutine());
    }

    private IEnumerator DissolveRoutine()
    {
        float t = 0f;
        while (t < Duration)
        {
            t += Time.deltaTime;
            float a = t / Duration;
            mat.SetFloat("_DissolveAmount", a);
            yield return null;
        }

        gameObject.SetActive(false);
    }
}