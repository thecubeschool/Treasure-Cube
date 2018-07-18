using UnityEngine;
using System.Collections;

#pragma warning disable 0618

public class LighthouseManager : MonoBehaviour {

	public float lightTurnerSpeed = 1f;
	public float rotatingSpeed = 1f;

	[HideInInspector]
	public TodWeather todWeather;
	[HideInInspector]
	public Light myLight;
	[HideInInspector]
	public ParticleSystem particleSys;
	
	void Start() {
		if (GameObject.Find ("[GameManager]")) {
			todWeather = GameObject.Find ("[GameManager]").GetComponent<TodWeather> ();
		}
		myLight = GetComponent<Light> ();
		particleSys = GetComponentInChildren<ParticleSystem> ();
	}

	void Update() {
		if (todWeather != null) {
			if (todWeather.nowItIs == PartOfDay.Night) {
				if (myLight.range < 80f && particleSys.emissionRate > 10f) {
					myLight.range += Time.deltaTime * lightTurnerSpeed;
				}
				if (particleSys.emissionRate < 32f) {
					particleSys.emissionRate += Time.deltaTime * 2.5f;
				}

				if (myLight.range > 60f) {
					transform.Rotate (Vector3.up, Time.deltaTime * rotatingSpeed);
				}
			} else {
				if (myLight.range > 0f && particleSys.emissionRate < 20f) {
					myLight.range -= Time.deltaTime * lightTurnerSpeed;
				}
				if (particleSys.emissionRate > 0f) {
					particleSys.emissionRate -= Time.deltaTime * 2.5f;
				}
			}
		} 
		else {
			if (myLight.range < 80f && particleSys.emissionRate > 10f) {
				myLight.range += Time.deltaTime * lightTurnerSpeed;
			}
			if (particleSys.emissionRate < 32f) {
				particleSys.emissionRate += Time.deltaTime * 2.5f;
			}
			
			if (myLight.range > 60f) {
				transform.Rotate (Vector3.up, Time.deltaTime * rotatingSpeed);
			}
		}
	}

	public void IntroCinematicRotate() {
		myLight.range = 80f;
		particleSys.emissionRate = 32f;
		transform.Rotate (Vector3.up, Time.deltaTime * rotatingSpeed);
	}
}
