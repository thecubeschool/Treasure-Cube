using UnityEngine;
using System.Collections;

public class ToonWater : MonoBehaviour {

	public float waveSpeed = 0.01f;

	private Renderer rend;

	void Start() {
		rend = GetComponent<Renderer>();
	}

	void Update() {
		float offset = Time.time * waveSpeed; 
		rend.material.mainTextureOffset = new Vector2 (offset%1,0);
		rend.material.renderQueue = -200;
	}
}
