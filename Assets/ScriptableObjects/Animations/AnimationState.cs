using UnityEngine;

[CreateAssetMenu(menuName = "Animation/State")]
public class AnimationState : ScriptableObject
{
    public string stateName;
    public AnimationClip clip;
    public bool loop = true;
    public float playbackSpeed = 1f;
    public float crossfadeDuration = 0.1f;

    public AnimationEventDef[] events;
}