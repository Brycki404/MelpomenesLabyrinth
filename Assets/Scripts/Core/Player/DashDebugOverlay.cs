using UnityEngine;

public class DashDebugOverlay : MonoBehaviour
{
    public DashMovement dash;
    public Vector2 offset = new Vector2(10, 80);

    void OnGUI()
    {
        if (dash == null) return;

        GUI.Label(new Rect(offset.x, offset.y, 300, 20),
            $"Dash: {(dash.IsDashing ? "DASHING" : "Idle")}");
        GUI.Label(new Rect(offset.x, offset.y + 20, 300, 20),
            $"I-Frames: {(dash.IsInvulnerable ? "ON" : "off")}");
    }
}