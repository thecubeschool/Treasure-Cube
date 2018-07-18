using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AlchemyRecipe : MonoBehaviour {

	public GameObject potionPrefab;

	public string recipeName;
	public List<string> ingredientsNeeded;
	[TextArea(1, 10)]
	public string effectOfPotion;

}
