using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StoreSlot : MonoBehaviour, IPointerClickHandler {

	public bool merchantsSlot = false;
	[Space(10f)]
	public Image slotIcon;
	public Text stackText;
	public Text nameText;
	public Text healthText;
	public Text powerText;
	public Text valueText;
	public Text weightText;
	[Space(10f)]
	public GameObject itemInSlot;
	[Space(10f)]
	public StorePlayerInventory inventory;
	public StoreMerchantsInventory merchantsInventory;

	public void OnPointerClick(PointerEventData eventData) {
		if(eventData.button == PointerEventData.InputButton.Left) {
			if(merchantsSlot == false) {
				inventory.SELLItem(itemInSlot);
			}
			else {
				merchantsInventory.BUYItem(itemInSlot);
			}
		}
	}
}
