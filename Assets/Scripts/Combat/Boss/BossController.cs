using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossController : MonoBehaviour
{
    // ============================================================
    // HP SETTINGS
    // ============================================================
    [Header("HP Settings")]
    public float bossMaxHP = 1000;
    public float bossHP;

    public float handMaxHP = 200;
    public float leftHandHP;
    public float rightHandHP;

    [Header("Bonus Damage When Hand Dies")]
    public float handDeathDamageChunk = 150;

    // ============================================================
    // REFERENCES
    // ============================================================
    [Header("References")]
    public Transform head;
    public Transform leftHand;
    public Transform rightHand;

    [Header("Hand Gesture Clips")]
    public AnimationClip rightIdleClip;
    public AnimationClip rightChargeClip;
    public AnimationClip leftSlamClip;
    public AnimationClip rightRecoilClip;
    public AnimationClip leftReleaseClip;
    public AnimationClip downBeamClip;
    public AnimationClip leftPunchClip;
    
    private Transform player;

    private AnimationClipPlayer leftAnim;
    private AnimationClipPlayer rightAnim;
    private AnimationClipPlayer headAnim;

    private BulletSpawner leftbs;
    private BulletSpawner rightbs;
    private BulletSpawner headbs;

    private MaterialFX leftmfx;
    private MaterialFX rightmfx;
    private MaterialFX headmfx;

    private DamageFlash leftdf;
    private DamageFlash rightdf;
    private DamageFlash headdf;

    private SpriteRenderer leftsr;
    private SpriteRenderer rightsr;

    // ============================================================
    // PHASE SETTINGS
    // ============================================================
    [Header("Phase Thresholds")]
    public float phase2Threshold = 0.70f;
    public float phase3Threshold = 0.40f;

    private int currentPhase = 1;
    private bool leftHandDead = false;
    private bool rightHandDead = false;

    // ============================================================
    // IDLE MOVEMENT SETTINGS
    // ============================================================

    [Header("Head Infinity Path")]
    public MovementPath infinityPath;
    public float headInfinitySpeed = 1.2f;

    [Header("Hand Float Idle")]
    public float handFloatAmplitude = 0.25f;
    public float handFloatSpeed = 1f;

    [Header("Head Drift Idle")]
    public float driftSpeed = 1f;
    public float driftAmplitude = 1f;

    [Header("Hand Recoil Distance")]
    public float recoilDistance = 1.5f;

    private Vector3 headBasePos;
    private Vector3 leftBasePos;
    private Vector3 rightBasePos;

    private Coroutine headRoutine;
    private Coroutine leftRoutine;
    private Coroutine rightRoutine;
    private Coroutine headIdleRoutine;
    private Coroutine leftIdleRoutine;
    private Coroutine rightIdleRoutine;


    // ============================================================
    // ATTACK SETTINGS (Slam, Beam, etc.)
    // ============================================================
    [Header("Slam")]
    public float slamLiftHeight = 1.2f;
    public float slamTelegraphTime = 0.4f;
    public float slamShakeStrength = 0.15f;
    public float slamDownTime = 0.12f;
    public float slamLiftTime = 0.25f;

    // ============================================================
    // UNITY LIFECYCLE
    // ============================================================
    void Awake()
    {
        leftbs = leftHand.GetComponent<BulletSpawner>();
        rightbs = rightHand.GetComponent<BulletSpawner>();
        headbs = head.GetComponent<BulletSpawner>();

        leftAnim = leftHand.GetComponentInChildren<AnimationClipPlayer>();
        rightAnim = rightHand.GetComponentInChildren<AnimationClipPlayer>();
        headAnim = head.GetComponentInChildren<AnimationClipPlayer>();

        leftsr = leftHand.GetComponentInChildren<SpriteRenderer>();
        rightsr = rightHand.GetComponentInChildren<SpriteRenderer>();

        leftmfx = leftHand.GetComponentInChildren<MaterialFX>();
        rightmfx = rightHand.GetComponentInChildren<MaterialFX>();
        headmfx = head.GetComponentInChildren<MaterialFX>();

        leftdf = leftHand.GetComponentInChildren<DamageFlash>();
        rightdf = rightHand.GetComponentInChildren<DamageFlash>();
        headdf = head.GetComponentInChildren<DamageFlash>();

        player = GameObject.Find("Player").transform;
    }

    void Start()
    {
        bossHP = bossMaxHP;
        leftHandHP = handMaxHP;
        rightHandHP = handMaxHP;

        headBasePos = head.localPosition;
        leftBasePos = leftHand.localPosition;
        rightBasePos = rightHand.localPosition;

        // Start idle movement
        headIdleRoutine = StartCoroutine(HeadInfinityIdle());
        leftIdleRoutine = StartCoroutine(HandFloatIdle(leftHand, leftBasePos));
        rightIdleRoutine = StartCoroutine(HandFloatIdle(rightHand, rightBasePos));

        // Start attack loops
        StartCoroutine(PhaseWatcher());
        headRoutine = StartCoroutine(HeadAttackLoop());
        leftRoutine = StartCoroutine(LeftHandAttackLoop());
        rightRoutine = StartCoroutine(RightHandAttackLoop());
    }

    // ============================================================
    // IDLE MOVEMENT COROUTINES
    // ============================================================

    IEnumerator HeadInfinityIdle()
    {
        float t = 0f;

        while (true)
        {
            t += Time.deltaTime * headInfinitySpeed;
            Vector2 offset = infinityPath.Evaluate(t);
            head.localPosition = headBasePos + (Vector3)offset;
            yield return null;
        }
    }

    IEnumerator HandFloatIdle(Transform hand, Vector3 basePos, float newHandFloatSpeed = 1f)
    {
        float t = 0f;

        while (true)
        {
            t += Time.deltaTime * newHandFloatSpeed;
            float y = Mathf.Sin(t) * handFloatAmplitude;
            hand.localPosition = basePos + new Vector3(0, y, 0);
            yield return null;
        }
    }

    void StopHandIdle(Transform hand)
    {
        if (hand == leftHand && leftRoutine != null)
            StopCoroutine(leftIdleRoutine);

        if (hand == rightHand && rightRoutine != null)
            StopCoroutine(rightIdleRoutine);
    }

    void ResumeHandIdle(Transform hand)
    {
        if (hand == leftHand)
            leftAnim.Play(rightIdleClip);
            leftsr.flipX = true;
            leftIdleRoutine = StartCoroutine(HandFloatIdle(leftHand, leftBasePos));

        if (hand == rightHand)
            rightAnim.Play(rightIdleClip);
            rightsr.flipX = false;
            rightIdleRoutine = StartCoroutine(HandFloatIdle(rightHand, rightBasePos));
    }

    IEnumerator HeadDriftIdle()
    {
        float t = 0f;

        while (true)
        {
            t += Time.deltaTime * driftSpeed;

            float x = Mathf.Sin(t) * driftAmplitude;
            float y = Mathf.Cos(t * 0.5f) * driftAmplitude;

            head.localPosition = headBasePos + new Vector3(x, y, 0);
            yield return null;
        }
    }

    IEnumerator HeadInfinityDriftBlend()
    {
        float t = 0f;

        while (true)
        {
            t += Time.deltaTime;

            Vector2 infinity = infinityPath.Evaluate(t * headInfinitySpeed);
            Vector2 drift = new Vector2(
                Mathf.Sin(t * driftSpeed) * driftAmplitude,
                Mathf.Cos(t * driftSpeed * 0.5f) * driftAmplitude
            );

            head.localPosition = headBasePos + (Vector3)(infinity + drift * 0.5f);
            yield return null;
        }
    }

    void ApplyPhaseIdlePatterns()
    {
        // Stop all idles first
        if (headIdleRoutine != null) StopCoroutine(headIdleRoutine);
        if (leftIdleRoutine != null) StopCoroutine(leftIdleRoutine);
        if (rightIdleRoutine != null) StopCoroutine(rightIdleRoutine);

        //Stop all other routines second? I guess
        if (headRoutine != null) StopCoroutine(headRoutine);
        if (leftRoutine != null) StopCoroutine(leftRoutine);
        if (rightRoutine != null) StopCoroutine(rightRoutine);

        if (currentPhase == 1)
        {
            headIdleRoutine = StartCoroutine(HeadInfinityIdle());
            leftIdleRoutine = StartCoroutine(HandFloatIdle(leftHand, leftBasePos, handFloatSpeed));
            rightIdleRoutine = StartCoroutine(HandFloatIdle(rightHand, rightBasePos, handFloatSpeed));
        }
        else if (currentPhase == 2)
        {
            headIdleRoutine = StartCoroutine(HeadDriftIdle());
            leftIdleRoutine = StartCoroutine(HandFloatIdle(leftHand, leftBasePos, handFloatSpeed * 1.5f));
            rightIdleRoutine = StartCoroutine(HandFloatIdle(rightHand, rightBasePos, handFloatSpeed * 1.5f));
        }
        else if (currentPhase == 3)
        {
            headIdleRoutine = StartCoroutine(HeadInfinityDriftBlend());
            // Hands dead or irrelevant
        }
    }

    // ============================================================
    // DAMAGE HANDLING
    // ============================================================
    public void DamageBoss(float amount)
    {
        bossHP -= amount;
        if (bossHP <= 0)
            Die();
        else
            headdf.TriggerFlash();
    }

    public void DamageLeftHand(float amount)
    {
        if (leftHandDead) return;

        leftHandHP -= amount;
        if (leftHandHP <= 0)
        {
            leftHandDead = true;
            StartCoroutine(LeftHandDissolveOut());
            DamageBoss(handDeathDamageChunk);
        }
        else
            leftdf.TriggerFlash();
    }

    public void DamageRightHand(float amount)
    {
        if (rightHandDead) return;

        rightHandHP -= amount;
        if (rightHandHP <= 0)
        {
            rightHandDead = true;
            StartCoroutine(RightHandDissolveOut());
            DamageBoss(handDeathDamageChunk);
        }
        else
            rightdf.TriggerFlash();
    }

    // ============================================================
    // PHASE LOGIC
    // ============================================================
    IEnumerator PhaseWatcher()
    {
        while (bossHP > 0)
        {
            float hpPercent = bossHP / bossMaxHP;

            if (currentPhase == 1 &&
                (leftHandDead || rightHandDead || hpPercent <= phase2Threshold))
            {
                currentPhase = 2;
                OnEnterPhase2();
            }

            if (currentPhase == 2 &&
                (leftHandDead && rightHandDead || hpPercent <= phase3Threshold))
            {
                currentPhase = 3;
                OnEnterPhase3();
            }

            yield return null;
        }
    }

    void OnEnterPhase2()
    {
        Debug.Log("Boss entering Phase 2!");
        ApplyPhaseIdlePatterns();
        // Example: head attacks faster
        StopCoroutine(headIdleRoutine);
        headRoutine = StartCoroutine(HeadAttackLoop(phase: 2));
        // Example: surviving hand becomes more aggressive
        if (!leftHandDead)
        {
            StopCoroutine(leftIdleRoutine);
            leftRoutine = StartCoroutine(LeftHandAttackLoop(phase: 2));
        }
        if (!rightHandDead)
        {
            StopCoroutine(rightIdleRoutine);
            rightRoutine = StartCoroutine(RightHandAttackLoop(phase: 2));
        }
    }

    void OnEnterPhase3()
    {
        Debug.Log("Boss entering Phase 3!");
        ApplyPhaseIdlePatterns();
        // Head goes into final mode
        StopCoroutine(headIdleRoutine);
        headRoutine = StartCoroutine(HeadAttackLoop(phase: 3));
        // Hands are dead or irrelevant now
    }


    // ============================================================
    // ATTACK LOOPS
    // ============================================================
    IEnumerator HeadAttackLoop(int phase = 1)
    {
        while (true)
        {
            if (phase == 1)
            {
                // Shoot single bullets
                yield return StartCoroutine(HeadShootSingle());
            }
            else if (phase == 2)
            {
                // Rotating ring + single shots
                // yield return StartCoroutine(HeadRotatingRing());
                yield return StartCoroutine(HeadShootSingle());
                yield return StartCoroutine(HeadSwoopAttack());
            }
            else if (phase == 3)
            {
                // Charge beam + ring + rapid fire
                // yield return StartCoroutine(HeadChargeBeam());
                // yield return StartCoroutine(HeadRotatingRing());
                yield return StartCoroutine(HeadRapidFire());
            }

            yield return new WaitForSeconds(1f);
        }
    }
    
    IEnumerator LeftHandAttackLoop(int phase = 1)
    {
        while (!leftHandDead)
        {
            if (phase == 1)
            {
                //yield return StartCoroutine(HandBeamAttack(leftHand, leftbs));
                yield return StartCoroutine(HandReleaseAttack(leftHand, leftbs));
                yield return StartCoroutine(HandSlamAttack(leftHand, leftbs));
            }
            else if (phase == 2)
            {
                yield return StartCoroutine(HandReleaseAttack(leftHand, leftbs));
            }

            yield return new WaitForSeconds(1f);
        }
    }


    IEnumerator RightHandAttackLoop(int phase = 1)
    {
        while (!rightHandDead)
        {
            if (phase == 1)
            {
                yield return StartCoroutine(HandSlamAttack(rightHand, rightbs));
                yield return StartCoroutine(HandReleaseAttack(rightHand, rightbs));
                //yield return StartCoroutine(HandBeamAttack(rightHand, rightbs));
            }
            else if (phase == 2)
            {
                yield return StartCoroutine(HandSlamAttack(rightHand, rightbs));
            }

            yield return new WaitForSeconds(1f);
        }
    }

    // ============================================================
    // HEAD ATTACKS
    // ============================================================
    IEnumerator HeadSwoopAttack()
    {
        // Stop idle
        if (headIdleRoutine != null) StopCoroutine(headIdleRoutine);

        Vector3 start = head.position;
        Vector3 target = player.position + new Vector3(0, -1f, 0);

        float t = 0f;
        float swoopTime = 1.55f;

        // Swoop toward player
        while (t < swoopTime)
        {
            t += Time.deltaTime;
            float p = Mathf.SmoothStep(0, 1, t / swoopTime);
            head.position = Vector3.Lerp(start, target, p);
            yield return null;
        }

        // Impact bullets or shockwave
        // SpawnSwoopImpact(head.position);

        // Return to idle
        t = 0f;
        float returnTime = 0.4f;

        while (t < returnTime)
        {
            t += Time.deltaTime;
            float p = Mathf.SmoothStep(0, 1, t / returnTime);
            head.position = Vector3.Lerp(target, headBasePos, p);
            yield return null;
        }

        // Resume idle
        headIdleRoutine = StartCoroutine(
            currentPhase == 1 ? HeadInfinityIdle() :
            currentPhase == 2 ? HeadDriftIdle() :
                                HeadInfinityDriftBlend()
        );
    }

    IEnumerator HeadShootSingle()
    {
        Vector2 mouthOffset = (Vector2) head.Find("Mouth").localPosition;
        Vector2 leftEyeOffset = (Vector2) head.Find("LeftEye").localPosition;
        Vector2 rightEyeOffset = (Vector2) head.Find("RightEye").localPosition;

        Vector2 rngOffset = Vector2.zero;
        int rng = Random.Range(1, 3);
        if (rng == 1)
            rngOffset = mouthOffset;
        else if (rng == 2)
            rngOffset = leftEyeOffset;
        else if (rng == 3)
            rngOffset = rightEyeOffset;
        Vector2 spawnPos = (Vector2) head.position + rngOffset;
        

        Vector2 dir = ((Vector2) player.position -  spawnPos).normalized;
        Vector3 pos = new Vector3(spawnPos.x, spawnPos.y, -1f);

        BulletFactory.SpawnBullet(headbs, pos, dir, 4f, BulletFaction.Enemy);

        // 5. Recovery delay
        yield return new WaitForSeconds(0.3f);
    }

    IEnumerator HeadRotatingRing()
    {


        yield return null;
    }

    IEnumerator HeadChargeBeam()
    {
        // // 1. Start mouth glow
        // float t = 0f;
        // while (t < chargeTime)
        // {
        //     t += Time.deltaTime;
        //     float glow = Mathf.Lerp(0f, 1f, t / chargeTime);
        //     mouthGlowRenderer.color = new Color(1f, glow, glow, 1f);
        //     yield return null;
        // }

        // // 2. Telegraph line (thin laser)
        // GameObject telegraph = Instantiate(laserTelegraphPrefab);
        // telegraph.transform.position = head.position;
        // telegraph.transform.up = (player.position - head.position).normalized;

        // yield return new WaitForSeconds(telegraphDuration);

        // // 3. FIRE BEAM
        // GameObject beam = Instantiate(laserBeamPrefab);
        // beam.transform.position = head.position;
        // beam.transform.up = (player.position - head.position).normalized;

        // beam.GetComponent<DamageZone>().Activate();

        // yield return new WaitForSeconds(beamDuration);

        // // Cleanup
        // Destroy(telegraph);
        // Destroy(beam);

        // // Reset glow
        // mouthGlowRenderer.color = new Color(1f, 0f, 0f, 1f);
        
        yield return null;
    }
    IEnumerator HeadRapidFire()
    {
        Vector2 mouthOffset = (Vector2) head.Find("Mouth").localPosition;
        Vector2 spawnPos = (Vector2) head.position + mouthOffset;
        Vector2 dir = ((Vector2) player.localPosition -  spawnPos).normalized;
        Vector3 pos = new Vector3(spawnPos.x, spawnPos.y, -1f);

        int bulletCount = 15;
        for (int i = 1; i <= bulletCount; i++)
        {
            BulletFactory.SpawnBullet(headbs, pos, dir, 4f, BulletFaction.Enemy);
            yield return new WaitForSeconds(0.1f);
        }

        // 5. Recovery delay
        yield return new WaitForSeconds(2.5f);
    }

    IEnumerator HandBeamAttack(Transform hand, BulletSpawner spawner)
    {
        // // Telegraph object (a big red rectangle)
        // GameObject telegraph = Instantiate(beamTelegraphPrefab);
        // telegraph.transform.position = hand.position;

        // // Stretch telegraph across the player area
        // telegraph.transform.localScale = new Vector3(beamWidth, beamLength, 1f);

        // // Fade-in telegraph (warning)
        // float t = 0f;
        // while (t < telegraphFadeInTime)
        // {
        //     t += Time.deltaTime;
        //     float a = t / telegraphFadeInTime;
        //     telegraph.GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, a);
        //     yield return null;
        // }

        // // Hold telegraph fully visible
        // yield return new WaitForSeconds(telegraphHoldTime);

        // // FIRE BEAM
        // GameObject beam = Instantiate(beamPrefab);
        // beam.transform.position = hand.position;
        // beam.transform.localScale = new Vector3(beamWidth, beamLength, 1f);

        // // Damage zone active
        // beam.GetComponent<DamageZone>().Activate();

        // // Beam stays active
        // yield return new WaitForSeconds(beamDuration);

        // // Cleanup
        // Destroy(telegraph);
        // Destroy(beam);

        yield return null;
    }

    // ============================================================
    // HAND ATTACKS
    // ============================================================
    IEnumerator HandRecoil(Transform hand, Vector3 basePos, Vector2 direction)
    {
        SpriteRenderer sr = hand.GetComponentInChildren<SpriteRenderer>();
        AnimationClipPlayer anim = hand.GetComponentInChildren<AnimationClipPlayer>();

        anim.Play(rightRecoilClip);
        if (hand.gameObject.name.Contains("LeftHand"))
            sr.flipX = true;
        else if (hand.gameObject.name.Contains("RightHand"))
            sr.flipX = false;

        float t = 0f;
        float recoilTime = 0.12f;
        float returnTime = 0.18f;

        Vector3 recoilPos = basePos - (Vector3)direction.normalized * recoilDistance;

        // Move backward
        while (t < recoilTime)
        {
            t += Time.deltaTime;
            float p = t / recoilTime;
            hand.localPosition = Vector3.Lerp(basePos, recoilPos, p);
            yield return null;
        }

        anim.Play(rightIdleClip);
        if (hand.gameObject.name.Contains("LeftHand"))
            sr.flipX = true;
        else if (hand.gameObject.name.Contains("RightHand"))
            sr.flipX = false;

        // Return to base
        t = 0f;
        while (t < returnTime)
        {
            t += Time.deltaTime;
            float p = t / returnTime;
            hand.localPosition = Vector3.Lerp(recoilPos, basePos, p);
            yield return null;
        }
    }

    IEnumerator HandReleaseAttack(Transform hand, BulletSpawner spawner) {
        Vector2 startPos = (Vector2) hand.position;
        Vector2 startDir = ((Vector2) player.localPosition -  startPos).normalized;
        startDir *= new Vector2(0f, -1f);
        float startAngle = PatternMath.DirectionToAngle(startDir);
        Vector3 pos = new Vector3(startPos.x, startPos.y, -1f);
        float gravity = -4.9f;

        var behaviors = new List<IBulletBehavior>()
        {
            new GravityBehavior(new Vector2(0, gravity))
        };

        int bulletCount = 15;
        for (int i = 1; i <= bulletCount; i++)
        {
            float randomAngle = startAngle + Random.Range(-60f, 60f);
            Vector2 dir = PatternMath.AngleToDirection(randomAngle);

            Vector3 thispos = (Vector2) pos + dir.normalized * 24f / 16f;

            BulletFactory.SpawnBullet(spawner, thispos, dir, 3f, BulletFaction.Enemy, behaviors);
        }

        // 5. Recovery delay
        yield return new WaitForSeconds(2f);
    }

    IEnumerator HandSlamAttack(Transform hand, BulletSpawner spawner)
    {
        // Stop idle
        StopHandIdle(hand);

        // Telegraph
        float t = 0f;
        Vector3 basePos = hand.localPosition;
        SpriteRenderer sr = hand.GetComponentInChildren<SpriteRenderer>();
        AnimationClipPlayer anim = hand.GetComponentInChildren<AnimationClipPlayer>();

        anim.Play(rightChargeClip);
        if (hand.gameObject.name.Contains("LeftHand"))
            sr.flipX = true;
        else if (hand.gameObject.name.Contains("RightHand"))
            sr.flipX = false;

        while (t < slamTelegraphTime)
        {
            t += Time.deltaTime;
            hand.localPosition = basePos + (Vector3)Random.insideUnitCircle * slamShakeStrength;

            float f = Mathf.PingPong(t * 12f, 1f);
            sr.color = Color.Lerp(Color.white, Color.red, f);

            yield return null;
        }

        hand.localPosition = basePos;
        sr.color = Color.white;

        // Lift
        Vector3 liftedPos = basePos + new Vector3(0, slamLiftHeight, 0);
        t = 0f;
        while (t < slamLiftTime)
        {
            t += Time.deltaTime;
            float p = t / slamLiftTime;
            hand.localPosition = Vector3.Lerp(basePos, liftedPos, p);
            yield return null;
        }

        // Slam
        t = 0f;
        while (t < slamDownTime)
        {
            t += Time.deltaTime;
            float p = t / slamDownTime;
            hand.localPosition = Vector3.Lerp(liftedPos, basePos, p);
            yield return null;
        }

        anim.Play(leftSlamClip);
        if (hand.gameObject.name.Contains("LeftHand"))
            sr.flipX = false;
        else if (hand.gameObject.name.Contains("RightHand"))
            sr.flipX = true;

        // Impact bullets
        int bulletCount = 12;
        float bulletSpeed = 5f;
        float padding = 1.375f / bulletCount;
        float gravity = -9.8f / 16f;

        Vector2 center = hand.localPosition - new Vector3(0, 1.3125f / 2f, 0);

        var behaviors = new List<IBulletBehavior>()
        {
            new GravityBehavior(new Vector2(0, gravity))
        };

        var offsets = PatternMath.LineCenterXOffsets(bulletCount, padding);

        foreach (var offset in offsets)
        {
            float randomAngle = 270f + Random.Range(-30f, 30f);
            Vector2 dir = PatternMath.AngleToDirection(randomAngle);

            Vector2 pos = center + offset;
            BulletFactory.SpawnBullet(spawner, pos, dir, bulletSpeed, BulletFaction.Enemy, behaviors);
        }

        yield return StartCoroutine(HandRecoil(hand, basePos, Vector2.down));

        yield return new WaitForSeconds(0.3f);
    }

    // ============================================================
    // DEATH
    // ============================================================
    void Die()
    {
        Debug.Log("Boss defeated!");
        StopAllCoroutines();
        // TODO: death animation, loot, etc.
        StartCoroutine(BossDiedDissolveOut());
    }

    IEnumerator LeftHandDissolveOut()
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

            leftmfx.SetDissolve(dissolveValue);

            yield return null;
        }

        // Ensure final value is exactly 0
        leftmfx.SetDissolve(0f);

        leftHand.gameObject.SetActive(false);
    }

    IEnumerator RightHandDissolveOut()
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

            rightmfx.SetDissolve(dissolveValue);

            yield return null;
        }

        // Ensure final value is exactly 0
        rightmfx.SetDissolve(0f);
        
        rightHand.gameObject.SetActive(false);
    }

    IEnumerator BossDiedDissolveOut()
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

            headmfx.SetDissolve(dissolveValue);
            if (!leftHandDead)
                leftmfx.SetDissolve(dissolveValue);
            if (!rightHandDead)
                rightmfx.SetDissolve(dissolveValue);

            yield return null;
        }

        // Ensure final value is exactly 0
        headmfx.SetDissolve(0f);
        leftmfx.SetDissolve(0f);
        rightmfx.SetDissolve(0f);

        gameObject.SetActive(false);
    }
}