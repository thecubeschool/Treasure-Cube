using UnityEngine;
using UnityEditor;

public class NpcListing : EditorWindow {
	[MenuItem("The Fallen Chronicler/Utility/Npc Listing")]

	public static void  ShowWindow () {
		EditorWindow.GetWindow(typeof(NpcListing));
	}
	
	void OnGUI () {
		if(GUILayout.Button("List Npcs")) {
			GameObject[] allNpcs = GameObject.FindGameObjectsWithTag("Npc");

			float elinians = 0;
			float ariyans = 0;
			float sintarians = 0;
			float koronians = 0;
			float elevirians = 0;
			float bauks = 0;
			float wolves = 0;

			foreach(GameObject n in allNpcs) {
				if(n.GetComponent<NPC>().race == CharacterRace.Elinian) {
					elinians++;
				}
				else if(n.GetComponent<NPC>().race == CharacterRace.Ariyan) {
					ariyans++;
				}
				else if(n.GetComponent<NPC>().race == CharacterRace.Sintarian) {
					sintarians++;
				}
				else if(n.GetComponent<NPC>().race == CharacterRace.Koronian) {
					koronians++;
				}
				else if(n.GetComponent<NPC>().race == CharacterRace.Elevirian) {
					elevirians++;
				}
				else if(n.GetComponent<NPC>().race == CharacterRace.Bauk) {
					bauks++;
				}
				else if(n.GetComponent<NPC>().race == CharacterRace.BeastWolf) {
					wolves++;
				}
			}
			Debug.Log("NPCS Listing \n\n" +
			          "Elinians: " + elinians.ToString() + "\n\n" +
			          "Ariyans: " + ariyans.ToString() + "\n\n" +
			          "Sintarians: " + sintarians.ToString() + "\n\n" +
			          "Koronians: " + koronians.ToString() + "\n\n" +
			          "Elevirians: " + elevirians.ToString() + "\n\n" +
			          "Bauks: " + bauks.ToString() + "\n\n" +
			          "Wolves: " + wolves.ToString() + "\n\n" +
			          "ALL: " + allNpcs.Length.ToString());
		}
	}
}
