using UnityEngine;
using System.Collections;

public class SpecialTopicYesNoActivator : MonoBehaviour {

	public GameObject conversationTopic;
	public GameObject npcTopic;
	public SpecialTopicClass specialTopicClass;
	public int classValue;
	[Tooltip("classObject is used as transform position if this is for fast traveling.")]
	public GameObject classObject;

    public GameManager gameManager;
	public PlayerStats playerStats;
	public ShowMessage showMessage;
	public PlaySFX playSfx;

	private GameObject playerGo;
	private UIManager uiManager;

    public bool buyPropertyScript = false;
    public string buyPropertyDoorName;

	void Start() {
        gameManager = GameObject.Find("[GameManager]").GetComponent<GameManager>();
        playerGo = GameObject.Find("[Player]");
		playerStats = playerGo.GetComponent<PlayerStats>();
		playSfx = playerGo.GetComponentInChildren<PlaySFX>();
		showMessage = GameObject.Find("_UICanvasGame").GetComponentInChildren<ShowMessage>();
		uiManager = GameObject.Find ("_UICanvasGame").GetComponent<UIManager> ();
	}
	
	public void SpecialTopicYes() {
        if (buyPropertyScript == true) {
            if (playerStats.playerMoney >= classValue) {
                playerStats.playerMoney -= classValue;
                npcTopic.GetComponentInChildren<DoorManager>().doorName = buyPropertyDoorName;
                npcTopic.GetComponentInChildren<DoorManager>().locked = false;
                npcTopic.GetComponentInChildren<DoorManager>().owner.Clear();
                npcTopic.GetComponentInChildren<DoorManager>().owner.Add(playerGo);
                showMessage.SendTheMessage("You have bought a house for " + classValue + " gold coins.");

                Destroy(conversationTopic);
            }
            else {
                showMessage.SendTheMessage("You do not have enough coins.");
            }

            buyPropertyScript = false;
        }
        else {
            if (playerStats.playerMoney >= classValue) {
                playerStats.playerMoney -= classValue;
                if (specialTopicClass == SpecialTopicClass.BuyHorse) {
                    if (classObject.activeSelf == false) {
                        classObject.SetActive(true);
                    }
                    if(classObject.GetComponent<HorseManager>().hasEquipment == false) {
                        classObject.GetComponent<HorseManager>().hasEquipment = true;
                    }

                    showMessage.SendTheMessage("You bought " + classObject.GetComponent<HorseManager>().horseName + ".\n(" + classValue + ") coins removed.");
                    classObject.GetComponent<HorseManager>().owner = GameObject.FindGameObjectWithTag("Player");
                    string oldName = classObject.GetComponent<HorseManager>().horseName;
                    classObject.GetComponent<HorseManager>().horseName = "My " + oldName;

                    Destroy(conversationTopic);
                    Destroy(npcTopic);
                }
                if (specialTopicClass == SpecialTopicClass.FastTravel) {
                    playerGo.transform.position = new Vector3(classObject.transform.position.x,
                                                              classObject.transform.position.y,
                                                              classObject.transform.position.z);
                }
                if (specialTopicClass == SpecialTopicClass.RentRoom) {
                    if (gameManager.innRoomRented != classObject) {
                        classObject.GetComponent<DoorManager>().locked = false;
                        showMessage.SendTheMessage("You rented a room.\n(" + classValue + ") coins removed.");
                        gameManager.innRoomRented = classObject;
                        gameManager.dayRoomRented = gameManager.todClock.dayCount;
                    }
                    else {
                        showMessage.SendTheMessage("You already rented this room.");
                    }
                }
                playSfx.PlayBuySFX();
            }
            else {
                showMessage.SendTheMessage("You do not have enough coins.");
            }

            uiManager.conv.EndTheConversation();
            uiManager.dialogUI.SetActive(false);
        }
        gameObject.SetActive(false);
	}

	public void SpecialTopicNo() {
        buyPropertyScript = false;
        gameObject.SetActive(false);
	}
}
