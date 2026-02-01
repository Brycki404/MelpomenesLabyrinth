using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class MaterialPresetApplier : MonoBehaviour
{
    public MaterialPreset Preset;

    private void Awake()
    {
        if (Preset == null) return;
        var sr = GetComponent<SpriteRenderer>();
        sr.material = Instantiate(Preset.BaseMaterial);
    }

    public Material Mat => GetComponent<SpriteRenderer>().material;
}