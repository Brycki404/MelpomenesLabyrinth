using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MaterialPresetApplier))]
public class PlayerIFramesVisual : MonoBehaviour
{
    public float DefaultIFrameDuration = 1f;
    public Color DefaultIFrameColor = Color.white;

    private Material mat;

    private void Awake()
    {
        mat = GetComponent<MaterialPresetApplier>().Mat;
    }

    public void PlayIFrames(float duration = -1f, Color? color = null)
    {
        // Use defaults if caller didn't specify values
        float finalDuration = duration > 0f ? duration : DefaultIFrameDuration;
        Color finalColor = color ?? DefaultIFrameColor;

        StopAllCoroutines();
        StartCoroutine(IFrameRoutine(finalDuration, finalColor));
    }

    private IEnumerator IFrameRoutine(float IFrameDuration, Color IFrameColor)
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