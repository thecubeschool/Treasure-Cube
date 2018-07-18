using UnityEngine;
using System.Collections;

public class PlayerDeathCamera : MonoBehaviour {

	public bool grounded;

	void Update () {
		if(grounded == false) {
			GetComponent<Rigidbody>().AddTorque(transform.forward * 0.2f);
		}
		else {
			GetComponent<Rigidbody>().freezeRotation = true;
		}
	}

	void OnCollisionEnter(Collision col) {
		if(col.gameObject.tag != "Npc") {
			grounded = true;
		}
		else {
			grounded = false;
		}
	}
}
