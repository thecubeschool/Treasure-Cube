using UnityEngine;
using System.Collections;
using System.Linq;

public class PickupItem : MonoBehaviour {

    private UIManager uiManager;
	private WeightedInventory inventory;
	private ShowMessage showMessage;
	private PlayerStats playerStats;

    private int arrowQuiverNumber;

	void Start() {
        uiManager = GameObject.Find("_UICanvasGame").GetComponent<UIManager>();
        inventory = uiManager.inventoryUI.GetComponent<WeightedInventory>();
		showMessage = uiManager.gameObject.GetComponentInChildren<ShowMessage>();
		playerStats = GameObject.Find("[Player]").GetComponent<PlayerStats>();
	}

    public void PickupThisItem() {
        int weightDifference = playerStats.maxCarryWeight - playerStats.currentWeight;

		if(gameObject.GetComponent<Item>().itemWeight <= weightDifference) {
            if(gameObject.GetComponent<Item>().name.Contains("Mortar")) {
                uiManager.tutorialUI.GetComponentInChildren<TutorialUI>().ShowCraftingTutorial();
            }
			inventory.AddItemToInventory(gameObject);
			GetComponent<Rigidbody>().useGravity = false;

            uiManager.tutorialUI.GetComponentInChildren<TutorialUI>().ShowInventoryTutorial();

            if (GetComponent<Item>().owner != null) {
				GetComponent<Item>().stolen = true;
			}
		}
		else {
            uiManager.tutorialUI.GetComponentInChildren<TutorialUI>().ShowWeightTutorial();
			showMessage.SendTheMessage("Inventory is full, I can not carry any more.");
		}
	}
}