using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScriptedGyro : MonoBehaviour
{

	[SerializeField] private TextAsset _instructionSet;
	private GyroInstructionSet _instructions;
	
	void Awake()
	{
		_instructions = JsonUtility.FromJson<GyroInstructionSet>(_instructionSet.text);
	}

	private void FixedUpdate()
	{
		Gyro.VirtualTilt = _instructions.Next;
	}
}

[Serializable]
public struct GyroInstructionSet
{
	[SerializeField] private List<SerializableQuaternion> _instructions;

	private IEnumerator<SerializableQuaternion> _iterator;
	
	public GyroInstructionSet(List<Quaternion> set)
	{
		_instructions = new List<SerializableQuaternion>(set.Count);
		foreach (SerializableQuaternion quat in set)
		{
			_instructions.Add(quat);
		}
		
		_iterator = null; // Let Next handle this so that we aren't initializing something when recording
	}


	public Quaternion Next
	{
		get
		{
			if (_iterator == null)
			{
				_iterator = _instructions.GetEnumerator();
			}

			return _iterator.MoveNext() ? (Quaternion)_iterator.Current : Quaternion.identity;
		}
	}
}

[System.Serializable]
public struct SerializableQuaternion
{
	/// <summary>
	/// x component
	/// </summary>
	public float x;
     
	/// <summary>
	/// y component
	/// </summary>
	public float y;
     
	/// <summary>
	/// z component
	/// </summary>
	public float z;
     
	/// <summary>
	/// w component
	/// </summary>
	public float w;
     
	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="rX"></param>
	/// <param name="rY"></param>
	/// <param name="rZ"></param>
	/// <param name="rW"></param>
	public SerializableQuaternion(float rX, float rY, float rZ, float rW)
	{
		x = rX;
		y = rY;
		z = rZ;
		w = rW;
	}
     
	/// <summary>
	/// Returns a string representation of the object
	/// </summary>
	/// <returns></returns>
	public override string ToString()
	{
		return String.Format("[{0}, {1}, {2}, {3}]", x, y, z, w);
	}
     
	/// <summary>
	/// Automatic conversion from SerializableQuaternion to Quaternion
	/// </summary>
	/// <param name="rValue"></param>
	/// <returns></returns>
	public static implicit operator Quaternion(SerializableQuaternion rValue)
	{
		return new Quaternion(rValue.x, rValue.y, rValue.z, rValue.w);
	}
     
	/// <summary>
	/// Automatic conversion from Quaternion to SerializableQuaternion
	/// </summary>
	/// <param name="rValue"></param>
	/// <returns></returns>
	public static implicit operator SerializableQuaternion(Quaternion rValue)
	{
		return new SerializableQuaternion(rValue.x, rValue.y, rValue.z, rValue.w);
	}
}
