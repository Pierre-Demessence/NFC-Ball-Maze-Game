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

#if UNITY_EDITOR || DEVELOPMENT_BUILD
    private Quaternion _pureTilt;
    private Quaternion _attitude;
    private GUIStyle _style;

    public static Quaternion VirtualTilt { get; set; }
#endif
    
    private IEnumerator Start()
    {
        if (SystemInfo.supportsGyroscope)
        {
            Input.gyro.enabled = true;
            // Sometimes, the gyroscope isn't ready and returns a bad quaternion.
            // Unity checks the dot product for equality, we need to check each component
            // Could probably do a straight check for zero given the nature of the bug
            // This should never be waiting for more than a few frames
            // There's a lot to say about these two lines, isn't there
            if (Math.Abs(Input.gyro.attitude.w) < Mathf.Epsilon && 
                Math.Abs(Input.gyro.attitude.x) < Mathf.Epsilon && 
                Math.Abs(Input.gyro.attitude.y) < Mathf.Epsilon &&
                Math.Abs(Input.gyro.attitude.z) < Mathf.Epsilon)
                yield return new WaitUntil(() => Math.Abs(Input.gyro.attitude.w) < Mathf.Epsilon && 
                                                 Math.Abs(Input.gyro.attitude.x) < Mathf.Epsilon && 
                                                 Math.Abs(Input.gyro.attitude.y) < Mathf.Epsilon &&
                                                 Math.Abs(Input.gyro.attitude.z) < Mathf.Epsilon);

            _gyroNeutral = Input.gyro.attitude;
        }
#if UNITY_EDITOR
        if (Math.Abs(VirtualTilt.w) < Mathf.Epsilon && 
            Math.Abs(VirtualTilt.x) < Mathf.Epsilon && 
            Math.Abs(VirtualTilt.y) < Mathf.Epsilon &&
            Math.Abs(VirtualTilt.z) < Mathf.Epsilon)
            yield return new WaitUntil(() => Math.Abs(VirtualTilt.w) < Mathf.Epsilon && 
                                             Math.Abs(VirtualTilt.x) < Mathf.Epsilon && 
                                             Math.Abs(VirtualTilt.y) < Mathf.Epsilon &&
                                             Math.Abs(VirtualTilt.z) < Mathf.Epsilon);
        
        _gyroNeutral = VirtualTilt;
#endif
        _gravityNeutral = Physics.gravity;
        
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        _style = new GUIStyle() {fontSize = 48};
#endif
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        _attitude = SystemInfo.supportsGyroscope ? Input.gyro.attitude : VirtualTilt;
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

#if UNITY_EDITOR
        _gyroNeutral = SystemInfo.supportsGyroscope ? Input.gyro.attitude : VirtualTilt;
#endif
        
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
