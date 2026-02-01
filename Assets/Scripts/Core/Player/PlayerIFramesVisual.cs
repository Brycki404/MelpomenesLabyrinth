using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MaterialFX))]
public class PlayerIFramesVisual : MonoBehaviour
{
    public float DefaultIFrameDuration = 1f;
    public Color DefaultIFrameColor = Color.white;

    private MaterialFX mat;

    private void Awake()
    {
        mat = GetComponent<MaterialFX>();
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
        
        mat.SetFlashColor(IFrameColor);
        float t = 0f;
        while (t < IFrameDuration)
        {
            t += Time.deltaTime;
            float a = Mathf.PingPong(t * 8f, 1f);
            mat.SetFlash(a);
            yield return null;
        }
        mat.SetFlash(0f);
    }
}