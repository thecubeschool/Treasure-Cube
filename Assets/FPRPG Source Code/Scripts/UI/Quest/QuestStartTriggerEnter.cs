using UnityEngine;
using System.Collections;

public class QuestStartTriggerEnter : MonoBehaviour {

	public QuestBase questToStart;

	void OnTriggerEnter(Collider col) {
		if(col.tag == "Player") {
			questToStart.questStarted = true;
			questToStart.questPhase = QuestPhase.Phase1;
		}
	}
}
