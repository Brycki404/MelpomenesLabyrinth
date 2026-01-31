using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossAIBrain : MonoBehaviour
{
    public BossController Boss;
    public PhaseController Phase;

    [System.Serializable]
    public class WeightedAttack
    {
        public AttackSO Attack;
        public float Weight = 1f;
    }

    public List<WeightedAttack> AttackPool = new List<WeightedAttack>();
    public float Cooldown = 1f;

    private bool running;

    private void Start()
    {
        StartCoroutine(BrainLoop());
    }

    private IEnumerator BrainLoop()
    {
        while (Phase.CurrentHP > 0)
        {
            if (!running)
            {
                var atk = ChooseAttack();
                StartCoroutine(RunAttack(atk));
            }

            yield return null;
        }
    }

    private AttackSO ChooseAttack()
    {
        float total = 0f;
        foreach (var w in AttackPool)
            total += w.Weight;

        float r = Random.value * total;

        foreach (var w in AttackPool)
        {
            if (r < w.Weight)
                return w.Attack;
            r -= w.Weight;
        }

        return AttackPool[0].Attack;
    }

    private IEnumerator RunAttack(AttackSO atk)
    {
        running = true;
        yield return StartCoroutine(atk.Run(Boss));
        yield return new WaitForSeconds(Cooldown);
        running = false;
    }
}