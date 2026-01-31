using UnityEngine;
using System.Collections;

public abstract class AttackSO : ScriptableObject
{
    public string AttackName;
    public float Duration;

    public abstract IEnumerator Run(BossController boss);
}