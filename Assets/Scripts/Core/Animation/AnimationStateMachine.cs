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

        // Apply base speed immediately
        player.SetBaseSpeed(next.playbackSpeed);

        // Apply runtime modifier (Shift key, buffs, etc.)
        player.SetSpeedModifier(next.playbackSpeedModifier);

        // Choose fade or hard switch
        if (next.crossfadeDuration > 0f)
        {
            player.PlayWithFadeAndBaseSpeed(
                next.clip,
                next.crossfadeDuration,
                next.playbackSpeed
            );
        }
        else
        {
            player.PlayWithBaseSpeed(
                next.clip,
                next.playbackSpeed
            );
        }

        current = next;
    }

    public void UpdateSpeedModifier(float speedModifier)
    {
        if (current == null) return;

        current.playbackSpeedModifier = speedModifier;
        player.SetSpeedModifier(speedModifier);
    }

    public string CurrentStateName => current != null ? current.stateName : "None";
}