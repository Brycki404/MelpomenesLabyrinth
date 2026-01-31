using UnityEngine;

public class AnimationStateMachine
{
    AnimationClipPlayer player;
    AnimationState current;

    public AnimationStateMachine(AnimationClipPlayer player)
    {
        this.player = player;
    }

    public void Play(AnimationState next)
    {
        if (next == current) return;

        player.Play(next.clip, next.crossfadeDuration);
        current = next;
    }

    public string CurrentStateName => current != null ? current.stateName : "None";
}