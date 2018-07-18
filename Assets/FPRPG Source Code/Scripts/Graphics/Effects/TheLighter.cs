using UnityEngine;
using System.Collections;

public class TheLighter : MonoBehaviour {

	[Tooltip("Should this window light0 up at night?")]
	public bool windowLightupInside = false;
	public bool windowLightupOutside = true;
	public bool swapObjects = false; //Do we swap two objects between on, off
	public GameObject objectOn;
	public GameObject objectOff;

	private TodWeather todWeather;

	private Light light0;

	void Awake() {

		todWeather = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TodWeather>();
		light0 = gameObject.GetComponentInChildren<Light> ();

		if (!windowLightupInside && !windowLightupOutside && !swapObjects) {
			light0.enabled = false;
		}
	}



	void Update() {
        if(windowLightupInside && !windowLightupOutside) {
            if (todWeather.nowItIs == PartOfDay.Night) { //What will we do at night if this is inside window
                if (light0.enabled == true) {
                    light0.enabled = false;
                }

                if (swapObjects) {
                    objectOn.SetActive(false);
                    objectOff.SetActive(true);
                }
                else {
                    gameObject.GetComponent<MeshRenderer>().enabled = true;
                }
            }
            else {
                if (light0.enabled == false) {
                    light0.enabled = true;
                }
                if (swapObjects) {
                    objectOn.SetActive(true);
                    objectOff.SetActive(false);
                }
                else {
                    gameObject.GetComponent<MeshRenderer>().enabled = false;
                }
            }
        }
        else if (!windowLightupInside && windowLightupOutside) {
            if (todWeather.nowItIs == PartOfDay.Day) { //What will we do at day if this is outside window
                if (light0.enabled == true) {
                    light0.enabled = false;
                }

                if (swapObjects) {
                    objectOn.SetActive(false);
                    objectOff.SetActive(true);
                }
                else {
                    gameObject.GetComponent<MeshRenderer>().enabled = true;
                }
            }
            else {
                if (light0.enabled == false) {
                    light0.enabled = true;
                }
                if (swapObjects) {
                    objectOn.SetActive(true);
                    objectOff.SetActive(false);
                }
                else {
                    gameObject.GetComponent<MeshRenderer>().enabled = false;
                }
            }
        }
        else {
            Debug.LogWarning("You should not have a window that is set as both inside and outside window or as neither. Window " + name + " is just that!");
        }
	}
}
