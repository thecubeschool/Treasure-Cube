using UnityEngine;
using System.Collections;

public class RandomSFXPlayer : MonoBehaviour {

	[Tooltip("Will this SFX play while it is day or if false at night?")]
	public bool playAtDay = true;
	private float randomTimer;
	private float cooldown;

	private AudioSource aS;
	private TodWeather todWeather;

	void Start() {
		aS = GetComponent<AudioSource> ();
		todWeather = GameObject.Find ("[GameManager]").GetComponent<TodWeather> ();

		aS.minDistance = 20f;
		aS.maxDistance = 30f;
	}

	void Update() {
		if (randomTimer > 0) {
			cooldown = Random.Range(10f, 120f);
			randomTimer -= Time.deltaTime;

			if(playAtDay == true && todWeather.nowItIs == PartOfDay.Day) {
				aS.Play();
			}
			else if(playAtDay == false && todWeather.nowItIs == PartOfDay.Night) {
				aS.Play();
			}
			else {
				aS.Stop();
			}
		} 
		else {
			cooldown -= Time.deltaTime;
			aS.Stop();

			if(cooldown <= 0f) {
				randomTimer = Random.Range(10f, 120f);
			}
		}
	}
}
