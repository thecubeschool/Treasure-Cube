using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class PrepareSceneForStart : EditorWindow {
	[MenuItem("The Fallen Chronicler/Utility/Prepare Scene")]

	public static void  ShowWindow () {
		EditorWindow.GetWindow(typeof(PrepareSceneForStart));
	}

	void OnGUI () {
		if(GUILayout.Button("Prepare Scene")) {

			Scene currentScene = EditorSceneManager.GetActiveScene ();
			string sceneName = currentScene.name;

			if (GameObject.Find ("[GameManager]").activeSelf == false) {
				GameObject.Find ("[GameManager]").SetActive (true);
			}
			if (GameObject.Find ("_UICanvasGame").activeSelf == false) {
				GameObject.Find ("_UICanvasGame").SetActive (true);
			}
			if (GameObject.Find ("_UIEventSystem").activeSelf == false) {
				GameObject.Find ("_UIEventSystem").SetActive (true);
			}
			if (GameObject.Find ("[Player]").activeSelf == false) {
				GameObject.Find ("[Player]").SetActive (true);
			}
			if (GameObject.Find ("[WorldMapSystem]").activeSelf == true) {
				GameObject.Find ("[WorldMapSystem]").SetActive (false);
			}
			Debug.Log ("Scene has been reset.");
		}
		GUILayout.Label ("This will reset the scene and prepare it for startup, which means if a GameObject that is needed for game to run is disabled it will reset it and enable it.");
	}
}
