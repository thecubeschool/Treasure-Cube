#pragma warning disable 618 

using UnityEngine;
using System.Collections;

public class FireplaceFireController : MonoBehaviour {

	public GameObject fire;
	public Light fireLight;
	public ParticleSystem fireParticle;
	
	private bool npcsInRadius;
	private bool playerInRadius;

	private TodWeather todWeather;

	public PlayerStats playerStats;

	void Start() {
		
		todWeather = GameObject.Find("[GameManager]").GetComponent<TodWeather>();
		playerStats = GameObject.Find ("[Player]").GetComponent<PlayerStats> ();

		if(fire == null) {
			fire = GetComponentInChildren<Light>().gameObject;
		}
	}

	void Update() {
		if(npcsInRadius == true) {
			if(todWeather.currentTimeOfDay > 0.8f || todWeather.currentTimeOfDay < 0.2f) {
				LightTheFire();
			}
			else {
				if(playerInRadius == false) {
					fireParticle.emissionRate -= Time.deltaTime * 2f;
				}
			} 
		}
		else {
			if(fireParticle.emissionRate > 0f) {
				if(playerInRadius == false && npcsInRadius == false) {
					fireParticle.emissionRate -= Time.deltaTime * 2f;
				}
			}
		}

		if (fireParticle.emissionRate > 5f) {
			if(playerInRadius == true) {
				playerStats.atFire = true;
			}
			else {
				playerStats.atFire = true;
			}
		}
		else {
			playerStats.atFire = true;
		}

		fireLight.intensity = fireParticle.emissionRate / 10f;
	}
	
	void OnTriggerEnter (Collider npc) {
		if(npc.gameObject.CompareTag("Npc")) {
			npcsInRadius = true;			
		}
		if(npc.gameObject.CompareTag("Player")) {
			playerInRadius = true;
		}
	}

	void OnTriggerExit (Collider npc) {
		if(npc.gameObject.CompareTag("Npc")) {
			npcsInRadius = false;			
		}
		if(npc.gameObject.CompareTag("Player")) {
			playerInRadius = false;
			playerStats.atFire = false;
		}
	}

	public void LightTheFire() {
		fireParticle.emissionRate = 10f;
		if(fireLight.intensity < 1f) {
			fireLight.intensity += Time.deltaTime;
		}
	}
}
