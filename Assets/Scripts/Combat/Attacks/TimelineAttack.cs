using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Attacks/Timeline Attack")]
public class TimelineAttack : AttackSO
{
    public List<TimelineEvent> Events;

    public override IEnumerator Run(BossController boss)
    {
        float t = 0f;
        int index = 0;

        while (t < Duration)
        {
            if (index < Events.Count && t >= Events[index].Time)
            {
                Events[index].Event.Invoke();
                index++;
            }

            t += Time.deltaTime;
            yield return null;
        }
    }
}