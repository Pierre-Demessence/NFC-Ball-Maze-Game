using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Alternative controller system directly modifies the physics gravity
public class GravityController : MonoBehaviour
{

    [SerializeField] private float _maxAngle;
    [SerializeField] private float _threshold;

    [SerializeField, Tooltip("Reference to the directional light in the scene for rotation")]
    private GameObject _directionalLight;

    private Quaternion _baseLightRotation;

    private Quaternion _gyroNeutral;
    private Vector3 _gravityNeutral;

    public event Action OnGyroReset;

    private void Start()
    {
        Input.gyro.enabled = true;
        _gyroNeutral = Input.gyro.attitude;
        _gravityNeutral = Physics.gravity;

        Screen.orientation = ScreenOrientation.LandscapeRight;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GravityTilt();
    }

#if UNITY_EDITOR
    private void OnGUI()
    {

        GUILayout.Label("Gravity: " + Physics.gravity, GUILayout.Height(100));
        if (GUILayout.Button("Reset", GUILayout.Width(200), GUILayout.Height(100))) ResetGyro();

    }
#endif

    private void GravityTilt()
    {
        Quaternion rotation = OffsetRotation(Input.gyro.attitude);
        Physics.gravity = rotation * _gravityNeutral;

        // TODO: Implement a better way to rotate the light so that we can have a base rotation
        _directionalLight.transform.LookAt(Physics.gravity);
    }

    public void ResetGyro()
    {
        _gyroNeutral = Input.gyro.attitude;
        Physics.gravity = _gravityNeutral;
        //_directionalLight.transform.rotation = _baseLightRotation;

        OnGyroReset?.Invoke();
    }

    private Quaternion OffsetRotation(Quaternion q)
    {
        Vector3 euler = q.eulerAngles - _gyroNeutral.eulerAngles;

        // Unity Editor has a problem with screen rotation. This should force the editor to interpret the gyroscope in the same way that LandscapeRight would
#if UNITY_EDITOR
        return Quaternion.Euler(ThresholdClamp(euler.y), ThresholdClamp(euler.z), -ThresholdClamp(euler.x));
#else
		return Quaternion.Euler(ThresholdClamp(euler.x), ThresholdClamp(euler.y), -ThresholdClamp(euler.z));
#endif
        //return new Quaternion(q.x, q.y, -q.z, -q.w);
    }

    // Clamp the angle if exceeds threshold, otherwise 0
    private float ThresholdClamp(float angle)
    {
        if (angle >= 180f) angle -= 360f;
        return Mathf.Abs(angle) >= _threshold ? angle > _maxAngle ? _maxAngle : angle < -_maxAngle ? -_maxAngle : angle : angle;
        //return Mathf.Abs(angle) > _threshold ? Mathf.Clamp(angle + _maxAngle, 0, _maxAngle * 2) - _maxAngle : 0;
    }

}
