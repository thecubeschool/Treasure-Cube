using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class StoreMerchantsInventory : MonoBehaviour {

	public GameObject storeSlotPrefab;
	public int merchantsGold;
	public Text merchantsGoldTxt;
	public Transform merchantStoreHolder;
	public List<GameObject> merchantItems;
	public int itemsInstantiated;
	public List<GameObject> instantiatedSlots;

	public StorePlayerInventory playerStoreInventory;
	public ShowMessage showMessage;
	public PlayerSkillsAndAttributes skillsAttributes;

	void Awake() {
		playerStoreInventory = GameObject.Find("_UICanvasGame").GetComponentInChildren<StorePlayerInventory>();
		showMessage = GameObject.Find("_UICanvasGame").GetComponentInChildren<ShowMessage>();
		skillsAttributes = GameObject.Find("[Player]").GetComponent<PlayerSkillsAndAttributes>();
	}

	public void STOREGetMerchantItems() {
		if(merchantItems.Count > 0) {
			if(itemsInstantiated < merchantItems.Count) {
				foreach(GameObject i in merchantItems) {
					GameObject newItemSlot = (GameObject)Instantiate(storeSlotPrefab);
					RectTransform rect = newItemSlot.GetComponent<RectTransform>();
					newItemSlot.transform.SetParent(transform.Find("MSScrollRect/Slots").transform, false);
					
					Item ii = i.GetComponent<Item>();
					
					newItemSlot.GetComponent<StoreSlot>().merchantsSlot = true;
					newItemSlot.GetComponent<StoreSlot>().slotIcon.sprite = ii.itemIcon;
					newItemSlot.GetComponent<StoreSlot>().nameText.text = ii.itemName;
					newItemSlot.GetComponent<StoreSlot>().healthText.text = ii.health.ToString();
					newItemSlot.GetComponent<StoreSlot>().powerText.text = ii.powerScore.ToString();
					if(skillsAttributes.speechcraft >= 80) {
						newItemSlot.GetComponent<StoreSlot>().valueText.text = ii.vendorPrice.ToString();
					}
					else {
						newItemSlot.GetComponent<StoreSlot>().valueText.text = ii.merchantPrice.ToString();
					}
					newItemSlot.GetComponent<StoreSlot>().weightText.text = ii.itemWeight.ToString();
					newItemSlot.GetComponent<StoreSlot>().itemInSlot = i;
					newItemSlot.GetComponent<StoreSlot>().merchantsInventory = this;
					
					instantiatedSlots.Add(newItemSlot);
					
					if(itemsInstantiated == 0) {
						rect.localPosition = new Vector3(0, 0);
					}
					else {
						rect.localPosition = new Vector3(0, itemsInstantiated * -33);
					}
					
					rect.localScale = new Vector3(1, 1, 1);						
					transform.Find("MSScrollRect/Slots").transform.GetComponent<RectTransform>().sizeDelta = new Vector2(194, itemsInstantiated * 42);

					if(i.gameObject.activeSelf == true) {
						i.gameObject.SetActive(false);
					}

					itemsInstantiated++;
				}
			}
		}
	}

	public void RESETMerchantItems() {
		if(instantiatedSlots.Count > 0) {
			foreach(GameObject g in instantiatedSlots) {
				Destroy(g);
			}
		}
	
		itemsInstantiated = 0;
		instantiatedSlots.Clear();
	}

	public void CLEANMerchantStore() {
		if(instantiatedSlots.Count > 0) {
			foreach(GameObject g in instantiatedSlots) {
				Destroy(g);
			}
		}
		
		merchantsGold = 0;
		merchantsGoldTxt.text = string.Empty;
		merchantStoreHolder = null;
		merchantItems.Clear();
		itemsInstantiated = 0;
		instantiatedSlots.Clear();
	}

	public void BUYItem(GameObject itemBuy) {
		if (skillsAttributes.speechcraft >= 80) {
			if (playerStoreInventory.playerStats.playerMoney >= itemBuy.GetComponent<Item> ().vendorPrice) {
				foreach (GameObject i in merchantItems.ToArray()) {
					if (i == itemBuy) {
						i.transform.parent = playerStoreInventory.itemsHolder;
						playerStoreInventory.inventory.itemsCount++;
						
						skillsAttributes.speechcraftAdvancement += 0.15f;
						
						playerStoreInventory.playerStats.playerMoney -= i.GetComponent<Item> ().vendorPrice;
						merchantsGold += i.GetComponent<Item> ().vendorPrice;
						merchantItems.Remove (i);
						playerStoreInventory.inventory.items.Add (i);
						RESETMerchantItems ();
						STOREGetMerchantItems ();
						playerStoreInventory.RESETPlayerStore ();
						playerStoreInventory.STOREGetPlayerItems ();
					}
				}
			} else {
				showMessage.SendTheMessage ("I do not have enough gold to buy this item.");
			}
		} 
		else {
			if (playerStoreInventory.playerStats.playerMoney >= itemBuy.GetComponent<Item> ().merchantPrice) {
				foreach (GameObject i in merchantItems.ToArray()) {
					if (i == itemBuy) {
						i.transform.parent = playerStoreInventory.itemsHolder;
						playerStoreInventory.inventory.itemsCount++;

						skillsAttributes.speechcraftAdvancement += 0.15f;

						playerStoreInventory.playerStats.playerMoney -= i.GetComponent<Item> ().merchantPrice;
						merchantsGold += i.GetComponent<Item> ().merchantPrice;
						merchantItems.Remove (i);
						playerStoreInventory.inventory.items.Add (i);
						RESETMerchantItems ();
						STOREGetMerchantItems ();
						playerStoreInventory.RESETPlayerStore ();
						playerStoreInventory.STOREGetPlayerItems ();
					}
				}
			} else {
				showMessage.SendTheMessage ("I do not have enough gold to buy this item.");
			}
		}
	}
}
