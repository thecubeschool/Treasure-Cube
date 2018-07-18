using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PointLightLODe : MonoBehaviour {

	public float distance;
	public List<Light> pointLight;
	private Light[] tmp;

	private GameObject playerGo;

	void Awake () {
	
		playerGo = GameObject.Find ("[Player]");

		tmp = GameObject.FindObjectsOfType<Light> ();
		
		foreach (Light l in tmp) {
			if (l.type == LightType.Point) {
				pointLight.Add (l);
			}
		}

		InvokeRepeating ("UpdatePointLightDistance", 0f, 1.75f);
	}

	void UpdatePointLightDistance () {

		//CheckLights ();

		foreach(Light l in pointLight) {
			if(l.GetComponent<LighthouseManager>() == null) {
				if(Vector3.Distance(playerGo.transform.position, l.gameObject.transform.position) <= distance) {
					if(l.gameObject.activeSelf == false) {
						l.enabled = true;
					}
				}
				else {
					l.enabled = false;
				}
			}
		}
	}

	void CheckLights() {

		pointLight.Clear ();

		foreach (Light l in tmp) {
			if (l.type == LightType.Point) {
				pointLight.Add (l);
			}
		}

		print ("da");
	}
}
