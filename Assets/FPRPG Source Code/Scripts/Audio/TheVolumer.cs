using UnityEngine;
using System.Collections;
using System;

public enum VolumerType {
	Effects = 0,
	Music = 1,
}

public class TheVolumer : MonoBehaviour {

	public VolumerType volType;
	public AudioSource aS;

	public float volume;

	void Start() {
	}
}
