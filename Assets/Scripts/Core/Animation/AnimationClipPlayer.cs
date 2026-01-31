using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class AnimationClipPlayer : MonoBehaviour
{
    public AnimationClip CurrentClip { get; private set; }
    public float NormalizedTime => playable.IsValid() && playable.GetDuration() > 0
        ? (float)(playable.GetTime() / playable.GetDuration())
        : 0f;

    PlayableGraph graph;
    AnimationClipPlayable playable;
    AnimationPlayableOutput output;

    void Awake()
    {
        graph = PlayableGraph.Create("CustomAnimationPlayer");
        output = AnimationPlayableOutput.Create(graph, "AnimationOutput", GetComponent<Animator>());
        graph.Play();
    }

    public void Play(AnimationClip clip)
    {
        if (clip == null) return;

        if (playable.IsValid())
            playable.Destroy();

        playable = AnimationClipPlayable.Create(graph, clip);
        playable.SetApplyFootIK(false);
        playable.SetApplyPlayableIK(false);

        output.SetSourcePlayable(playable);
        CurrentClip = clip;
    }
    
    public void Play(AnimationClip clip, float fadeDuration)
    {
        // If no fade needed, use the simple version
        if (fadeDuration <= 0f || !playable.IsValid())
        {
            Play(clip);
            return;
        }

        // Create new playable
        var newPlayable = AnimationClipPlayable.Create(graph, clip);
        newPlayable.SetApplyFootIK(false);
        newPlayable.SetApplyPlayableIK(false);

        // Create mixer
        var mixer = AnimationMixerPlayable.Create(graph, 2);

        // Connect old + new
        graph.Connect(playable, 0, mixer, 0);
        graph.Connect(newPlayable, 0, mixer, 1);

        mixer.SetInputWeight(0, 1f);
        mixer.SetInputWeight(1, 0f);

        output.SetSourcePlayable(mixer);

        StartCoroutine(FadeMixer(mixer, fadeDuration));

        playable = newPlayable;
        CurrentClip = clip;
    }

    private IEnumerator FadeMixer(AnimationMixerPlayable mixer, float duration)
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float w = t / duration;

            mixer.SetInputWeight(0, 1f - w);
            mixer.SetInputWeight(1, w);

            yield return null;
        }
    }

    public void SetTimeNormalized(float t)
    {
        if (!playable.IsValid() || !double.IsFinite(playable.GetDuration())) return;
        playable.SetTime(t * playable.GetDuration());
    }

    public float GetCurveValue(string curveName)
    {
        if (CurrentClip == null) return 0f;
        // Unity doesn’t expose curves directly at runtime; you’d usually
        // mirror important curves into AnimationEvents or ScriptableObjects.
        // For now, this is a placeholder hook for your own curve system.
        return 0f;
    }

    void OnDestroy()
    {
        if (graph.IsValid())
            graph.Destroy();
    }
}