using UnityEngine;

public class BulletBehaviorDebugger : MonoBehaviour
{
    public Bullet Bullet;
    public Vector3 Offset = new Vector3(0, 0.5f, 0);

    private void OnGUI()
    {
        if (Bullet == null) return;

        Vector3 screenPos = Camera.main.WorldToScreenPoint(Bullet.transform.position + Offset);
        if (screenPos.z < 0) return;

        string text = Bullet.DebugBehaviorList;
        GUI.Label(new Rect(screenPos.x, Screen.height - screenPos.y, 200, 20), text);
    }
}