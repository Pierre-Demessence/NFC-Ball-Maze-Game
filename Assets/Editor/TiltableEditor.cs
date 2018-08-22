using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Tiltable))]
public class TiltableEditor : Editor
{

    private SerializedProperty _maxAngle;
    private SerializedProperty _tiltFactor;
    private SerializedProperty _orbitTarget;
    private SerializedProperty _target;
    private SerializedProperty _baseOffset;
    private SerializedProperty _freezeX;
    private SerializedProperty _freezeY;
    private SerializedProperty _freezeZ;

    private void OnEnable()
    {
        _maxAngle = serializedObject.FindProperty("_maxAngle");
        _tiltFactor = serializedObject.FindProperty("_tiltFactor");
        _orbitTarget = serializedObject.FindProperty("_orbitTarget");
        _target = serializedObject.FindProperty("_target");
        _baseOffset = serializedObject.FindProperty("_baseOffset");
        _freezeX = serializedObject.FindProperty("_freezeRotationX");
        _freezeY = serializedObject.FindProperty("_freezeRotationY");
        _freezeZ = serializedObject.FindProperty("_freezeRotationZ");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.Slider(_maxAngle, 0, 179);
        EditorGUILayout.DelayedFloatField(_tiltFactor);
        EditorGUILayout.Space();
        _orbitTarget.boolValue = EditorGUILayout.Toggle("Orbit Target?", _orbitTarget.boolValue);
        if (_orbitTarget.boolValue)
        {
            EditorGUILayout.PropertyField(_target);
            _baseOffset.vector3Value = EditorGUILayout.Vector3Field("Base Offset", _baseOffset.vector3Value);
        }
        EditorGUILayout.Space();
        
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Freeze Rotation");
        _freezeX.boolValue = EditorGUILayout.ToggleLeft("X", _freezeX.boolValue, GUILayout.Width(30));
        _freezeY.boolValue = EditorGUILayout.ToggleLeft("Y", _freezeY.boolValue, GUILayout.Width(30));
        _freezeZ.boolValue = EditorGUILayout.ToggleLeft("Z", _freezeZ.boolValue);
        EditorGUILayout.EndHorizontal();
        
        serializedObject.ApplyModifiedProperties();
    }
}
