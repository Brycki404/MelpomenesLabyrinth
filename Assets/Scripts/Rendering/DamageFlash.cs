using UnityEngine;

[RequireComponent(typeof(MaterialFX))]
public class DamageFlash : MonoBehaviour
{
    public float flashDuration = 0.1f;

    MaterialFX mat;
    float timer;
    public bool damageFlashEnabled = true;

    void Awake()
    {
        mat = GetComponent<MaterialFX>();
    }

    public void TriggerFlash()
    {
        timer = flashDuration;
    }
    
    public void StopFlash()
    {
        timer = 0f;
        damageFlashEnabled = false;
        mat.SetFlash(0f);
    }

    void Update()
    {
        if (timer > 0f)
        {
            damageFlashEnabled = true;
            timer -= Time.deltaTime;
            float t = timer / flashDuration;
            mat.SetFlash(t);
        }
        else if (damageFlashEnabled == true)
        {
            StopFlash();
        }
    }
}