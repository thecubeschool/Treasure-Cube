using UnityEngine;
using System.Collections;

public class PlaySFXOnAnimation : MonoBehaviour {

	public AudioSource sfxAS;
	public AudioSource sfxASs;

	public AudioClip[] wepSwooshSfx;
	public AudioClip[] wepHitSfx;
	public AudioClip[] wepHitObjectSfx;
	public AudioClip[] wepBowStringSfx;
	public AudioClip[] wepEquipSfx;

	public void WeaponSwooshFXPlay() {
		PlaySound(wepSwooshSfx);
	}

	public void WeaponHitFXPlay() {
		PlaySound(wepHitSfx);
	}

	public void WeaponHitObjectFXPlay() {
		PlaySound(wepHitObjectSfx);
	}

	public void BowStringReleaseFXPlay() {
		PlaySound(wepBowStringSfx);
	}

	public void WeaponEquipFXPlay() {
		if (sfxAS.isPlaying) {
			PlaySound2(wepEquipSfx);
		} 
		else {
			PlaySound(wepEquipSfx);
		}
	}

	private void PlaySound(AudioClip[] name) {
		if(sfxAS != null) {
			sfxAS.clip = name[Random.Range(0, name.Length)];
			sfxAS.Play();
		}
	}

	private void PlaySound2(AudioClip[] name) {
		if(sfxASs != null) {
			sfxASs.clip = name[Random.Range(0, name.Length)];
			sfxASs.Play();
		}
	}
}
