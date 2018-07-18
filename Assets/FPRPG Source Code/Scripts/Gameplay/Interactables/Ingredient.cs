using UnityEngine;
using System.Collections;

public class Ingredient : MonoBehaviour {

	public string ingredientName;
	public GameObject ingredientPrefab;
	public float growupTime;
    public GameObject lightPart;

	private WeightedInventory inventory;
	private ShowMessage showMessage;
	private PlayerStats playerStats;
    private PlayerSkillsAndAttributes pcSkills;

	private MeshRenderer mesh;
	private float growUpTimer;

	private bool addToInventoryOnce;

	void Awake() {
		if (GameObject.Find ("_UICanvasGame") != null) {
			inventory = GameObject.Find ("_UICanvasGame").GetComponentInChildren<WeightedInventory> ();
			showMessage = GameObject.Find ("_UICanvasGame").GetComponentInChildren<ShowMessage> ();
		}
		playerStats = GameObject.Find("[Player]").GetComponent<PlayerStats>();
        pcSkills = GameObject.Find("[Player]").GetComponent<PlayerSkillsAndAttributes>();
        mesh = GetComponent<MeshRenderer>();

        if (pcSkills.focus >= 25f) {
            lightPart.SetActive(true);
        }
        else {
            lightPart.SetActive(false);
        }
    }

	void LateUpdate() {
		if(growUpTimer > 0) {
			growUpTimer -= Time.deltaTime;

            if(lightPart.activeSelf == true) {
                lightPart.SetActive(false);
            }
        }
		else {
			if(mesh.enabled == false) {
				gameObject.tag = "Ingredient";
				mesh.enabled = true;
            }

            if (pcSkills.focus >= 25f) {
                lightPart.SetActive(true);
            }
            else {
                lightPart.SetActive(false);
            }
        }


	}

	public void PickupIngredient() {

		int weightDifference = playerStats.maxCarryWeight - playerStats.currentWeight;
		
		if(ingredientPrefab.GetComponent<Item>().itemWeight <= weightDifference) {
			/*
			if(ingredientPrefab.GetComponent<Item>().maxStackSize > 1) {
				foreach(GameObject i in inventory.items.ToArray()) {
					if(i.GetComponent<Item>().itemName == ingredientPrefab.GetComponent<Item>().itemName) {
						inventory.playerStats.currentWeightF += i.GetComponent<Item>().itemWeight;
						i.GetComponent<Item>().currentStackSize++;
					}
					else {
						GameObject tempItem = Instantiate(ingredientPrefab, transform.position, Quaternion.identity) as GameObject;
						inventory.AddItemToInventory(tempItem);
						tempItem.GetComponent<Rigidbody>().useGravity = false;
						mesh.enabled = false;
					}
				}
			}
			else {
				GameObject tempItem = Instantiate(ingredientPrefab, transform.position, Quaternion.identity) as GameObject;
				inventory.AddItemToInventory(tempItem);
				tempItem.GetComponent<Rigidbody>().useGravity = false;
				mesh.enabled = false;
			}
			*/
			GameObject tempItem = Instantiate(ingredientPrefab, transform.position, Quaternion.identity) as GameObject;
			inventory.AddItemToInventory(tempItem);
			tempItem.GetComponent<Rigidbody>().useGravity = false;
			mesh.enabled = false;

			gameObject.tag = "Untagged";
			growUpTimer = growupTime;
		}
		else {
			showMessage.SendTheMessage("Inventory is full, I can not carry any more.");
		}
	}
}
