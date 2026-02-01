using UnityEngine;

[CreateAssetMenu(menuName = "Boss/Phase")]
public class Phase : ScriptableObject
{
    [Header("Phase Metadata")]
    public string phaseName;

    [Header("HP Threshold (0–1)")]
    public float hpThreshold = 1f; 
    // Example: 0.7 = triggers when total HP <= 70%

    [Header("Attack Sets")]
    public Attack[] headAttacks;
    public Attack[] leftHandAttacks;
    public Attack[] rightHandAttacks;

    [Header("Behavior Flags")]
    public bool leftHandActive = true;
    public bool rightHandActive = true;
}