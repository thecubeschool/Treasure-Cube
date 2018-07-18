using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CompassNeedle : MonoBehaviour {

	public Transform playerTransform;
	
	public int north;
	public int needleOffset;

	private int degreeOffset;

	private RectTransform rectTransform;

	void Start() {
		playerTransform = GameObject.Find("[Player]").transform;
		rectTransform = GetComponent<RectTransform>();
	}

	void Update () {
		rectTransform.rotation = Quaternion.Euler(0f, 0f, north);

		north = (int)Mathf.Abs(playerTransform.eulerAngles.y + needleOffset);
		if (north > 360) north = north % 360;
		
		degreeOffset = north;
		
		if(degreeOffset > 180) degreeOffset = degreeOffset - 360;
	}
}
