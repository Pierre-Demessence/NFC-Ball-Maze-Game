using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: If needed, add a way to store the original object's layer and set it back to the object on exit
// 		 If all the objects end up needing to exit onto the same layer, this task is unnecessary
public class CollisionLayerModifier : MonoBehaviour
{

	[SerializeField, Tooltip("If non-empty, will only modify objects with a tag in this list")] 
	private string[] _validTags;
	[SerializeField, Tooltip("The layer that the colliding object will move to when entering the trigger")] 
	private int _enterLayer;
	[SerializeField, Tooltip("The layer that the colliding object will move to when exiting the trigger")] 
	private int _exitLayer;
	
	private void OnTriggerEnter(Collider other)
	{
		if (_validTags.Length > 0)
		{
			foreach (var tag in _validTags)
			{
				if (other.gameObject.CompareTag(tag))
				{
					other.gameObject.layer = _enterLayer;
					return; // No reason to continue the search
				}
			}
		}
		else
		{
			other.gameObject.layer = _enterLayer;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (_validTags.Length > 0)
		{
			foreach (var tag in _validTags)
			{
				if (other.gameObject.CompareTag(tag))
				{
					other.gameObject.layer = _exitLayer;
					return; // No reason to continue the search
				}
			}
		}
		else
		{
			other.gameObject.layer = _exitLayer;
		}
	}
}
