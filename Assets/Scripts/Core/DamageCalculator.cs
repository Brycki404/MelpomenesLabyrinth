using UnityEngine;
public class DamageCalculator
{
    public int Calculate(int baseDamage, float multiplier)
    {
        return Mathf.RoundToInt(baseDamage * multiplier);
    }
}