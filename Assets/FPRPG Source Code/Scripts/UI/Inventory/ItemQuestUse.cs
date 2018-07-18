using UnityEngine;
using System.Collections;

public enum QuestItemWhat {
	UtalirsAmulet = 0,
}

public class ItemQuestUse : MonoBehaviour {

	public QuestItemWhat thisIs;

	private GameObject playerGo;
	private FirstPersonPlayer fpsPlayer;
	private FirstPersonCameraLook fpsCamera;

	void Start() {
		playerGo = GameObject.FindGameObjectWithTag("Player");
		fpsPlayer = playerGo.GetComponent<FirstPersonPlayer>();
		fpsCamera = playerGo.GetComponent<FirstPersonCameraLook>();
	}

	public void QuestItemUsageEffect() {
		if(thisIs == QuestItemWhat.UtalirsAmulet) {
			Time.timeScale = 0.5f;
			fpsPlayer.timeSpeedFactor = 2f;
			fpsCamera.timeSpeedFactor = 2f;
		}
	}

	public void QuestItemUneqipReset() {
		if(thisIs == QuestItemWhat.UtalirsAmulet) {
			Time.timeScale = 1f;
			fpsPlayer.timeSpeedFactor = 1f;
			fpsCamera.timeSpeedFactor = 1f;
		}
	}
}
