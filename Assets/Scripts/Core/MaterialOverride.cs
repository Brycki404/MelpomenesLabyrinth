using UnityEngine;

public class MaterialOverride : MonoBehaviour
{
    public Material RuntimeMaterial;
    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.material = Instantiate(RuntimeMaterial);
    }

    public Material Mat => sr.material;
}