using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Container : MonoBehaviour {

	public string containerName;
	private string containerNameStored;
	public List<GameObject> itemsInContainer;

	void Start() {
		containerNameStored = containerName;
	}

	void Update() {
		if(itemsInContainer.Count == 0) {
			containerName = containerNameStored + " (Empty)";
		}
	}
}
