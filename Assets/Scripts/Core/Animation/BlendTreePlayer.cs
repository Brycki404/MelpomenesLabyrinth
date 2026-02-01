using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;

[RequireComponent(typeof(Animator))]
public class BlendTreePlayer : MonoBehaviour
{
    public AnimationClip CurrentA { get; private set; }
    public AnimationClip CurrentB { get; private set; }
    public float Blend { get; private set; } // 0 = A, 1 = B

    PlayableGraph graph;
    AnimationMixerPlayable mixer;
    AnimationPlayableOutput output;
    AnimationClipPlayable playableA;
    AnimationClipPlayable playableB;

    void Awake()
    {
        graph = PlayableGraph.Create("BlendTreePlayer");
        mixer = AnimationMixerPlayable.Create(graph, 2);
        output = AnimationPlayableOutput.Create(graph, "AnimationOutput", GetComponent<Animator>());
        output.SetSourcePlayable(mixer);
        graph.Play();
    }

    public void SetClips(AnimationClip a, AnimationClip b)
    {
        if (playableA.IsValid()) mixer.DisconnectInput(0);
        if (playableB.IsValid()) mixer.DisconnectInput(1);

        if (a != null)
        {
            playableA = AnimationClipPlayable.Create(graph, a);
            mixer.ConnectInput(0, playableA, 0);
            mixer.SetInputWeight(0, 1f - Blend);
            CurrentA = a;
        }

        if (b != null)
        {
            playableB = AnimationClipPlayable.Create(graph, b);
            mixer.ConnectInput(1, playableB, 0);
            mixer.SetInputWeight(1, Blend);
            CurrentB = b;
        }
    }

    public void SetBlend(float t)
    {
        Blend = Mathf.Clamp01(t);
        mixer.SetInputWeight(0, 1f - Blend);
        mixer.SetInputWeight(1, Blend);
    }

    public float GetNormalizedTime()
    {
        if (playableA.IsValid() && playableA.GetDuration() > 0)
            return (float)(playableA.GetTime() / playableA.GetDuration());
        return 0f;
    }

    void OnDestroy()
    {
        if (graph.IsValid())
            graph.Destroy();
    }
}