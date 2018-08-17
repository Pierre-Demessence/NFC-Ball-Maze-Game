using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The only reason why this is separate from Tiltable is because Camera movement needs to be in LateUpdate()
public class TiltableCamera : MonoBehaviour
{

    [SerializeField, Tooltip("The maximum angle in either direction on an unconstrained axis\n0 is unclamped")] 
    private float _maxAngle;
    [SerializeField, Tooltip("How much the object tilts with the gyroscope\nAt 1, the object tilts with normally\nAt 0.5, the object tilts 1° for every 2° of gyro tilt")] 
    private float _tiltFactor;
    
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _baseOffset;

    private Quaternion _baseTilt;

    private void OnEnable()
    {
        Gyro.OnGyroReset += ResetTilt;
    }
    
    private void OnDisable()
    {
        Gyro.OnGyroReset -= ResetTilt;
    }
    
    private void Start()
    {
        transform.position = new Vector3(_target.position.x + _baseOffset.x, _target.position.y + _baseOffset.y, _target.position.z + _baseOffset.z);
        _baseTilt = transform.rotation;

    }
    
    private void LateUpdate()
    {
        //transform.position = new Vector3(_target.position.x, _target.position.y + _offset, _target.position.z);
        Tilt();
    }

    private void Tilt()
    {
        Vector3 euler = Gyro.Tilt.eulerAngles;
        euler.x = Gyro.ClampAngle(Gyro.SignedAngle(euler.x) * _tiltFactor, -_maxAngle, _maxAngle);
        euler.y = 0;
        euler.z = Gyro.ClampAngle(Gyro.SignedAngle(euler.z) * _tiltFactor, -_maxAngle, _maxAngle);

        transform.position = _target.position + Quaternion.Euler(euler) * _baseOffset;
        transform.rotation = Quaternion.Euler(euler) * _baseTilt;
    }
    
    private void ResetTilt()
    {
        transform.position = new Vector3(_target.position.x + _baseOffset.x, _target.position.y + _baseOffset.y, _target.position.z + _baseOffset.z);
        transform.rotation = _baseTilt;
    }
}
