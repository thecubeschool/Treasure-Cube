using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class UIManager : MonoBehaviour {

	public AudioSource uiAs;

	public bool disableUi;
	public List<GameObject> componentsToDisable;
	private int uisDisabled = 0;
	private int uisEnabled = 0;
	public bool disableUiCinematic;

	public GameObject fpsCounter;

	public GameObject healthUI;

	public GameObject crosshairUI;
	public GameObject targetHealthUI;
	private float targetHealthLerper = 0f;
	public GameObject inventoryUI;
	public GameObject waiterUI;
	public GameObject worldMapUI;
	public GameObject worldCameraGo;
    public GameObject worldCameraUI;
	public GameObject compassUI;
	public GameObject dialogUI;
	public GameObject specialTopicerUI;
	public GameObject storeUI;
	public GameObject bookUI;
	public GameObject alchemyUI;
	public GameObject consoleUI;
	public GameObject pauseUI;
	public GameObject bugReportUI;
	public GameObject locationTxtUI;
    public GameObject tutorialUI;
    public GameObject blackscreenUI;

	public UISkillsAndAttributes uISkillsAndAttributes;

	[Space(10f)]
	[Header("Quest Popup Window")]
	public AudioClip questPopupSfx;
	public GameObject questPopupWindow;
	public Text questStatus;
	public Text questName;
	public Text questDate;
	public Text questDescripton;
	[Space(10f)]
	[Header("Input Reliable")]
	public GameObject bookKeyboard;
	public GameObject bookGamepad;
	public GameObject conversationKeyboard;
	public GameObject conversationGamepad;

	private GameObject playerGo;
	private ShowMessage showMessage;
	private FirstPersonPlayer fpsController;
	private FirstPersonCameraLook fpsCamLook;
	private FirstPersonAnimate fpAnimate;
	private WeaponMeleeAnimate wepAnimate;
	private WeaponRangedAnimate wepRangeAnimate;
	public HorseRiding horseRiding;
	private CharacterController controller;
	private QuestLogManager questLogManager;
	private GameManager gameManager;
	[HideInInspector]
	public ConversationBase conv;

	public Color whiteAlpha;

	void Awake () {

		uisEnabled = componentsToDisable.Count;

		if(inventoryUI.activeSelf == false) {
			inventoryUI.SetActive(true);
		}
		if(worldMapUI.activeSelf == false) {
			worldCameraGo.SetActive (false);
            worldCameraUI.SetActive(false);
            worldMapUI.SetActive(true);
		}
		//StartCoroutine(DelayTheFireOfUI());

		playerGo = GameObject.Find("[Player]");
		showMessage = GetComponentInChildren<ShowMessage>();
		fpsController = playerGo.GetComponent<FirstPersonPlayer>();
		fpsCamLook = playerGo.GetComponent<FirstPersonCameraLook>();
		fpAnimate = playerGo.GetComponentInChildren<FirstPersonAnimate>();
		wepAnimate = playerGo.GetComponentInChildren<WeaponMeleeAnimate>();
		wepRangeAnimate = playerGo.GetComponentInChildren<WeaponRangedAnimate>();
		if (horseRiding == null) {
			horseRiding = playerGo.GetComponentInChildren<HorseRiding> ();
		}
		controller = playerGo.GetComponent<CharacterController>();
		questLogManager = GetComponentInChildren<QuestLogManager>();
		conv = dialogUI.GetComponent<ConversationBase>();
		gameManager = GameObject.Find("[GameManager]").GetComponent<GameManager>();

		fpsCounter.SetActive(false);
		bugReportUI.SetActive (false);

		/*
		fpsCamLook.enabled = true;
		fpsController.enabled = false;
		horseRiding.enabled = false;
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		crosshairUI.SetActive(false);
		healthUI.SetActive(false);
		fpsCamLook.timeSpeedFactor = 0f;

		if(horseRiding.playerMounted == false) {
			fpAnimate.stopAnimation = true;
			wepAnimate.stopAnimation = true;
			wepRangeAnimate.stopAnimation = true;
			horseRiding.stopAnimation = true;
		}
		else {
			fpAnimate.stopAnimation = false;
			wepAnimate.stopAnimation = false;
			wepRangeAnimate.stopAnimation = false;
			horseRiding.stopAnimation = false;
		}*/
	}

	void Start() {

		if (gameManager.debugingMode == true) {
			Debug.Log ("UIManager started initilazing.");
		}

		DelayTheFireOfUI ();

		if (gameManager.debugingMode == true) {
			Debug.Log ("UIManager finished initilazing.");
		}
	}

	void Update () {

		if(gameManager.inputUsed == InputUsed.Keyboard) {
			bookKeyboard.SetActive(true);
			conversationKeyboard.SetActive(true);
			bookGamepad.SetActive(false);
			conversationGamepad.SetActive(false);
		}
		else if (gameManager.inputUsed == InputUsed.XboxController) {
			bookKeyboard.SetActive(false);
			conversationKeyboard.SetActive(false);
			bookGamepad.SetActive(true);
			conversationGamepad.SetActive(true);
		}

		if(Input.GetButtonDown("Console")) {
			if(disableUiCinematic == false) {
				if(consoleUI.activeSelf == false) {
					consoleUI.SetActive(true);
					consoleUI.GetComponentInChildren<InputField>().ActivateInputField();
					consoleUI.GetComponentInChildren<InputField>().text = "";
					Time.timeScale = 0.0001f;

				}
				else {
					consoleUI.GetComponentInChildren<InputField>().DeactivateInputField();
					consoleUI.GetComponentInChildren<InputField>().text = "";
					consoleUI.SetActive(false);
				}
			}
		}

		if(Input.GetKey(KeyCode.F) && Input.GetKey(KeyCode.P) && Input.GetKeyDown(KeyCode.S)) {
			if(consoleUI.activeSelf == false) {
				if(fpsCounter.activeSelf == false) {
					fpsCounter.SetActive(true);
				}
				else {
					fpsCounter.SetActive(false);
				}
			}
		}

		if(Input.GetKey(KeyCode.U) && Input.GetKey(KeyCode.I) && Input.GetKeyDown(KeyCode.T)) {
			if(consoleUI.activeSelf == false) {
				if(disableUi == false) {
					disableUi = true;
				}
				else {
					disableUi = false;
				}
			}
		}

		if(Input.GetKeyDown(KeyCode.F1)) {
            //bugReportUI.GetComponent<BugReporter>().brHolder.SetActive(false);
            //bugReportUI.SetActive(true);
			//Time.timeScale = 0.0001f;
            //Debug.Log("Opening Bug Report.");
		}

		if(Time.timeScale < 3) {
			if(consoleUI.activeSelf == false) {
				if(Input.GetButtonDown("Back/Close")) { //Opens up PAUSE MENU
					if(disableUiCinematic == false) {
						if(dialogUI.activeSelf == false && inventoryUI.activeSelf == false && waiterUI.activeSelf == false &&
						   specialTopicerUI.activeSelf == false && storeUI.activeSelf == false && consoleUI.activeSelf == false && 
						   bookUI.activeSelf == false && alchemyUI.activeSelf == false && questPopupWindow.activeSelf == false &&
                           worldMapUI.activeSelf == false) {
							pauseUI.SetActive(true);
							EventSystem.current.SetSelectedGameObject(pauseUI.transform.Find("PauseBar/BtnContinue").gameObject);
							Time.timeScale = 0.0001f;
						}
					}
				}
			}
		}
		if(dialogUI.activeSelf == true) {
			if(consoleUI.activeSelf == false) {
				if(Input.GetButtonDown("Cancel")) { //Closes DIALOG MENU
					if(disableUiCinematic == false) {
						conv.EndTheConversation();
						dialogUI.SetActive(false);
					}
				}
			}
            tutorialUI.GetComponent<RectTransform>().localScale = new Vector3(0.01f, 0.01f, 0.01f);
            //tutorialUI.SetActive(false);
		}
        else {
            tutorialUI.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
            //tutorialUI.SetActive(true);
        }
        if (inventoryUI.activeSelf == true) {
			if(consoleUI.activeSelf == false) {
				if(Input.GetButtonDown("Cancel")) { //Closes INVENTORY MENU
					if(disableUiCinematic == false) {
						inventoryUI.GetComponent<WeightedInventory>().ResetInventory();
						questLogManager.DestroyQuestList();
						ResetTime();
						fpsCamLook.timeSpeedFactor = 1f;
						inventoryUI.SetActive(false);
					}
				}
			}
		}
		if(worldMapUI.activeSelf == true) {
			if(consoleUI.activeSelf == false) {
				if(Input.GetButtonDown("Cancel")) { //Closes WORLD MAP MENU
					if(disableUiCinematic == false) {
                        locationTxtUI.SetActive (true);
						ResetTime();
						fpsCamLook.timeSpeedFactor = 1f;
						worldCameraGo.SetActive (false);
                        worldCameraUI.SetActive(false);
                        worldMapUI.SetActive(false);
					}
				}
			}
		}

        /* We disabled the compass turn on/off and it will be showed on screen all the time.
		if(Input.GetButtonUp("ShowCompass")) {
			if(disableUiCinematic == false) {
				if(consoleUI.activeSelf == false) {
					if(compassUI.activeSelf == true) {
						compassUI.SetActive(false);
					}
					else {
						compassUI.SetActive(true);
					}
				}
			}
		}
        */

		if(Input.GetButtonDown("Cancel")) { //Closes ANY MENU
			if(disableUiCinematic == false) {
				if(storeUI.activeSelf == true) {
					storeUI.GetComponentInChildren<StorePlayerInventory>().RESETPlayerStore();
					storeUI.GetComponentInChildren<StoreMerchantsInventory>().CLEANMerchantStore();
					dialogUI.SetActive(true);
					storeUI.SetActive(false);
				}
				if(bookUI.activeSelf == true) {
					bookUI.GetComponent<BookUi>().BookClearContent();
					bookUI.SetActive(false);
					inventoryUI.SetActive(true);
					EventSystem.current.SetSelectedGameObject(inventoryUI.transform.Find("TabAll").gameObject);
				}
				if(waiterUI.activeSelf == true) {
					waiterUI.SetActive(false);
                    waiterUI.transform.Find("WaitBtn").gameObject.SetActive(true);
                    waiterUI.transform.Find("CancelBtn").gameObject.SetActive(false);

                }
                if (alchemyUI.activeSelf == true) {
					alchemyUI.SetActive(false);
					if(alchemyUI.GetComponent<AlchemyUi>().uiAmbientAS.clip == alchemyUI.GetComponent<AlchemyUi>().boilingWaterSFX) {
						if(alchemyUI.GetComponent<AlchemyUi>().uiAmbientAS.volume > 0) {
							alchemyUI.GetComponent<AlchemyUi>().uiAmbientAS.volume -= Time.deltaTime;
						}
					}
					inventoryUI.SetActive(true);
					EventSystem.current.SetSelectedGameObject(inventoryUI.transform.Find("TabAll").gameObject);
				}
			}
		}

		if(Time.timeScale < 3f && controller.isGrounded == true && pauseUI.activeSelf == false) {
			if(consoleUI.activeSelf == false) {
				if (Input.GetButtonDown("OpenInventory")) { //Opens INVENTORY MENU
					if (disableUiCinematic == false) {
						if (waiterUI.activeSelf == false && dialogUI.activeSelf == false && alchemyUI.activeSelf == false && bookUI.activeSelf == false
							&& questPopupWindow.activeSelf == false && storeUI.activeSelf == false && worldMapUI.activeSelf == false) {
							if (inventoryUI.activeSelf == true) {
								inventoryUI.GetComponent<WeightedInventory> ().ResetInventory ();
								questLogManager.DestroyQuestList ();
								ResetTime ();
								fpsCamLook.timeSpeedFactor = 1f;
								inventoryUI.SetActive (false);
							} else {
								inventoryUI.GetComponent<WeightedInventory> ().SetUpTheInventory ();
								fpsCamLook.timeSpeedFactor = 0f;
								Time.timeScale = 0.0001f;
								inventoryUI.SetActive (true);
								if (inventoryUI.transform.Find ("CharacterStats/QuestLogScrollRect").gameObject.activeSelf == true) {
									questLogManager.UpdateQuestList ();
								} else if (inventoryUI.transform.Find ("CharacterStats/Skills").gameObject.activeSelf == true &&
								        inventoryUI.transform.Find ("CharacterStats/QuestLogScrollRect").gameObject.activeSelf == true) {
									inventoryUI.transform.Find ("CharacterStats/Skills").gameObject.SetActive (false);
								}
								if (inventoryUI.transform.Find ("CharacterStats/Skills").gameObject.activeSelf == true) {
									questLogManager.UpdateQuestsInfo ();
								}
								EventSystem.current.SetSelectedGameObject (inventoryUI.transform.Find ("TabAll").gameObject);
							}
						}
					}
				}
				else if (Input.GetButtonDown("OpenWorldmap")) { //Opens WORLD MAP MENU
					if (disableUiCinematic == false) {
						if (waiterUI.activeSelf == false && dialogUI.activeSelf == false && alchemyUI.activeSelf == false && bookUI.activeSelf == false
							&& questPopupWindow.activeSelf == false && storeUI.activeSelf == false && inventoryUI.activeSelf == false) {
							if (worldMapUI.activeSelf == true) {
                                locationTxtUI.SetActive (true);
								ResetTime ();
								fpsCamLook.timeSpeedFactor = 1f;
								worldCameraGo.SetActive (false);
                                if (worldCameraUI != null) {
                                    worldCameraUI.SetActive(false);
                                }
                                worldMapUI.SetActive (false);
							}
							else {
                                locationTxtUI.SetActive (false);
								fpsCamLook.timeSpeedFactor = 0f;
								Time.timeScale = 0.0001f;
								worldCameraGo.SetActive (true);
                                if (worldCameraUI != null) {
                                    worldCameraUI.SetActive(true);
                                }
                                worldMapUI.SetActive (true);
							}
						}
					}
					
				}

                if (disableUiCinematic == false) {
                    if (waiterUI.activeSelf == false && dialogUI.activeSelf == false && alchemyUI.activeSelf == false && bookUI.activeSelf == false
                        && questPopupWindow.activeSelf == false && storeUI.activeSelf == false && worldMapUI.activeSelf == false && specialTopicerUI == true) {
                        
                        fpsCamLook.timeSpeedFactor = 0f;
                        Time.timeScale = 0.0001f;
                        EventSystem.current.SetSelectedGameObject(specialTopicerUI.transform.Find("BtnYes").gameObject);
                    }
                }

                /*if(Input.GetButtonUp("OpenWaiter")) {
					if(disableUiCinematic == false) {
						if(horseRiding.playerMounted == false && gameManager.playerState != PlayerState.Fighting) {
							if(inventoryUI.activeSelf == false && dialogUI.activeSelf == false && alchemyUI.activeSelf == false) {
								if(waiterUI.activeSelf == true) {
									waiterUI.SetActive(false);
								}
								else {
									if(Time.timeScale == 1f) {
										waiterUI.SetActive(true);
										EventSystem.current.SetSelectedGameObject(waiterUI.transform.FindChild("WaitBtn").gameObject);
									}
									else {
										showMessage.SendTheMessage("I already feel like time is passing by.");
									}
								}
							}
						}
						else {
							showMessage.SendTheMessage("I can not do that right now.");
						}
					}
				}*/
            }
		}
		else {
			if(Input.GetButtonDown("OpenInventory") || Input.GetButtonDown("OpenWorldmap")) {
				if(disableUiCinematic == false) {
					showMessage.SendTheMessage("I can not do that right now.");
				}
			}
		}

		if(disableUiCinematic == false && disableUi == false) {
			if(inventoryUI.activeSelf == false && waiterUI.activeSelf == false && dialogUI.activeSelf == false && questPopupWindow.activeSelf == false &&
			   pauseUI.activeSelf == false && consoleUI.activeSelf == false && storeUI.activeSelf == false && bookUI.activeSelf == false &&
				alchemyUI.activeSelf == false && bugReportUI.activeSelf == false && worldMapUI.activeSelf == false && specialTopicerUI.activeSelf == false) {
				fpsCamLook.enabled = true;
				fpsController.enabled = true;
                compassUI.SetActive(true);
                fpAnimate.stopAnimation = false;
				wepAnimate.stopAnimation = false;
				wepRangeAnimate.stopAnimation = false;
				horseRiding.enabled = true;
				horseRiding.stopAnimation = false;
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
				crosshairUI.SetActive(true);
				healthUI.SetActive(true);
				fpsCamLook.timeSpeedFactor = 1f;
				Time.timeScale = 1f;
			}
			else {
				if(alchemyUI.activeSelf == true && alchemyUI.GetComponent<AlchemyUi>().uiAmbientAS.volume < 1) {
					alchemyUI.GetComponent<AlchemyUi>().uiAmbientAS.Play();
					alchemyUI.GetComponent<AlchemyUi>().uiAmbientAS.volume += Time.deltaTime;
				}
				fpsCamLook.enabled = false;
				fpsController.enabled = false;
				horseRiding.enabled = false;
				if(gameManager.inputUsed == InputUsed.Keyboard) {
					Cursor.lockState = CursorLockMode.None;
					Cursor.visible = true;
				}
				else {
					Cursor.lockState = CursorLockMode.Locked;
					Cursor.visible = false;
				}
				crosshairUI.SetActive(false);
				healthUI.SetActive(false);
                compassUI.SetActive(false);
                fpsCamLook.timeSpeedFactor = 0f;
				if(waiterUI.activeSelf == false) {
					Time.timeScale = 0.0001f;
				}

				if(horseRiding.playerMounted == false) {
					fpAnimate.stopAnimation = true;
					wepAnimate.stopAnimation = true;
					wepRangeAnimate.stopAnimation = true;
					horseRiding.stopAnimation = true;
				}
				else {
					fpAnimate.stopAnimation = false;
					wepAnimate.stopAnimation = false;
					wepRangeAnimate.stopAnimation = false;
					horseRiding.stopAnimation = false;
				}
			}
		}
		else if(disableUiCinematic == true && disableUi == false) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            crosshairUI.SetActive(false);
            healthUI.SetActive(false);
            compassUI.SetActive(false);
		}

		if (disableUi == true) {
			if (uisDisabled < componentsToDisable.Count) {
				foreach (GameObject go in componentsToDisable) {
                    go.SetActive(false);
                    uisDisabled++;
                }
			}
			else {
				uisEnabled = 0;
			}
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
		else {
			if (uisEnabled < componentsToDisable.Count) {
				foreach (GameObject go in componentsToDisable) {
                    go.SetActive(false);
                    uisEnabled++;
                }
			}
			else {
				uisDisabled = 0;
			}
		}
	}

	void DelayTheFireOfUI() {
		if(inventoryUI.activeSelf == true) {
			inventoryUI.SetActive(false);
		}
		if(worldMapUI.activeSelf == true) {
			worldCameraGo.SetActive (false);
            if (worldCameraUI != null) {
                worldCameraUI.SetActive(false);
            }
            worldMapUI.SetActive(false);
		}
		if(waiterUI.activeSelf == true) {
			waiterUI.SetActive(false);
		}
		if(dialogUI.activeSelf == true) {
			dialogUI.SetActive(false);
		}
		if(questPopupWindow.activeSelf == true) {
			questPopupWindow.SetActive(false);
		}
		if(consoleUI.activeSelf == true) {
			consoleUI.SetActive(false);
		}
		if(pauseUI.activeSelf == true) {
			if(pauseUI.transform.Find("QuitDialog").gameObject.activeSelf == true) {
				pauseUI.transform.Find("QuitDialog").gameObject.SetActive(false);
			}
			pauseUI.SetActive(false);
		}
		if(specialTopicerUI.activeSelf == true) {
			specialTopicerUI.SetActive(false);
		}
		if(storeUI.activeSelf == true) {
			storeUI.SetActive(false);
		}
		if(bookUI.activeSelf == true) {
			bookUI.SetActive(false);
		}
		if(alchemyUI.activeSelf == true) {
			alchemyUI.SetActive(false);
		}
		ResetTime();
	}

	public void SpeedUpTime(float multiplier) {
		Time.timeScale = multiplier;
	}

	public void ResetTime() {
		Time.timeScale = 1f;
	}

	public void QuestInfoWindowPopup(string qState, string qName, string qDate, string qDesc) {

		if(questPopupWindow.activeSelf == false) {
			questStatus.text = qState;
			questName.text = qName;
			questDate.text = qDate;
			questDescripton.text = qDesc;

			uiAs.PlayOneShot(questPopupSfx);

			SetActiveUIElement(questPopupWindow.transform.Find("BtnContinue").gameObject);

			questPopupWindow.SetActive(true);
		}
	}

	public void SetActiveUIElement(GameObject element) {
		EventSystem.current.SetSelectedGameObject(element);
	}

	public void TargetHealthUIShow(bool showBool) {
		if(showBool == true) {
			if (targetHealthUI.GetComponent<Image> ().color != Color.white) {
				targetHealthLerper += Time.deltaTime * 0.5f;
				targetHealthUI.GetComponent<Image> ().color = Color.Lerp (targetHealthUI.GetComponent<Image> ().color, Color.white, targetHealthLerper);
			} 
			else {
				targetHealthLerper = 0;
			}
			showBool = false;
		}
	}

	public void TargetHealthUIHide(bool showBool) {
		if(showBool == true) {
			if (targetHealthUI.GetComponent<Image> ().color != whiteAlpha) {
				targetHealthLerper += Time.deltaTime * 0.5f;
				targetHealthUI.GetComponent<Image> ().color = Color.Lerp (targetHealthUI.GetComponent<Image> ().color, whiteAlpha, targetHealthLerper);
			} 
			else {
				targetHealthLerper = 0;
			}
			showBool = false;
		}
	}
}
