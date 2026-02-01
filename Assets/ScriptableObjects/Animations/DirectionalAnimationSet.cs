using UnityEngine;

[CreateAssetMenu(menuName = "Animation/Directional State Set (Flexible)")]
public class DirectionalAnimationSet : ScriptableObject
{
    public AnimationState up;
    public AnimationState right;
    public AnimationState down;
    public AnimationState left;   // optional
}