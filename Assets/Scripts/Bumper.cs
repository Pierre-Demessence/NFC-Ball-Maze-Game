using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour {

    public float BumperForce;

	void Start () {
		
	}
	
	void Update () {
	    	
	}

    private void OnCollisionEnter(Collision collision)
    {
        Vector2 force = transform.position - collision.gameObject.transform.position;
        force *= -BumperForce;
        collision.gameObject.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
        Debug.Log("Bump, " + collision.gameObject.name + ", " + force);
    }
}
