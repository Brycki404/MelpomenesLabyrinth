using System;
using UnityEngine;

public class AnimationEventRunner
{
    AnimationState state;
    bool[] fired;
    Action<string> callback;

    public void SetState(AnimationState newState, Action<string> onEvent)
    {
        state = newState;
        callback = onEvent;
        if (state != null && state.events != null)
            fired = new bool[state.events.Length];
        else
            fired = null;
    }

    public void Update(float normalizedTime)
    {
        if (state == null || state.events == null || fired == null) return;

        for (int i = 0; i < state.events.Length; i++)
        {
            if (fired[i]) continue;
            if (normalizedTime >= state.events[i].normalizedTime)
            {
                fired[i] = true;
                callback?.Invoke(state.events[i].name);
            }
        }
    }
}