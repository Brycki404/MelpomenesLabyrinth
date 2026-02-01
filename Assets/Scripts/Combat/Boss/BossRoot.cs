using UnityEngine;

public class BossRoot : MonoBehaviour
{
    public PartHealth head;
    public PartHealth leftHand;
    public PartHealth rightHand;

    public HeadAttackController headController;
    public HandAttackController leftController;
    public HandAttackController rightController;

    public PhaseController phaseController;

    void Start()
    {
        head.OnPartDeath += HandlePartDeath;
        leftHand.OnPartDeath += HandlePartDeath;
        rightHand.OnPartDeath += HandlePartDeath;
    }

    void HandlePartDeath(PartHealth part)
    {
        if (part == leftHand)
            phaseController.TriggerEvent("LeftHandDead");

        if (part == rightHand)
            phaseController.TriggerEvent("RightHandDead");
    }

    public float TotalHP =>
        head.CurrentHP + leftHand.CurrentHP + rightHand.CurrentHP;

    public float TotalHPPercent =>
        TotalHP / (float)(head.maxHP + leftHand.maxHP + rightHand.maxHP);

    public void ApplyPhase(Phase p)
    {
        // Head attacks
        headController.attacks = p.headAttacks;

        // Left hand
        if (p.leftHandActive && leftHand.CurrentHP > 0)
            leftController.attacks = p.leftHandAttacks;
        else
            leftController.enabled = false;

        // Right hand
        if (p.rightHandActive && rightHand.CurrentHP > 0)
            rightController.attacks = p.rightHandAttacks;
        else
            rightController.enabled = false;
    }


}