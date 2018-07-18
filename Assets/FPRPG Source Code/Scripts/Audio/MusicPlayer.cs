using UnityEngine;
using System.Collections;
using System;

public class MusicPlayer : MonoBehaviour {

	public GameManager gameManager;
	public PlayerState musicPlaying;
	public TheVolumer theVolumer;

	public AudioSource musicAs;
	[Space(10f)]
	public AudioClip[] exploreTracks;
	public AudioClip[] battleTracks;
	public AudioClip[] dungeonTracks;
	public AudioClip[] specialEventTracks;
	public AudioClip[] mainMenu;
	[Space(10f)]
	public AudioClip trackPlaying;
	public bool changeTrack = true;
	public bool fadeInOutAudio = false;
	public bool specialEvent = false;

	public float volume;

	void Start() {
		gameManager = GetComponent<GameManager>();

		musicAs.Play();
	}

	void LateUpdate() {
		if(Time.timeScale < 3f) {
			if(gameManager != null && gameManager.playerState != musicPlaying) {
				changeTrack = true;
				musicPlaying = gameManager.playerState;
			}

			if(changeTrack) {
				fadeInOutAudio = true;
				ChangeTrack();
			}

			if(fadeInOutAudio) {
				StartCoroutine(FadeInOutAudio());
			}

			if(specialEvent == true) {
				changeTrack = true;
			}
		}
	}

	void ChangeTrack() {
		if(musicPlaying == PlayerState.Exploring && specialEvent == false) {
			StartCoroutine(PlayTrack(exploreTracks));
		}
		else if(musicPlaying == PlayerState.Fighting && specialEvent == false) {
			StartCoroutine(PlayTrack(battleTracks));
		}
		else if(musicPlaying == PlayerState.Dungeoning && specialEvent == false) {
			StartCoroutine(PlayTrack(dungeonTracks));
		}
		else if(musicPlaying == PlayerState.SpecialEvent && specialEvent == true) {
			StartCoroutine(PlayTrack(specialEventTracks));
		}
		else if(musicPlaying == PlayerState.MainMenu && specialEvent == false) {
			StartCoroutine(PlayTrack(mainMenu));
		}
	}

	IEnumerator PlayTrack(AudioClip[] trackToPlay) {

		trackPlaying = trackToPlay[UnityEngine.Random.Range(0, trackToPlay.Length)];
		if(musicAs.clip != trackPlaying) {
			musicAs.clip = trackPlaying;
			musicAs.Play();
		}

		changeTrack = false;

		while (musicAs.isPlaying) {
			yield return new WaitForSeconds (0.1f);
		}
		
		changeTrack = true;
	}

	IEnumerator FadeInOutAudio() {
		if(musicAs.volume > 0f) {
			musicAs.volume -= Time.deltaTime / 6;
		}

		yield return new WaitForSeconds(1f);

		if(musicAs.volume < volume) {
			musicAs.volume += Time.deltaTime / 6;
		}

		yield return new WaitForSeconds(1f);

		fadeInOutAudio = false;
	}
}