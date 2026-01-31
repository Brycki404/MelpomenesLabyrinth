using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BehaviorPreset))]
public class BehaviorPresetEditor : Editor
{
    SerializedProperty behaviorsProp;

    private void OnEnable()
    {
        behaviorsProp = serializedObject.FindProperty("Behaviors");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("Behavior Stack", EditorStyles.boldLabel);

        for (int i = 0; i < behaviorsProp.arraySize; i++)
        {
            var element = behaviorsProp.GetArrayElementAtIndex(i);
            DrawBehaviorRequest(element, i);
        }

        if (GUILayout.Button("Add Behavior"))
            behaviorsProp.InsertArrayElementAtIndex(behaviorsProp.arraySize);

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawBehaviorRequest(SerializedProperty prop, int index)
    {
        EditorGUILayout.BeginVertical(GUI.skin.box);

        var typeProp = prop.FindPropertyRelative("Type");
        EditorGUILayout.PropertyField(typeProp);

        BehaviorType type = (BehaviorType)typeProp.enumValueIndex;

        switch (type)
        {
            case BehaviorType.Homing:
                EditorGUILayout.PropertyField(prop.FindPropertyRelative("Target"));
                EditorGUILayout.PropertyField(prop.FindPropertyRelative("FloatA"), new GUIContent("Turn Speed"));
                break;

            case BehaviorType.SineWave:
                EditorGUILayout.PropertyField(prop.FindPropertyRelative("FloatA"), new GUIContent("Frequency"));
                EditorGUILayout.PropertyField(prop.FindPropertyRelative("FloatB"), new GUIContent("Amplitude"));
                break;

            case BehaviorType.Accelerate:
                EditorGUILayout.PropertyField(prop.FindPropertyRelative("FloatA"), new GUIContent("Acceleration"));
                break;

            case BehaviorType.Fade:
                EditorGUILayout.PropertyField(prop.FindPropertyRelative("Renderer"));
                EditorGUILayout.PropertyField(prop.FindPropertyRelative("FloatA"), new GUIContent("Duration"));
                break;

            case BehaviorType.Shrink:
                EditorGUILayout.PropertyField(prop.FindPropertyRelative("FloatA"), new GUIContent("Duration"));
                break;

            case BehaviorType.Explode:
                EditorGUILayout.PropertyField(prop.FindPropertyRelative("Spawner"));
                EditorGUILayout.PropertyField(prop.FindPropertyRelative("FloatA"), new GUIContent("Lifetime"));
                EditorGUILayout.PropertyField(prop.FindPropertyRelative("FloatB"), new GUIContent("Count"));
                EditorGUILayout.PropertyField(prop.FindPropertyRelative("FloatC"), new GUIContent("Speed"));
                break;
        }

        if (GUILayout.Button("Remove"))
            behaviorsProp.DeleteArrayElementAtIndex(index);

        EditorGUILayout.EndVertical();
    }
}