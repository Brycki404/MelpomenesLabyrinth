using UnityEngine;

[RequireComponent(typeof(AnimationClipPlayer))]
public class PlayerAnimationController : MonoBehaviour
{
    [Header("Base states")]
    public DirectionalAnimationSet idleSet;
    public DirectionalAnimationSet moveSet;

    AnimationStateMachine fsm;

    //[Header("Special States")]

    AnimationClipPlayer player;
    AnimationState current;
    AnimationEventRunner eventRunner = new AnimationEventRunner();

    private DashMovement dash;
    private SpriteRenderer sr;

    Vector2 moveInput;
    Vector2 facing = Vector2.down;

    void Awake()
    {
        player = GetComponent<AnimationClipPlayer>();
        dash = GetComponentInParent<DashMovement>();
        sr = GetComponent<SpriteRenderer>(); // NEW
        fsm = new AnimationStateMachine(player);
    }

    void Update()
    {
        // Replace with your real input
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (moveInput.sqrMagnitude > 0.01f)
            facing = moveInput.normalized;

        AnimationState next = DecideState();
        if (next != current)
        {
            current = next;
            fsm.Play(current);
            eventRunner.SetState(current, OnAnimationEvent);
        }

        eventRunner.Update(player.NormalizedTime);
    }

    AnimationState DecideState()
    {
        var ctx = new DirectionAnimationContext
        {
            MoveDir = moveInput,
            FacingDir = facing
        };

        // Movement
        if (moveInput.sqrMagnitude > 0.01f)
        {
            var resolved = DirectionResolver.Resolve(ctx, moveSet);
            sr.flipX = resolved.flipX;     // NEW
            return resolved.state;
        }

        // Idle
        {
            var resolved = DirectionResolver.Resolve(ctx, idleSet);
            sr.flipX = resolved.flipX;     // NEW
            return resolved.state;
        }
    }

    void OnAnimationEvent(string evt)
    {
        Debug.Log($"AnimEvent: {current.stateName} -> {evt}");

        if (evt == "DashEnd")
            dash.EndDash();
    }

    public string CurrentStateName => current != null ? current.stateName : "None";
    public float CurrentNormalizedTime => player.NormalizedTime;
    public Vector2 GetFacingDir() => facing;

    public float GetNormalizedTimeForState(AnimationState state)
    {
        return current == state ? player.NormalizedTime : 0f;
    }

    public SpriteRenderer GetSpriteRenderer() => sr;
}