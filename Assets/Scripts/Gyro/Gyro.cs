using System;
using System.Collections;
using UnityEngine;

// Alternative controller system directly modifies the physics gravity
public class Gyro : MonoBehaviour
{

    [SerializeField, Tooltip("The maximum angle in either direction on an unconstrained axis\n0 is unclamped")] 
    private float _maxAngle;
    [SerializeField, Tooltip("Represents the dead zone of the gyroscope, this affects all tilting objects")] private float _threshold;

    private Quaternion _gyroNeutral;
    private Vector3 _gravityNeutral;

    public static event Action OnGyroReset;
    
    public static Quaternion Tilt { get; private set; }
    
    private static readonly Quaternion Zero = new Quaternion(0, 0, 0, 0);

#if UNITY_EDITOR || DEVELOPMENT_BUILD
    private Quaternion _pureTilt;
    private Quaternion _attitude;
    private GUIStyle _style;
#endif
    
    private void Awake()
    {
        Input.gyro.enabled = true;
        
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        _style = new GUIStyle() {fontSize = 48};
#endif
    }
    
    private IEnumerator Start()
    {
        // Sometimes, the gyroscope input doesn't start up right away and attitude will return Zero, an invalid quaternion
        // In these cases, just wait until it returns a valid quat. This never takes more than a few frames.
        if (Input.gyro.attitude == Zero)
            yield return new WaitUntil(() => Input.gyro.attitude != Zero);
        _gyroNeutral = Input.gyro.attitude;
        _gravityNeutral = Physics.gravity;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        _attitude = Input.gyro.attitude;
        _pureTilt = Quaternion.Inverse(_gyroNeutral) * _attitude;
        Vector3 euler = _pureTilt.eulerAngles;
#else
        Vector3 euler = (Quaternion.Inverse(_gyroNeutral) * Input.gyro.attitude).eulerAngles; // In full builds, we don't need all those variables, just get the value directly
#endif
        Tilt = Quaternion.Euler(ThresholdAngle(euler.x), ThresholdAngle(euler.z), ThresholdAngle(euler.y)); // Not a typo, need to reinterpret gyroscope
        TiltGravity();
    }

#if UNITY_EDITOR || DEVELOPMENT_BUILD
    private void OnGUI()
    {
        GUILayout.Label("Gyroscope Enabled:" + Input.gyro.enabled, _style);
        GUILayout.Label("Neutral: " + _gyroNeutral, _style);
        GUILayout.Label("Attitude: " + _attitude, _style);
        GUILayout.Label("Gravity: " + Physics.gravity, _style);
        GUILayout.Label("Pure: " + _pureTilt.eulerAngles, _style, GUILayout.Width(200));
        GUILayout.Label("Rotation: " + Tilt.eulerAngles, _style, GUILayout.Width(200));
        if (GUILayout.Button("Reset Gyroscope", GUILayout.Height(200), GUILayout.Width(500))) ResetGyro();
    }
#endif

    private void TiltGravity()
    {
        Vector3 euler = Tilt.eulerAngles;
        // Could be one line
        euler.x = ClampAngle(euler.x, -_maxAngle, _maxAngle);
        euler.y = ClampAngle(euler.y, -_maxAngle, _maxAngle); // It just so happens that this won't affect gravity at all, could force zero
        euler.z = ClampAngle(euler.z, -_maxAngle, _maxAngle);
        Physics.gravity = Quaternion.Euler(euler) * _gravityNeutral;
    }

    public void ResetGyro()
    {
        Input.gyro.enabled = true; // Might be unnecessary
        _gyroNeutral = Input.gyro.attitude;
        Physics.gravity = _gravityNeutral;

        OnGyroReset?.Invoke();
    }

    // Only gyro should ever have control over threshold, the value it gives others is threshold dependent
    private float ThresholdAngle(float angle)
    {
        angle = SignedAngle(angle);
        angle = Mathf.Abs(angle) > _threshold ? angle - (Mathf.Sign(angle) * _threshold) : 0;
       
        return UnsignedAngle(angle);
    }

    // These are used by other classes for tilting
    public static float ClampAngle(float angle, float min, float max)
    {
        angle = SignedAngle(angle);
        if (min.Equals(max) && min < Mathf.Epsilon) return UnsignedAngle(angle); // No clamping if both values are zero
        angle = Mathf.Clamp(angle, min, max);
        
        return UnsignedAngle(angle);
    }

    public static float SignedAngle(float angle)
    {
        return angle > 180f ? angle - 360f : angle;
    }

    public static float UnsignedAngle(float angle)
    {
        return angle < 0f ? angle + 360f : angle;
    }
}
