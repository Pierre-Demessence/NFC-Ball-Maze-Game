using UnityEngine;

public class Ball : MonoBehaviour {

	[SerializeField] float pFactor = 1f;
	[SerializeField] float vFactor = 1f;
	[SerializeField] AudioSource _aSource;
	[SerializeField] Rigidbody _rBody;
	
	// Update is called once per frame
	private void Update () {
		Debug.Log(_rBody.maxAngularVelocity);
		Debug.Log(_rBody.velocity.magnitude);
		float factor = _rBody.velocity.magnitude / _rBody.maxAngularVelocity;
		_aSource.pitch = factor / pFactor;
		_aSource.volume = factor / vFactor;
	}
}
