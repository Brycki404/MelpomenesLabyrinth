using UnityEngine;
using System.Collections;

public class BossDeathOrchestrator : MonoBehaviour
{
    public BossDissolveDeath BossDissolve;
    public CameraShake CamShake;
    public CanvasGroup ArenaFadeCanvas; // full‑screen black image with CanvasGroup
    public float ArenaFadeDelay = 0.5f;
    public float ArenaFadeDuration = 1.5f;

    public bool DisablePlayerOnDeath = true;
    public GameObject PlayerRoot;

    public void PlaySequence()
    {
        StartCoroutine(SequenceRoutine());
    }

    private IEnumerator SequenceRoutine()
    {
        // 1) Optional: stop player
        if (DisablePlayerOnDeath && PlayerRoot != null)
            PlayerRoot.SetActive(false);

        // 2) Camera shake hit
        if (CamShake != null)
            CamShake.Shake();

        // 3) Boss dissolve
        if (BossDissolve != null)
            BossDissolve.PlayDissolve();

        // 4) Wait a bit, then fade arena
        yield return new WaitForSeconds(ArenaFadeDelay);

        if (ArenaFadeCanvas != null)
        {
            float t = 0f;
            while (t < ArenaFadeDuration)
            {
                t += Time.deltaTime;
                ArenaFadeCanvas.alpha = Mathf.Clamp01(t / ArenaFadeDuration);
                yield return null;
            }
        }

        // 5) TODO: load next scene, show results, etc.
        // SceneManager.LoadScene("NextScene");
    }
}