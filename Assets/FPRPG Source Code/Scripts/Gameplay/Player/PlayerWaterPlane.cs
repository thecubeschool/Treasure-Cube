using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWaterPlane : MonoBehaviour {

    public float waterYPosition;
    public Transform playerGo;

	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(playerGo.position.x, waterYPosition, playerGo.position.z);
	}
}
