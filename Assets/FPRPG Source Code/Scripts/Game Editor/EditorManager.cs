using UnityEngine;
using System.Collections;

public class EditorManager : MonoBehaviour {

	public GameObject editorCamera;
	public GameObject playerObject;
	public GameObject playerCamera;
	[Space(10f)]
	public GameObject gameUi;
	public GameObject editorUi;

	public bool inEditMode = false;

	void Awake() {
		inEditMode = false;

		editorUi.SetActive(false);
		gameUi.SetActive(true);
	}

	void Update () {
		if(inEditMode == false) {
			editorCamera.GetComponent<Camera>().enabled = false;
			if(playerObject != null) {
				playerObject.SetActive(true);
			}
			if(gameUi != null) {
				gameUi.SetActive(true);
			}
			if(editorUi != null) {
				editorUi.SetActive(false);
			}

			if(editorCamera.GetComponent<AudioListener>()) {
				Destroy(editorCamera.GetComponent<AudioListener>());
			}
			if(!playerCamera.GetComponent<AudioListener>()) {
				playerCamera.AddComponent<AudioListener>();
			}
		}
		else {
			editorCamera.GetComponent<Camera>().enabled = true;
			if(playerObject != null) {
				playerObject.SetActive(false);
			}
			if(gameUi != null) {
				gameUi.SetActive(false);
			}
			if(editorUi != null) {
				editorUi.SetActive(true);
			}

			if(playerCamera.GetComponent<AudioListener>()) {
				Destroy(playerCamera.GetComponent<AudioListener>());
			}
			if(!editorCamera.GetComponent<AudioListener>()) {
				editorCamera.AddComponent<AudioListener>();
			}

			if(Input.GetKey(KeyCode.Mouse1)) {
				Cursor.visible = false;
			}
			else {
				Cursor.visible = true;
			}
		}

		if (Input.GetKeyDown (KeyCode.F11)) {
			if(inEditMode == false) {
				inEditMode = true;
				editorCamera.transform.position = playerCamera.transform.position;
				editorCamera.transform.localEulerAngles = playerCamera.transform.localEulerAngles;
			}
			else {
				inEditMode = false;
			}
		}
	}
}
