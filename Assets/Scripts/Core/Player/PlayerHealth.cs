using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int MaxHP = 5;
    public float InvulnDuration = 1f;

    public int CurrentHP { get; private set; }
    public bool IsInvulnerable { get; private set; }

    private void Start()
    {
        CurrentHP = MaxHP;
    }

    public void Damage(int amount)
    {
        if (IsInvulnerable) return;

        CurrentHP -= amount;
        if (CurrentHP <= 0)
        {
            CurrentHP = 0;
            Die();
            return;
        }

        StartCoroutine(InvulnRoutine());
    }

    private IEnumerator InvulnRoutine()
    {
        IsInvulnerable = true;
        yield return new WaitForSeconds(InvulnDuration);
        IsInvulnerable = false;
    }

    private void Die()
    {
        // TODO: death handling, restart, etc.
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
            Damage(1);
    }
}