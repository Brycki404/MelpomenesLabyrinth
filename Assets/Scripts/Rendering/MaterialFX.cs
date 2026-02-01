using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class MaterialFX : MonoBehaviour
{
    [Header("Preset")]
    public MaterialPreset preset;

    [HideInInspector] public Material runtimeMaterial;

    SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        if (preset == null)
        {
            Debug.LogWarning($"{name} has no MaterialPreset assigned.");
            return;
        }

        // Create unique material instance
        runtimeMaterial = new Material(preset.baseMaterial);
        sr.material = runtimeMaterial;

        ApplyPresetDefaults();
    }

    void ApplyPresetDefaults()
    {
        runtimeMaterial.SetColor("_FlashColor", preset.flashColor);
        runtimeMaterial.SetFloat("_FlashAmount", preset.flashAmount);

        if (preset.dissolveTex != null)
            runtimeMaterial.SetTexture("_DissolveTex", preset.dissolveTex);

        runtimeMaterial.SetFloat("_DissolveAmount", preset.dissolveAmount);
        runtimeMaterial.SetColor("_DissolveEdgeColor", preset.dissolveEdgeColor);
        runtimeMaterial.SetFloat("_DissolveEdgeWidth", preset.dissolveEdgeWidth);

        runtimeMaterial.SetFloat("_PulseSpeed", preset.pulseSpeed);
        runtimeMaterial.SetFloat("_PulseStrength", preset.pulseStrength);

        runtimeMaterial.SetColor("_OutlineColor", preset.outlineColor);
        runtimeMaterial.SetFloat("_OutlineThickness", preset.outlineThickness);
    }

    // -----------------------------
    // Override Methods (formerly MaterialOverride)
    // -----------------------------

    public void SetFlashColor(Color newColor)
    {
        runtimeMaterial.SetColor("_FlashColor", newColor);
    }

    public void SetFlash(float amount)
    {
        runtimeMaterial.SetFloat("_FlashAmount", amount);
    }

    public void SetDissolve(float amount)
    {
        runtimeMaterial.SetFloat("_DissolveAmount", amount);
    }

    public void SetDissolveEdgeWidth(float width)
    {
        runtimeMaterial.SetFloat("_DissolveEdgeWidth", width);
    }

    public void SetPulseStrength(float strength)
    {
        runtimeMaterial.SetFloat("_PulseStrength", strength);
    }

    public void SetOutline(float thickness)
    {
        runtimeMaterial.SetFloat("_OutlineThickness", thickness);
    }
}