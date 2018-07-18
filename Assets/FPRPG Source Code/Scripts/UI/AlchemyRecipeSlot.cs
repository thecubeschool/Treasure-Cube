using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class AlchemyRecipeSlot : MonoBehaviour, IPointerClickHandler {

	public AlchemyUi alchemyUi;
	public Text recipeName;
	public AlchemyRecipe recipeInSlot;

	public List<string> ingredientsNeeded;
	[TextArea(1, 10)]
	public string effectOfPotion;

	public void OnPointerClick(PointerEventData eventData) {
		if(eventData.button == PointerEventData.InputButton.Left) {
			alchemyUi.UpdateUi(this);
		}
	}
}
