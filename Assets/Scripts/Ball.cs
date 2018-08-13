using UnityEngine;

public class Ball : MonoBehaviour {

	[SerializeField] private Rigidbody _rBody;
	
	[SerializeField] private AudioSource _sfxRolling;
	[SerializeField] private float _sfxRollingPitchFactor = 1f;
	[SerializeField] private float  _sfxRollingVolumeFactor = 1f;
	
	[SerializeField] private AudioSource _sfxColliding;
	[SerializeField] private float _sfxCollidingPitchMin = 0.5f;
	[SerializeField] private float _sfxCollidingPitchMax = 1;
	[SerializeField] private float _sfxCollidingVolumeMin = 0;
	[SerializeField] private float _sfxCollidingVolumeMax = 0.5f;
	

	// Update is called once per frame
	private void Update () {
		//Debug.Log(_rBody.maxAngularVelocity);
		//Debug.Log(_rBody.velocity.magnitude);
		float factor = _rBody.velocity.magnitude / _rBody.maxAngularVelocity;
		_sfxRolling.pitch = factor / _sfxRollingPitchFactor;
		_sfxRolling.volume = factor / _sfxRollingVolumeFactor * MainMenu.SoundVolume;
	}

	private void OnCollisionEnter(Collision other)
	{
		// Checking the layer containing the walls
		if (other.collider.gameObject.layer == 10)
		{
			float factor = other.impulse.magnitude / 5;
			_sfxColliding.pitch = Mathf.Lerp(_sfxCollidingPitchMin, _sfxCollidingPitchMax, factor);
			_sfxColliding.volume = Mathf.Lerp(_sfxCollidingVolumeMin, _sfxCollidingVolumeMax, factor) * MainMenu.SoundVolume;
			_sfxColliding.Play();
		}
	}
}
