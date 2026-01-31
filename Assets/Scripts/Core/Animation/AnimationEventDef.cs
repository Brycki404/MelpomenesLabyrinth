using UnityEngine;

[System.Serializable]
public class AnimationEventDef
{
    public string name;
    [Range(0f, 1f)] public float normalizedTime;
}