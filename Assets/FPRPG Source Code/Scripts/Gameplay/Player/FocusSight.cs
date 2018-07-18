using UnityEngine;
using System.Collections;

public class FocusSight : MonoBehaviour {

	public Light pointLight;
	public PlayerSkillsAndAttributes skillsAttributes;

	void Start () {
		pointLight = GetComponent<Light> ();
		skillsAttributes = transform.parent.GetComponent<PlayerSkillsAndAttributes> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (skillsAttributes.focus < 25) {
			pointLight.range = 10;
		}
		else {
			pointLight.range = 20;
		}
	}
}
