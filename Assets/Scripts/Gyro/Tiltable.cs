using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tiltable : MonoBehaviour
{

    [SerializeField, Tooltip("The maximum angle in either direction on an unconstrained axis\n0 is unclamped")] 
    private float _maxAngle;
    [SerializeField, Tooltip("How much the object tilts with the gyroscope\nAt 1, the object tilts with normally\nAt 0.5, the object tilts 1° for every 2° of gyro tilt")] 
    private float _tiltFactor;

    [SerializeField, Tooltip("Tiltable will orbit around a target")] 
    private bool _orbitTarget;
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _baseOffset;

    [SerializeField, Tooltip("Optional. Rotate the rigidbody instead of the transform")] 
    private Rigidbody _rigidbody;
    
    [SerializeField] private bool _freezeRotationX;
    [SerializeField] private bool _freezeRotationY;
    [SerializeField] private bool _freezeRotationZ;
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
        if (_orbitTarget)
        {
            transform.RotateAround(_target.position, Vector3.up, 0f);
        } 
        else if (_rigidbody != null)
        {
            _baseTilt = _rigidbody.rotation;
        }
        else
        {
            _baseTilt = transform.rotation;
        }
    }

    private void FixedUpdate()
    {
        Tilt();
    }

    private void Tilt()
    {
        Vector3 euler = Gyro.Tilt.eulerAngles;
        euler.x = _freezeRotationX ? 0 : Gyro.ClampAngle(Gyro.SignedAngle(euler.x) * _tiltFactor, -_maxAngle, _maxAngle);
        euler.y = _freezeRotationY ? 0 : Gyro.ClampAngle(Gyro.SignedAngle(euler.y) * _tiltFactor, -_maxAngle, _maxAngle);
        euler.z = _freezeRotationZ ? 0 : Gyro.ClampAngle(Gyro.SignedAngle(euler.z) * _tiltFactor, -_maxAngle, _maxAngle);

        if (_orbitTarget)
        {
            transform.position = _target.position + Quaternion.Euler(euler) * _baseOffset;
            transform.rotation = Quaternion.Euler(euler) * _baseTilt;
        }
        else if (_rigidbody != null)
        {
            _rigidbody.MoveRotation(Quaternion.Euler(euler) * _baseTilt);
        }
        else
        {
            transform.rotation = Quaternion.Euler(euler) * _baseTilt;
        }
    }
    
    private void ResetTilt()
    {
        if (_orbitTarget)
        {
            transform.position = new Vector3(_target.position.x + _baseOffset.x, _target.position.y + _baseOffset.y, _target.position.z + _baseOffset.z);
            transform.rotation = _baseTilt;
        }
        else if (_rigidbody != null)
        {
            _rigidbody.rotation = _baseTilt;
        }
        else
        {
            transform.rotation = _baseTilt;
        }
    }
}
