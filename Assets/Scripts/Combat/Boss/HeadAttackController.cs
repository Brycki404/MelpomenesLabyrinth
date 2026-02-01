using UnityEngine;

public class HeadAttackController : MonoBehaviour
{
    public Attack[] attacks;
    public float[] durations;

    public BulletSpawner spawner;
    public AnimationStateMachine anim;

    int index;
    float timer;
    Attack current;

    void Start()
    {
        StartAttack(0);
    }

    void Update()
    {
        timer -= Time.deltaTime;
        current.UpdateAttack(Time.deltaTime);

        if (timer <= 0f || current.IsFinished)
            NextAttack();
    }

    void StartAttack(int i)
    {
        index = i;
        timer = durations[i];

        current = attacks[i];
        current.BeginAttack(transform, spawner, BulletFaction.Enemy);
    }

    void NextAttack()
    {
        int next = (index + 1) % attacks.Length;
        StartAttack(next);
    }
}