using UnityEngine;
using System.Collections;

public class ThunderSFX : MonoBehaviour {

	public AudioSource aS;
	public AudioClip[] thunderSfx;
	public ParticleSystem particleSys;

	private int _paticleCount;
	private int _numberOfParticles;

	private TodWeather todWeather;

	void Start() {
		todWeather = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<TodWeather> ();
	}

	void Update() {
		_paticleCount = particleSys.particleCount;

		if (_numberOfParticles < _paticleCount) {
			if(aS.isPlaying == false) {
				aS.PlayOneShot(todWeather.thunderSfx[Random.Range(0, thunderSfx.Length)]);
				_numberOfParticles = 1;
			}
		}
	}
}
