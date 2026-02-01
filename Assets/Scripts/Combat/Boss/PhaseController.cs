using UnityEngine;
using System;
using UnityEngine.XR;

public class PhaseController : MonoBehaviour
{
    public BossRoot bossRoot;
    public Phase[] phases;
    public BossRoot boss;

    int currentPhase;

    void Start()
    {
        currentPhase = 0;
        ApplyPhase(phases[0]);
    }

    void Update()
    {
        float hpPercent = boss.TotalHPPercent;

        if (hpPercent <= phases[currentPhase + 1].hpThreshold)
        {
            TryAdvancePhase();
        }
    }

    public void TriggerEvent(string eventName)
    {
        // Example: "LeftHandDead" → switch to next phase
        TryAdvancePhase();
    }

    void ApplyPhase(Phase p)
    {
        bossRoot.ApplyPhase(p);
    }

    void TryAdvancePhase()
    {
        if (currentPhase < phases.Length - 1)
        {
            currentPhase++;
            ApplyPhase(phases[currentPhase]);
        }
    }
}