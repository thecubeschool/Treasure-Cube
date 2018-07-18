using UnityEngine;
using System.Collections;

public class EquipmentManager : MonoBehaviour {
	
	public bool weaponDrawn = false;
	public bool torchDrawn = false;
	
	private PlayerSkillsAndAttributes skillsAttributes;
	private UIManager uiManager;

	public int weaponDamage;
	public int shieldScore;
	public int armorScore;
	private int armorTmp;
	public int missileDamage;

	public GameObject weaponSlot;
	public Slot weaponSlott;
	public GameObject shieldSlot;
	public Slot shieldSlott;
	public GameObject torchSlot;
	public Slot torchSlott;
	public GameObject headSlot;
	public Slot headSlott;
	public GameObject chestSlot;
	public Slot chestSlott;
	public GameObject necklaceSlot;
	public Slot necklaceSlott;
	public GameObject missileSlot;
	public Slot missileSlott;
	
	public bool weapon = false;
	public bool shield = false;
	public bool torch = false;
	public bool head = false;
	public bool chest = false;
	public bool necklace = false;
	public bool missile = false;
	public int missileCount;

	///void Awake() {
	///	DisableEquipment();
	///}

	void Start() {
		skillsAttributes = GameObject.Find("[Player]").GetComponent<PlayerSkillsAndAttributes>();
		if (GameObject.Find ("_UICanvasGame") != null) {
			uiManager = GameObject.Find ("_UICanvasGame").GetComponent<UIManager> ();
		}
	}

	void Update() {

		if (uiManager != null) {
			if (Input.GetButtonDown("DrawWeapon")) {
				if (weapon == true && !uiManager.alchemyUI.activeSelf && !uiManager.bookUI.activeSelf && !uiManager.inventoryUI.activeSelf &&
				  	!uiManager.consoleUI.activeSelf && !uiManager.dialogUI.activeSelf && !uiManager.pauseUI.activeSelf && !uiManager.specialTopicerUI.activeSelf &&
				  	!uiManager.storeUI.activeSelf && !uiManager.waiterUI.activeSelf && !uiManager.questPopupWindow.activeSelf) {
					weaponDrawn = !weaponDrawn;
				}
			}
		}

		if(weapon == false) {
			weaponDrawn = false;
		}

		if (torch == true) {
			torchDrawn = true;
		} 
		else {
			torchDrawn = false;
		}

		if(weaponSlot != null) {
			if(skillsAttributes.melee < 25) {
				float tmpDamageBase = (weaponSlot.GetComponent<Item>().powerScore * 0.8f) + (skillsAttributes.body * 0.125f);
				weaponDamage = (int)tmpDamageBase;
			}
			else if(skillsAttributes.melee >= 25 && skillsAttributes.melee < 50) {
				float tmpDamageBase = (weaponSlot.GetComponent<Item>().powerScore * 0.8f) + (skillsAttributes.body * 0.125f);
				float tmpDamagePercent = tmpDamageBase * 0.15f;
				weaponDamage = (int)tmpDamageBase + (int)tmpDamagePercent;
			}
			else if(skillsAttributes.melee >= 50) {
				float tmpDamageBase = (weaponSlot.GetComponent<Item>().powerScore * 0.8f) + (skillsAttributes.body * 0.125f);
				float tmpDamagePercent = tmpDamageBase * 0.25f;
				weaponDamage = (int)tmpDamageBase + (int)tmpDamagePercent;
			}
		
			if (torch == true && torchDrawn == true) {
				if(torchSlot != null) {
					if(torchSlot.GetComponent<Item>().itemName.Contains("Torch")) {
						torchSlot.GetComponent<Item>().itemGO.SetActive(true);
						if(torchSlot.gameObject.activeSelf == true) {
							torchSlot.GetComponent<Item>().itemGO.GetComponentInChildren<Light>().enabled = true;
						}

					}
				}
			}
			else {
				if(torchSlot != null) {
					if(torchSlot.GetComponent<Item>().itemName.Contains("Torch")) {
						torchSlot.GetComponent<Item>().itemGO.SetActive(false);
						if(torchSlot.gameObject.activeSelf == true) {
							torchSlot.GetComponent<Item>().itemGO.GetComponentInChildren<Light>().enabled = false;
						}
					}
				}
			}
		}
		else {
			weaponDamage = 0;
		}

		if(shieldSlot != null) {
			shieldScore = shieldSlot.GetComponent<Item>().powerScore;
		}
		else {
			shieldScore = 0;
		}

		if(missileSlot != null) {
			if(weaponSlot != null) {
				missileDamage = missileSlot.GetComponent<Item>().powerScore + weaponDamage;
			}
		}
		else {
			missileDamage = 0;
		}

		if(chestSlot != null && headSlot != null && shieldSlot != null) {
			if(chestSlot.GetComponent<Item>().armorType == headSlot.GetComponent<Item>().armorType) {
				if(chestSlot.GetComponent<Item>().armorType == ArmorType.LightArmor) {
					if(skillsAttributes.lightArmor < 25) {
						int armorSkill = skillsAttributes.lightArmor;
						float armorExtra = (chestSlot.GetComponent<Item>().powerScore + headSlot.GetComponent<Item>().powerScore + 
						                    shieldSlot.GetComponent<Item>().powerScore) * ((armorSkill / 200f) + 2f);
						armorScore = (int)armorExtra;
					}
					else if(skillsAttributes.lightArmor >= 25 && skillsAttributes.lightArmor < 80) {
						int armorSkill = skillsAttributes.lightArmor;
						float armorExtra = (chestSlot.GetComponent<Item>().powerScore + headSlot.GetComponent<Item>().powerScore + 
						                    shieldSlot.GetComponent<Item>().powerScore) * ((armorSkill / 200f) + 2f);
						float eightPercent = armorExtra * 0.08f;
						float calculatedAS = ((int)armorExtra) + (int)eightPercent;
						armorScore = (int)calculatedAS;
					}
					else if(skillsAttributes.lightArmor >= 80) {
						int armorSkill = skillsAttributes.lightArmor;
						float armorExtra = (chestSlot.GetComponent<Item>().powerScore + headSlot.GetComponent<Item>().powerScore + 
						                    shieldSlot.GetComponent<Item>().powerScore) * ((armorSkill / 200f) + 2f);
						float fortyPercent = armorExtra * 0.4f;
						float calculatedAS = ((int)armorExtra) + (int)fortyPercent;
						armorScore = (int)calculatedAS;
					}
				}
				else if(chestSlot.GetComponent<Item>().armorType == ArmorType.HeavyArmor) {
					if(skillsAttributes.heavyArmor < 25) {
						int armorSkill = skillsAttributes.heavyArmor;
						float armorExtra = (chestSlot.GetComponent<Item>().powerScore + headSlot.GetComponent<Item>().powerScore + 
						                    shieldSlot.GetComponent<Item>().powerScore) * ((armorSkill / 200f) + 2f);
						armorScore = (int)armorExtra;
					}
					else if(skillsAttributes.heavyArmor >= 25 && skillsAttributes.heavyArmor < 80) {
						int armorSkill = skillsAttributes.heavyArmor;
						float armorExtra = (chestSlot.GetComponent<Item>().powerScore + headSlot.GetComponent<Item>().powerScore + 
						                    shieldSlot.GetComponent<Item>().powerScore) * ((armorSkill / 200f) + 2f);
						float eightPercent = armorExtra * 0.15f;
						float calculatedAS = ((int)armorExtra) + (int)eightPercent;
						armorScore = (int)calculatedAS;
					}
				}
			}
			else {
				float tmp  = (chestSlot.GetComponent<Item>().powerScore + headSlot.GetComponent<Item>().powerScore + 
				              shieldSlot.GetComponent<Item>().powerScore) * 1.5f;
				armorScore = (int)tmp;
			}
		}
		else if(chestSlot != null && headSlot == null && shieldSlot != null) {
			armorScore = chestSlot.GetComponent<Item>().powerScore + shieldSlot.GetComponent<Item>().powerScore;
		}
		else if(chestSlot != null && headSlot == null && shieldSlot == null) {
			armorScore = chestSlot.GetComponent<Item>().powerScore;
		}
		else if(chestSlot != null && headSlot != null && shieldSlot == null) {
			armorScore = chestSlot.GetComponent<Item>().powerScore + headSlot.GetComponent<Item>().powerScore;
		}
		else if(chestSlot == null && headSlot != null && shieldSlot != null) {
			armorScore = headSlot.GetComponent<Item>().powerScore + shieldSlot.GetComponent<Item>().powerScore;
		}
		else if(chestSlot == null && headSlot != null && shieldSlot == null) {
			armorScore = headSlot.GetComponent<Item>().powerScore;
		}
		else if(chestSlot == null && headSlot == null && shieldSlot != null) {
			armorScore = shieldSlot.GetComponent<Item>().powerScore;
		}
		else {
			armorScore = 0;
		}

		if(missileSlot != null) {
			//missileCount = missileSlot.itemsInStack.Count;
		}
	}

	void DisableEquipment() {		
		weapon = false;
		shield = false;
		torch = false;
		head = false;
		chest = false;
		necklace = false;
		missile = false;
	}
}
