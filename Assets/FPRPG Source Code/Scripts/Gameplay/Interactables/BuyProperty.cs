using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyProperty : MonoBehaviour {

    [Tooltip("Here we set the property gameObject that we are buying with this. The house model from gameworld containing door as a child - NOT A INTERIOR GAMEOBJECT!")]
    public GameObject property;
    [Tooltip("Here we set the the price of the property.")]
    public int price;
    [Tooltip("Here we set name our door will change to when property bought. Must contain word 'Player' in it")]
    public string propertyDoor;
    [Tooltip("Here we put the text that we want to appear in the window when this object is activated.")]
    public string dialogWindowText;

    private GameObject playerGo;
    private UIManager uiManager;
    private GameManager gameManager;

    void Start() {
        playerGo = GameObject.Find("[Player]");
        uiManager = GameObject.Find("_UICanvasGame").GetComponent<UIManager>();
        gameManager = GameObject.Find("[GameManager]").GetComponent<GameManager>();

        if (propertyDoor.Contains("Waterhold")) {
            if (gameManager.waterholdHouseBought == true) {
                property.GetComponentInChildren<DoorManager>().doorName = propertyDoor;
                property.GetComponentInChildren<DoorManager>().locked = false;
                property.GetComponentInChildren<DoorManager>().owner.Clear();
                property.GetComponentInChildren<DoorManager>().owner.Add(playerGo);
                Destroy(gameObject.transform.parent.gameObject);
            }
        }
        else if (propertyDoor.Contains("Hightown")) {
            if (gameManager.hightownHouseBought == true) {
                property.GetComponentInChildren<DoorManager>().doorName = propertyDoor;
                property.GetComponentInChildren<DoorManager>().locked = false;
                property.GetComponentInChildren<DoorManager>().owner.Clear();
                property.GetComponentInChildren<DoorManager>().owner.Add(playerGo);
                Destroy(gameObject.transform.parent.gameObject);
            }
        }
        else if (propertyDoor.Contains("Woodmere")) {
            if (gameManager.woodmereHouseBought == true) {
                property.GetComponentInChildren<DoorManager>().doorName = propertyDoor;
                property.GetComponentInChildren<DoorManager>().locked = false;
                property.GetComponentInChildren<DoorManager>().owner.Clear();
                property.GetComponentInChildren<DoorManager>().owner.Add(playerGo);
                Destroy(gameObject.transform.parent.gameObject);
            }
        }
        else if (propertyDoor.Contains("Norburg")) {
            if (gameManager.norburgHouseBought == true) {
                property.GetComponentInChildren<DoorManager>().doorName = propertyDoor;
                property.GetComponentInChildren<DoorManager>().locked = false;
                property.GetComponentInChildren<DoorManager>().owner.Clear();
                property.GetComponentInChildren<DoorManager>().owner.Add(playerGo);
                Destroy(gameObject.transform.parent.gameObject);
            }
        }
        else if (propertyDoor.Contains("Larskeep")) {
            if (gameManager.larskeepHouseBought == true) {
                property.GetComponentInChildren<DoorManager>().doorName = propertyDoor;
                property.GetComponentInChildren<DoorManager>().locked = false;
                property.GetComponentInChildren<DoorManager>().owner.Clear();
                property.GetComponentInChildren<DoorManager>().owner.Add(playerGo);
                Destroy(gameObject.transform.parent.gameObject);
            }
        }
        else if (propertyDoor.Contains("Newtown")) {
            if (gameManager.newtownHouseBought == true) {
                property.GetComponentInChildren<DoorManager>().doorName = propertyDoor;
                property.GetComponentInChildren<DoorManager>().locked = false;
                property.GetComponentInChildren<DoorManager>().owner.Clear();
                property.GetComponentInChildren<DoorManager>().owner.Add(playerGo);
                Destroy(gameObject.transform.parent.gameObject);
            }
        }
    }

    public void AskToBuyProperty() {

        uiManager.specialTopicerUI.SetActive(true);
        SpecialTopicYesNoActivator sp = uiManager.specialTopicerUI.GetComponent<SpecialTopicYesNoActivator>();

        sp.buyPropertyScript = true;
        sp.classValue = price;
        sp.transform.Find("TopicTxt").GetComponent<Text>().text = dialogWindowText;
        sp.npcTopic = property; //We set npcTopic as a temp holder of the property game object
        sp.buyPropertyDoorName = propertyDoor;
        sp.conversationTopic = gameObject.transform.parent.gameObject;

        if (propertyDoor.Contains("Waterhold")) {
            gameManager.waterholdHouseBought = true;
        }
        else if (propertyDoor.Contains("Newtown")) {
            gameManager.newtownHouseBought = true;
        }
        else if (propertyDoor.Contains("Woodmere")) {
            gameManager.woodmereHouseBought = true;
        }
        else if (propertyDoor.Contains("Norburg")) {
            gameManager.norburgHouseBought = true;
        }
        else if (propertyDoor.Contains("Larskeep")) {
            gameManager.larskeepHouseBought = true;
        }
        else if (propertyDoor.Contains("Newtown")) {
            gameManager.newtownHouseBought = true;
        }
    }
}
