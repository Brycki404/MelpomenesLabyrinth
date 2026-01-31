using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MaterialPresetApplier))]
public class PlayerIFramesVisual : MonoBehaviour
{
    public float IFrameDuration = 1f;
    public Color IFrameColor = Color.white;

    private Material mat;

    private void Awake()
    {
        mat = GetComponent<MaterialPresetApplier>().Mat;
    }

    public void PlayIFrames()
    {
        StopAllCoroutines();
        StartCoroutine(IFrameRoutine());
    }

    private IEnumerator IFrameRoutine()
    {
        mat.SetColor("_FlashColor", IFrameColor);
        float t = 0f;
        while (t < IFrameDuration)
        {
            t += Time.deltaTime;
            float a = Mathf.PingPong(t * 8f, 1f);
            mat.SetFloat("_FlashAmount", a);
            yield return null;
        }
        mat.SetFloat("_FlashAmount", 0f);
    }
}