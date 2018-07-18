using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DoorManager : MonoBehaviour {

    [Tooltip("This is the name of the cell/room/whatever that door lead to.")]
    public string doorName;
    [Tooltip("Door outside is the door that is outside the cell/room/whatrever. \n Door inside is the door that is inside the cell/room/whatever.")]
    public Transform doorOutside;
    public Transform doorInside;
    [Tooltip("Is this door locked?")]
    public bool locked = false;
    [Tooltip("Is this door located in the interior? If it is we do not want it to enable/disable any objects.")]
    public bool inInterior = false;
    [Tooltip("If the room/cell/whatever this door leads to have a owner, we set who he is. NPC(or NPCs) set here will register this interior as his(their) home.")]
    public List<GameObject> owner;
    [Space(10f)]
    [Tooltip("We use interior gameobject to disable/enable objects inside cell/room/whatever when player exits/enters.")]
    public Transform interiorGameobject;
    //[Tooltip("Here we put all objects that are inside the room/cell/whatever this door leads to.")]
    //public GameObject[] objectsInside;
    [Tooltip("Since ambient intensity has been removed, we now change the sky and ambient color.")]
    public Color lightInside;

    private GameObject playerGo;
    private TodWeather todWeather;
    private GameObject weatherParticlesGo;

    void Awake() {

        if (owner.Count > 0) {
            foreach (GameObject tmp in owner) {
                if (tmp.GetComponent<NPCAiNavmesh>().npcsHome == null) {
                    tmp.GetComponent<NPCAiNavmesh>().npcsHome = this;
                }
            }
        }

        if (interiorGameobject == null) {
            if (locked == false && inInterior == false) {
                Debug.LogWarning("'" + gameObject.name + "' DOOR has no interiorGameobject assigned too");
            }
        }
        else {
            foreach (Transform o in interiorGameobject) {
                o.gameObject.SetActive(false);
            }
        }

        playerGo = GameObject.Find("[Player]");
        todWeather = GameObject.Find("[GameManager]").GetComponent<TodWeather>();

        weatherParticlesGo = todWeather.gameObject.transform.Find("Skybox/Weather").gameObject;

    }

    public void UseTheDoor(GameObject doorUsed) {

        Debug.Log("Door used.");

        if (doorUsed == doorOutside.parent.gameObject || doorUsed == doorOutside.gameObject) {
            if (inInterior == false) {
                foreach (Transform o in interiorGameobject) {
                    o.gameObject.SetActive(true);
                }
                todWeather.interiorLight = lightInside;
                todWeather.inInterior = true;
                weatherParticlesGo.SetActive(false);
            }
            if (doorUsed == doorOutside.gameObject) {
                playerGo.transform.position = new Vector3(doorInside.transform.position.x,
                                                          doorInside.transform.position.y + 1f,
                                                          doorInside.transform.position.z);
            }
            else {
                playerGo.transform.position = new Vector3(doorInside.transform.position.x,
                                                          doorInside.transform.position.y,
                                                          doorInside.transform.position.z);
            }
            playerGo.transform.rotation = doorInside.transform.rotation;

            Debug.Log("Outside doors used.");
        }
        else if (doorUsed == doorInside.parent.gameObject || doorUsed == doorInside.gameObject) {
            if (inInterior == false) {
                foreach (Transform o in interiorGameobject) {
                    o.gameObject.SetActive(false);
                }
                todWeather.inInterior = false;
                weatherParticlesGo.SetActive(true);
            }
            if (doorUsed == doorInside.gameObject) {
                playerGo.transform.position = new Vector3(doorOutside.transform.position.x,
                                                          doorOutside.transform.position.y + 1f,
                                                          doorOutside.transform.position.z);
            }
            else {
                playerGo.transform.position = new Vector3(doorOutside.transform.position.x,
                                                          doorOutside.transform.position.y,
                                                          doorOutside.transform.position.z);
            }
            playerGo.transform.rotation = doorOutside.transform.rotation;

            Debug.Log("Outside doors used.");
        }
    }

    public void LockTheDoor(string lockState /*a simple `locked` means it is locked `unlocked` means it is unlocked*/) {
        if (lockState == "locked") {
            locked = true;
        }
        else if (lockState == "unlocked") {
            locked = false;
        }
    }
}
