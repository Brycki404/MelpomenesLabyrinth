using UnityEngine;

public class AnimationDebugOverlay : MonoBehaviour
{
    public PlayerAnimationController controller;
    public Vector2 screenOffset = new Vector2(100, 10);

    void OnGUI()
    {
        if (controller == null) return;

        GUI.Label(new Rect(screenOffset.x, screenOffset.y, 400, 20),
            $"State: {controller.CurrentStateName}");

        GUI.Label(new Rect(screenOffset.x, screenOffset.y + 20, 400, 20),
            $"Time: {controller.CurrentNormalizedTime:0.00}");

        // Extend: show blend weights, facing dir, move dir, etc.
    }
}