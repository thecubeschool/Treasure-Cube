using UnityEngine;
using System.Collections;

public class PlayerLevelManager : MonoBehaviour {

	public int pointsForLeveling = 0;

	public int playerLevel = 1;
	[Tooltip("Player level will increase after every 5 skill advances")]
	public int skillsAdvanced = 0;

	private ShowMessage showMessage;

	void Start() {
		if (GameObject.Find ("_UICanvasGame")) {
			showMessage = GameObject.Find ("_UICanvasGame").GetComponentInChildren<ShowMessage> ();
		}
	}

	void Update() {
		if(skillsAdvanced == 5) {
			playerLevel++;
			pointsForLeveling += 4;

			if (showMessage != null) {
				showMessage.SendTheMessage ("You feel stronger!");
			}

			skillsAdvanced = 0;
		}
	}
}
