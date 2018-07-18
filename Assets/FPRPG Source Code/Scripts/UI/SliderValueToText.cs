using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SliderValueToText : MonoBehaviour {

	public Slider slider;
	public Text text;
	public Text currentTime;

	private string sliderValue;

	public TodClock todClock;

	void Update() {

		sliderValue = slider.value.ToString();

		if(slider.value == 1) {
			text.text = "Wait 1 hour";
		}
		else if(slider.value > 1) {
			text.text = "Wait " + sliderValue + " hours";
		}

		currentTime.text = todClock.currentHour + "h" + ":" + todClock.currentMinute + "m";
	}
}
