using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class AlchemyUi : MonoBehaviour {

	public AudioClip[] brewingSFX;
	public AudioClip boilingWaterSFX;
	public AudioSource uiAS;
	public AudioSource uiAmbientAS;
	private bool step = true;
	[Space(10f)]
	public GameObject recipeSlotPrefab;
	[Space(10f)]
	public Transform recipesHolder;
	public Transform inventoryHolder;
	public List<GameObject> recipesObjects;
	public List<AlchemyRecipe> recipes;
	public int recipesCount;
	public int instantiatedRecipes;
	[Space(10f)]
	private AlchemyRecipeSlot activeSlot;
	[Space(10f)]
	public Image potionSprite;
	public Text ingredientsTxt;
	public Text effectTxt;
	public Button craftBtn;
	[Space(10f)]
	//private PlayerSkillsAndAttributes skillsAttributes;
	private PlayerStats playerStats;
	private ShowMessage showMessage;
	private WeightedInventory inventory;
	private PlayerSkillsAndAttributes skillsAttributes;
	[Space(10f)]
	public float craftingCooldown;
	public float craftingTimer;

	void Start() {
		//skillsAttributes = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSkillsAndAttributes>();
		playerStats = GameObject.Find("[Player]").GetComponent<PlayerStats>();
		showMessage = GameObject.Find("_UICanvasGame").GetComponentInChildren<ShowMessage>();
		inventory = GameObject.Find("_UICanvasGame").GetComponentInChildren<WeightedInventory>();
		skillsAttributes = GameObject.Find("[Player]").GetComponent<PlayerSkillsAndAttributes>();
		
		potionSprite.enabled = false;
		ingredientsTxt.text = string.Empty;
		effectTxt.text = string.Empty;
	}

	public void SetupAlchemyWindow() {

		recipesCount = recipesObjects.Count;

		uiAmbientAS.clip = boilingWaterSFX;
		if(recipesCount > 0) {
			if(recipesCount > instantiatedRecipes) {
				foreach(GameObject r in recipesObjects) {
					GameObject newItemSlot = (GameObject)Instantiate(recipeSlotPrefab);
					RectTransform rect = newItemSlot.GetComponent<RectTransform>();
					newItemSlot.transform.SetParent(transform.Find("ListOfRecipes").transform, false);
					AlchemyRecipe a = r.GetComponent<AlchemyRecipe>();

					newItemSlot.GetComponent<AlchemyRecipeSlot>().alchemyUi = this;
					newItemSlot.GetComponent<AlchemyRecipeSlot>().recipeName.text = a.recipeName;
					newItemSlot.GetComponent<AlchemyRecipeSlot>().ingredientsNeeded = a.ingredientsNeeded;
					newItemSlot.GetComponent<AlchemyRecipeSlot>().effectOfPotion = a.effectOfPotion;
					newItemSlot.GetComponent<AlchemyRecipeSlot>().recipeInSlot = a;

					recipes.Add(newItemSlot.GetComponent<AlchemyRecipe>());
					
					if(instantiatedRecipes == 0) {
						rect.localPosition = new Vector3(0, 0);
					}
					else {
						rect.localPosition = new Vector3(0, instantiatedRecipes * -33);
					}
					
					rect.localScale = new Vector3(1, 1, 1);						
					//transform.Find("ListOfRecipes").transform.GetComponent<RectTransform>().sizeDelta = new Vector2(194, instantiatedRecipes * 42);
					
					instantiatedRecipes++;
				}
			}
		}
	}

	void Update() {
		if(craftingCooldown > 0) {
			craftingCooldown -= Time.deltaTime;
		}
		if(activeSlot != null && craftingCooldown <= 0) {
			craftBtn.interactable = true;
			inventory.ResetInventory();
			inventory.SetUpTheInventory();
		}
		else {
			craftBtn.interactable = false;
		}
	}

	public void CraftThePotion() {
		float availableWeight = playerStats.maxCarryWeight - playerStats.currentWeightF;

		if(availableWeight >= activeSlot.recipeInSlot.potionPrefab.GetComponent<Item>().itemWeight) {

			bool foundSameItem = false;
			GameObject itemFound = null;
			GameObject itemNeeded1 = null;
			GameObject itemNeeded2 = null;

			foreach(Transform go in inventoryHolder) {
				if(go.GetComponent<Item>().itemName == activeSlot.ingredientsNeeded[0] && go.GetComponent<Item>().type == ItemType.Ingredient) {
					itemNeeded1 = go.gameObject;
                    Debug.Log("We have item number 1: " + activeSlot.ingredientsNeeded[0]);
				}
			}
            foreach (Transform go in inventoryHolder) {
                if (go.GetComponent<Item>().itemName == activeSlot.ingredientsNeeded[1] && go.GetComponent<Item>().type == ItemType.Ingredient) {
                    itemNeeded2 = go.gameObject;
                    Debug.Log("We have item number 2: " + activeSlot.ingredientsNeeded[1]);
                }
            }

            if (skillsAttributes.alchemy < 80 && activeSlot.recipeInSlot.potionPrefab.GetComponent<Item>().itemName.Contains("Permanent")) {
				if(itemNeeded1 != null && itemNeeded2 != null) {

					GameObject potion = Instantiate(activeSlot.recipeInSlot.potionPrefab, inventoryHolder.position, Quaternion.identity) as GameObject;

					if(skillsAttributes.alchemy > 25) {
						float tmp = potion.GetComponent<Item>().vendorPrice * 0.2f;
						float tmp1 = potion.GetComponent<Item>().vendorPrice + tmp;
						potion.GetComponent<Item>().vendorPrice = (int)tmp1;
						string newName = potion.GetComponent<Item>().itemName;
						potion.GetComponent<Item>().itemName = "Tasty " + newName;
					}
					if(skillsAttributes.alchemy > 50) {
						float tmp = potion.GetComponent<Item>().powerScore * 0.4f;
						float tmp1 = potion.GetComponent<Item>().powerScore + tmp;
						potion.GetComponent<Item>().powerScore = (int)tmp1;
						string newName1 = potion.GetComponent<Item>().itemName;
						potion.GetComponent<Item>().itemName = "Greater " + newName1;
					}

					foreach(GameObject i in inventory.items.ToArray()) {
						if(i.GetComponent<Item>().itemName == activeSlot.recipeInSlot.potionPrefab.GetComponent<Item>().itemName &&
						   i.GetComponent<Item>().type == activeSlot.recipeInSlot.potionPrefab.GetComponent<Item>().type &&
						   i.GetComponent<Item>().maxStackSize > 1 &&
						   i.GetComponent<Item>().currentStackSize > i.GetComponent<Item>().maxStackSize &&
						   i.GetComponent<Item>().vendorPrice == potion.GetComponent<Item>().vendorPrice &&
						   i.GetComponent<Item>().powerScore == potion.GetComponent<Item>().powerScore) { //if this is same item
							itemFound = i;
							foundSameItem = true;
						}
					}

					if(foundSameItem == true) {
						itemFound.GetComponent<Item>().currentStackSize++;
						playerStats.currentWeightF += itemFound.GetComponent<Item>().itemWeight * potion.GetComponent<Item>().currentStackSize;
						Destroy(potion);
					}
					else {
						inventory.AddItemToInventory(potion);
						potion.GetComponent<Rigidbody>().useGravity = false;
					}
					skillsAttributes.alchemyAdvancement += 0.25f;
					showMessage.SendTheMessage("I successfully made " + activeSlot.recipeInSlot.recipeName + ".");
					if(step) {
						StartCoroutine(PlayBrewingSound());
					}
					playerStats.currentWeightF -= itemNeeded1.GetComponent<Item>().itemWeight;
					playerStats.currentWeightF -= itemNeeded2.GetComponent<Item>().itemWeight;
					if(itemNeeded1.GetComponent<Item>().currentStackSize > 1) {
						itemNeeded1.GetComponent<Item>().currentStackSize--;
					}
					else {
						inventory.items.Remove(itemNeeded1);
						Destroy(itemNeeded1.gameObject);
					}
					if(itemNeeded2.GetComponent<Item>().currentStackSize > 1) {
						itemNeeded2.GetComponent<Item>().currentStackSize--;
					}
					else {
						inventory.items.Remove(itemNeeded2);
						Destroy(itemNeeded2.gameObject);
					}

					craftingCooldown = craftingTimer;
				}
				else {
					showMessage.SendTheMessage("I do not have needed ingredients.");
				}
			}
			else if (skillsAttributes.alchemy >= 80 && !activeSlot.recipeInSlot.potionPrefab.GetComponent<Item>().itemName.Contains("Permanent")){
				if((itemNeeded1 != null && itemNeeded2 == null) || (itemNeeded1 == null && itemNeeded2 != null)) {
					GameObject potion = Instantiate(activeSlot.recipeInSlot.potionPrefab, inventoryHolder.position, Quaternion.identity) as GameObject;
					
					float tmp = potion.GetComponent<Item>().vendorPrice * 0.2f;
					float tmp1 = potion.GetComponent<Item>().vendorPrice + tmp;
					potion.GetComponent<Item>().vendorPrice = (int)tmp1;
					string newName = potion.GetComponent<Item>().itemName;
					potion.GetComponent<Item>().itemName = "Tasty " + newName;
					
					float tmp2 = potion.GetComponent<Item>().powerScore * 0.4f;
					float tmp3 = potion.GetComponent<Item>().powerScore + tmp2;
					potion.GetComponent<Item>().powerScore = (int)tmp3;
					string newName1 = potion.GetComponent<Item>().itemName;
					potion.GetComponent<Item>().itemName = "Greater " + newName1;
					
					foreach(GameObject i in inventory.items.ToArray()) {
						if(i.GetComponent<Item>().itemName == activeSlot.recipeInSlot.potionPrefab.GetComponent<Item>().itemName &&
						   i.GetComponent<Item>().type == activeSlot.recipeInSlot.potionPrefab.GetComponent<Item>().type &&
						   i.GetComponent<Item>().maxStackSize > 1 &&
						   i.GetComponent<Item>().currentStackSize > i.GetComponent<Item>().maxStackSize &&
						   i.GetComponent<Item>().vendorPrice == potion.GetComponent<Item>().vendorPrice &&
						   i.GetComponent<Item>().powerScore == potion.GetComponent<Item>().powerScore) { //if this is same item
							itemFound = i;
							foundSameItem = true;
						}
					}
					
					if(foundSameItem == true) {
						itemFound.GetComponent<Item>().currentStackSize++;
						playerStats.currentWeightF += itemFound.GetComponent<Item>().itemWeight * potion.GetComponent<Item>().currentStackSize;
						Destroy(potion);
					}
					else {
						inventory.AddItemToInventory(potion);
						potion.GetComponent<Rigidbody>().useGravity = false;
					}
					skillsAttributes.alchemyAdvancement += 0.125f;
					showMessage.SendTheMessage("I successfully made " + activeSlot.recipeInSlot.recipeName + ".");
					if(step) {
						StartCoroutine(PlayBrewingSound());
					}
					playerStats.currentWeightF -= itemNeeded1.GetComponent<Item>().itemWeight;
					playerStats.currentWeightF -= itemNeeded2.GetComponent<Item>().itemWeight;

					if(itemNeeded1 != null) {
						if(itemNeeded1.GetComponent<Item>().currentStackSize > 1) {
							itemNeeded1.GetComponent<Item>().currentStackSize--;
						}
						else {
							inventory.items.Remove(itemNeeded1);
							Destroy(itemNeeded1.gameObject);
						}
					}

					if(itemNeeded2 != null) {
						if(itemNeeded2.GetComponent<Item>().currentStackSize > 1) {
							itemNeeded2.GetComponent<Item>().currentStackSize--;
						}
						else {
							inventory.items.Remove(itemNeeded2);
							Destroy(itemNeeded2.gameObject);
						}
					}

					craftingCooldown = craftingTimer;
				}
				else if (itemNeeded1 == null && itemNeeded2 == null){
					showMessage.SendTheMessage("I do not have needed ingredients.");
				}
			}
		}
		else {
			showMessage.SendTheMessage("I can not brew this because my inventory is full.");
		}
	}

	public void UpdateUi(AlchemyRecipeSlot slot) {
		activeSlot = slot;
		potionSprite.enabled = true;
		potionSprite.sprite = slot.recipeInSlot.potionPrefab.GetComponent<Item>().itemIcon;
		ingredientsTxt.text = "1x " + slot.ingredientsNeeded[0] + "\n" + "1x " + slot.ingredientsNeeded[1];
		effectTxt.text = slot.effectOfPotion;
	}

	IEnumerator PlayBrewingSound() {
		if(uiAS != null) {
			step = false;
			uiAS.clip = brewingSFX[Random.Range(0, brewingSFX.Length)];
			uiAS.Play();
			yield return new WaitForSeconds(0.25f);
			step = true;
		}
	} 
}
