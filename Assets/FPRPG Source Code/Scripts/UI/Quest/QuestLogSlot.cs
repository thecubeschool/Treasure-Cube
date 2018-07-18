using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class QuestLogSlot : MonoBehaviour, IPointerClickHandler {

	public bool trackThisQuest;
	public GameObject checkmarkerTracking;

	public Text questNameTxt;
	public Text dateQuestTakenTxt;
	public Text questDescriptionTxt;
	private Image img;
	
	public string questName;
	public string questId;
	public string dateQuestTaken;
	public string questDescription;

	public QuestBase questInSlot;

	private GameManager gm;

	void Start() {
		gm = GameObject.Find("[GameManager]").GetComponent<GameManager>();

		img = GetComponent<Image>();

		img.enabled = true;
		questNameTxt.text = questName;

		if(dateQuestTaken != null) {
			dateQuestTakenTxt.text = dateQuestTaken;
		}
		if(questDescription != null) {
			questDescriptionTxt.text = questDescription;
		}
	}
		
	void ActivateThisQuest() {
		if(gm.trackingQuestId != "") {
			QuestLogSlot[] tmpQs = transform.parent.GetComponentsInChildren<QuestLogSlot>();

			foreach(QuestLogSlot qL in tmpQs) {
				if(qL.questId == questId && qL.questId == gm.trackingQuestId) {
					gm.trackingQuestId = string.Empty;
				}
				else if(qL.questId == questId && qL.questId != gm.trackingQuestId) {
					gm.trackingQuestId = questId;
				}
			}
		}
		else {
			trackThisQuest = true;
			gm.trackingQuestId = questId;
		}
	}

	void Update() {
		if(questInSlot == true) {
			questDescription = questInSlot.currentDescription;

			if(questInSlot.questDone == true) {
				//Destroy(gameObject);
			}

			if(gm.trackingQuestId == questId) {
				trackThisQuest = true;
			}
			else {
				trackThisQuest = false;
			}

			if(trackThisQuest == true) {
				checkmarkerTracking.SetActive(true);
			}
			else {
				checkmarkerTracking.SetActive(false);
			}
		}
	}

	public void OnPointerClick(PointerEventData eventData) {
		if(eventData.button == PointerEventData.InputButton.Left) {
			ActivateThisQuest();
		}
	}
}
