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
    AnimationPlayableOutput output;

    AnimationMixerPlayable mixer;          // persistent mixer
    AnimationClipPlayable playable;        // current clip playable

    void Awake()
    {
        graph = PlayableGraph.Create("CustomAnimationPlayer");

        var animator = GetComponent<Animator>();
        output = AnimationPlayableOutput.Create(graph, "AnimationOutput", animator);

        // Persistent 2‑input mixer
        mixer = AnimationMixerPlayable.Create(graph, 2);
        mixer.SetInputCount(2);

        // Output always comes from the mixer
        output.SetSourcePlayable(mixer);

        graph.Play();
    }

    public void Play(AnimationClip clip)
    {
        if (clip == null) return;

        // Create new playable for this clip
        var newPlayable = AnimationClipPlayable.Create(graph, clip);
        newPlayable.SetApplyFootIK(false);
        newPlayable.SetApplyPlayableIK(false);

        // Disconnect old input 0 if any
        if (mixer.GetInputCount() > 0)
        {
            var oldInput = mixer.GetInput(0);
            if (oldInput.IsValid())
                graph.Disconnect(mixer, 0);
        }

        // Connect new playable to input 0
        graph.Connect(newPlayable, 0, mixer, 0);
        mixer.SetInputWeight(0, 1f);
        mixer.SetInputWeight(1, 0f); // ensure fade slot is off

        // Destroy old playable
        if (playable.IsValid())
            playable.Destroy();

        playable = newPlayable;
        CurrentClip = clip;
    }

    public void Play(AnimationClip clip, float fadeDuration)
    {
        // If no fade or nothing currently playing, just hard switch
        if (fadeDuration <= 0f || !playable.IsValid())
        {
            Play(clip);
            return;
        }

        if (clip == null) return;

        // New playable for the target clip
        var newPlayable = AnimationClipPlayable.Create(graph, clip);
        newPlayable.SetApplyFootIK(false);
        newPlayable.SetApplyPlayableIK(false);

        // Ensure mixer has 2 inputs
        if (mixer.GetInputCount() < 2)
            mixer.SetInputCount(2);

        // Connect new playable to input 1 (fade‑in slot)
        var input1 = mixer.GetInput(1);
        if (input1.IsValid())
            graph.Disconnect(mixer, 1);

        graph.Connect(newPlayable, 0, mixer, 1);
        mixer.SetInputWeight(1, 0f);   // start from 0
        mixer.SetInputWeight(0, 1f);   // current stays at 1

        // Start fade between playable (old) and newPlayable
        StartCoroutine(FadeMixer(mixer, playable, newPlayable, fadeDuration));

        // We consider newPlayable the "current" target
        playable = newPlayable;
        CurrentClip = clip;
    }

    private IEnumerator FadeMixer(AnimationMixerPlayable mixer, AnimationClipPlayable oldPlayable, AnimationClipPlayable newPlayable, float duration)
    {
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float w = Mathf.Clamp01(t / duration);

            // Crossfade weights
            mixer.SetInputWeight(0, 1f - w); // old
            mixer.SetInputWeight(1, w);      // new

            yield return null;
        }

        // Ensure final weights
        mixer.SetInputWeight(0, 0f);
        mixer.SetInputWeight(1, 1f);

        // Clean up old playable and its connection
        if (oldPlayable.IsValid())
        {
            graph.Disconnect(mixer, 0);
            oldPlayable.Destroy();
        }

        // Move newPlayable to slot 0 as the sole active input
        graph.Disconnect(mixer, 1);
        graph.Connect(newPlayable, 0, mixer, 0);
        mixer.SetInputWeight(0, 1f);
        mixer.SetInputWeight(1, 0f);
    }

    public void SetTimeNormalized(float t)
    {
        if (!playable.IsValid() || !double.IsFinite(playable.GetDuration())) return;
        playable.SetTime(t * playable.GetDuration());
    }

    public float GetCurveValue(string curveName)
    {
        if (CurrentClip == null) return 0f;
        // Placeholder hook for your own curve system.
        return 0f;
    }

    void OnDestroy()
    {
        if (graph.IsValid())
            graph.Destroy();
    }
}