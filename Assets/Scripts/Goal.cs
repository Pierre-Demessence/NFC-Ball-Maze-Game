using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {
	
	// Action is easier to use, but this is technically more true to the pattern
	// A static version, although less safe, might be easier to work with (how else can it be subscribed to in code without a known reference?)
	public event EventHandler OnGoal;
	
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			Debug.Log("You're winner!");

			OnGoal?.Invoke(this, EventArgs.Empty);
		}
	}
}
