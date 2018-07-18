using UnityEngine;
using System.Collections;

public class StartingGear : MonoBehaviour {

    [HideInInspector]
	public PlayerStats ps;
    [HideInInspector]
    public GameManager gm;
    [HideInInspector]
    public WeightedInventory wi;

	private bool gotHat;
	private bool gotOutfit;
	private bool gotWeapon;
	private bool gotWeapon1;
	private bool gotShield;
	private bool gotMissile;

	public bool firstStartup = false;

    void Start() {

        ps = GameObject.FindObjectOfType<PlayerStats>();
        gm = GameObject.FindObjectOfType<GameManager>();
        wi = GameObject.FindObjectOfType<UIManager>().inventoryUI.GetComponent<WeightedInventory>();
    }

    private void Update() {
		if (firstStartup == true) {
			if (ps.playerProfession == CharacterProfession.Warrior) {
				foreach (GameObject item in gm.allGameItems) {
					if (item.name == "ArmorIron") {
						if (gotOutfit == false) {
							GameObject itemTmp = Instantiate (item, Vector3.zero, Quaternion.identity) as GameObject;
							itemTmp.GetComponent<Rigidbody> ().useGravity = false;
							wi.AddItemToInventory (itemTmp);
							wi.EquipItemFromInventory (itemTmp.GetComponent<Item> ());
							gotOutfit = true;
						}
					}
					if (item.name == "SwordIron") {
						if (gotWeapon == false) {
							GameObject itemTmp = Instantiate (item, Vector3.zero, Quaternion.identity) as GameObject;
							itemTmp.GetComponent<Rigidbody> ().useGravity = false;
							wi.AddItemToInventory (itemTmp);
							wi.EquipItemFromInventory (itemTmp.GetComponent<Item> ());
							gotWeapon = true;
						}
					}
					if (item.name == "ShieldWooden") {
						if (gotShield == false) {
							GameObject itemTmp = Instantiate (item, Vector3.zero, Quaternion.identity) as GameObject;
							itemTmp.GetComponent<Rigidbody> ().useGravity = false;
							wi.AddItemToInventory (itemTmp);
							wi.EquipItemFromInventory (itemTmp.GetComponent<Item> ());
							gotShield = true;
						}
					}
					ps.playerMoney = 15;
				}
			}
			if (ps.playerProfession == CharacterProfession.Knight) {
				foreach (GameObject item in gm.allGameItems) {
					if (item.name == "ClothesCoat") {
						if (gotOutfit == false) {
							GameObject itemTmp = Instantiate (item, Vector3.zero, Quaternion.identity) as GameObject;
							itemTmp.GetComponent<Rigidbody> ().useGravity = false;
							wi.AddItemToInventory (itemTmp);
							wi.EquipItemFromInventory (itemTmp.GetComponent<Item> ());
							gotOutfit = true;
						}
					}
					if (item.name == "SwordIron") {
						if (gotWeapon == false) {
							GameObject itemTmp = Instantiate (item, Vector3.zero, Quaternion.identity) as GameObject;
							itemTmp.GetComponent<Rigidbody> ().useGravity = false;
							wi.AddItemToInventory (itemTmp);
							wi.EquipItemFromInventory (itemTmp.GetComponent<Item> ());
							gotWeapon = true;
						}
					}
					if (item.name == "MaceIron") {
						if (gotWeapon1 == false) {
							GameObject itemTmp = Instantiate (item, Vector3.zero, Quaternion.identity) as GameObject;
							itemTmp.GetComponent<Rigidbody> ().useGravity = false;
							wi.AddItemToInventory (itemTmp);
							gotWeapon1 = true;
						}
					}
					ps.playerMoney = 25;
				}	
			}
			if (ps.playerProfession == CharacterProfession.Cleric) {
				foreach (GameObject item in gm.allGameItems) {
					if (item.name == "ClothesBlackRobe") {
						if (gotOutfit == false) {
							GameObject itemTmp = Instantiate (item, Vector3.zero, Quaternion.identity) as GameObject;
							itemTmp.GetComponent<Rigidbody> ().useGravity = false;
							wi.AddItemToInventory (itemTmp);
							wi.EquipItemFromInventory (itemTmp.GetComponent<Item> ());
							gotOutfit = true;
						}
					}
					if (item.name == "MaceIron") {
						if (gotWeapon == false) {
							GameObject itemTmp = Instantiate (item, Vector3.zero, Quaternion.identity) as GameObject;
							itemTmp.GetComponent<Rigidbody> ().useGravity = false;
							wi.AddItemToInventory (itemTmp);
							wi.EquipItemFromInventory (itemTmp.GetComponent<Item> ());
							gotWeapon = true;
						}
					}
					if (item.name == "CraftingMortarAndPestle") {
						if (gotWeapon1 == false) {
							GameObject itemTmp = Instantiate (item, Vector3.zero, Quaternion.identity) as GameObject;
							itemTmp.GetComponent<Rigidbody> ().useGravity = false;
							wi.AddItemToInventory (itemTmp);
							wi.EquipItemFromInventory (itemTmp.GetComponent<Item> ());
							gotWeapon1 = true;
						}
					}
					ps.playerMoney = 10;
				}		
			}
			if (ps.playerProfession == CharacterProfession.Hunter) {
				foreach (GameObject item in gm.allGameItems) {
					if (item.name == "ClothesGreen") {
						if (gotOutfit == false) {
							GameObject itemTmp = Instantiate (item, Vector3.zero, Quaternion.identity) as GameObject;
							itemTmp.GetComponent<Rigidbody> ().useGravity = false;
							wi.AddItemToInventory (itemTmp);
							wi.EquipItemFromInventory (itemTmp.GetComponent<Item> ());
							gotOutfit = true;
						}
					}
					if (item.name == "AxeIron") {
						if (gotWeapon == false) {
							GameObject itemTmp = Instantiate (item, Vector3.zero, Quaternion.identity) as GameObject;
							itemTmp.GetComponent<Rigidbody> ().useGravity = false;
							wi.AddItemToInventory (itemTmp);
							gotWeapon = true;
						}
					}
					if (item.name == "BowWooden") {
						if (gotWeapon1 == false) {
							GameObject itemTmp = Instantiate (item, Vector3.zero, Quaternion.identity) as GameObject;
							itemTmp.GetComponent<Rigidbody> ().useGravity = false;
							wi.AddItemToInventory (itemTmp);
							wi.EquipItemFromInventory (itemTmp.GetComponent<Item> ());
							gotWeapon1 = true;
						}
					}
					if (item.name == "ArrowIron") {
						if (gotMissile == false) {
							GameObject itemTmp = Instantiate (item, Vector3.zero, Quaternion.identity) as GameObject;
							itemTmp.GetComponent<Rigidbody> ().useGravity = false;
							itemTmp.GetComponent<Item> ().currentStackSize = 15;
							wi.AddItemToInventory (itemTmp);
							gotMissile = true;
						}
					}
					ps.playerMoney = 10;
				}			
			}
			if (ps.playerProfession == CharacterProfession.Agent) {
				foreach (GameObject item in gm.allGameItems) {
					if (item.name == "ClothesCoat") {
						if (gotOutfit == false) {
							GameObject itemTmp = Instantiate (item, Vector3.zero, Quaternion.identity) as GameObject;
							itemTmp.GetComponent<Rigidbody> ().useGravity = false;
							wi.AddItemToInventory (itemTmp);
							wi.EquipItemFromInventory (itemTmp.GetComponent<Item> ());
							gotOutfit = true;
						}
					}
					if (item.name == "SwordIron") {
						if (gotWeapon == false) {
							GameObject itemTmp = Instantiate (item, Vector3.zero, Quaternion.identity) as GameObject;
							itemTmp.GetComponent<Rigidbody> ().useGravity = false;
							wi.AddItemToInventory (itemTmp);
							wi.EquipItemFromInventory (itemTmp.GetComponent<Item> ());
							gotWeapon = true;
						}
					}
					if (item.name == "BowWooden") {
						if (gotWeapon == false) {
							GameObject itemTmp = Instantiate (item, Vector3.zero, Quaternion.identity) as GameObject;
							itemTmp.GetComponent<Rigidbody> ().useGravity = false;
							wi.AddItemToInventory (itemTmp);
							gotWeapon = true;
						}
					}
					if (item.name == "ArrowIron") {
						if (gotMissile == false) {
							GameObject itemTmp = Instantiate (item, Vector3.zero, Quaternion.identity) as GameObject;
							itemTmp.GetComponent<Rigidbody> ().useGravity = false;
							itemTmp.GetComponent<Item> ().currentStackSize = 15;
							wi.AddItemToInventory (itemTmp);
							gotMissile = true;
						}
					}
					ps.playerMoney = 25;
				}
			}
			if (ps.playerProfession == CharacterProfession.Ranger) {
				foreach (GameObject item in gm.allGameItems) {
					if (item.name == "ClothesRed") {
						if (gotOutfit == false) {
							GameObject itemTmp = Instantiate (item, Vector3.zero, Quaternion.identity) as GameObject;
							itemTmp.GetComponent<Rigidbody> ().useGravity = false;
							wi.AddItemToInventory (itemTmp);
							wi.EquipItemFromInventory (itemTmp.GetComponent<Item> ());
							gotOutfit = true;
						}
					}
					if (item.name == "SwordIron") {
						if (gotWeapon == false) {
							GameObject itemTmp = Instantiate (item, Vector3.zero, Quaternion.identity) as GameObject;
							itemTmp.GetComponent<Rigidbody> ().useGravity = false;
							wi.AddItemToInventory (itemTmp);
							gotWeapon = true;
						}
					}
					if (item.name == "BowWooden") {
						if (gotWeapon == false) {
							GameObject itemTmp = Instantiate (item, Vector3.zero, Quaternion.identity) as GameObject;
							itemTmp.GetComponent<Rigidbody> ().useGravity = false;
							wi.AddItemToInventory (itemTmp);
							wi.EquipItemFromInventory (itemTmp.GetComponent<Item> ());
							gotWeapon = true;
						}
					}
					if (item.name == "ArrowIron") {
						if (gotMissile == false) {
							GameObject itemTmp = Instantiate (item, Vector3.zero, Quaternion.identity) as GameObject;
							itemTmp.GetComponent<Rigidbody> ().useGravity = false;
							itemTmp.GetComponent<Item> ().currentStackSize = 10;
							wi.AddItemToInventory (itemTmp);
							gotMissile = true;
						}
					}
					ps.playerMoney = 10;
				}
			}
			StartCoroutine(DestroyAfterSeconds());
		}
	}

	IEnumerator DestroyAfterSeconds() {
		yield return new WaitForSeconds(6f);
		Destroy(this);
	}
}
