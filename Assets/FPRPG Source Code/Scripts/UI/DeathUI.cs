using UnityEngine;
using System.Collections;

public class DeathUI : MonoBehaviour {

	public GameObject loadGameGo;

	void Start () {
	
		if(loadGameGo.activeSelf == true) {
			loadGameGo.SetActive(false);
		}
	}
}
