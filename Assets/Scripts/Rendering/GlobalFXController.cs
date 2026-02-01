using UnityEngine;
using System;

public class GlobalFXController : MonoBehaviour
{
    public static GlobalFXController Instance;

    public event Action<float> OnGlobalFlash;

    void Awake()
    {
        Instance = this;
    }

    public void TriggerGlobalFlash(float strength)
    {
        OnGlobalFlash?.Invoke(strength);
    }
}