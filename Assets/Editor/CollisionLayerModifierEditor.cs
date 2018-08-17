using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CollisionLayerModifier)), CanEditMultipleObjects]
public class CollisionLayerModifierEditor : Editor
{

	private SerializedProperty _validTags;
	private SerializedProperty _enterLayer;
	private SerializedProperty _exitLayer;

	private void OnEnable()
	{
		_validTags = serializedObject.FindProperty("_validTags");
		_enterLayer = serializedObject.FindProperty("_enterLayer");
		_exitLayer = serializedObject.FindProperty("_exitLayer");
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		// Wish children could be relabeled, but there doesn't appear to be a way to do it with just PropertyField
		EditorGUILayout.PropertyField(_validTags, true);
		// LayoutField is a bit weird, other Fields can directly change the property, but LayoutField does not have that overload
		_enterLayer.intValue = EditorGUILayout.LayerField("Enter Layer", _enterLayer.intValue);
		_exitLayer.intValue = EditorGUILayout.LayerField("Exit Layer", _exitLayer.intValue);

		serializedObject.ApplyModifiedProperties();
	}
}
