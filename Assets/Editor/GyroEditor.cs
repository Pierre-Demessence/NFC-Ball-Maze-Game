using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Gyro))]
public class GyroEditor : Editor
{

    private SerializedProperty _override;
    private SerializedProperty _maxAngle;
    private SerializedProperty _threshold;

    private void OnEnable()
    {
        _override = serializedObject.FindProperty("_virtualOverride");
        _maxAngle = serializedObject.FindProperty("_maxAngle");
        _threshold = serializedObject.FindProperty("_threshold");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        _override.boolValue = EditorGUILayout.Toggle("Virtual Override", _override.boolValue);
        EditorGUILayout.Slider(_maxAngle, 0, 179);
        EditorGUILayout.Slider(_threshold, 0, 179);

        serializedObject.ApplyModifiedProperties();
    }
}
