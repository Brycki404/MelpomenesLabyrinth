using UnityEditor;
using UnityEngine;

public class DamageFlashWindow : EditorWindow
{
    [MenuItem("Tools/Visual/Debug Damage Flash")]
    public static void Open()
    {
        GetWindow<DamageFlashWindow>("Damage Flash");
    }

    private SpriteRenderer targetSR;
    private float flashAmount;

    private void OnGUI()
    {
        targetSR = (SpriteRenderer)EditorGUILayout.ObjectField("Sprite Renderer", targetSR, typeof(SpriteRenderer), true);

        if (targetSR == null) return;

        flashAmount = EditorGUILayout.Slider("Flash Amount", flashAmount, 0, 1);

        if (GUILayout.Button("Apply Flash"))
        {
            var mat = targetSR.sharedMaterial;
            if (mat != null)
                mat.SetFloat("_FlashAmount", flashAmount);
        }
    }
}