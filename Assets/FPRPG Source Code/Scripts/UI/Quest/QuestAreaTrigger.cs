using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
public class QuestAreaTrigger : MonoBehaviour {

	public bool playerInTrigger;

	void OnTriggerEnter(Collider col) {
		if(col.transform.tag == "Player") {
			playerInTrigger = true;
		}
		else {
			playerInTrigger = false;
		}
	}
}
