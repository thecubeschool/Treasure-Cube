using UnityEngine;
using System.Collections;

public class TodClimater : MonoBehaviour {

	public ClimateIs thisIsClimate;
	public TodWeather todWeather;

	void OnTriggerEnter(Collider col) {
		if(col.gameObject.name == "[Player]") {
			todWeather.climateIs = thisIsClimate;
		}
	}

	void OnTriggerExit(Collider col) {
		if(col.gameObject.name == "[Player]") {
			todWeather.climateIs = ClimateIs.SouthernElin;
		}
	}
}
