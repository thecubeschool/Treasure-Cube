using UnityEngine;
using System.Collections;

public class WaterProbe : MonoBehaviour {

	public float yPos = 0f;
	public Vector3 playerPos;
	public GameObject playerGo;

	void Update () {
		playerPos = new Vector3 (playerGo.transform.position.x, yPos, playerGo.transform.position.z);
		//transform.position = Vector3.Slerp (transform.position, playerPos, Time.deltaTime);
		transform.position = playerPos;
	}
}
