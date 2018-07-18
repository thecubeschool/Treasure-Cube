//New quests are added to Log Manager by activeQuests.Add(questBase)
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class QuestLogManager : MonoBehaviour {

	public GameObject questPrefab;
	public List<QuestBase> activeQuests;
	public List<GameObject> questHolder;

	public int questsToActivate;
	[Space(6f)]
	public int activeQuestsInt;
	public int questsFinished;
	
	private TodClock todClock;
	private GameManager gm;
	public UISkillsAndAttributes uiSA;

	void Awake() {
		todClock = GameObject.Find("[GameManager]").GetComponent<TodClock>();
		gm = GameObject.Find("[GameManager]").GetComponent<GameManager>();
	}

	public void UpdateQuestsInfo() {
		uiSA.activeQuests.text = "Active Quests: " + activeQuests.Count.ToString();
		uiSA.questsCompleted.text = "Quests Completed: " + questsFinished.ToString();
	}

	public void UpdateQuestList() {

		if(activeQuests.Count > 0 && questHolder.Count > 0) {
			questHolder.Clear();
		}

		questHolder = new List<GameObject>();

		if(activeQuests.Count > 0) {
			if(questsToActivate < activeQuests.Count) {
				foreach(QuestBase aqb in activeQuests) {
					if(aqb.questDone == false) {
						GameObject newQuest = (GameObject)Instantiate(questPrefab);
						newQuest.transform.SetParent(transform);
						newQuest.GetComponent<QuestLogSlot>().questName = aqb.questName;
						newQuest.GetComponent<QuestLogSlot>().questId = aqb.questId;
						newQuest.GetComponent<QuestLogSlot>().questDescription = aqb.currentDescription;
						newQuest.GetComponent<QuestLogSlot>().dateQuestTaken = todClock.dayCount + " " + todClock.currentMonth + ", " + todClock.currentYear;
						newQuest.GetComponent<QuestLogSlot>().questInSlot = aqb;
						
						RectTransform rectQuest = newQuest.GetComponent<RectTransform>();
						
						newQuest.name = "Quest_" + aqb.questName;

						if(gm.trackingQuestId == newQuest.GetComponent<QuestLogSlot>().questId) {
							newQuest.GetComponent<QuestLogSlot>().trackThisQuest = true;
						}
						else{
							newQuest.GetComponent<QuestLogSlot>().trackThisQuest = false;
						}

						if(questsToActivate == 0) {
							rectQuest.localPosition = new Vector3(0, 0);
						}
						else {
							rectQuest.localPosition = new Vector3(0, questsToActivate * -50);
						}
						rectQuest.localScale = new Vector3(1, 1, 1);
						transform.GetComponent<RectTransform>().sizeDelta = new Vector2(296, questsToActivate * 62);
						
						questHolder.Add(newQuest);
						questsToActivate++;
					}
				}
			}
		}
	}

	public void DestroyQuestList() {
		questsToActivate = 0;
		foreach (Transform t in transform) {
			Destroy(t.gameObject);
		}
		questHolder.Clear ();
	}

	public bool AddQuestToLog(QuestBase questToAdd) {
		activeQuests.Add(questToAdd);
		return true;
	}

	public bool RemoveQuestFromLog(QuestBase questToRemove) {
		activeQuests.Remove(questToRemove);
		return true;
	}
}
