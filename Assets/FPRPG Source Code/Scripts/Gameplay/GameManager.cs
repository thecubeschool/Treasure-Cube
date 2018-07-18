using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public enum PlayerState {
	Exploring = 0,
	Dungeoning = 1,
	Fighting = 2,
	SpecialEvent = 3,
	MainMenu = 4,
}

public enum InputUsed {
	Keyboard = 0,
	XboxController = 1,
}

public class GameManager : MonoBehaviour {

	public PlayerState playerState;
	//private PlayerStats playerStats;
	public UIManager uiManager;
    public TodClock todClock;

    public string trackingQuestId;

	public GameObject activeHorse;
	public bool playerSeen;
	public List<GameObject> whoSeePlayer;
	public LayerMask sunLayer;
	public bool playerInSunlight;
	public bool worldMapMode;
	public bool flyingMode;
	public bool playerImmortal;

	public GameObject currentOpponentFighting;
	public List<GameObject> playerOpponents;

	private GameObject playerGo;
	private GameObject playerUi;
	public GameObject sunGo;

	public List<GameObject> allGameItems;
	public List<GameObject> allGameNpcs;

	public InputUsed inputUsed;

	public bool debugingMode;
	[HideInInspector]
	public float lastTimeOpponentHit;

    public bool waterholdHouseBought = false;
    public bool hightownHouseBought = false;
    public bool woodmereHouseBought = false;
    public bool norburgHouseBought = false;
    public bool larskeepHouseBought = false;
    public bool newtownHouseBought = false;

    public GameObject innRoomRented;
    public int dayRoomRented;

	void Awake() {

		transform.eulerAngles = new Vector3 (0f, -90f, 0f);

		playerUi = GameObject.Find("_UICanvasGame");
		playerGo = GameObject.Find("[Player]");

		//playerStats = playerGo.GetComponent<PlayerStats>();

		if (playerUi != null) {
			if (playerUi.activeSelf == false) {
				playerUi.SetActive (true);
			}
			if (playerGo.activeSelf == false) {
				playerGo.SetActive (true);
			}
		}
		inputUsed = InputUsed.Keyboard;
	
		if (debugingMode == true) {
			Debug.Log("GameManager.cs initialized.");
		}

        uiManager = GameObject.FindObjectOfType<UIManager>();

    }

	void Update() {
		if(playerOpponents.Count > 0) {
			playerState = PlayerState.Fighting;
            uiManager.tutorialUI.GetComponentInChildren<TutorialUI>().ShowWeaponTutorial();
        }
		else {
			playerState = PlayerState.Exploring;
		}

		if(Input.GetKey(KeyCode.LeftControl) && Input.GetKeyUp(KeyCode.I)) {
			if(playerUi.activeSelf == false) {
				playerUi.SetActive(true);
			}
			else {
				playerUi.SetActive(false);
			}
		}

		SunlightOnPlayerCast();

		if(flyingMode == true) {
			playerGo.GetComponent<FirstPersonPlayer>().enabled = false;
			playerGo.GetComponent<FirstPersonCameraLook>().enabled = false;
			playerGo.GetComponent<EditorCamera>().enabled = true;
		}
		else {
			playerGo.GetComponent<FirstPersonPlayer>().enabled = true;
			playerGo.GetComponent<FirstPersonCameraLook>().enabled = true;
			playerGo.GetComponent<EditorCamera>().enabled = false;
		}

		//if(playerImmortal == true) {
			//if(playerStats.currentHealth < 50) {
			//	playerStats.currentHealth = 1;
			//}
		//}

		if (whoSeePlayer.Count > 0) {
			playerSeen = true;
		} 
		else {
			playerSeen = false;
		}

        if (uiManager != null) {
            if (currentOpponentFighting != null) {
                float distance = Vector3.Distance(playerGo.transform.position, currentOpponentFighting.transform.position);
                if (distance < 18f) {
                    uiManager.TargetHealthUIShow(true);
                }
                else {
                    uiManager.TargetHealthUIHide(true);
                }

                uiManager.targetHealthUI.GetComponent<Image>().fillAmount =
                    (float)currentOpponentFighting.GetComponent<NPC>().npcHealth / currentOpponentFighting.GetComponent<NPC>().npcMaxHealth;
            }
            else {
                uiManager.TargetHealthUIHide(true);
            }
        }

        if(innRoomRented != null) {
            if(todClock.dayCount > dayRoomRented) {
                innRoomRented.GetComponent<DoorManager>().locked = true;
                innRoomRented = null;
            }
        }
	}

	void SunlightOnPlayerCast() {
		RaycastHit hit;
		Vector3 rayDirection = playerGo.transform.position - sunGo.transform.position;

		if(Physics.Raycast (sunGo.transform.position, rayDirection, out hit, 4000f, sunLayer)){
			if(hit.transform.CompareTag("Player")){
				playerInSunlight = true;
			}
			else {
				playerInSunlight = false;
			}
		}

		Debug.DrawLine(sunGo.transform.position, playerGo.transform.position, Color.yellow);
	}

	void OnGUI() {
		InputUpdater();
	}

	private void InputUpdater() {
		if(Input.GetKey(KeyCode.JoystickButton0)  ||
		   Input.GetKey(KeyCode.JoystickButton1)  ||
		   Input.GetKey(KeyCode.JoystickButton2)  ||
		   Input.GetKey(KeyCode.JoystickButton3)  ||
		   Input.GetKey(KeyCode.JoystickButton4)  ||
		   Input.GetKey(KeyCode.JoystickButton5)  ||
		   Input.GetKey(KeyCode.JoystickButton6)  ||
		   Input.GetKey(KeyCode.JoystickButton7)  ||
		   Input.GetKey(KeyCode.JoystickButton8)  ||
		   Input.GetKey(KeyCode.JoystickButton9)  ||
		   Input.GetKey(KeyCode.JoystickButton10) ||
		   Input.GetKey(KeyCode.JoystickButton11) ||
		   Input.GetKey(KeyCode.JoystickButton12) ||
		   Input.GetKey(KeyCode.JoystickButton13) ||
		   Input.GetKey(KeyCode.JoystickButton14) ||
		   Input.GetKey(KeyCode.JoystickButton15) ||
		   Input.GetKey(KeyCode.JoystickButton16) ||
		   Input.GetKey(KeyCode.JoystickButton17) ||
		   Input.GetKey(KeyCode.JoystickButton18) ||
		   Input.GetKey(KeyCode.JoystickButton19)) {
			inputUsed = InputUsed.XboxController;

		}
		else if(Event.current.isKey == true || 
		        Event.current.isMouse == true) {
			inputUsed = InputUsed.Keyboard;
		}
	}
}
