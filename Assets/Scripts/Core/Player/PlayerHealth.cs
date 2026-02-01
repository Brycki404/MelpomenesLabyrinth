using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int MaxHP = 5;
    public float InvulnDuration = 1f;

    public int CurrentHP { get; private set; }

    private bool localIFramesActive = false;
    private DashMovement dash;

    private void Awake()
    {
        dash = GetComponent<DashMovement>();
    }

    private void Start()
    {
        CurrentHP = MaxHP;
    }

    public bool IsInvulnerable
    {
        get
        {
            if (dash != null && dash.IsInvulnerable) return true;
            if (localIFramesActive) return true;

            // Add more checks later:
            // if (parry.IsActive) return true;
            // if (shield.IsBlocking) return true;
            // if (cutscene.IsPlaying) return true;

            return false;
        }
    }

    public void Damage(int amount, GameObject hit)
    {
        if (IsInvulnerable) return;

        if (hit)
            hit.gameObject.SetActive(false);

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
        localIFramesActive = true;
        yield return new WaitForSeconds(InvulnDuration);
        localIFramesActive = false;
    }

    private void Die()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var GO = other.gameObject;
        var bullet = GO.GetComponent<Bullet>();
        if ((bullet != null && (bullet.Faction == BulletFaction.Enemy || bullet.Faction == BulletFaction.Neutral)) || other.CompareTag("BossAttack"))
            Damage(1, GO);
    }
}