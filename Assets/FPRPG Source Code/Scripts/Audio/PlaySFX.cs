using UnityEngine;
using System.Collections;

public class PlaySFX : MonoBehaviour {

	public AudioClip coinsSfx;

	public AudioSource audioSource;

	public void PlayBuySFX() {
		audioSource.PlayOneShot(coinsSfx);
	}
}
