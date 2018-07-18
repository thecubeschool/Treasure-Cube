﻿using UnityEngine;
using System.Collections;

public class CameraShaker : MonoBehaviour {
	
	public Transform camTransform;

	public float shake = 0f;

	public float shakeAmount = 0.7f;
	public float decreaseFactor = 1.0f;
	
	Vector3 originalPos;
	
	void Awake() {
		if (camTransform == null) {
			camTransform = GetComponent(typeof(Transform)) as Transform;
		}
	}
	
	void OnEnable() {
		originalPos = camTransform.localPosition;
	}
	
	void Update() {
		if (shake > 0) {
			camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
			
			shake -= Time.deltaTime * decreaseFactor;
		}
		else {
			shake = 0f;
			camTransform.localPosition = originalPos;
		}
	}
}
