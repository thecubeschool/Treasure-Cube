using UnityEngine;
using System.Collections;

public class ProjectileStick : MonoBehaviour {

	public bool hit = false;
	
	void OnCollisionEnter(Collision other){
		if (!hit) {

			transform.position = other.contacts[0].point;
			transform.parent = other.transform;

			Destroy(gameObject.GetComponent<Rigidbody>());
			Destroy(gameObject.GetComponent<BoxCollider>());

			hit = true; 
		}
	}
}
