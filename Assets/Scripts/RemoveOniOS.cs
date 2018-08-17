using System.Diagnostics;
using UnityEngine;

public class RemoveOniOS : MonoBehaviour {

	public void Awake ()
	{
		Delete();
	}
	
	[Conditional("UNITY_IOS")]
	private void Delete()
	{
		gameObject.transform.SetParent(null);
		Destroy(gameObject);
	}

}