using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour {

    [SerializeField] private float _bumpForce;

    private Animator _anim;
    private AudioSource _sound;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _sound = GetComponent<AudioSource>();

    }
    private void OnCollisionEnter(Collision other)
    {
        _anim.SetTrigger("Hit");
        _sound.Play();
        other.gameObject.GetComponent<Rigidbody>().AddForce(-_bumpForce * other.contacts[0].normal, ForceMode.Impulse);
    }
}
