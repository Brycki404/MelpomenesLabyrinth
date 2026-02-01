using UnityEngine;

public abstract class Attack : ScriptableObject
{
    [Header("Metadata")]
    public string attackName;
    [Header("Timing")]
    public float immediateAttackDelay = 0f;   // delay before first action
    public float duration = 2f;               // total attack duration

    private float delayTimer;
    private float durationTimer;

    protected Transform part;        // the hand or head running this attack
    protected BulletSpawner spawner; // assigned by controller
    protected BulletFaction faction;
    protected bool finished;

    public virtual void BeginAttack(Transform part, BulletSpawner spawner, BulletFaction faction)
    {
        this.part = part;
        this.spawner = spawner;
        this.faction = faction;
        finished = false;
        delayTimer = immediateAttackDelay;
        durationTimer = duration;
    }

    public virtual void UpdateAttack(float dt)
    {
        // Handle initial delay
        if (delayTimer > 0f)
        {
            delayTimer -= dt;
            return;
        }

        // Handle duration
        durationTimer -= dt;
        if (durationTimer <= 0f)
        {
            finished = true;
            return;
        }

        // Attack-specific logic goes here
    }

    public virtual void EndAttack()
    {
        if (durationTimer != 0f)
        {
            durationTimer = 0f;
            finished = true;
        }
    }

    public bool IsFinished => finished;
}