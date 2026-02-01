using UnityEngine;

public class PlayerAnimationDebugOverlay : MonoBehaviour
{
    public PlayerAnimationController anim;
    public Vector2 offset = new Vector2(20, 20);

    private Texture2D white;

    void Awake()
    {
        white = new Texture2D(1, 1);
        white.SetPixel(0, 0, Color.white);
        white.Apply();
    }

    void OnGUI()
    {
        Debug.Log("IMGUI WINDOW DRAW");
        // Create a draggable, self-clearing IMGUI window
        debugWindowRect = GUI.Window(
            123456,                     // unique ID
            debugWindowRect,            // position + size
            DrawDebugWindow,            // callback
            "Player Debug"              // window title
        );
    }

    // The window rect (position + size)
    private Rect debugWindowRect = new Rect(20, 20, 300, 400);

    void DrawDebugWindow(int windowID)
    {
        if (anim == null)
        {
            GUILayout.Label("No animation controller assigned.");
            GUI.DragWindow();
            return;
        }

        float x = 10;
        float y = 20;

        // ---------------------------------------------------------
        // 1. COMPASS ARROW
        // ---------------------------------------------------------
        GUI.Label(new Rect(x, y, 200, 20), "Facing:");
        DrawArrow(new Vector2(x + 100, y + 40), anim.GetFacingDir(), 30, Color.yellow);
        y += 80;

        // ---------------------------------------------------------
        // 2. STATE MACHINE
        // ---------------------------------------------------------
        GUI.Label(new Rect(x, y, 200, 20), "State Machine:");
        y += 25;

        DrawStateBox("Idle", anim.CurrentStateName.Contains("Idle"), x, ref y);
        DrawStateBox("Move", anim.CurrentStateName.Contains("Move"), x, ref y);

        y += 10;

        // ---------------------------------------------------------
        // 3. LIVE SPRITE PREVIEW
        // ---------------------------------------------------------
        GUI.Label(new Rect(x, y, 200, 20), "Live Preview:");
        y += 25;

        SpriteRenderer sr = anim.GetSpriteRenderer();
        if (sr != null && sr.sprite != null)
        {
            Sprite s = sr.sprite;
            Rect r = new Rect(x, y, 64, 64);

            Rect uv = new Rect(
                s.textureRect.x / s.texture.width,
                s.textureRect.y / s.texture.height,
                s.textureRect.width / s.texture.width,
                s.textureRect.height / s.texture.height
            );

            GUI.DrawTextureWithTexCoords(r, s.texture, uv);
        }
        y += 80;

        // ---------------------------------------------------------
        // 4. flipX INDICATOR
        // ---------------------------------------------------------
        GUI.Label(new Rect(x, y, 200, 20), "flipX:");
        DrawColorBox(new Rect(x + 60, y, 20, 20), sr.flipX ? Color.red : Color.green);
        y += 30;

        // ---------------------------------------------------------
        // 5. QUADRANT VISUALIZER
        // ---------------------------------------------------------
        GUI.Label(new Rect(x, y, 200, 20), "Direction Quadrant:");
        y += 25;

        DrawQuadrant(new Rect(x, y, 100, 100), anim.GetFacingDir());

        // Make window draggable
        GUI.DragWindow();
    }
    // ---------------------------------------------------------
    // Helpers
    // ---------------------------------------------------------

    void DrawArrow(Vector2 center, Vector2 dir, float length, Color color)
    {
        if (dir.sqrMagnitude < 0.01f) return;

        Vector2 end = center + dir.normalized * length;
        DrawLine(center, end, color, 3f);
    }

    void DrawLine(Vector2 a, Vector2 b, Color color, float width)
    {
        Matrix4x4 oldMatrix = GUI.matrix;
        Color oldColor = GUI.color;

        GUI.color = new Color(color.r, color.g, color.b, 1f); // force opaque

        float angle = Vector3.Angle(b - a, Vector2.right);
        if (a.y > b.y) angle = -angle;

        GUIUtility.RotateAroundPivot(angle, a);
        GUI.DrawTexture(new Rect(a.x, a.y, (b - a).magnitude, width), white);

        GUI.matrix = oldMatrix;
        GUI.color = oldColor;
    }

    void DrawStateBox(string label, bool active, float x, ref float y)
    {
        Color c = active ? Color.green : Color.gray;
        DrawColorBox(new Rect(x, y, 20, 20), c);
        GUI.Label(new Rect(x + 30, y, 200, 20), label);
        y += 25;
    }

    void DrawColorBox(Rect r, Color c)
    {
        Color old = GUI.color;
        GUI.color = c;
        GUI.DrawTexture(r, white);
        GUI.color = old;
    }

    void DrawQuadrant(Rect r, Vector2 dir)
    {
        // Background
        DrawColorBox(r, new Color(0, 0, 0, 0.25f));

        // Cross lines
        DrawLine(new Vector2(r.x + r.width / 2, r.y), new Vector2(r.x + r.width / 2, r.y + r.height), Color.white, 1f);
        DrawLine(new Vector2(r.x, r.y + r.height / 2), new Vector2(r.x + r.width, r.y + r.height / 2), Color.white, 1f);

        // Dot for direction
        Vector2 center = new Vector2(r.x + r.width / 2, r.y + r.height / 2);
        Vector2 pos = center + dir.normalized * (r.width / 2 - 10);

        DrawColorBox(new Rect(pos.x - 5, pos.y - 5, 10, 10), Color.yellow);
    }
}