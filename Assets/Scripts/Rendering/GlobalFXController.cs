using UnityEngine;
using System.Collections.Generic;

public class GlobalFXController : MonoBehaviour
{
    public static GlobalFXController Instance { get; private set; }

    private readonly List<IGlobalFlashListener> flashListeners = new();
    private CameraShake camShake;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        camShake = FindAnyObjectByType<CameraShake>();
    }

    public void RegisterFlashListener(IGlobalFlashListener l)
    {
        if (!flashListeners.Contains(l))
            flashListeners.Add(l);
    }

    public void UnregisterFlashListener(IGlobalFlashListener l)
    {
        flashListeners.Remove(l);
    }

    public void GlobalHitFlash(float amount, Color color)
    {
        foreach (var l in flashListeners)
            l.OnGlobalFlash(amount, color);
    }

    public void GlobalCameraShake()
    {
        if (camShake != null)
            camShake.Shake();
    }
}

public interface IGlobalFlashListener
{
    void OnGlobalFlash(float amount, Color color);
}