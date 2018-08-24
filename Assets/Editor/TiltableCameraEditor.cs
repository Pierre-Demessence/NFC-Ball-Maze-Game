using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TiltableCamera))]
public class TiltableCameraEditor : Editor {

    private SerializedProperty _maxAngle;
    private SerializedProperty _tiltFactor;
    private SerializedProperty _target;
    private SerializedProperty _baseOffset;

    private void OnEnable()
    {
        _maxAngle = serializedObject.FindProperty("_maxAngle");
        _tiltFactor = serializedObject.FindProperty("_tiltFactor");
        _target = serializedObject.FindProperty("_target");
        _baseOffset = serializedObject.FindProperty("_baseOffset");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.Slider(_maxAngle, 0, 179);
        EditorGUILayout.DelayedFloatField(_tiltFactor);
        EditorGUILayout.Space();
        
        EditorGUILayout.PropertyField(_target);
        _baseOffset.vector3Value = EditorGUILayout.Vector3Field("Base Offset", _baseOffset.vector3Value);
        
        serializedObject.ApplyModifiedProperties();
    }
}
