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

    float baseSpeed = 1f;                  // from AnimationState
    float speedModifier = 1f;              // runtime multiplier

    void Awake()
    {
        graph = PlayableGraph.Create("CustomAnimationPlayer");

        var animator = GetComponent<Animator>();
        output = AnimationPlayableOutput.Create(graph, "AnimationOutput", animator);

        mixer = AnimationMixerPlayable.Create(graph, 2);
        mixer.SetInputCount(2);

        output.SetSourcePlayable(mixer);

        graph.Play();
    }

    // -------------------------
    // NEW: Set base speed + modifier
    // -------------------------
    public void SetBaseSpeed(float speed)
    {
        baseSpeed = speed;
        RefreshSpeed();
    }

    public void SetSpeedModifier(float modifier)
    {
        speedModifier = modifier;
        RefreshSpeed();
    }

    void RefreshSpeed()
    {
        if (playable.IsValid())
            playable.SetSpeed(baseSpeed * speedModifier);
    }

    // -------------------------
    // Play without fade (with base speed)
    // -------------------------
    public void PlayWithBaseSpeed(AnimationClip clip, float clipBaseSpeed)
    {
        baseSpeed = clipBaseSpeed;
        speedModifier = 1f;

        Play(clip);
        RefreshSpeed();
    }

    public void Play(AnimationClip clip)
    {
        if (clip == null) return;

        var newPlayable = AnimationClipPlayable.Create(graph, clip);
        newPlayable.SetApplyFootIK(false);
        newPlayable.SetApplyPlayableIK(false);

        if (mixer.GetInputCount() > 0)
        {
            var oldInput = mixer.GetInput(0);
            if (oldInput.IsValid())
                graph.Disconnect(mixer, 0);
        }

        graph.Connect(newPlayable, 0, mixer, 0);
        mixer.SetInputWeight(0, 1f);
        mixer.SetInputWeight(1, 0f);

        if (playable.IsValid())
            playable.Destroy();

        playable = newPlayable;
        CurrentClip = clip;

        RefreshSpeed();
    }

    // -------------------------
    // Play with fade (with base speed)
    // -------------------------
    public void PlayWithFadeAndBaseSpeed(AnimationClip clip, float fadeDuration, float clipBaseSpeed)
    {
        baseSpeed = clipBaseSpeed;
        speedModifier = 1f;

        PlayWithFade(clip, fadeDuration);
        RefreshSpeed();
    }

    public void PlayWithFade(AnimationClip clip, float fadeDuration)
    {
        if (fadeDuration <= 0f || !playable.IsValid())
        {
            Play(clip);
            return;
        }

        if (clip == null) return;

        var newPlayable = AnimationClipPlayable.Create(graph, clip);
        newPlayable.SetApplyFootIK(false);
        newPlayable.SetApplyPlayableIK(false);

        if (mixer.GetInputCount() < 2)
            mixer.SetInputCount(2);

        var input1 = mixer.GetInput(1);
        if (input1.IsValid())
            graph.Disconnect(mixer, 1);

        graph.Connect(newPlayable, 0, mixer, 1);
        mixer.SetInputWeight(1, 0f);
        mixer.SetInputWeight(0, 1f);

        StartCoroutine(FadeMixer(mixer, playable, newPlayable, fadeDuration));

        playable = newPlayable;
        CurrentClip = clip;

        RefreshSpeed();
    }

    private IEnumerator FadeMixer(AnimationMixerPlayable mixer, AnimationClipPlayable oldPlayable, AnimationClipPlayable newPlayable, float duration)
    {
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float w = Mathf.Clamp01(t / duration);

            mixer.SetInputWeight(0, 1f - w);
            mixer.SetInputWeight(1, w);

            yield return null;
        }

        mixer.SetInputWeight(0, 0f);
        mixer.SetInputWeight(1, 1f);

        if (oldPlayable.IsValid())
        {
            graph.Disconnect(mixer, 0);
            oldPlayable.Destroy();
        }

        graph.Disconnect(mixer, 1);
        graph.Connect(newPlayable, 0, mixer, 0);
        mixer.SetInputWeight(0, 1f);
        mixer.SetInputWeight(1, 0f);

        RefreshSpeed();
    }

    public void SetTimeNormalized(float t)
    {
        if (!playable.IsValid() || !double.IsFinite(playable.GetDuration())) return;
        playable.SetTime(t * playable.GetDuration());
    }

    public float GetCurveValue(string curveName)
    {
        if (CurrentClip == null) return 0f;
        return 0f;
    }

    void OnDestroy()
    {
        if (graph.IsValid())
            graph.Destroy();
    }
}