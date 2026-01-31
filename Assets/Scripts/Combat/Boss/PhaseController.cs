using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PhaseController : MonoBehaviour
{
    [System.Serializable]
    public class Phase
    {
        public string Name;
        public float HPThreshold;     // When HP drops BELOW this, phase activates
        public List<AttackSO> Attacks;
    }

    public List<Phase> Phases;
    public BossController Boss;
    public float MaxHP = 100f;
    public float CurrentHP;

    private int currentPhaseIndex = -1;
    private bool runningPhase = false;

    private void Start()
    {
        CurrentHP = MaxHP;
        StartCoroutine(PhaseWatcher());
    }

    public void Damage(float amount)
    {
        CurrentHP = Mathf.Max(0, CurrentHP - amount);
    }

    private IEnumerator PhaseWatcher()
    {
        while (CurrentHP > 0)
        {
            for (int i = 0; i < Phases.Count; i++)
            {
                if (CurrentHP <= Phases[i].HPThreshold && i != currentPhaseIndex)
                {
                    currentPhaseIndex = i;
                    StartCoroutine(RunPhase(Phases[i]));
                }
            }

            yield return null;
        }
    }

    private IEnumerator RunPhase(Phase phase)
    {
        if (runningPhase)
            yield break;

        runningPhase = true;

        foreach (var atk in phase.Attacks)
            yield return StartCoroutine(atk.Run(Boss));

        runningPhase = false;
    }
}