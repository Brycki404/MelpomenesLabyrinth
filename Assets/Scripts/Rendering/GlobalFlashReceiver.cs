using UnityEngine;

[RequireComponent(typeof(MaterialFX))]
public class GlobalFlashReceiver : MonoBehaviour
{
    MaterialFX mat;

    void Awake()
    {
        mat = GetComponent<MaterialFX>();
        GlobalFXController.Instance.OnGlobalFlash += HandleFlash;
    }

    void HandleFlash(float strength)
    {
        mat.SetFlash(strength);
    }
}