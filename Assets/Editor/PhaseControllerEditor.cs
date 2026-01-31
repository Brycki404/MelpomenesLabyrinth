// Assets/Editor/PhaseControllerEditor.cs
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PhaseController))]
public class PhaseControllerEditor : Editor
{
    private SerializedProperty phasesProp;
    private SerializedProperty maxHPProp;

    private void OnEnable()
    {
        phasesProp = serializedObject.FindProperty("Phases");
        maxHPProp = serializedObject.FindProperty("MaxHP");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(maxHPProp);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Phases", EditorStyles.boldLabel);

        for (int i = 0; i < phasesProp.arraySize; i++)
        {
            var phaseProp = phasesProp.GetArrayElementAtIndex(i);
            var nameProp = phaseProp.FindPropertyRelative("Name");
            var hpProp = phaseProp.FindPropertyRelative("HPThreshold");
            var attacksProp = phaseProp.FindPropertyRelative("Attacks");

            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.BeginHorizontal();
            nameProp.stringValue = EditorGUILayout.TextField("Name", nameProp.stringValue);

            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                phasesProp.DeleteArrayElementAtIndex(i);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
                break;
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.PropertyField(hpProp, new GUIContent("HP Threshold"));
            EditorGUILayout.PropertyField(attacksProp, new GUIContent("Attacks"), true);

            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
        }

        if (GUILayout.Button("Add Phase"))
            phasesProp.InsertArrayElementAtIndex(phasesProp.arraySize);

        serializedObject.ApplyModifiedProperties();
    }
}