using UnityEngine;

[CreateAssetMenu(menuName = "Animation/Movement Curve")]
public class MovementCurve : ScriptableObject
{
    public AnimationState state;
    public AnimationCurve speedOverTime; // 0–1 normalized time
}