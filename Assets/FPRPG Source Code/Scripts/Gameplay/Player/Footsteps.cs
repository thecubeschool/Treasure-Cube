/// <summary>
/// Script written by Nenad Radojcic
/// All rights reserved.
/// 3/19/2014
/// </summary>
using UnityEngine;
using System.Collections;

[AddComponentMenu("RPG/Player/Footsteps")]
public class Footsteps : MonoBehaviour {

	public GameObject soundsGO;
	public AudioClip[] footSounds;
	public AudioClip[] horseSounds;
	public AudioClip[] waterSounds;
	public float audioStepLengthWalk = 0.45f;
	public float audioStepLengthRun = 0.25f;
	public float audioStepLengthHorse = 0.25f;
	public float audioVolumeWalk = 0.3f;
	public float audioVolumeRun = 0.4f;
	public float audioVolumeHorse = 1.0f;


	void Horseshoe() {
		soundsGO.GetComponent<AudioSource>().clip = horseSounds[Random.Range(0, horseSounds.Length)];
		soundsGO.GetComponent<AudioSource>().volume = audioVolumeHorse;
		soundsGO.GetComponent<AudioSource>().Play();
	}

	void Footstep() {
		soundsGO.GetComponent<AudioSource>().clip = footSounds[Random.Range(0, footSounds.Length)];
		soundsGO.GetComponent<AudioSource>().volume = audioVolumeWalk;
		soundsGO.GetComponent<AudioSource>().Play();
	}

	void Water() {
		soundsGO.GetComponent<AudioSource>().clip = waterSounds[Random.Range(0, waterSounds.Length)];
		soundsGO.GetComponent<AudioSource>().volume = audioVolumeRun;
		soundsGO.GetComponent<AudioSource>().Play();
	}
}