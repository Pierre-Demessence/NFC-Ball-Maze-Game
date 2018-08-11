using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Controller : MonoBehaviour
{

    [SerializeField] private float _maxAngle;

    private Rigidbody _rb;

    private Vector3 _rotation;

#if UNITY_ANDROID || UNITY_IOS
    private Vector3 _lowPassValue;
#endif
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.isKinematic = true; // Idiot-proofing
#if UNITY_ANDROID || UNITY_IOS
    _lowPassValue = Input.acceleration;
#endif
    }

    private void Update()
    {
        UpdateInput();
    }

    private void FixedUpdate()
    {
        Tilt(_rotation);
    }

    private void UpdateInput()
    {
#if UNITY_ANDROID || UNITY_IOS
        Vector3 filteredAcceleration = LowPassFilterAccelerometer(Input.acceleration);
        // Debug.Log($"Acceleration ({Input.acceleration}) ; Filtered Acceleration ({filteredAcceleration})");
        _rotation = Vector3.zero;
        _rotation.x = filteredAcceleration.y * _maxAngle;
        _rotation.z = -filteredAcceleration.x * _maxAngle;
#else
        _rotation.x = Input.GetAxis("Vertical") * _maxAngle;
        _rotation.z = -Input.GetAxis("Horizontal") * _maxAngle; // Flip for intuitive rotation
#endif
        _rotation.y = 0; // Ensure no y-axis rotations
    }
    
    private void Tilt(Vector3 rotation)
    {
#if UNITY_ANDROID || UNITY_IOS
   
        // Can directly put gyroscope rotation into rigidbody
        _rb.rotation = Quaternion.Euler(Mathf.Clamp(rotation.x, -_maxAngle, _maxAngle), 0, Mathf.Clamp(rotation.z, -_maxAngle, _maxAngle));
#else
        const float maxDegreesDelta = 5f;
        // Must limit travel speed
        _rb.rotation =
            Quaternion.RotateTowards(_rb.rotation, Quaternion.Euler(rotation.x, 0, rotation.z), maxDegreesDelta);
#endif
    }
    
#if UNITY_ANDROID || UNITY_IOS
    private Vector3 LowPassFilterAccelerometer(Vector3 acceleration) {
        const float accelerometerUpdateInterval = 1.0f / 60.0f;
        const float lowPassKernelWidthInSeconds = 0.1f;

        const float lowPassFilterFactor = accelerometerUpdateInterval / lowPassKernelWidthInSeconds;

        _lowPassValue = Vector3.Lerp(_lowPassValue, acceleration, lowPassFilterFactor);
        return _lowPassValue;
    }
#endif

}
