/// <summary>
/// Bilboard Look At.
/// Written by Nenad Radojcic, Grownup Games
/// 5/29/2015
/// </summary>
using UnityEngine;
using System.Collections;

public class BilboardLookAt : MonoBehaviour {

	private Transform playerTransform;
	private Transform editorCameraTransform;

	private Camera playerCamera;
	private Camera editorCamera;

	private float offset = 180f;

	void  Awake () {
		if (GameObject.Find ("[Player]")) {
			playerTransform = GameObject.Find ("[Player]").transform;
		}

		if(GameObject.Find("_EditorCamera")) {
			editorCameraTransform = GameObject.Find("_EditorCamera").transform;
		}
		else {
			editorCameraTransform = null;
		}

		offset = 0;
	}

	void  Start () {
		if (!playerCamera) {
			playerCamera = playerTransform.GetComponentInChildren<Camera>();
		}
		if (!editorCamera) {
			if(editorCameraTransform != null) {
				editorCamera = editorCameraTransform.GetComponent<Camera>();
			}
		}

		if(gameObject.GetComponentInParent<NPC>()) {
			offset = 180f;
		}
		else if(gameObject.GetComponentInParent<HorseManager>()) {
			offset = 180f;
		}
		else {
			offset = 0;
		}
	}
	
	void Update () {
		if(playerTransform != null) {
			float distance = Vector3.Distance(transform.position, playerTransform.position);

			if(distance < 60f) {
				if(editorCameraTransform != null) {
					if(playerTransform.gameObject.activeSelf == true && editorCameraTransform.gameObject.GetComponent<Camera>().enabled == false) {

						Quaternion newRot = Quaternion.LookRotation(playerCamera.transform.position - transform.position);
						Quaternion cRot = transform.localRotation;
						newRot.eulerAngles= new Vector3(cRot.eulerAngles.x, newRot.eulerAngles.y + offset, cRot.eulerAngles.z);
						transform.localRotation = newRot;
					}
					else if(editorCameraTransform.gameObject.GetComponent<Camera>().enabled == true && playerTransform.gameObject.activeSelf == false) {

						Quaternion newRot = Quaternion.LookRotation(editorCamera.transform.position - transform.position);
						Quaternion cRot = transform.localRotation;
						newRot.eulerAngles= new Vector3(cRot.eulerAngles.x, newRot.eulerAngles.y + offset, cRot.eulerAngles.z);
						transform.localRotation = newRot;
					}
				}
				else {
					Quaternion newRot = Quaternion.LookRotation(playerCamera.transform.position - transform.position);
					Quaternion cRot = transform.localRotation;
					newRot.eulerAngles= new Vector3(cRot.eulerAngles.x, newRot.eulerAngles.y + offset, cRot.eulerAngles.z);
					transform.localRotation = newRot;
				}
			}
		}
		else {
			return;
		}
	}
}
