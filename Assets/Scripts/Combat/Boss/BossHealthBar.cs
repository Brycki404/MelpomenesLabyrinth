using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    public PhaseController Phase;
    public Slider Slider;

    private void Start()
    {
        if (Phase == null)
            Phase = Object.FindFirstObjectByType<PhaseController>();
    }

    private void Update()
    {
        if (Phase == null || Slider == null) return;

        Slider.maxValue = Phase.MaxHP;
        Slider.value = Phase.CurrentHP;
    }
}