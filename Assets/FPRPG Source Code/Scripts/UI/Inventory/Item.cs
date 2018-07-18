using UnityEngine;
using System.Collections;

public enum ArmorType {
	None = 0,
	LightArmor = 1,
	HeavyArmor = 2,
}

public enum ItemType {
	Weapon = 0,
	Shield = 1,
	Helmet = 2,
	Chest = 3,
	Potion = 6,
	Food = 7,
	Key = 8,
	Lightsource = 9,
	Necklace = 10,
	Missile = 11,
	Book = 12,
	Ingredient = 13,
	Torch = 14,
	Alchemy = 15,
}

[AddComponentMenu("[The Fallen Chronicler]/Items/Make Item")]
[RequireComponent(typeof (BoxCollider))]
[RequireComponent(typeof (Rigidbody))]
[RequireComponent(typeof (PickupItem))]

public class Item : MonoBehaviour {
    public int itemId;
	[Header("Ownership and Quest values")]
	public GameObject owner;
	public bool questItem = false;
	public string questId;
	public QuestPhase questPhase;
	public ItemQuestUse effectScript;
	[Header("Visual values")]
	public string itemName;
	public Sprite itemIcon;
	[Space(10f)]
	public string itemGOName = "/";
    [HideInInspector]
    public Transform weaponsGO;
	private Transform shieldsGO;
	[Header("Essential values")]
	public ItemType type;
	[Tooltip("Damage/Magic. Leave ZERO if this item has no damage, because we do not wont inventory to show the value.")]
	public int powerScore; //It is armor score for armor item and damage score for weapon item
	public float health;
	public int vendorPrice;
	[HideInInspector]
	public int merchantPrice = 0;
	public float itemWeight;
	[Tooltip("If this is part of the armor we must set its type.")]
	public ArmorType armorType;
	public GameObject itemGO;
	[Space(10f)]
	public int maxStackSize;
	public int currentStackSize;
	[HideInInspector]
	public bool stolen;
    [HideInInspector]
    public bool equiped; //We added this so we can equip this item from save

	void Awake() {

        Item[] allItems = GameObject.FindObjectsOfType<Item>();

        for (int i = 0; i < allItems.Length; i++) {
            if (allItems[i] != this && allItems[i].itemId == itemId) {
                itemId = Random.Range(0, 1000);
                Debug.Log("We are having two items using the same ID. Changin ID of item "+gameObject.name+" to ID "+itemId);
            }
        }

        if (itemGOName.Contains("Bow")) {
			weaponsGO = GameObject.Find("[Player]").transform.Find("FPCameraGO/FPCamera/EquipmentGO/WeaponsRangedGO");
		}
		else {
			weaponsGO = GameObject.Find("[Player]").transform.Find("FPCameraGO/FPCamera/EquipmentGO/WeaponsMeleeGO");
		}
		shieldsGO = GameObject.Find("[Player]").transform.Find("FPCameraGO/FPCamera/EquipmentGO/ShieldsGO");

		if(itemGOName == "/") {
			itemGO = null;
		}
		else {
			if(type == ItemType.Weapon) {
				itemGO = weaponsGO.Find(itemGOName).gameObject;
			}
			if(type == ItemType.Shield) {
				itemGO = shieldsGO.Find(itemGOName).gameObject;
			}
			if(type == ItemType.Torch) {
				itemGO = shieldsGO.Find(itemGOName).gameObject;
			}
		}

		if(currentStackSize > maxStackSize) {
			currentStackSize = maxStackSize;
		}
		if(currentStackSize < 1) {
			currentStackSize = 1;
		}
	}

	void Update() {
		if(merchantPrice <= 0) {
			float percent20 = vendorPrice * 0.2f;
			int tmp = (int)percent20;
			merchantPrice = tmp + vendorPrice;
		}
	}
}
