using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldCameraController : MonoBehaviour {

	public float minX = 30f;
	public float maxX = 1050f;
	[Space(10f)]
	public float minY = 300f;
	public float maxY = 450f;
	[Space(10f)]
	public float minZ = 270f;
	public float maxZ = 1030f;
	[Space(10f)]
	public float minRotAngle = -15f;
	public float maxRotAngle = 15f;
	[Space(10f)]
	private float horMovement;
	private float zoomMovement;
	private float verMovement;
	private float rotMovement;
	public float movingSpeed = 100000f;
	public float rotationSpeed = 100000f;

	private Quaternion tmpRotate;

	void Update () {

		if (Input.GetAxis ("Horizontal") > 0.1f) {
			horMovement = Input.GetAxis ("Horizontal") * movingSpeed;

		} 
		else if (Input.GetAxis ("Horizontal") < 0.1f) {
			horMovement = Input.GetAxis ("Horizontal") * -movingSpeed;
		}

		if (Input.GetAxis ("Vertical") > 0.1f) {
			verMovement = Input.GetAxis ("Vertical") * movingSpeed;
		} 
		else if (Input.GetAxis ("Vertical") < 0.1f) {
			verMovement = Input.GetAxis ("Vertical") * -movingSpeed;
		}

		if (Input.GetAxis ("Mouse Scrollwheel") > 0.1f) {
			zoomMovement = Input.GetAxis ("Mouse Scrollwheel") * movingSpeed;

		} 
		else if (Input.GetAxis ("Mouse Scrollwheel") < 0.1f) {
			zoomMovement = Input.GetAxis ("Mouse Scrollwheel") * -movingSpeed;
		}

		Vector3 tmpMove = new Vector3 (Mathf.Clamp(transform.position.x + horMovement, minX, maxX), 
								       Mathf.Clamp(transform.position.y + zoomMovement, minY, maxY), 
								       Mathf.Clamp(transform.position.z + verMovement, minZ, maxZ));

		/*
		if (Input.GetButton ("Fire2")) {
			if (Input.GetAxis ("Mouse X For WorldMap") > 0.1f) {
				if (rotMovement > 1f) {
					rotMovement -= Input.GetAxis ("Mouse X For WorldMap") * -rotationSpeed;
				} 
				else {
					rotMovement = 0f;
				}

			} 
			if (Input.GetAxis ("Mouse X For WorldMap") < 0.1f) {
				if (rotMovement < 30f) {
					rotMovement += Input.GetAxis ("Mouse X For WorldMap") * rotationSpeed;
				} 
				else {
					rotMovement = 29f;
				}
			}

			tmpRotate = Quaternion.Euler (0f, 
				Mathf.Clamp (transform.rotation.y + rotMovement, minRotAngle, maxRotAngle), 
				0f);
		} 

		transform.rotation = Quaternion.RotateTowards (transform.rotation, tmpRotate, rotationSpeed);*/

		transform.position = Vector3.MoveTowards (transform.position, tmpMove, movingSpeed);

	}
}
