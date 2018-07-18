using UnityEngine;
using System.Collections;

public class DoNotDestroyOnLoad : MonoBehaviour {
	
	void Start () {
		DontDestroyOnLoad (gameObject);
	}
}
