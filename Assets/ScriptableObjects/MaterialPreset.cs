using UnityEngine;

[CreateAssetMenu(menuName = "FX/Material Preset")]
public class MaterialPreset : ScriptableObject
{
    public Material baseMaterial;

    [Header("Flash Defaults")]
    public Color flashColor = Color.white;
    public float flashAmount = 0f;

    [Header("Dissolve Defaults")]
    public Texture2D dissolveTex;
    public float dissolveAmount = 0f;
    public Color dissolveEdgeColor = Color.yellow;
    public float dissolveEdgeWidth = 0.1f;

    [Header("Pulse Defaults")]
    public float pulseSpeed = 8f;
    public float pulseStrength = 0.4f;

    [Header("Outline Defaults")]
    public Color outlineColor = Color.black;
    public float outlineThickness = 1f;
}