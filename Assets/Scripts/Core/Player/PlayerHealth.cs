using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int MaxHP = 5;
    public float InvulnDuration = 1f;

    public int CurrentHP { get; private set; }

    private bool localIFramesActive = false;
    private DashMovement dash;
    private DamageFlash damageFlash;
    private MaterialFX mat;

    private void Awake()
    {
        dash = GetComponent<DashMovement>();
        damageFlash = GetComponentInChildren<DamageFlash>();
        mat = GetComponentInChildren<MaterialFX>();
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
        if (CurrentHP <= 0) return;
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

        damageFlash.TriggerFlash();
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
        StartCoroutine(DissolveOut());
    }

    IEnumerator DissolveOut()
    {
        float dur = 3f;
        float t = 0f;

        while (t < dur)
        {
            t += Time.deltaTime;
            float p = t / dur; // 0 → 1

            // Apply easing
            float eased = EasingLibrary.EaseOutQuad(p);

            // Dissolve goes from 1 → 0
            float dissolveValue = Mathf.Lerp(1f, 0f, eased);

            mat.SetDissolve(dissolveValue);

            yield return null;
        }
        
        // Ensure final value is exactly 0
        mat.SetDissolve(0f);

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