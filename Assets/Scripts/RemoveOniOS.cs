using System.Diagnostics;
using UnityEngine;

public class RemoveOniOS : MonoBehaviour {

	public void Awake ()
	{
		Delete();
		Destroy(this);
	}
	
	[Conditional("UNITY_IOS")]
	private void Delete()
	{
		gameObject.transform.SetParent(null);
		Destroy(gameObject);
	}

}