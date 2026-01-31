using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class BossController : MonoBehaviour
{
    public BulletSpawner Spawner;

    public List<AttackSO> Attacks;

    public IEnumerator RunPattern()
    {
        foreach (var atk in Attacks)
            yield return StartCoroutine(atk.Run(this));
    }
}