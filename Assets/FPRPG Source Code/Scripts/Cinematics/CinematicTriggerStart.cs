using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class CinematicTriggerStart : MonoBehaviour {

    private void Start() {
        if(GetComponent<BoxCollider>().isTrigger == false) {
            GetComponent<BoxCollider>().isTrigger = true;
        }
    } 

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player") {
            GetComponent<CinematicManager>().CinematicPlay();
        }
    }
}
