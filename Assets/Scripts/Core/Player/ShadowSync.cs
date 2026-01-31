using UnityEngine;

public class ShadowSync : MonoBehaviour
{
    public Animator visualAnimator;
    public Animator shadowAnimator;

    void LateUpdate()
    {
        if (visualAnimator == null || shadowAnimator == null) return;

        AnimatorStateInfo info = visualAnimator.GetCurrentAnimatorStateInfo(0);
        shadowAnimator.Play(info.fullPathHash, 0, info.normalizedTime);
    }
}