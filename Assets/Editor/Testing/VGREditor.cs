using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(VirtualGyroRecorder))]
public class VGREditor : Editor
{

	private SerializedProperty _output;
	private SerializedProperty _record;

	public void OnEnable()
	{
		_output = serializedObject.FindProperty("_output");
		_record = serializedObject.FindProperty("_recording");
	}
	
	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		
		EditorGUILayout.TextField("Output", _output.stringValue);
		if (EditorApplication.isPlaying)
		{
			if (!_record.boolValue)
			{
				_record.boolValue = GUILayout.Button("Record");
			}
			else
			{
				if (GUILayout.Button("Stop Recording"))
				{
					_record.boolValue = false;
					((VirtualGyroRecorder) serializedObject.targetObject).ToFile();
				}
			}
		}

		serializedObject.ApplyModifiedPropertiesWithoutUndo();
	}
}
