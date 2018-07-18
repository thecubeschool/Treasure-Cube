using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SceneFadeInOut : MonoBehaviour {

	public float fadeSpeed = 3f;
	private Image fadeImage;
	private bool fadeAtStart = true;
	private bool fadeAtWaiter = false;

	private bool fadeOut;

	void Awake () {
		GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width * 2f, Screen.height * 2f);
		fadeImage = GetComponent<Image>();
		fadeImage.color = Color.black;
	}

	void Update () {
		if(fadeAtStart == true) {
			FadeInAtStart();
		}

		if(Time.timeScale > 60f) {
			if(fadeAtWaiter == false) {
				FadeOutAtTimeReset();
			}
		}
		else {
			if(fadeAtWaiter == true) {
				FadeInAtTimeForward();
			}
		}
	}

	void FadeToClear () {
		fadeImage.color = Color.Lerp(fadeImage.color, Color.clear, fadeSpeed * Time.deltaTime);
	}
		
	void FadeToBlack () {
		fadeImage.color = Color.Lerp(fadeImage.color, Color.black, fadeSpeed * Time.deltaTime);
	}

	void FadeInAtStart() {

		FadeToClear();

		if(fadeImage.color.a <= 0.05f) {
			fadeImage.color = Color.clear;
			fadeImage.enabled = false;

			fadeAtStart = false;
		}
	}

	void FadeOutAtTimeReset() {

		fadeImage.enabled = true;

		FadeToBlack();

		if(fadeImage.color.a >= 0.95f) {
			fadeAtWaiter = true;
		}
	}

	void FadeInAtTimeForward() {
		
		FadeToClear();
		
		if(fadeImage.color.a <= 0.05f) {
			fadeImage.color = Color.clear;
			fadeImage.enabled = false;

			fadeAtWaiter = false;
		}
	}
}