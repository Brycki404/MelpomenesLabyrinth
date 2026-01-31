using UnityEngine;
using System.Collections;

public class ExampleBossSetup : MonoBehaviour
{
    public BossController Boss;
    public PhaseController Phase;
    public Transform Player;

    private void Start()
    {
        // Example: damage boss over time to test phases
        StartCoroutine(DebugDamageRoutine());
    }

    private IEnumerator DebugDamageRoutine()
    {
        while (Phase.CurrentHP > 0)
        {
            yield return new WaitForSeconds(2f);
            Phase.Damage(5f);
        }
    }
}