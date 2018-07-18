using UnityEngine;
using System.Collections;

#pragma warning disable 0618

public class RainSFX : MonoBehaviour {

	public ParticleSystem particleSys;
	public AudioSource audioSource;

	private TodWeather todWeather;
	
	void Start() {
		todWeather = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<TodWeather> ();
	}

	void Update() {
		if(particleSys.emissionRate > 49f && particleSys.emissionRate < 149.99f) {
			audioSource.clip = todWeather.rainSfx[0];
			if(audioSource.isPlaying == false) {
				audioSource.Play();
			}
		}
		else if(particleSys.emissionRate > 150f && particleSys.emissionRate < 219.99f) {
			audioSource.clip = todWeather.rainSfx[1];
			if(audioSource.isPlaying == false) {
				audioSource.Play();
			}
		}
		else if(particleSys.emissionRate > 220f) {
			audioSource.clip = todWeather.rainSfx[1];
			if(audioSource.isPlaying == false) {
				audioSource.Play();
			}
		}
		else if(particleSys.emissionRate < 1f) {
			audioSource.Stop();
			audioSource.clip = null;
		}
	}
}
