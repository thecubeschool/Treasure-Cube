using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class StorePlayerInventory : MonoBehaviour {

	public GameObject itemSlotPrefab;
	[Space(10f)]
	public Transform itemsHolder;
	public List<GameObject> itemsInInventory;
	public List<GameObject> instantiatedSlots;
	[Space(10f)]
	public int itemsCount;
	public int itemsInstantiated;
	[Space(10f)]
	public Text playerGold;
	[Space(10f)]
	public WeightedInventory inventory;
	public ShowMessage showMessage;
	public StoreMerchantsInventory merchantsInventory;
	public PlayerStats playerStats;
	public EquipmentManager equipManager;
	public PlayerSkillsAndAttributes skillsAttributes;

	void Awake() {
		inventory = GameObject.Find("_UICanvasGame").GetComponentInChildren<WeightedInventory>();
		showMessage = GameObject.Find("_UICanvasGame").GetComponentInChildren<ShowMessage>();
		merchantsInventory = GameObject.Find("_UICanvasGame").GetComponentInChildren<StoreMerchantsInventory>();
		playerStats = GameObject.Find("[Player]").GetComponent<PlayerStats>();
		equipManager = GameObject.Find("[Player]").GetComponentInChildren<EquipmentManager>();
		skillsAttributes = GameObject.Find("[Player]").GetComponent<PlayerSkillsAndAttributes>();

		STOREGetPlayerItems();
	}

    public void STOREGetPlayerItems() {
        if (playerStats != null) {
            playerGold.text = playerStats.playerMoney.ToString();
        }
		merchantsInventory.merchantsGoldTxt.text = merchantsInventory.merchantsGold.ToString();
		itemsInInventory = inventory.items;
		itemsCount = itemsInInventory.Count;

		if(itemsCount > 0) {
			if(itemsInstantiated < itemsCount) {
				foreach(GameObject i in itemsInInventory) {
					if(skillsAttributes.speechcraft < 25) {
						if(i.GetComponent<Item>().stolen == false) {
							GameObject newItemSlot = (GameObject)Instantiate(itemSlotPrefab);
							RectTransform rect = newItemSlot.GetComponent<RectTransform>();
							newItemSlot.transform.SetParent(transform.Find("PIScrollRect/Slots").transform, false);
							
							Item ii = i.GetComponent<Item>();

							newItemSlot.GetComponent<StoreSlot>().merchantsSlot = false;
							newItemSlot.GetComponent<StoreSlot>().slotIcon.sprite = ii.itemIcon;
							newItemSlot.GetComponent<StoreSlot>().nameText.text = ii.itemName;
							newItemSlot.GetComponent<StoreSlot>().healthText.text = ii.health.ToString();
							newItemSlot.GetComponent<StoreSlot>().powerText.text = ii.powerScore.ToString();
							newItemSlot.GetComponent<StoreSlot>().valueText.text = ii.vendorPrice.ToString();
							newItemSlot.GetComponent<StoreSlot>().weightText.text = ii.itemWeight.ToString();
							newItemSlot.GetComponent<StoreSlot>().itemInSlot = i;
							newItemSlot.GetComponent<StoreSlot>().inventory = this;

							instantiatedSlots.Add(newItemSlot);
							
							if(itemsInstantiated == 0) {
								rect.localPosition = new Vector3(0, 0);
							}
							else {
								rect.localPosition = new Vector3(0, itemsInstantiated * -33);
							}
							
							rect.localScale = new Vector3(1, 1, 1);						
							transform.Find("PIScrollRect/Slots").transform.GetComponent<RectTransform>().sizeDelta = new Vector2(194, itemsInstantiated * 42);
							
							itemsInstantiated++;
						}
					}
					else {
						GameObject newItemSlot = (GameObject)Instantiate(itemSlotPrefab);
						RectTransform rect = newItemSlot.GetComponent<RectTransform>();
						newItemSlot.transform.SetParent(transform.Find("PIScrollRect/Slots").transform, false);
						
						Item ii = i.GetComponent<Item>();
						
						newItemSlot.GetComponent<StoreSlot>().merchantsSlot = false;
						newItemSlot.GetComponent<StoreSlot>().slotIcon.sprite = ii.itemIcon;
						newItemSlot.GetComponent<StoreSlot>().nameText.text = ii.itemName;
						newItemSlot.GetComponent<StoreSlot>().healthText.text = ii.health.ToString();
						newItemSlot.GetComponent<StoreSlot>().powerText.text = ii.powerScore.ToString();
						newItemSlot.GetComponent<StoreSlot>().valueText.text = ii.vendorPrice.ToString();
						newItemSlot.GetComponent<StoreSlot>().weightText.text = ii.itemWeight.ToString();
						newItemSlot.GetComponent<StoreSlot>().itemInSlot = i;
						newItemSlot.GetComponent<StoreSlot>().inventory = this;
						
						instantiatedSlots.Add(newItemSlot);
						
						if(itemsInstantiated == 0) {
							rect.localPosition = new Vector3(0, 0);
						}
						else {
							rect.localPosition = new Vector3(0, itemsInstantiated * -33);
						}
						
						rect.localScale = new Vector3(1, 1, 1);						
						transform.Find("PIScrollRect/Slots").transform.GetComponent<RectTransform>().sizeDelta = new Vector2(194, itemsInstantiated * 42);
						
						itemsInstantiated++;
					}
				}
			}
		}
	}

	public void SELLItem(GameObject itemSold) {
		if(itemSold.GetComponent<Item>().questItem == false) {
			if(merchantsInventory.merchantsGold >= itemSold.GetComponent<Item>().vendorPrice) {
				foreach(GameObject i in inventory.items.ToArray()) {
					if(i == itemSold) {
						if(equipManager.weaponSlot == i) {
							equipManager.weapon = false;
							equipManager.weaponSlot = null;
						}
						else if(equipManager.shieldSlot == i) {
							equipManager.shield = false;
							equipManager.shieldSlot = null;
						}
						else if(equipManager.headSlot == i) {
							equipManager.head = false;
							equipManager.headSlot = null;
						}
						else if(equipManager.chestSlot == i) {
							equipManager.chest = false;
							equipManager.chestSlot = null;
						}
						else if(equipManager.necklaceSlot == i) {
							equipManager.necklace = false;
							equipManager.necklaceSlot = null;
						}
						else if(equipManager.missileSlot == i) {
							equipManager.missile = false;
							equipManager.missileSlot = null;
						}

						i.transform.parent = merchantsInventory.merchantStoreHolder;

						skillsAttributes.speechcraftAdvancement += 0.15f;

						playerStats.playerMoney += i.GetComponent<Item>().vendorPrice;
						merchantsInventory.merchantsGold -= i.GetComponent<Item>().vendorPrice;
						merchantsInventory.merchantItems.Add(i);
						inventory.items.Remove(i);
						inventory.itemsCount--;
						merchantsInventory.RESETMerchantItems();
						merchantsInventory.STOREGetMerchantItems();
						RESETPlayerStore();
						STOREGetPlayerItems();
					}
				}
			}
			else {
				showMessage.SendTheMessage("Merchant does not have enough gold to buy this item.");
			}
		}
		else {
			showMessage.SendTheMessage("I can not sell this item.");
		}
	}

	public void RESETPlayerStore() {
		if(instantiatedSlots.Count > 0) {
			foreach(GameObject g in instantiatedSlots) {
				Destroy(g);
			}
		}
		itemsInstantiated = 0;
		instantiatedSlots.Clear();
	}
}
