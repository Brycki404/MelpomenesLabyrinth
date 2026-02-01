using UnityEngine;

[RequireComponent(typeof(MaterialFX))]
public class DamageFlash : MonoBehaviour
{
    public float flashDuration = 0.1f;

    MaterialFX mat;
    float timer;

    void Awake()
    {
        mat = GetComponent<MaterialFX>();
    }

    public void TriggerFlash()
    {
        timer = flashDuration;
    }

    void Update()
    {
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
            float t = timer / flashDuration;
            mat.SetFlash(t);
        }
        else
        {
            mat.SetFlash(0f);
        }
    }
}