using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUI : MonoBehaviour {

    public float tutorialTime;

    public int movingTutShown = 0; //done
    public int interactTutShown = 0; //done
    public int inventTutShown = 0; //done
    public int wepMechTutShown = 0; //done
    public int mapTutShown = 0;
    public int staminaTutShown = 0; //done
    public int healthTutShown = 0; //done
    public int weightTutShown = 0; //done
    public int craftingTutShown = 0; //done
    public int crimeTutShown = 0; //done

    public GameObject movingTut; //done
    public GameObject interactTut; //done
    public GameObject inventTut; //done
    public GameObject wepMechTut; //done
    public GameObject mapTut;
    public GameObject staminaTut; //done
    public GameObject healthTut; //done
    public GameObject weightTut; //done
    public GameObject craftingTut; //done
    public GameObject crimeTut; //done

    public PlayerStats pcStats;

    private void Start() {
        pcStats = GameObject.FindObjectOfType<PlayerStats>();
    }

    private void Update() {
        if(pcStats.currentHealth < 10) {
            if (healthTutShown == 0) {
                StartCoroutine(ShowTutorialMessage(healthTut));
                healthTutShown = 1;
            }
        }
        if (pcStats.currentStamina < 10) {
            if (staminaTutShown == 0) {
                StartCoroutine(ShowTutorialMessage(staminaTut));
                staminaTutShown = 1;
            }
        }
    }

    public void ShowMovingTutorial() { //showed after pressing continue button on quest start MQ01TheArrivale after intro
        if (movingTutShown == 0) {
            StartCoroutine(ShowTutorialMessage(movingTut));
            movingTutShown = 1;
        }
    }

    public void ShowInteractTutorial() { //showed when first time pointed at something interactiable
        if (interactTutShown == 0) {
            StartCoroutine(ShowTutorialMessage(interactTut));
            interactTutShown = 1;
        }
    }

    public void ShowCrimeTutorial() { //showed when first time pointed at something that will make crime if used
        if (crimeTutShown == 0) {
            StartCoroutine(ShowTutorialMessage(crimeTut));
            crimeTutShown = 1;
        }
    }

    public void ShowWeaponTutorial() { //showed when first time enemy is added to enemy list in gameManager.cs
        if (wepMechTutShown == 0) {
            StartCoroutine(ShowTutorialMessage(wepMechTut));
            wepMechTutShown = 1;
        }
    }

    public void ShowWeightTutorial() { //showed when item should be added to inventory but there is not enough space for it.
        if (weightTutShown == 0) {
            StartCoroutine(ShowTutorialMessage(weightTut));
            weightTutShown = 1;
        }
    }

    public void ShowCraftingTutorial() { //shows when mortar and pestle are added to the inventory (picked up)
        if (craftingTutShown == 0) {
            StartCoroutine(ShowTutorialMessage(craftingTut));
            craftingTutShown = 1;
        }
    }

    public void ShowInventoryTutorial() { //shows when some item has been picked up for the first time
        if (inventTutShown == 0) {
            StartCoroutine(ShowTutorialMessage(inventTut));
            inventTutShown = 1;
        }
    }

    public IEnumerator ShowTutorialMessage(GameObject message) {
        message.SetActive(true);
        yield return new WaitForSeconds(tutorialTime);
        message.SetActive(false);
    }
}
