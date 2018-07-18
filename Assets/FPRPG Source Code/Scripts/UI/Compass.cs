﻿using UnityEngine;
using System.Collections;

public class Compass : MonoBehaviour {

	// Feel free to set these all to private once you are done tweaking:
	private Transform myTransform;
	public int facingDir;
	public int degreeOffset;
	
	void Start () {
		myTransform = transform;
	}
	
	// Minor adjustments from 0,90,180,270 due to Font width
	// Adjust letter offset to match your Font size/width
	
	void OnGUI() {
		
		if(degreeOffset > -85 && degreeOffset < 90) {
			GUI.Label( new Rect((Screen.width/2)-degreeOffset*2,
			                    (Screen.height)-50,
			                    180,
			                    25),
			          "N");
		}
		
		if(degreeOffset > 5 && degreeOffset < 180) {
			GUI.Label( new Rect((Screen.width/2)-degreeOffset*2+180,
			                    (Screen.height)-50,
			                    180,
			                    25),
			          "E");
		}    
		
		if((facingDir > 95 && degreeOffset> 95) || (facingDir < 276 && degreeOffset < -90)) {
			GUI.Label( new Rect((Screen.width/2)-facingDir*2+360,
			                    (Screen.height)-50,
			                    180,
			                    25),
			          "S");    
		}
		
		if((facingDir > 186 && degreeOffset < -5)) {
			GUI.Label( new Rect((Screen.width/2)-facingDir*2+540,
			                    (Screen.height)-50,
			                    180,
			                    25),
			          "W");
		}
		
		GUI.Box( new Rect((Screen.width/2)-180,
		                  (Screen.height)-65,
		                  360,
		                  35),
		        "Heading");
	}
	
	void Update () {
		
		facingDir = (int)Mathf.Abs(myTransform.eulerAngles.y);
		if (facingDir > 360) facingDir = facingDir % 360;
		
		degreeOffset = facingDir;
		
		if(degreeOffset > 180) degreeOffset = degreeOffset - 360;
		
	}
}
