using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerClickHandler {

	public Sprite slotNormal;
	public Sprite slotEquiped;

	public Image slotIcon;
	public Text stackText;
	public Text nameText;
	private int healthInt;
	public Text healthText;
	public Text powerText;
	public Text valueText;
	public Text weightText;

	public bool wearingThisItem = false;

	[HideInInspector]
	public GameObject itemsGo;

	private EquipmentManager equipManager;
	private PlayerSkillsAndAttributes skillsAttributes;
	private WeaponRangedAnimate wepRange;
	private UIManager uiManager;
	private Interact interact;

	public Item itemInSlot;

	public WeightedInventory inventory;
	
	void Start() {
		equipManager = GameObject.Find("[Player]").GetComponentInChildren<EquipmentManager>();
		skillsAttributes = GameObject.Find("[Player]").GetComponent<PlayerSkillsAndAttributes>();
		wepRange = GameObject.Find("[Player]").GetComponentInChildren<WeaponRangedAnimate>();
		uiManager = GameObject.Find("_UICanvasGame").GetComponent<UIManager>();
		interact = GameObject.Find("[Player]").GetComponent<Interact>();

		RectTransform slotRect = GetComponent<RectTransform>();
		RectTransform txtRect = stackText.GetComponent<RectTransform>();

		int txtScaleFactor = (int)slotRect.sizeDelta.x * (int)0.60;
		stackText.resizeTextMaxSize = txtScaleFactor;
		stackText.resizeTextMinSize = txtScaleFactor;

		txtRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotRect.sizeDelta.x);
		txtRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotRect.sizeDelta.y);	
	}

	public void UpdateThisSlot() {
		if(itemInSlot.type == ItemType.Weapon) {
			if(equipManager.weaponSlot != itemInSlot.gameObject) { //If current weapon equipped is not itemInSlot.gameObject one, we want to disable its gameobject.
				itemInSlot.itemGO.SetActive(false);
			}

			float tmpWeaponHealthF = itemInSlot.health;
			int tmpWeaponHealthI = (int)tmpWeaponHealthF;
			healthText.text = tmpWeaponHealthI.ToString();
			
			//float tmpWeaponDamageF = (itemInSlot.powerScore + (skillsAttributes.melee/10) + tmpWeaponHealthF * 0.03f);
			float tmpWeaponDamageF = ((itemInSlot.powerScore * 0.15f) + (skillsAttributes.melee/10)) + (tmpWeaponHealthF * 0.03f); 
			int tmpWeaponDamageI = (int)tmpWeaponDamageF;
			powerText.text = tmpWeaponDamageI.ToString();
			
			//float tmpWeaponValueF = ((tmpWeaponDamageF * 0.06f) * tmpWeaponHealthF);
			//int tmpWeaponValueI = (int)tmpWeaponValueF;
			//valueText.text = tmpWeaponValueI.ToString();
		}
		else if(itemInSlot.type == ItemType.Shield) {
			if(equipManager.shieldSlot != itemInSlot.gameObject) { //If current weapon equipped is not itemInSlot.gameObject one, we want to disable its gameobject.
				itemInSlot.itemGO.SetActive(false);
			}

			float tmpShieldHealthF = itemInSlot.health;
			int tmpShieldHealthI = (int)tmpShieldHealthF;
			healthText.text = tmpShieldHealthI.ToString();
			
			float tmpShieldBlockingF = ((itemInSlot.powerScore * 0.15f) + (skillsAttributes.block/10)) + (tmpShieldHealthF * 0.03f); 
			int tmpShieldBlockingI = (int)tmpShieldBlockingF;
			powerText.text = tmpShieldBlockingI.ToString();
			
			//float tmpShieldValueF = ((tmpShieldBlockingF * 0.06f) * tmpShieldHealthF);
			//int tmpShieldValueI = (int)tmpShieldValueF;
			//valueText.text = tmpShieldValueI.ToString();
		}
		else if(itemInSlot.type == ItemType.Helmet) {
			if(itemInSlot.armorType == ArmorType.LightArmor) {
				float tmpHelmHealthF = itemInSlot.health;
				int tmpHelmHealthI = (int)tmpHelmHealthF;
				healthText.text = tmpHelmHealthI.ToString();

				float tmpHelmArmorF = ((itemInSlot.powerScore * 0.15f) + (skillsAttributes.lightArmor/10)) + (tmpHelmHealthF * 0.03f); 
				int tmpHelmArmorI = (int)tmpHelmArmorF;
				powerText.text = tmpHelmArmorI.ToString();

				//float tmpHelmValueF = ((tmpHelmArmorF * 0.06f) * tmpHelmHealthF);
				//int tmpHelmValueI = (int)tmpHelmValueF;
				//valueText.text = tmpHelmValueI.ToString();
			}
			else {
				float tmpHelmHealthF = itemInSlot.health;
				int tmpHelmHealthI = (int)tmpHelmHealthF;
				healthText.text = tmpHelmHealthI.ToString();
				
				float tmpHelmArmorF = ((itemInSlot.powerScore * 0.15f) + (skillsAttributes.heavyArmor/10)) + (tmpHelmHealthF * 0.03f); 
				int tmpHelmArmorI = (int)tmpHelmArmorF;
				powerText.text = tmpHelmArmorI.ToString();
				
				//float tmpHelmValueF = ((tmpHelmArmorF * 0.06f) * tmpHelmHealthF);
				//int tmpHelmValueI = (int)tmpHelmValueF;
				//valueText.text = tmpHelmValueI.ToString();
			}

		}
		else if(itemInSlot.type == ItemType.Chest) {
			if(itemInSlot.armorType == ArmorType.LightArmor) {
				float tmpChestHealthF = itemInSlot.health;
				int tmpChestHealthI = (int)tmpChestHealthF;
				healthText.text = tmpChestHealthI.ToString();
				
				float tmpChestArmorF = ((itemInSlot.powerScore * 0.15f) + (skillsAttributes.lightArmor/10)) + (tmpChestHealthF * 0.03f); 
				int tmpChestArmorI = (int)tmpChestArmorF;
				powerText.text = tmpChestArmorI.ToString();
				
				//float tmpChestValueF = ((tmpChestArmorF * 0.06f) * tmpChestHealthF);
				//int tmpChestValueI = (int)tmpChestValueF;
				//valueText.text = tmpChestValueI.ToString();
			}
			else {
				float tmpChestHealthF = itemInSlot.health;
				int tmpChestHealthI = (int)tmpChestHealthF;
				healthText.text = tmpChestHealthI.ToString();
				
				float tmpChestArmorF = ((itemInSlot.powerScore * 0.15f) + (skillsAttributes.heavyArmor/10)) + (tmpChestHealthF * 0.03f); 
				int tmpChestArmorI = (int)tmpChestArmorF;
				powerText.text = tmpChestArmorI.ToString();
				
				//float tmpChestValueF = ((tmpChestArmorF * 0.06f) * tmpChestHealthF);
				//int tmpChestValueI = (int)tmpChestValueF;
				//valueText.text = tmpChestValueI.ToString();
			}
			
		}
		else if(itemInSlot.type == ItemType.Missile) {

			healthText.text = string.Empty;
			
			//float tmpMissileDamageF = (itemInSlot.powerScore + (skillsAttributes.melee/10) + tmpMissileHealthF * 0.03f);
			float tmpMissileDamageF = ((itemInSlot.powerScore * 0.15f) + (skillsAttributes.marksman/10)); 
			int tmpMissileDamageI = (int)tmpMissileDamageF;
			powerText.text = tmpMissileDamageI.ToString();
			
			//float tmpMissileValueF = ((tmpMissileDamageF * 0.06f));
			//int tmpMissileValueI = (int)tmpMissileValueF;
			//valueText.text = tmpMissileValueI.ToString();
		}
	}

	public void UseItem() {
		if(itemInSlot.type == ItemType.Weapon) { //If it is equipment we want it to stay in inventory	

			if(equipManager.weapon == false) { //If there is no weapon currently eqquiped

				wearingThisItem = true;
				equipManager.weaponSlot = itemInSlot.gameObject;
				equipManager.weaponSlott = this;
				equipManager.weapon = true;
				GetComponent<Image>().sprite = slotEquiped;
				itemInSlot.itemGO.SetActive(true);
                itemInSlot.equiped = true;


                if (itemInSlot.itemName.Contains("Bow")) {
					if(equipManager.missileSlot != null) {
						wepRange.FindTheArrowInUse();
					}
                    else {
                        Transform t = itemInSlot.itemGO.transform.Find("ArrowHolder").transform;
                        foreach (GameObject g in t) {
                            g.SetActive(false);
                        }
                    }

					if(equipManager.shieldSlot != null) {
						equipManager.shieldSlott.GetComponent<Image>().sprite = slotNormal;
						equipManager.shieldSlott.wearingThisItem = false;
						equipManager.shieldSlot.GetComponent<Item>().itemGO.SetActive(false);
						equipManager.shield = false;
						equipManager.shieldSlott = null;
						equipManager.shieldSlot = null;
					}
				}

				inventory.PlaySound(inventory.equipWeaponSFX);					
			}
			else if(equipManager.weapon == true) { //If on other hand there is a weapon currently eqquiped swap them

				if(equipManager.weaponSlot == itemInSlot.gameObject) {
					//print("This weapon is equipped. Unequiping now.");
					wearingThisItem = false;
					GetComponent<Image>().sprite = slotNormal;
					itemInSlot.itemGO.SetActive(false);
                    itemInSlot.equiped = false;
                    equipManager.weaponSlot = null;
					equipManager.weaponSlott = null;
					equipManager.weapon = false;
                    inventory.PlaySound(inventory.equipWeaponSFX);					
				}
				else if(equipManager.weaponSlot != itemInSlot.gameObject) {
					//print("This weapon is not equipped. Equiping now.");
					equipManager.weaponSlott.wearingThisItem = false;
					equipManager.weaponSlott.GetComponent<Image>().sprite = slotNormal;
					equipManager.weaponSlott.itemInSlot.itemGO.SetActive(false);
                    equipManager.weaponSlott.itemInSlot.equiped = false;
					equipManager.weaponDamage = 0;
					wearingThisItem = true;
					equipManager.weaponSlot = itemInSlot.gameObject;
					equipManager.weaponSlott = this;
                    itemInSlot.equiped = true;
                    GetComponent<Image>().sprite = slotEquiped;
					itemInSlot.itemGO.SetActive(true);

					if(itemInSlot.itemName.Contains("Bow")) {
						if(equipManager.missileSlot != null) {
							wepRange.FindTheArrowInUse();
						}
						
						if(equipManager.shieldSlot != null) {
							if(equipManager.shieldSlott != null) {
								equipManager.shieldSlott.GetComponent<Image>().sprite = slotNormal;
								equipManager.shieldSlott.wearingThisItem = false;
							}
							equipManager.shieldSlot.GetComponent<Item>().itemGO.SetActive(false);
							equipManager.shieldSlot = null;
							equipManager.shield = false;
						}
					}

					inventory.PlaySound(inventory.equipWeaponSFX);					
				}
			}
		}
		if(itemInSlot.type == ItemType.Shield) { //If it is equipment we want it to stay in inventory	
			
			if(equipManager.shield == false && equipManager.torch == false) { //If there is no shield currently eqquiped
				
				wearingThisItem = true;
				equipManager.shieldSlot = itemInSlot.gameObject;
				equipManager.shieldSlott = this;
				equipManager.shield = true;
                itemInSlot.equiped = true;
                GetComponent<Image>().sprite = slotEquiped;
				itemInSlot.itemGO.SetActive(true);

				inventory.PlaySound(inventory.equipArmorSFX);					
			}
			else if(equipManager.shield == true && equipManager.torch == false) { //If on other hand there is a shield currently eqquiped swap them
				
				if(equipManager.shieldSlot == itemInSlot.gameObject) {
					//print("This shield is equipped. Unequiping now.");
					wearingThisItem = false;
					GetComponent<Image>().sprite = slotNormal;
					itemInSlot.itemGO.SetActive(false);
                    itemInSlot.equiped = false;
                    equipManager.shieldSlot = null;
					equipManager.shieldSlott = null;
					equipManager.shield = false;
                    inventory.PlaySound(inventory.equipArmorSFX);
				}
				else if(equipManager.shieldSlot != itemInSlot.gameObject) {
					//print("This shield is not equipped. Equiping now.");
					equipManager.shieldSlott.wearingThisItem = false;
					equipManager.shieldSlott.GetComponent<Image>().sprite = slotNormal;
					equipManager.shieldSlott.itemInSlot.itemGO.SetActive(false);
                    equipManager.shieldSlott.itemInSlot.equiped = false;
                    wearingThisItem = true;
					equipManager.shieldSlot = itemInSlot.gameObject;
					equipManager.shieldSlott = this;
                    itemInSlot.equiped = true;
                    GetComponent<Image>().sprite = slotEquiped;
					itemInSlot.itemGO.SetActive(true);

					if(equipManager.weaponSlot != null) {
						if(equipManager.weaponSlot.GetComponent<Item>().itemName.Contains("Bow")) {
							equipManager.weaponSlott.GetComponent<Image>().sprite = slotNormal;
							equipManager.weaponSlott.wearingThisItem = false;
							equipManager.weaponSlott.GetComponent<Item>().itemGO.SetActive(false);
							equipManager.weaponSlott = null;
							equipManager.weapon = false;
						}
					}

					inventory.PlaySound(inventory.equipArmorSFX);				
				}
			}
			else if(equipManager.shield == false && equipManager.torch == true) {
				equipManager.torchSlott.wearingThisItem = false;
                equipManager.torchSlott.itemInSlot.equiped = false;
                equipManager.torchSlott.GetComponent<Image>().sprite = slotNormal;
				equipManager.torchSlott.itemInSlot.itemGO.SetActive(false);
				equipManager.torchSlot = null;
				equipManager.torchSlott = null;
				equipManager.torch = false;

				wearingThisItem = true;
				equipManager.shieldSlot = itemInSlot.gameObject;
				equipManager.shieldSlott = this;
				GetComponent<Image>().sprite = slotEquiped;
				itemInSlot.itemGO.SetActive(true);
				equipManager.shield = true;
                itemInSlot.enabled = true;
				inventory.PlaySound(inventory.equipWeaponSFX);
			}
		}
		if(itemInSlot.type == ItemType.Chest) { //If it is equipment we want it to stay in inventory	
			
			if(equipManager.chest == false) { //If there is no weapon currently eqquiped
				
				wearingThisItem = true;
				equipManager.chestSlot = itemInSlot.gameObject;
				equipManager.chestSlott = this;
				equipManager.chest = true;
                itemInSlot.equiped = true;
                GetComponent<Image>().sprite = slotEquiped;
				inventory.PlaySound(inventory.equipArmorSFX);					
			}
			else if(equipManager.chest == true) { //If on other hand there is a weapon currently eqquiped swap them
				
				if(equipManager.chestSlot == itemInSlot.gameObject) {
					//print("This armor is equipped. Unequiping now.");
					wearingThisItem = false;
					GetComponent<Image>().sprite = slotNormal;
                    itemInSlot.equiped = false;
                    equipManager.chestSlot = null;
					equipManager.chestSlott = null;
					equipManager.chest = false;
					inventory.PlaySound(inventory.equipArmorSFX);					
				}
				else if(equipManager.chestSlot != itemInSlot.gameObject) {
					//print("This armor is not equipped. Equiping now.");
					equipManager.chestSlott.wearingThisItem = false;
					equipManager.chestSlott.GetComponent<Image>().sprite = slotNormal;
                    equipManager.chestSlott.itemInSlot.equiped = false;

                    wearingThisItem = true;
					equipManager.chestSlot = itemInSlot.gameObject;
					equipManager.chestSlott = this;
					GetComponent<Image>().sprite = slotEquiped;
					inventory.PlaySound(inventory.equipArmorSFX);					
				}
			}
		}
		if(itemInSlot.type == ItemType.Helmet) { //If it is equipment we want it to stay in inventory	
			
			if(equipManager.head == false) { //If there is no helmry currently eqquiped
				
				wearingThisItem = true;
				equipManager.headSlot = itemInSlot.gameObject;
				equipManager.headSlott = this;
				equipManager.head = true;
                itemInSlot.equiped = true;
                GetComponent<Image>().sprite = slotEquiped;
				inventory.PlaySound(inventory.equipArmorSFX);					
			}
			else if(equipManager.head == true) { //If on other hand there is a weapon currently eqquiped swap them
				
				if(equipManager.headSlot == itemInSlot.gameObject) {
					//print("This helmet is equipped. Unequiping now.");
					wearingThisItem = false;
					GetComponent<Image>().sprite = slotNormal;
                    itemInSlot.equiped = false;
                    equipManager.headSlot = null;
					equipManager.headSlott = null;
					equipManager.head = false;
					inventory.PlaySound(inventory.equipArmorSFX);
				}
				else if(equipManager.headSlot != itemInSlot.gameObject) {
					//print("This weapon is not equipped. Equiping now.");
					equipManager.headSlott.wearingThisItem = false;
					equipManager.headSlott.GetComponent<Image>().sprite = slotNormal;
                    equipManager.chestSlott.itemInSlot.equiped = false;

                    wearingThisItem = true;
					equipManager.headSlot = itemInSlot.gameObject;
					equipManager.headSlott = this;
					GetComponent<Image>().sprite = slotEquiped;
					inventory.PlaySound(inventory.equipArmorSFX);					
				}
			}
		}
		if(itemInSlot.type == ItemType.Necklace) { //If it is equipment we want it to stay in inventory	
			
			if(equipManager.necklace == false) { //If there is no necklace currently eqquiped

				wearingThisItem = true;
				equipManager.necklaceSlot = itemInSlot.gameObject;
				equipManager.necklaceSlott = this;
				equipManager.necklace = true;
                itemInSlot.equiped = true;
                GetComponent<Image>().sprite = slotEquiped;
				inventory.PlaySound(inventory.equipArmorSFX);					
			}
			else if(equipManager.necklace == true) { //If on other hand there is a necklace currently eqquiped swap them
				
				if(equipManager.necklaceSlot == itemInSlot.gameObject) {
					//print("This necklace is equipped. Unequiping now.");
					wearingThisItem = false;
					GetComponent<Image>().sprite = slotNormal;
                    itemInSlot.equiped = false;
                    equipManager.necklaceSlot = null;
					equipManager.necklaceSlott = null;
					equipManager.necklace = false;
					inventory.PlaySound(inventory.equipArmorSFX);
				}
				else if(equipManager.necklaceSlot != itemInSlot.gameObject) {
					//print("This necklace is not equipped. Equiping now.");
					equipManager.necklaceSlott.wearingThisItem = false;
					equipManager.necklaceSlott.GetComponent<Image>().sprite = slotNormal;
                    equipManager.chestSlott.itemInSlot.equiped = false;

                    wearingThisItem = true;
					equipManager.necklaceSlot = itemInSlot.gameObject;
					equipManager.necklaceSlott = this;
					GetComponent<Image>().sprite = slotEquiped;
					inventory.PlaySound(inventory.equipArmorSFX);					
				}
			}
		}
		if(itemInSlot.type == ItemType.Missile) { //If it is equipment we want it to stay in inventory	
			
			if(equipManager.missile == false) { //If there is no shield currently eqquiped

				wearingThisItem = true;
				equipManager.missileSlot = itemInSlot.gameObject;
				equipManager.missileSlott = this;
				equipManager.missile = true;
                itemInSlot.equiped = true;
                GetComponent<Image>().sprite = slotEquiped;
				inventory.PlaySound(inventory.equipWeaponSFX);					
				
				if(equipManager.weaponSlot != null) {
					wepRange.FindTheArrowInUse();
				}
			}
			else if(equipManager.missile == true) { //If on other hand there is a shield currently eqquiped swap them
				
				if(equipManager.missileSlot == itemInSlot.gameObject) {
					//print("This shield is equipped. Unequiping now.");
					wearingThisItem = false;
					GetComponent<Image>().sprite = slotNormal;
                    itemInSlot.equiped = false;
                    equipManager.missileSlot = null;
					equipManager.missileSlott = null;
					equipManager.missile = false;
					inventory.PlaySound(inventory.equipWeaponSFX);
				}
				else if(equipManager.missileSlot != itemInSlot.gameObject) {
					//print("This shield is not equipped. Equiping now.");
					equipManager.missileSlott.wearingThisItem = false;
					equipManager.missileSlott.GetComponent<Image>().sprite = slotNormal;
                    equipManager.chestSlott.itemInSlot.equiped = false;

                    wearingThisItem = true;
					equipManager.missileSlot = itemInSlot.gameObject;
					equipManager.missileSlott = this;
					GetComponent<Image>().sprite = slotEquiped;
					inventory.PlaySound(inventory.equipWeaponSFX);					

					if(equipManager.weaponSlot != null) {
						wepRange.FindTheArrowInUse();
					}
				}
			}
		}
		if(itemInSlot.type == ItemType.Book) {
			uiManager.bookUI.SetActive(true);
			BookManager bm = itemInSlot.GetComponent<BookManager>();
			uiManager.bookUI.GetComponent<BookUi>().BookUpdateContent(bm);
			uiManager.SetActiveUIElement(uiManager.bookUI.transform.Find("Scrollbar").gameObject);
			inventory.PlaySound(inventory.readBookSFX);
			uiManager.inventoryUI.SetActive(false);
		}
		if(itemInSlot.type == ItemType.Potion) {
			if(itemInSlot.currentStackSize > 1) {
				inventory.PlaySound(inventory.drinkPotionSFX);
				if(itemInSlot.itemName.Contains("Regenerate")) {
					if(itemInSlot.itemName.Contains("Health")) {
						inventory.playerStats.currentWeightF -= itemInSlot.itemWeight;
						inventory.playerStats.currentHealth += itemInSlot.powerScore;
						itemInSlot.currentStackSize--;
						inventory.ResetInventory();
						if(inventory.tabActive == InventoryTabActive.TabAll) {
							inventory.SetUpTheInventory();
						}
						else if(inventory.tabActive == InventoryTabActive.TabPotions) {
							inventory.SUTIPotionsTab();
						}
					}
					else if(itemInSlot.itemName.Contains("Stamina")) {
						inventory.playerStats.currentWeightF -= itemInSlot.itemWeight;
						inventory.playerStats.currentStamina += itemInSlot.powerScore;
						itemInSlot.currentStackSize--;
						inventory.ResetInventory();
						if(inventory.tabActive == InventoryTabActive.TabAll) {
							inventory.SetUpTheInventory();
						}
						else if(inventory.tabActive == InventoryTabActive.TabPotions) {
							inventory.SUTIPotionsTab();
						}
					}
				}
				else if(itemInSlot.itemName.Contains("Permanent")) {
					if(itemInSlot.itemName.Contains("Health")) {
						inventory.playerStats.currentWeightF -= itemInSlot.itemWeight;
						inventory.playerStats.maxHealth += itemInSlot.powerScore;
						itemInSlot.currentStackSize--;
						inventory.ResetInventory();
						if(inventory.tabActive == InventoryTabActive.TabAll) {
							inventory.SetUpTheInventory();
						}
						else if(inventory.tabActive == InventoryTabActive.TabPotions) {
							inventory.SUTIPotionsTab();
						}
					}
					else if(itemInSlot.itemName.Contains("Stamina")) {
						inventory.playerStats.currentWeightF -= itemInSlot.itemWeight;
						inventory.playerStats.maxStamina += itemInSlot.powerScore;
						itemInSlot.currentStackSize--;
						inventory.ResetInventory();
						if(inventory.tabActive == InventoryTabActive.TabAll) {
							inventory.SetUpTheInventory();
						}
						else if(inventory.tabActive == InventoryTabActive.TabPotions) {
							inventory.SUTIPotionsTab();
						}
					}
				}
			}
			else {
				inventory.PlaySound(inventory.drinkPotionSFX);
				if(itemInSlot.itemName.Contains("Regenerate")) {
					if(itemInSlot.itemName.Contains("Health")) {
						inventory.playerStats.currentWeightF -= itemInSlot.itemWeight;
						inventory.playerStats.currentHealth += itemInSlot.powerScore;
						inventory.items.Remove(itemInSlot.gameObject);
						inventory.ResetInventory();
						if(inventory.tabActive == InventoryTabActive.TabAll) {
							inventory.SetUpTheInventory();
						}
						else if(inventory.tabActive == InventoryTabActive.TabPotions) {
							inventory.SUTIPotionsTab();
						}
						Destroy(itemInSlot.gameObject);
					}
					else if(itemInSlot.itemName.Contains("Stamina")) {
						inventory.playerStats.currentWeightF -= itemInSlot.itemWeight;
						inventory.playerStats.currentStamina += itemInSlot.powerScore;
						inventory.items.Remove(itemInSlot.gameObject);
						inventory.ResetInventory();
						if(inventory.tabActive == InventoryTabActive.TabAll) {
							inventory.SetUpTheInventory();
						}
						else if(inventory.tabActive == InventoryTabActive.TabPotions) {
							inventory.SUTIPotionsTab();
						}
						Destroy(itemInSlot.gameObject);
					}
				}
				else if(itemInSlot.itemName.Contains("Permanent")) {
					if(itemInSlot.itemName.Contains("Health")) {
						inventory.playerStats.currentWeightF -= itemInSlot.itemWeight;
						inventory.playerStats.maxHealth += itemInSlot.powerScore;
						inventory.items.Remove(itemInSlot.gameObject);
						inventory.ResetInventory();
						if(inventory.tabActive == InventoryTabActive.TabAll) {
							inventory.SetUpTheInventory();
						}
						else if(inventory.tabActive == InventoryTabActive.TabPotions) {
							inventory.SUTIPotionsTab();
						}
						Destroy(itemInSlot.gameObject);
					}
					else if(itemInSlot.itemName.Contains("Stamina")) {
						inventory.playerStats.currentWeightF -= itemInSlot.itemWeight;
						inventory.playerStats.maxStamina += itemInSlot.powerScore;
						inventory.items.Remove(itemInSlot.gameObject);
						inventory.ResetInventory();
						if(inventory.tabActive == InventoryTabActive.TabAll) {
							inventory.SetUpTheInventory();
						}
						else if(inventory.tabActive == InventoryTabActive.TabPotions) {
							inventory.SUTIPotionsTab();
						}
						Destroy(itemInSlot.gameObject);
					}
				}
			}
		}
		if(itemInSlot.type == ItemType.Torch) { //If it is equipment we want it to stay in inventory				
			if(equipManager.torch == false && equipManager.shield == false) { //If there is no torch currently eqquiped				
				wearingThisItem = true;
				equipManager.torchSlot = itemInSlot.gameObject;
				equipManager.torchSlott = this;
				equipManager.torch = true;
                itemInSlot.equiped = true;
                GetComponent<Image>().sprite = slotEquiped;
				inventory.PlaySound(inventory.equipTorchSFX);
				equipManager.torchDrawn = true;
			}
			else if(equipManager.torch == true && equipManager.shield == false) { //If on other hand there is a torch currently eqquiped swap them				
				if(equipManager.torchSlot == itemInSlot.gameObject) {
					//print("This torch is equipped. Unequiping now.");
					itemInSlot.itemGO.SetActive(false);
					wearingThisItem = false;
					GetComponent<Image>().sprite = slotNormal;
                    itemInSlot.equiped = false;
                    equipManager.torchSlot = null;
					equipManager.torchSlott = null;
					equipManager.torch = false;
					equipManager.torchDrawn = false;
					inventory.PlaySound(inventory.equipTorchSFX);
				}
				else if(equipManager.torchSlot != itemInSlot.gameObject) {
					//print("This torch is not equipped. Equiping now.");
					equipManager.torchSlott.itemsGo.SetActive(false);
					equipManager.torchSlott.wearingThisItem = false;
					equipManager.torchSlott.GetComponent<Image>().sprite = slotNormal;
                    equipManager.chestSlott.itemInSlot.equiped = false;

                    wearingThisItem = true;
					equipManager.torchSlot = itemInSlot.gameObject;
					equipManager.torchSlott = this;
					GetComponent<Image>().sprite = slotEquiped;
					itemInSlot.itemGO.SetActive(true);
					inventory.PlaySound(inventory.equipTorchSFX);
					equipManager.torchDrawn = true;
				}
			}
			else if(equipManager.torch == false && equipManager.shield == true) { //If there is no torch currently eqquiped
				equipManager.shieldSlott.wearingThisItem = false;
				equipManager.shieldSlott.GetComponent<Image>().sprite = slotNormal;
				equipManager.shieldSlott.itemInSlot.itemGO.SetActive(false);
				equipManager.shieldSlot = null;
				equipManager.shieldSlott = null;
				equipManager.shield = false;

				wearingThisItem = true;
				equipManager.torchSlot = itemInSlot.gameObject;
				equipManager.torchSlott = this;
				GetComponent<Image>().sprite = slotEquiped;
				itemInSlot.itemGO.SetActive(true);
				equipManager.torch = true;
				equipManager.torchDrawn = true;
				inventory.PlaySound(inventory.equipTorchSFX);
			}
		}
		if(itemInSlot.type == ItemType.Alchemy) {
			inventory.PlaySound(interact.alchemySFX);

			uiManager.alchemyUI.SetActive (true);	
			uiManager.SetActiveUIElement (uiManager.alchemyUI.transform.Find ("CraftBtn").gameObject);
			uiManager.alchemyUI.GetComponent<AlchemyUi> ().SetupAlchemyWindow ();

            uiManager.tutorialUI.GetComponentInChildren<TutorialUI>().ShowCraftingTutorial();

			uiManager.inventoryUI.SetActive(false);
		}
		if (itemInSlot.type == ItemType.Food) {
			if(itemInSlot.currentStackSize > 1) {
				inventory.PlaySound(inventory.eatFoodSFX);
				inventory.playerStats.currentWeightF -= itemInSlot.itemWeight;
				inventory.playerStats.currentHealth += itemInSlot.powerScore;
				itemInSlot.currentStackSize--;
				inventory.ResetInventory();
				if(inventory.tabActive == InventoryTabActive.TabAll) {
					inventory.SetUpTheInventory();
				}
				else if(inventory.tabActive == InventoryTabActive.TabPotions) {
					inventory.SUTIPotionsTab();
				}
			}
			else {
				inventory.PlaySound(inventory.eatFoodSFX);
				inventory.playerStats.currentWeightF -= itemInSlot.itemWeight;
				inventory.playerStats.currentHealth += itemInSlot.powerScore;
				inventory.items.Remove(itemInSlot.gameObject);
				inventory.ResetInventory();
				if(inventory.tabActive == InventoryTabActive.TabAll) {
					inventory.SetUpTheInventory();
				}
				else if(inventory.tabActive == InventoryTabActive.TabPotions) {
					inventory.SUTIPotionsTab();
				}
				Destroy(itemInSlot.gameObject);
			}
		}
	}

	public void OnPointerClick(PointerEventData eventData) {
		if(eventData.button == PointerEventData.InputButton.Left && !Input.GetKey(KeyCode.LeftShift)) {
			UseItem();
		}
		else if(eventData.button == PointerEventData.InputButton.Left && Input.GetKey(KeyCode.LeftShift)){
			inventory.RemoveItemFromInventory(itemInSlot.gameObject);
		}
	}

	public void UseItemGamepad() {
		if(Input.GetButtonDown("Use")) {
			UseItem();
		}
		else if(Input.GetButtonDown("Back/Close")) {
			inventory.RemoveItemFromInventory(itemInSlot.gameObject);
		}
	}

	public void ResetEquipmentManager() {
		if(itemInSlot.type == ItemType.Weapon) {
			equipManager.weapon = false;
			equipManager.weaponSlot = null;
		}
		if(itemInSlot.type == ItemType.Shield) {
			equipManager.shield = false;
			equipManager.shieldSlot = null;
		}
		if(itemInSlot.type == ItemType.Necklace) {
			equipManager.necklace = false;
			equipManager.necklaceSlot = null;
		}
		if(itemInSlot.type == ItemType.Torch) {
			equipManager.torch = false;
			equipManager.torchSlot = null;
		}
	}
}
