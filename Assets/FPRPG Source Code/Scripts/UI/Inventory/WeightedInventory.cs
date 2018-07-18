/// <summary>
/// Our inventory system is a simple Get And Instantiate system. When picked up items get parented to the specific GameObject and added to the "items" list
/// in our inventory. Whenever 'SetUpInventory()' function is called, game picks up all objects from the 'items' list and instantiates 'itemSlotPrefab'
/// as much as there are items in the 'items' list and than system defines icon, name, stacking, healt, power, value and weight values acording to the
/// sprecific item it instantiates.
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public enum InventoryTabActive {
	TabAll = 0,
	TabWeapons = 1,
	TabArmors = 2,
	TabPotions = 3,
	TabMisc = 4,
}

public class WeightedInventory : MonoBehaviour {

	public AudioClip[] removeItemSFX;
	public AudioClip[] equipWeaponSFX;
	public AudioClip[] equipArmorSFX;
	public AudioClip[] equipTorchSFX;
	public AudioClip[] drinkPotionSFX;
	public AudioClip[] eatFoodSFX;
	public AudioClip[] readBookSFX;
	public AudioSource uiAS;
	public bool step = true;
	[Space(10f)]
	public InventoryTabActive tabActive;
	[Space(10f)]
	public GameObject itemSlotPrefab;
	[Space(10f)]
	public Transform playerHolder;
	public Transform worldHolder;
	public Transform itemDropPoint;
	public List<GameObject> items;
	public List<Slot> itemSlots;
	public int itemsCount;
	public int itemsInstantiated;
	[Space(10f)]
	public Text currentMaxWeight;
	public PlayerStats playerStats;
	public EquipmentManager equipManager;
	public WeaponRangedAnimate wepRange;
	
	void Awake() {
		playerStats = GameObject.Find("[Player]").GetComponent<PlayerStats>();
		equipManager = GameObject.Find("[Player]").GetComponentInChildren<EquipmentManager>();
        playerHolder = GameObject.FindObjectOfType<GameManager>().transform.Find("InventoryHolder").transform;

    }

	public void SetUpTheInventory() {

		itemSlots.Clear();

		if(itemsCount > 0) {
			//if(playerStats.currentWeight < playerStats.maxCarryWeight) {
				if(itemsInstantiated < itemsCount) {
				foreach(GameObject i in items) {
					GameObject newItemSlot = (GameObject)Instantiate(itemSlotPrefab);
					RectTransform rect = newItemSlot.GetComponent<RectTransform>();
					newItemSlot.transform.SetParent(transform.Find("InventoryScrollRect/Slots").transform, false);
					
					Item ii = i.GetComponent<Item>();

					if(ii.currentStackSize > 1) {
						newItemSlot.GetComponent<Slot>().stackText.text = ii.currentStackSize.ToString();
					}
					else {
						newItemSlot.GetComponent<Slot>().stackText.text = string.Empty;
					}
					newItemSlot.GetComponent<Slot>().slotIcon.enabled = true;
					newItemSlot.GetComponent<Slot>().slotIcon.sprite = ii.itemIcon;
					if(ii.stolen == true) {
						newItemSlot.GetComponent<Slot>().nameText.text = ii.itemName + " (stolen)";
					}
					else {
						newItemSlot.GetComponent<Slot>().nameText.text = ii.itemName;
					}
					int itemHealthTmp = (int)ii.health;
					newItemSlot.GetComponent<Slot>().healthText.text = itemHealthTmp.ToString();
					if(i.GetComponent<Item>().type != ItemType.Potion) {
						newItemSlot.GetComponent<Slot>().powerText.text = ii.powerScore.ToString();
					}
					newItemSlot.GetComponent<Slot>().valueText.text = ii.vendorPrice.ToString();
					newItemSlot.GetComponent<Slot>().weightText.text = ii.itemWeight.ToString();
					if(equipManager.weaponSlot == i) {
						newItemSlot.GetComponent<Slot>().wearingThisItem = true;
						newItemSlot.GetComponent<Image>().sprite = newItemSlot.GetComponent<Slot>().slotEquiped;
						equipManager.weaponSlott = newItemSlot.GetComponent<Slot>();
					}
					else if(equipManager.chestSlot == i) {
						newItemSlot.GetComponent<Slot>().wearingThisItem = true;
						newItemSlot.GetComponent<Image>().sprite = newItemSlot.GetComponent<Slot>().slotEquiped;
						equipManager.chestSlott = newItemSlot.GetComponent<Slot>();
					}
					else if(equipManager.headSlot == i) {
						newItemSlot.GetComponent<Slot>().wearingThisItem = true;
						newItemSlot.GetComponent<Image>().sprite = newItemSlot.GetComponent<Slot>().slotEquiped;
						equipManager.headSlott = newItemSlot.GetComponent<Slot>();
					}
					else if(equipManager.shieldSlot == i) {
						newItemSlot.GetComponent<Slot>().wearingThisItem = true;
						newItemSlot.GetComponent<Image>().sprite = newItemSlot.GetComponent<Slot>().slotEquiped;
						equipManager.shieldSlott = newItemSlot.GetComponent<Slot>();
					}
					else if(equipManager.necklaceSlot == i) {
						newItemSlot.GetComponent<Slot>().wearingThisItem = true;
						newItemSlot.GetComponent<Image>().sprite = newItemSlot.GetComponent<Slot>().slotEquiped;
						equipManager.necklaceSlott = newItemSlot.GetComponent<Slot>();
					}
					else if(equipManager.missileSlot == i) {
						newItemSlot.GetComponent<Slot>().wearingThisItem = true;
						newItemSlot.GetComponent<Image>().sprite = newItemSlot.GetComponent<Slot>().slotEquiped;
						equipManager.missileSlott = newItemSlot.GetComponent<Slot>();
					}
					else if(equipManager.torchSlot == i) {
						newItemSlot.GetComponent<Slot>().wearingThisItem = true;
						newItemSlot.GetComponent<Image>().sprite = newItemSlot.GetComponent<Slot>().slotEquiped;
						equipManager.torchSlott = newItemSlot.GetComponent<Slot>();
					}
					newItemSlot.GetComponent<Slot>().inventory = this;
					
					itemSlots.Add(newItemSlot.GetComponent<Slot>());
					newItemSlot.GetComponent<Slot>().itemInSlot = ii;

					if(itemsInstantiated == 0) {
						rect.localPosition = new Vector3(0, 0);
					}
					else {
						rect.localPosition = new Vector3(0, itemsInstantiated * -33);
					}
					
					rect.localScale = new Vector3(1, 1, 1);						
					transform.Find("InventoryScrollRect/Slots").transform.GetComponent<RectTransform>().sizeDelta = new Vector2(194, itemsInstantiated * 42);

					itemsInstantiated++;
				}
			}
		}

		currentMaxWeight.text = playerStats.currentWeight + " / " + playerStats.maxCarryWeight;
		tabActive = InventoryTabActive.TabAll;
	}

	public void SUTIWeaponsTab() {
		
		itemSlots.Clear();
		
		if(itemsCount > 0) {
			//if(playerStats.currentWeight < playerStats.maxCarryWeight) {
				if(itemsInstantiated < itemsCount) {
					foreach(GameObject i in items) {
						if(i.GetComponent<Item>().type == ItemType.Weapon || i.GetComponent<Item>().type == ItemType.Missile) {
							GameObject newItemSlot = (GameObject)Instantiate(itemSlotPrefab);
							RectTransform rect = newItemSlot.GetComponent<RectTransform>();
							newItemSlot.transform.SetParent(transform.Find("InventoryScrollRect/Slots").transform, false);
							
							Item ii = i.GetComponent<Item>();

							if(ii.currentStackSize > 1) {
								newItemSlot.GetComponent<Slot>().stackText.text = ii.currentStackSize.ToString();
							}
							else {
								newItemSlot.GetComponent<Slot>().stackText.text = string.Empty;
							}
							newItemSlot.GetComponent<Slot>().slotIcon.enabled = true;
							newItemSlot.GetComponent<Slot>().slotIcon.sprite = ii.itemIcon;
							newItemSlot.GetComponent<Slot>().nameText.text = ii.itemName;
							int itemHealthTmp = (int)ii.health;
							newItemSlot.GetComponent<Slot>().healthText.text = itemHealthTmp.ToString();
							newItemSlot.GetComponent<Slot>().powerText.text = ii.powerScore.ToString();
							newItemSlot.GetComponent<Slot>().valueText.text = ii.vendorPrice.ToString();
							newItemSlot.GetComponent<Slot>().weightText.text = ii.itemWeight.ToString();
							if(equipManager.weaponSlot == i) {
								newItemSlot.GetComponent<Slot>().wearingThisItem = true;
								newItemSlot.GetComponent<Image>().sprite = newItemSlot.GetComponent<Slot>().slotEquiped;
								equipManager.weaponSlott = newItemSlot.GetComponent<Slot>();
							}
							else if(equipManager.missileSlot == i) {
								newItemSlot.GetComponent<Slot>().wearingThisItem = true;
								newItemSlot.GetComponent<Image>().sprite = newItemSlot.GetComponent<Slot>().slotEquiped;
								equipManager.missileSlott = newItemSlot.GetComponent<Slot>();
							}
							newItemSlot.GetComponent<Slot>().inventory = this;
							
							itemSlots.Add(newItemSlot.GetComponent<Slot>());
							newItemSlot.GetComponent<Slot>().itemInSlot = ii;
							
							if(itemsInstantiated == 0) {
								rect.localPosition = new Vector3(0, 0);
							}
							else {
								rect.localPosition = new Vector3(0, itemsInstantiated * -33);
							}
							
							rect.localScale = new Vector3(1, 1, 1);						
							transform.Find("InventoryScrollRect/Slots").transform.GetComponent<RectTransform>().sizeDelta = new Vector2(194, itemsInstantiated * 42);
							
							itemsInstantiated++;
						}
					}
				}
			//}
		}
		
		currentMaxWeight.text = playerStats.currentWeight + " / " + playerStats.maxCarryWeight;
		tabActive = InventoryTabActive.TabWeapons;
	}

	public void SUTIArmorsTab() {
		
		itemSlots.Clear();
		
		if(itemsCount > 0) {
			//if(playerStats.currentWeight < playerStats.maxCarryWeight) {
				if(itemsInstantiated < itemsCount) {
					foreach(GameObject i in items) {
						if(i.GetComponent<Item>().type == ItemType.Chest || i.GetComponent<Item>().type == ItemType.Helmet || 
						   i.GetComponent<Item>().type == ItemType.Shield || i.GetComponent<Item>().type == ItemType.Necklace) {
							if(!i.GetComponent<Item>().itemName.Contains("Torch")) {
								GameObject newItemSlot = (GameObject)Instantiate(itemSlotPrefab);
								RectTransform rect = newItemSlot.GetComponent<RectTransform>();
								newItemSlot.transform.SetParent(transform.Find("InventoryScrollRect/Slots").transform, false);
								
								Item ii = i.GetComponent<Item>();
								
								if(ii.currentStackSize > 1) {
									newItemSlot.GetComponent<Slot>().stackText.text = ii.currentStackSize.ToString();
								}
								else {
									newItemSlot.GetComponent<Slot>().stackText.text = string.Empty;
								}
								newItemSlot.GetComponent<Slot>().slotIcon.enabled = true;
								newItemSlot.GetComponent<Slot>().slotIcon.sprite = ii.itemIcon;
								newItemSlot.GetComponent<Slot>().nameText.text = ii.itemName;
								int itemHealthTmp = (int)ii.health;
								newItemSlot.GetComponent<Slot>().healthText.text = itemHealthTmp.ToString();
								newItemSlot.GetComponent<Slot>().powerText.text = ii.powerScore.ToString();
								newItemSlot.GetComponent<Slot>().valueText.text = ii.vendorPrice.ToString();
								newItemSlot.GetComponent<Slot>().weightText.text = ii.itemWeight.ToString();
								if(equipManager.chestSlot == i) {
									newItemSlot.GetComponent<Slot>().wearingThisItem = true;
									newItemSlot.GetComponent<Image>().sprite = newItemSlot.GetComponent<Slot>().slotEquiped;
									equipManager.chestSlott = newItemSlot.GetComponent<Slot>();
								}
								else if(equipManager.headSlot == i) {
									newItemSlot.GetComponent<Slot>().wearingThisItem = true;
									newItemSlot.GetComponent<Image>().sprite = newItemSlot.GetComponent<Slot>().slotEquiped;
									equipManager.headSlott = newItemSlot.GetComponent<Slot>();
								}
								else if(equipManager.shieldSlot == i) {
									newItemSlot.GetComponent<Slot>().wearingThisItem = true;
									newItemSlot.GetComponent<Image>().sprite = newItemSlot.GetComponent<Slot>().slotEquiped;
									equipManager.shieldSlott = newItemSlot.GetComponent<Slot>();
								}
								else if(equipManager.necklaceSlot == i) {
									newItemSlot.GetComponent<Slot>().wearingThisItem = true;
									newItemSlot.GetComponent<Image>().sprite = newItemSlot.GetComponent<Slot>().slotEquiped;
									equipManager.necklaceSlott = newItemSlot.GetComponent<Slot>();
								}
								newItemSlot.GetComponent<Slot>().inventory = this;

								itemSlots.Add(newItemSlot.GetComponent<Slot>());
								newItemSlot.GetComponent<Slot>().itemInSlot = ii;
								
								if(itemsInstantiated == 0) {
									rect.localPosition = new Vector3(0, 0);
								}
								else {
									rect.localPosition = new Vector3(0, itemsInstantiated * -33);
								}
								
								rect.localScale = new Vector3(1, 1, 1);						
								transform.Find("InventoryScrollRect/Slots").transform.GetComponent<RectTransform>().sizeDelta = new Vector2(194, itemsInstantiated * 42);
								
								itemsInstantiated++;
							}
						}
					}
				}
			//}
		}
		
		currentMaxWeight.text = playerStats.currentWeight + " / " + playerStats.maxCarryWeight;
		tabActive = InventoryTabActive.TabArmors;
	}

	public void SUTIPotionsTab() {
		
		itemSlots.Clear();
		
		if(itemsCount > 0) {
			//if(playerStats.currentWeight < playerStats.maxCarryWeight) {
				if(itemsInstantiated < itemsCount) {
					foreach(GameObject i in items) {
						if(i.GetComponent<Item>().type == ItemType.Potion || i.GetComponent<Item>().type == ItemType.Ingredient ||
					  	   i.GetComponent<Item>().type == ItemType.Food) {
							GameObject newItemSlot = (GameObject)Instantiate(itemSlotPrefab);
							RectTransform rect = newItemSlot.GetComponent<RectTransform>();
							newItemSlot.transform.SetParent(transform.Find("InventoryScrollRect/Slots").transform, false);
							
							Item ii = i.GetComponent<Item>();
							
							if(ii.currentStackSize > 1) {
								newItemSlot.GetComponent<Slot>().stackText.text = ii.currentStackSize.ToString();
							}
							else {
								newItemSlot.GetComponent<Slot>().stackText.text = string.Empty;
							}
							newItemSlot.GetComponent<Slot>().slotIcon.enabled = true;
							newItemSlot.GetComponent<Slot>().slotIcon.sprite = ii.itemIcon;
							newItemSlot.GetComponent<Slot>().nameText.text = ii.itemName;
							int itemHealthTmp = (int)ii.health;
							newItemSlot.GetComponent<Slot>().healthText.text = itemHealthTmp.ToString();
							newItemSlot.GetComponent<Slot>().powerText.text = string.Empty;
							newItemSlot.GetComponent<Slot>().valueText.text = ii.vendorPrice.ToString();
							newItemSlot.GetComponent<Slot>().weightText.text = ii.itemWeight.ToString();
							newItemSlot.GetComponent<Slot>().inventory = this;

							itemSlots.Add(newItemSlot.GetComponent<Slot>());
							newItemSlot.GetComponent<Slot>().itemInSlot = ii;
							
							if(itemsInstantiated == 0) {
								rect.localPosition = new Vector3(0, 0);
							}
							else {
								rect.localPosition = new Vector3(0, itemsInstantiated * -33);
							}
							
							rect.localScale = new Vector3(1, 1, 1);						
							transform.Find("InventoryScrollRect/Slots").transform.GetComponent<RectTransform>().sizeDelta = new Vector2(194, itemsInstantiated * 42);
							
							itemsInstantiated++;
						}
					}
				}
			//}
		}
		
		currentMaxWeight.text = playerStats.currentWeight + " / " + playerStats.maxCarryWeight;
		tabActive = InventoryTabActive.TabPotions;
	}

	public void SUTIMiscTab() {
		
		itemSlots.Clear();
		
		if(itemsCount > 0) {
			//if(playerStats.currentWeight < playerStats.maxCarryWeight) {
				if(itemsInstantiated < itemsCount) {
					foreach(GameObject i in items) {
						if(i.GetComponent<Item>().type == ItemType.Key || 
						   i.GetComponent<Item>().type == ItemType.Lightsource || i.GetComponent<Item>().type == ItemType.Book ||
						   i.GetComponent<Item>().type == ItemType.Torch || i.GetComponent<Item>().type == ItemType.Alchemy) {
							GameObject newItemSlot = (GameObject)Instantiate(itemSlotPrefab);
							RectTransform rect = newItemSlot.GetComponent<RectTransform>();
							newItemSlot.transform.SetParent(transform.Find("InventoryScrollRect/Slots").transform, false);
							
							Item ii = i.GetComponent<Item>();
							
							if(ii.currentStackSize > 1) {
								newItemSlot.GetComponent<Slot>().stackText.text = ii.currentStackSize.ToString();
							}
							else {
								newItemSlot.GetComponent<Slot>().stackText.text = string.Empty;
							}
							newItemSlot.GetComponent<Slot>().slotIcon.enabled = true;
							newItemSlot.GetComponent<Slot>().slotIcon.sprite = ii.itemIcon;
							newItemSlot.GetComponent<Slot>().nameText.text = ii.itemName;
							int itemHealthTmp = (int)ii.health;
							newItemSlot.GetComponent<Slot>().healthText.text = itemHealthTmp.ToString();
							newItemSlot.GetComponent<Slot>().powerText.text = string.Empty;
							newItemSlot.GetComponent<Slot>().valueText.text = ii.vendorPrice.ToString();
							newItemSlot.GetComponent<Slot>().weightText.text = ii.itemWeight.ToString();
							newItemSlot.GetComponent<Slot>().inventory = this;
							
							itemSlots.Add(newItemSlot.GetComponent<Slot>());
							newItemSlot.GetComponent<Slot>().itemInSlot = ii;
							
							if(itemsInstantiated == 0) {
								rect.localPosition = new Vector3(0, 0);
							}
							else {
								rect.localPosition = new Vector3(0, itemsInstantiated * -33);
							}
							
							rect.localScale = new Vector3(1, 1, 1);						
							transform.Find("InventoryScrollRect/Slots").transform.GetComponent<RectTransform>().sizeDelta = new Vector2(194, itemsInstantiated * 42);
							
							itemsInstantiated++;
						}
					}
				}
			//}
		}
		
		currentMaxWeight.text = playerStats.currentWeight + " / " + playerStats.maxCarryWeight;
		tabActive = InventoryTabActive.TabMisc;
	}

	public void AddItemToInventory(GameObject item) {
		bool thereIsSameItem = false;
		GameObject sameItem = null;

		foreach(GameObject i in items.ToArray()) {//check all items for identic item and inform the inventory with a bool if ther is a same item or not
			if(i.GetComponent<Item>().itemName == item.GetComponent<Item>().itemName && 
			   i.GetComponent<Item>().type == item.GetComponent<Item>().type &&
			   i.GetComponent<Item>().maxStackSize > 1 &&
			   i.GetComponent<Item>().currentStackSize < i.GetComponent<Item>().maxStackSize) { //if this is the same item and stack is not full
				sameItem = i;
				thereIsSameItem = true;
			}
		}

		if(thereIsSameItem == true) { //if there is same item
			//Debug.Log("INV.Same type item found. Adding to stack.");
			sameItem.GetComponent<Item>().currentStackSize += item.GetComponent<Item>().currentStackSize; //add to our stack
			playerStats.currentWeightF += item.GetComponent<Item>().itemWeight * item.GetComponent<Item>().currentStackSize; //and add weight to inventory
			Destroy(item); //destroy stacked gameobject
		}
		else { // if there is no item of same type in inventory
			items.Add(item); //add item gameobjet to 'items' list
			item.transform.SetParent(playerHolder); //set item gameobjects parent to inventory holder transform
			item.transform.position = playerHolder.position; //reset the position of items gameobject
			
			playerStats.currentWeightF += item.GetComponent<Item>().itemWeight * item.GetComponent<Item>().currentStackSize; //add the weight of the item to stats
			item.SetActive(false); //disable items gameobject from world space
			
			itemsCount++; //add item to count so we can instantiate needed slot when showing the inventory inside ui
		}
	}

    public void AddItemStackToInventory(GameObject item, int stack) {
        bool thereIsSameItem = false;
        GameObject sameItem = null;

        foreach (GameObject i in items.ToArray()) {//check all items for identic item and inform the inventory with a bool if ther is a same item or not
            if (i.GetComponent<Item>().itemName == item.GetComponent<Item>().itemName &&
               i.GetComponent<Item>().type == item.GetComponent<Item>().type &&
               i.GetComponent<Item>().maxStackSize > 1 &&
               i.GetComponent<Item>().currentStackSize < i.GetComponent<Item>().maxStackSize) { //if this is the same item and stack is not full
                sameItem = i;
                thereIsSameItem = true;
            }
        }

        if (thereIsSameItem == true) { //if there is same item
                                       //Debug.Log("INV.Same type item found. Adding to stack.");
            sameItem.GetComponent<Item>().currentStackSize += item.GetComponent<Item>().currentStackSize + stack; //add to our stack
            playerStats.currentWeightF += item.GetComponent<Item>().itemWeight * item.GetComponent<Item>().currentStackSize; //and add weight to inventory
            Destroy(item); //destroy stacked gameobject
        }
        else { // if there is no item of same type in inventory
            items.Add(item); //add item gameobjet to 'items' list
            item.transform.SetParent(playerHolder); //set item gameobjects parent to inventory holder transform
            item.transform.position = playerHolder.position; //reset the position of items gameobject
            item.GetComponent<Item>().currentStackSize = stack;

            playerStats.currentWeightF += item.GetComponent<Item>().itemWeight * item.GetComponent<Item>().currentStackSize; //add the weight of the item to stats
            item.SetActive(false); //disable items gameobject from world space

            itemsCount++; //add item to count so we can instantiate needed slot when showing the inventory inside ui
        }
    }

    public void RemoveItemFromInventory(GameObject item) {
		PlaySound(removeItemSFX);

        if(item.GetComponent<Item>().type == ItemType.Weapon && item.GetComponent<Item>().weaponsGO.Find(item.GetComponent<Item>().itemGOName).gameObject.activeSelf == true) {
            item.GetComponent<Item>().weaponsGO.Find(item.GetComponent<Item>().itemGOName).gameObject.SetActive(false);
        }

		if(item.GetComponent<Item>().currentStackSize > 1) {
			GameObject dropedStackedItem = Instantiate(item, Vector3.zero, Quaternion.identity) as GameObject;
			dropedStackedItem.transform.parent = null;
			dropedStackedItem.transform.position = itemDropPoint.position;
			dropedStackedItem.name = item.name;
			dropedStackedItem.SetActive(true);
			dropedStackedItem.GetComponent<Rigidbody>().useGravity = true;
			//dropedStackedItem.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
			dropedStackedItem.GetComponent<Item>().currentStackSize = 1;
			playerStats.currentWeightF -= item.GetComponent<Item>().itemWeight;
			item.GetComponent<Item>().currentStackSize--;
		}
		else {
			foreach(GameObject i in items.ToArray()) {
				if(i == item) {
					items.Remove(i);
					i.transform.parent = null;
					i.transform.position = itemDropPoint.position;

					playerStats.currentWeightF -= item.GetComponent<Item>().itemWeight;
					i.SetActive(true);
					i.GetComponent<Rigidbody>().useGravity = true;
					//i.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
					itemsCount--;

					if(equipManager.weaponSlot == item) {
						equipManager.weapon = false;
						equipManager.weaponSlot = null;
					}
					else if(equipManager.shieldSlot == item) {
						equipManager.shield = false;
						equipManager.shieldSlot = null;
					}
					else if(equipManager.headSlot == item) {
						equipManager.head = false;
						equipManager.headSlot = null;
					}
					else if(equipManager.chestSlot == item) {
						equipManager.chest = false;
						equipManager.chestSlot = null;
					}
					else if(equipManager.necklaceSlot == item) {
						equipManager.necklace = false;
						equipManager.necklaceSlot = null;
					}
					else if(equipManager.missileSlot == item) {
						equipManager.missile = false;
						equipManager.missileSlot = null;
					}
				}
			}
		}
		currentMaxWeight.text = playerStats.currentWeight + " / " + playerStats.maxCarryWeight;

		if(tabActive == InventoryTabActive.TabAll) {
			ResetInventory();
			SetUpTheInventory();
		}
		else if(tabActive == InventoryTabActive.TabWeapons) {
			ResetInventory();
			SUTIWeaponsTab();
		}
		else if(tabActive == InventoryTabActive.TabArmors) {
			ResetInventory();
			SUTIArmorsTab();
		}
		else if(tabActive == InventoryTabActive.TabPotions) {
			ResetInventory();
			SUTIPotionsTab();
		}
		else if(tabActive == InventoryTabActive.TabMisc) {
			ResetInventory();
			SUTIMiscTab();
		}
	}

	public void ResetInventory() {
		if(itemSlots.Count > 0) {
			foreach(Slot s in itemSlots) {
				Destroy(s.gameObject);
			}
		}
		itemsInstantiated = 0;
		itemSlots.Clear();
	}

	public void PlaySound(AudioClip[] sfx) {
		if(uiAS != null) {
			uiAS.clip = sfx[Random.Range(0, sfx.Length)];
			uiAS.Play();
		}
	}

	public void EquipItemFromInventory(Item item) {
		
		if(item.type == ItemType.Weapon) { //If it is equipment we want it to stay in inventory	
			
			equipManager.weaponSlot = item.gameObject;
			item.itemGO.SetActive(true);
			
			if(item.itemName.Contains("Bow")) {					
				if(equipManager.shieldSlot != null) {
                    Transform t = equipManager.shieldSlot.GetComponent<Item>().itemGO.transform.Find("ArrowHolder").transform;
                    foreach (GameObject g in t) {
                        g.SetActive(false);
                    }
                    equipManager.shieldSlott.wearingThisItem = false;
					equipManager.shieldSlot.GetComponent<Item>().itemGO.SetActive(false);
					equipManager.shield = false;
					equipManager.shieldSlott = null;
					equipManager.shieldSlot = null;
				}
			}
			equipManager.weapon = true;
		}
		if(item.type == ItemType.Shield) { //If it is equipment we want it to stay in inventory	
			
			equipManager.shieldSlot = item.gameObject;
			equipManager.shield = true;
			item.itemGO.SetActive(true);
		}
		if(item.type == ItemType.Chest) { //If it is equipment we want it to stay in inventory	
			
			equipManager.chestSlot = item.gameObject;
			equipManager.chest = true;
		}
		if(item.type == ItemType.Helmet) { //If it is equipment we want it to stay in inventory	
			
			equipManager.headSlot = item.gameObject;
			equipManager.head = true;
		}
		if(item.type == ItemType.Necklace) { //If it is equipment we want it to stay in inventory	
			
			equipManager.necklaceSlot = item.gameObject;
			equipManager.necklace = true;
		}
		if(item.type == ItemType.Missile) { //If it is equipment we want it to stay in inventory	
			
			equipManager.missileSlot = item.gameObject;
			equipManager.missile = true;			
			
			if(equipManager.weaponSlot != null) {
				wepRange.FindTheArrowInUse();
			}
		}

        item.equiped = true;
	}
}
