using UnityEngine;
public class DamageFlash : MonoBehaviour
{
    public float Duration = 0.15f;
    private float t;
    private Material mat;

    private void Awake()
    {
        mat = GetComponent<MaterialOverride>().Mat;
    }

    public void Flash()
    {
        t = Duration;
    }

    private void Update()
    {
        if (t > 0)
        {
            t -= Time.deltaTime;
            mat.SetFloat("_FlashAmount", t / Duration);
        }
        else
        {
            mat.SetFloat("_FlashAmount", 0);
        }
    }
}