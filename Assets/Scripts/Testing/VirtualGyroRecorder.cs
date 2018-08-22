using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

// Needs to poll on FixedUpdate, Try to find a way that lets this happen as an EditorScript
public class VirtualGyroRecorder : MonoBehaviour
{

	[SerializeField] private string _output = "instructions";
	[SerializeField] private bool _recording;
	
	private List<Quaternion> _instructions;
	
	private void Awake()
	{
		#if !UNITY_EDITOR
		Destroy(this); // If this script is attached to an object in the build, remove it
		#endif
		_instructions = new List<Quaternion>(((int)(1/Time.fixedDeltaTime)+1) * 60); // Enough for one-ish minute of instructions	
	}
	
#if UNITY_EDITOR
	private void FixedUpdate()
	{
		if (_recording)
		{
			_instructions.Add(Gyro.VirtualTilt);
		}
	}

	public void ToFile()
	{
		Debug.Log("Starting Write");
		GyroInstructionSet instructions = new GyroInstructionSet(_instructions);

		if (!AssetDatabase.IsValidFolder("Assets/GyroInstructions"))
			AssetDatabase.CreateFolder("Assets", "GyroInstructions");

		File.WriteAllText(Application.dataPath + "/GyroInstructions/" + _output + ".json", JsonUtility.ToJson(instructions));
		Debug.Log("Finished Write");
	}
#endif
}
