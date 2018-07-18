using UnityEngine;
using System.Collections;

public class SQ05Undead : MonoBehaviour {

	public NPC npc;
	
	public NPC myNpc;
	
	void Update() {
		if(npc.npcDisposition == NPCDisposition.Hostile) {
			myNpc.npcFaction = NPCFaction.Monster;
			myNpc.npcDisposition = NPCDisposition.Hostile;
		}

		if (npc.npcHealth < 0f) {
			myNpc.npcHealth -= 100;
		}
		if (npc == null) {
			myNpc.npcHealth -= 100;
		}
	}
}
