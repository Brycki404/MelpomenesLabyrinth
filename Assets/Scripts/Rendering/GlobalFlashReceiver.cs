using UnityEngine;

[RequireComponent(typeof(MaterialPresetApplier))]
public class GlobalFlashReceiver : MonoBehaviour, IGlobalFlashListener
{
    private Material mat;

    private void Awake()
    {
        mat = GetComponent<MaterialPresetApplier>().Mat;
    }

    private void OnEnable()
    {
        if (GlobalFXController.Instance != null)
            GlobalFXController.Instance.RegisterFlashListener(this);
    }

    private void OnDisable()
    {
        if (GlobalFXController.Instance != null)
            GlobalFXController.Instance.UnregisterFlashListener(this);
    }

    public void OnGlobalFlash(float amount, Color color)
    {
        mat.SetColor("_FlashColor", color);
        mat.SetFloat("_FlashAmount", amount);
    }
}