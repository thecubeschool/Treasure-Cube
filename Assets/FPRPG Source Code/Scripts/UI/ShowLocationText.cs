using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShowLocationText : MonoBehaviour {

	public float fadeSpeed = 1.5f;

	public Text locationTxt;
	public Text discoveredTxt;

	public bool fadeAlpha = false;
	public bool fadeDiscovered = false;

	void Start() {
		locationTxt.text = string.Empty;
		locationTxt.color = Color.clear;
		discoveredTxt.color = Color.clear;
	}

	void Update() {
		if(fadeAlpha == true) {
			if(fadeDiscovered == true) {
				StartCoroutine(ShowDiscoveredText());
			}
			StartCoroutine(UpdateLocationName());
		}
	}

	IEnumerator UpdateLocationName() {
		locationTxt.color = Color.Lerp(locationTxt.color, Color.white, fadeSpeed * Time.deltaTime);

		yield return new WaitForSeconds(fadeSpeed*4f);

		locationTxt.color = Color.Lerp(locationTxt.color, Color.clear, fadeSpeed * Time.deltaTime);

		yield return null;

		fadeAlpha = false;
	}

	IEnumerator ShowDiscoveredText() {
		discoveredTxt.color = Color.Lerp(locationTxt.color, Color.white, fadeSpeed * Time.deltaTime);
		
		yield return new WaitForSeconds(fadeSpeed*4f);
		
		discoveredTxt.color = Color.Lerp(locationTxt.color, Color.clear, fadeSpeed * Time.deltaTime);
		
		yield return null;
		
		fadeDiscovered = false;
	}
}
