using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(QuestBase))]
public class QuestBaseEditor : Editor {

	public bool showObjectiveOne;
	public bool showObjectiveTwo;
	public bool showObjectiveThree;
	public bool showObjectiveFour;

	public override void OnInspectorGUI() {

		QuestBase questBase = (QuestBase)target;

		EditorStyles.textField.wordWrap = true;
		EditorStyles.textField.stretchWidth = false;
		EditorStyles.textField.stretchWidth = true;

		GUIStyle foldoutStyle = new GUIStyle(EditorStyles.foldout);
		foldoutStyle.fontStyle = FontStyle.Bold;

		EditorGUILayout.HelpBox("Take note that if you want some objects to enable or disable, you will need to go into debug mode and assing them to their objects lists!", MessageType.None);
		EditorGUILayout.Space ();
		EditorGUILayout.LabelField ("QUEST BASIC INFO", EditorStyles.boldLabel);
		questBase.questId = EditorGUILayout.TextField ("Quest ID:", questBase.questId);
		questBase.questName = EditorGUILayout.TextField ("Quest Name:", questBase.questName);
		EditorGUILayout.LabelField ("Quest Is Triggred By");
		questBase.questTriggeredBy = (QuestTriggeredBy)EditorGUILayout.EnumPopup ("", questBase.questTriggeredBy);

		if (questBase.questTriggeredBy == QuestTriggeredBy.TopicTrigger) {
			questBase.questTrigger = (GameObject)EditorGUILayout.ObjectField ("Quest NPC:", questBase.questTrigger, typeof(GameObject), true);
			if(questBase.questTrigger == null) {
				EditorGUILayout.HelpBox("You must assign the NPC game object from the scene here in order to start the quest using that NPC.", MessageType.Warning);
			}
			else {
				if(!questBase.questTrigger.GetComponent<NPC>()) {
					EditorGUILayout.HelpBox("This is not a valid NPC game object!", MessageType.Warning);
				}
				else {
					questBase.topicTriggerName = EditorGUILayout.TextField("Topic Name", questBase.topicTriggerName);
					questBase.topicTriggerText = EditorGUILayout.TextField("Topic Text", questBase.topicTriggerText, GUILayout.Height((questBase.topicTriggerText.Length / 2f) + 16.5f));
				}
			}			
		}
		else if (questBase.questTriggeredBy == QuestTriggeredBy.CourierTrigger) {
			questBase.questTrigger = (GameObject)EditorGUILayout.ObjectField ("Quest Courier:", questBase.questTrigger, typeof(GameObject), true);
			if(questBase.questTrigger == null) {
				EditorGUILayout.HelpBox("You must assign the NPC Courier game object from the scene here in order to start the quest using that NPC Courier.", MessageType.Warning);
			}
            if ((questBase.courierDialog != null) || ((questBase.courierDialog != ""))) {
                questBase.courierDialog = EditorGUILayout.TextField("Courier Dialog:", questBase.courierDialog, GUILayout.Height((questBase.courierDialog.Length / 2f) + 16.5f));
            }
            else {
                questBase.courierDialog = EditorGUILayout.TextField("Courier Dialog:", questBase.courierDialog, GUILayout.Height((questBase.courierDialog.Length / 2f) + 16.5f));
            }
            if (questBase.courierDialog == "") {
				EditorGUILayout.HelpBox("You must enter here the dialog text that will show up when NPC Courier starts the conversation with the player.", MessageType.Warning);
			}
		}
		else if (questBase.questTriggeredBy == QuestTriggeredBy.AreaTrigger) {
			questBase.questTrigger = (GameObject)EditorGUILayout.ObjectField ("Quest Area:", questBase.questTrigger, typeof(GameObject), true);
			if(questBase.questTrigger == null) {
				EditorGUILayout.HelpBox("You must assign the Game Object with BoxCollider and QuestAreaTrigger script for the quest to start when player collides with that Game Object.", MessageType.Warning);
			}
		}
		else if (questBase.questTriggeredBy == QuestTriggeredBy.OtherQuest) {
			EditorGUILayout.HelpBox("Other quest will automatically start this quest when it is completed.", MessageType.None);
		}
		else if (questBase.questTriggeredBy == QuestTriggeredBy.ObjectTrigger) {
			EditorGUILayout.HelpBox("Not implemented yet.", MessageType.None);
		}
		else if (questBase.questTriggeredBy == QuestTriggeredBy.TimeTrigger) {
			EditorGUILayout.HelpBox("Not implemented yet.", MessageType.None);
		}
		
		GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));

		EditorGUILayout.Space();
		EditorGUILayout.LabelField ("QUEST REQUIREMENTS", EditorStyles.boldLabel);
		EditorGUILayout.LabelField ("Requirements To Start The Quest");
		questBase.questRequrement = (QuestRequirement)EditorGUILayout.EnumPopup ("", questBase.questRequrement);
		if (questBase.questRequrement == QuestRequirement.OtherQuest) {
			questBase.questReqId = EditorGUILayout.TextField ("Other Quest ID:", questBase.questReqId);
			questBase.levelReq = 0;
		} 
		else if (questBase.questRequrement == QuestRequirement.PlayerLevel) {
			questBase.levelReq = EditorGUILayout.IntField ("Required Level:", questBase.levelReq);
			questBase.questReqId = null;
		} 
		else if(questBase.questRequrement == QuestRequirement.OtherQuestAndPlayerLevel) {
			questBase.questReqId = EditorGUILayout.TextField ("Other Quest ID:", questBase.questReqId);
			questBase.levelReq = EditorGUILayout.IntField ("Required Level:", questBase.levelReq);
		}
		else {
			EditorGUILayout.HelpBox("No requirements needed to start the quest.", MessageType.None);
			questBase.questReqId = null;
			questBase.levelReq = 0;
		}

		GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
		EditorGUILayout.Space();

		EditorGUILayout.LabelField ("QUEST OBJECTIVES", EditorStyles.boldLabel);
		
		showObjectiveOne = EditorGUILayout.Foldout(showObjectiveOne, "Objective - 1", foldoutStyle);
		if(showObjectiveOne) {
			if(questBase.qObjective1 != QuestObjective.None && questBase.qObjective1 != QuestObjective.ContinueQuest && questBase.qObjective1 != QuestObjective.FinishQuest) {
				EditorGUILayout.LabelField("Quest Log Description:");
                if (!string.IsNullOrEmpty(questBase.qObjectiveDesc1)) {
                    questBase.qObjectiveDesc1 = EditorGUILayout.TextField(" ", questBase.qObjectiveDesc1, GUILayout.Height((questBase.qObjectiveDesc1.Length / 3f) + 16.5f));
                }
                else {
                    questBase.qObjectiveDesc1 = EditorGUILayout.TextField(" ", questBase.qObjectiveDesc1);
                }
				EditorGUILayout.LabelField("Inform Player Of Quest State:");
				questBase.questStateUpdater1 = (QuestStateUpdater)EditorGUILayout.EnumPopup("", questBase.questStateUpdater1);
				GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
			}
			EditorGUILayout.Space ();
			questBase.qObjective1 = (QuestObjective)EditorGUILayout.EnumPopup ("", questBase.qObjective1);

			if (questBase.qObjective1 == QuestObjective.None) {
				EditorGUILayout.HelpBox ("There is no objective in this quest phase.", MessageType.None);
			} 
			else if (questBase.qObjective1 == QuestObjective.TalkToNpc) {
				EditorGUILayout.HelpBox("Player needs to talk to the specific npc and to activate the specific topic.", MessageType.None);
				questBase.qObject1 = (GameObject)EditorGUILayout.ObjectField ("NPC To Talk To", questBase.qObject1, typeof(GameObject), true);
				if(questBase.qObject1 != null) {
					questBase.qTopic1 = EditorGUILayout.TextField("Topic Name", questBase.qTopic1);
					EditorGUILayout.LabelField("Topic Text");
					questBase.qTopic1Text = EditorGUILayout.TextField(" ", questBase.qTopic1Text, GUILayout.Height((questBase.qTopic1Text.Length / 2f) + 16.5f));
                    
                    SerializedObject event1 = new SerializedObject(target);
                    SerializedProperty event1Property = event1.FindProperty("qTopic1Event");
                    EditorGUILayout.PropertyField(event1Property, true); // True means show children
                    event1.ApplyModifiedProperties(); // Remember to apply modified properties
                }
                questBase.qTopic1ItemGive = EditorGUILayout.Toggle("Will give item?", questBase.qTopic1ItemGive);
				if(questBase.qTopic1ItemGive == true) {
					questBase.qTopic1Item = (GameObject)EditorGUILayout.ObjectField ("Item NPC will Give", questBase.qTopic1Item, typeof(GameObject), true);
				}
			}
			else if(questBase.qObjective1 == QuestObjective.FindNpc) {
				EditorGUILayout.HelpBox("Player needs to get at certain range near the NPC to find him.", MessageType.None);
				questBase.qObject1 = (GameObject)EditorGUILayout.ObjectField ("NPC To Find", questBase.qObject1, typeof(GameObject), true);
				questBase.npcDistance1 = EditorGUILayout.FloatField("Distance Trigger", questBase.npcDistance1);
			}
			else if(questBase.qObjective1 == QuestObjective.KillNpc) {
				EditorGUILayout.HelpBox("Player must kill the specific NPC.", MessageType.None);
				questBase.qObject1 = (GameObject)EditorGUILayout.ObjectField ("NPC To Kill", questBase.qObject1, typeof(GameObject), true);
			}
			else if(questBase.qObjective1 == QuestObjective.GetItem) {
				EditorGUILayout.HelpBox("Player needs to pickup the specific ITEM.", MessageType.None);
				questBase.qObject1 = (GameObject)EditorGUILayout.ObjectField ("Item To Pickup", questBase.qObject1, typeof(GameObject), true);
			}
			else if(questBase.qObjective1 == QuestObjective.DeliverItem) {
				EditorGUILayout.HelpBox("Player needs to deliver the specific ITEM he got in his inventory to the specific NPC.", MessageType.None);
				questBase.qItem1 = (GameObject)EditorGUILayout.ObjectField ("Item To Deliver", questBase.qItem1, typeof(GameObject), true);
				if(questBase.qItem1 != null) {
					questBase.qObject1 = (GameObject)EditorGUILayout.ObjectField ("NPC To Deliver To", questBase.qObject1, typeof(GameObject), true);
					if(questBase.qObject1 != null) {
						questBase.qTopic1 = EditorGUILayout.TextField("Topic Name", questBase.qTopic1);
						EditorGUILayout.LabelField("Topic Text");
						questBase.qTopic1Text = EditorGUILayout.TextField(" ", questBase.qTopic1Text, GUILayout.Height((questBase.qTopic1Text.Length / 3f) + 16.5f));
					}
				}
			}
			else if(questBase.qObjective1 == QuestObjective.ContinueQuest) {
				EditorGUILayout.HelpBox("This quest is set as finished and the next specific quest starts automaticly, making a nice quest chain. This is used only when we want for quest to continue. When this is selected Quest Log Desctiption should stay empty.", MessageType.None);
				if(questBase.questId.Contains("MQ")) {
					questBase.valorReward = EditorGUILayout.IntField ("Valor Rewarder", questBase.valorReward);
				}
				questBase.nextQuest = EditorGUILayout.TextField ("Quest To Start", questBase.nextQuest);
			}
			else if(questBase.qObjective1 == QuestObjective.FinishQuest) {
				EditorGUILayout.HelpBox("This quest is set as finished.", MessageType.None);
				EditorGUILayout.LabelField("Quest Log Description:");
                if (!string.IsNullOrEmpty(questBase.qObjectiveDesc1)) {
                    questBase.qObjectiveDesc1 = EditorGUILayout.TextField(" ", questBase.qObjectiveDesc1, GUILayout.Height((questBase.qObjectiveDesc1.Length / 3f) + 16.5f));
                }
                else {
                    questBase.qObjectiveDesc1 = EditorGUILayout.TextField(" ", questBase.qObjectiveDesc1);
                }
                EditorGUILayout.LabelField("Inform Player Of Quest State:");
				questBase.questStateUpdater1 = (QuestStateUpdater)EditorGUILayout.EnumPopup("", questBase.questStateUpdater1);
				GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
				EditorGUILayout.LabelField("Quest Rewards");
				questBase.itemReward = (GameObject)EditorGUILayout.ObjectField("Item Reward", questBase.itemReward, typeof(GameObject), true);
				questBase.goldReward = EditorGUILayout.IntField("Gold Reward", questBase.goldReward);
				questBase.valorReward = EditorGUILayout.IntField ("Valor Rewarder", questBase.valorReward);
			}
			else if(questBase.qObjective1 == QuestObjective.SearchArea) {
				EditorGUILayout.HelpBox("Not yet implemented.", MessageType.None);
			}
			else if(questBase.qObjective1 == QuestObjective.ReadBook) {
				EditorGUILayout.HelpBox("Not yet implemented.", MessageType.None);
			}
			else if(questBase.qObjective1 == QuestObjective.TravelTo) {
				EditorGUILayout.HelpBox("Player needs to enter the Location Zone specified.", MessageType.None);
				//questBase.travelToZone1 = (LocationZone)EditorGUILayout.ObjectField("Location Zone", questBase.travelToZone1, typeof(LocationZone), true);
			}
			EditorGUILayout.Space();
		}
		showObjectiveTwo = EditorGUILayout.Foldout(showObjectiveTwo, "Objective - 2", foldoutStyle);
		if(showObjectiveTwo) {
			if(questBase.qObjective2 != QuestObjective.None && questBase.qObjective2 != QuestObjective.ContinueQuest && questBase.qObjective2 != QuestObjective.FinishQuest) {
				EditorGUILayout.LabelField("Quest Log Description:");
                if (!string.IsNullOrEmpty(questBase.qObjectiveDesc2)) {
                    questBase.qObjectiveDesc2 = EditorGUILayout.TextField(" ", questBase.qObjectiveDesc2, GUILayout.Height((questBase.qObjectiveDesc2.Length / 3f) + 16.5f));
                }
                else {
                    questBase.qObjectiveDesc2 = EditorGUILayout.TextField(" ", questBase.qObjectiveDesc2);
                }
                EditorGUILayout.LabelField("Inform Player Of Quest State:");
				questBase.questStateUpdater2 = (QuestStateUpdater)EditorGUILayout.EnumPopup("", questBase.questStateUpdater2);
				GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
			}
			EditorGUILayout.Space ();
			questBase.qObjective2 = (QuestObjective)EditorGUILayout.EnumPopup ("", questBase.qObjective2);
			
			if (questBase.qObjective2 == QuestObjective.None) {
				EditorGUILayout.HelpBox ("There is no objective in this quest phase.", MessageType.None);
			} 
			else if (questBase.qObjective2 == QuestObjective.TalkToNpc) {
				EditorGUILayout.HelpBox("Player needs to talk to the specific npc and to activate the specific topic.", MessageType.None);
				questBase.qObject2 = (GameObject)EditorGUILayout.ObjectField ("NPC To Talk To", questBase.qObject2, typeof(GameObject), true);
				if(questBase.qObject2 != null) {
					questBase.qTopic2 = EditorGUILayout.TextField("Topic Name", questBase.qTopic2);
					EditorGUILayout.LabelField("Topic Text");
					questBase.qTopic2Text = EditorGUILayout.TextField(" ", questBase.qTopic2Text, GUILayout.Height((questBase.qTopic2Text.Length / 3f) + 16.5f));
                    SerializedObject event2 = new SerializedObject(target);
                    SerializedProperty event2Property = event2.FindProperty("qTopic2Event");
                    EditorGUILayout.PropertyField(event2Property, true); // True means show children
                    event2.ApplyModifiedProperties(); // Remember to apply modified properties                
                }
                questBase.qTopic2ItemGive = EditorGUILayout.Toggle("Will give item?", questBase.qTopic2ItemGive);
				if(questBase.qTopic2ItemGive == true) {
					questBase.qTopic2Item = (GameObject)EditorGUILayout.ObjectField ("Item NPC will Give", questBase.qTopic2Item, typeof(GameObject), true);
				}
			}
			else if(questBase.qObjective2 == QuestObjective.FindNpc) {
				EditorGUILayout.HelpBox("Player needs to get at certain range near the NPC to find him.", MessageType.None);
				questBase.qObject2 = (GameObject)EditorGUILayout.ObjectField ("NPC To Find", questBase.qObject2, typeof(GameObject), true);
				questBase.npcDistance2 = EditorGUILayout.FloatField("Distance Trigger", questBase.npcDistance2);
			}
			else if(questBase.qObjective2 == QuestObjective.KillNpc) {
				EditorGUILayout.HelpBox("Player must kill the specific NPC.", MessageType.None);
				questBase.qObject2 = (GameObject)EditorGUILayout.ObjectField ("NPC To Kill", questBase.qObject2, typeof(GameObject), true);
			}
			else if(questBase.qObjective2 == QuestObjective.GetItem) {
				EditorGUILayout.HelpBox("Player needs to pickup the specific ITEM.", MessageType.None);
				questBase.qObject2 = (GameObject)EditorGUILayout.ObjectField ("Item To Pickup", questBase.qObject2, typeof(GameObject), true);
			}
			else if(questBase.qObjective2 == QuestObjective.DeliverItem) {
				EditorGUILayout.HelpBox("Player needs to deliver the specific ITEM he got in his inventory to the specific NPC.", MessageType.None);
				questBase.qItem2 = (GameObject)EditorGUILayout.ObjectField ("Item To Deliver", questBase.qItem2, typeof(GameObject), true);
				if(questBase.qItem2 != null) {
					questBase.qObject2 = (GameObject)EditorGUILayout.ObjectField ("NPC To Deliver To", questBase.qObject2, typeof(GameObject), true);
					if(questBase.qObject2 != null) {
						questBase.qTopic2 = EditorGUILayout.TextField("Topic Name", questBase.qTopic2);
						EditorGUILayout.LabelField("Topic Text");
						questBase.qTopic2Text = EditorGUILayout.TextField(" ", questBase.qTopic2Text, GUILayout.Height((questBase.qTopic2Text.Length / 3f) + 16.5f));
					}
				}
			}
			else if(questBase.qObjective2 == QuestObjective.ContinueQuest) {
				EditorGUILayout.HelpBox("This quest is set as finished and the next specific quest starts automaticly, making a nice quest chain. This is used only when we want for quest to continue. When this is selected Quest Log Desctiption should stay empty.", MessageType.None);
				if(questBase.questId.Contains("MQ")) {
					questBase.valorReward = EditorGUILayout.IntField ("Valor Rewarder", questBase.valorReward);
				}
				questBase.nextQuest = EditorGUILayout.TextField ("Quest To Start", questBase.nextQuest);
			}
			else if(questBase.qObjective2 == QuestObjective.FinishQuest) {
				EditorGUILayout.HelpBox("This quest is set as finished.", MessageType.None);
				EditorGUILayout.LabelField("Quest Log Description:");
                if (!string.IsNullOrEmpty(questBase.qObjectiveDesc2)) {
                    questBase.qObjectiveDesc2 = EditorGUILayout.TextField(" ", questBase.qObjectiveDesc2, GUILayout.Height((questBase.qObjectiveDesc2.Length / 3f) + 16.5f));
                }
                else {
                    questBase.qObjectiveDesc2 = EditorGUILayout.TextField(" ", questBase.qObjectiveDesc2);
                }
                EditorGUILayout.LabelField("Inform Player Of Quest State:");
				questBase.questStateUpdater2 = (QuestStateUpdater)EditorGUILayout.EnumPopup("", questBase.questStateUpdater2);
				GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
				EditorGUILayout.LabelField("Quest Rewards");
				questBase.itemReward = (GameObject)EditorGUILayout.ObjectField("Item Reward", questBase.itemReward, typeof(GameObject), true);
				questBase.goldReward = EditorGUILayout.IntField("Gold Reward", questBase.goldReward);
				questBase.valorReward = EditorGUILayout.IntField ("Valor Rewarder", questBase.valorReward);
			}
			else if(questBase.qObjective2 == QuestObjective.SearchArea) {
				EditorGUILayout.HelpBox("Not yet implemented.", MessageType.None);
			}
			else if(questBase.qObjective2 == QuestObjective.ReadBook) {
				EditorGUILayout.HelpBox("Not yet implemented.", MessageType.None);
			}
			else if(questBase.qObjective2 == QuestObjective.TravelTo) {
				EditorGUILayout.HelpBox("Player needs to enter the Location Zone specified.", MessageType.None);
				//questBase.travelToZone2 = (LocationZone)EditorGUILayout.ObjectField("Location Zone", questBase.travelToZone2, typeof(LocationZone), true);				EditorGUILayout.HelpBox("Not yet implemented.", MessageType.None);
			}
			EditorGUILayout.Space();
		}
		showObjectiveThree = EditorGUILayout.Foldout(showObjectiveThree, "Objective - 3", foldoutStyle);
		if(showObjectiveThree) {
			if(questBase.qObjective3 != QuestObjective.None && questBase.qObjective3 != QuestObjective.ContinueQuest && questBase.qObjective3 != QuestObjective.FinishQuest) {
				EditorGUILayout.LabelField("Quest Log Description:");
                if (!string.IsNullOrEmpty(questBase.qObjectiveDesc3)) {
                    questBase.qObjectiveDesc3 = EditorGUILayout.TextField(" ", questBase.qObjectiveDesc3, GUILayout.Height((questBase.qObjectiveDesc3.Length / 3f) + 16.5f));
                }
                else {
                    questBase.qObjectiveDesc3 = EditorGUILayout.TextField(" ", questBase.qObjectiveDesc3);
                }
                EditorGUILayout.LabelField("Inform Player Of Quest State:");
				questBase.questStateUpdater3 = (QuestStateUpdater)EditorGUILayout.EnumPopup("", questBase.questStateUpdater3);
				GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
			}
			EditorGUILayout.Space ();
			questBase.qObjective3 = (QuestObjective)EditorGUILayout.EnumPopup ("", questBase.qObjective3);
			
			if (questBase.qObjective3 == QuestObjective.None) {
				EditorGUILayout.HelpBox ("There is no objective in this quest phase.", MessageType.None);
			} 
			else if (questBase.qObjective3 == QuestObjective.TalkToNpc) {
				EditorGUILayout.HelpBox("Player needs to talk to the specific npc and to activate the specific topic.", MessageType.None);
				questBase.qObject3 = (GameObject)EditorGUILayout.ObjectField ("NPC To Talk To", questBase.qObject3, typeof(GameObject), true);
				if(questBase.qObject3 != null) {
					questBase.qTopic3 = EditorGUILayout.TextField("Topic Name", questBase.qTopic3);
					EditorGUILayout.LabelField("Topic Text");
					questBase.qTopic3Text = EditorGUILayout.TextField(" ", questBase.qTopic3Text, GUILayout.Height((questBase.qTopic3Text.Length / 3f) + 16.5f));
                    SerializedObject event3 = new SerializedObject(target);
                    SerializedProperty event3Property = event3.FindProperty("qTopic3Event");
                    EditorGUILayout.PropertyField(event3Property, true); // True means show children
                    event3.ApplyModifiedProperties(); // Remember to apply modified properties                
                }
                questBase.qTopic3ItemGive = EditorGUILayout.Toggle("Will give item?", questBase.qTopic3ItemGive);
				if(questBase.qTopic1ItemGive == true) {
					questBase.qTopic3Item = (GameObject)EditorGUILayout.ObjectField ("Item NPC will Give", questBase.qTopic3Item, typeof(GameObject), true);
				}
			}
			else if(questBase.qObjective3 == QuestObjective.FindNpc) {
				EditorGUILayout.HelpBox("Player needs to get at certain range near the NPC to find him.", MessageType.None);
				questBase.qObject3 = (GameObject)EditorGUILayout.ObjectField ("NPC To Find", questBase.qObject3, typeof(GameObject), true);
				questBase.npcDistance3 = EditorGUILayout.FloatField("Distance Trigger", questBase.npcDistance3);
			}
			else if(questBase.qObjective3 == QuestObjective.KillNpc) {
				EditorGUILayout.HelpBox("Player must kill the specific NPC.", MessageType.None);
				questBase.qObject3 = (GameObject)EditorGUILayout.ObjectField ("NPC To Kill", questBase.qObject3, typeof(GameObject), true);
			}
			else if(questBase.qObjective3 == QuestObjective.GetItem) {
				EditorGUILayout.HelpBox("Player needs to pickup the specific ITEM.", MessageType.None);
				questBase.qObject3 = (GameObject)EditorGUILayout.ObjectField ("Item To Pickup", questBase.qObject3, typeof(GameObject), true);
			}
			else if(questBase.qObjective3 == QuestObjective.DeliverItem) {
				EditorGUILayout.HelpBox("Player needs to deliver the specific ITEM he got in his inventory to the specific NPC.", MessageType.None);
				questBase.qItem3 = (GameObject)EditorGUILayout.ObjectField ("Item To Deliver", questBase.qItem3, typeof(GameObject), true);
				if(questBase.qItem3 != null) {
					questBase.qObject3 = (GameObject)EditorGUILayout.ObjectField ("NPC To Deliver To", questBase.qObject3, typeof(GameObject), true);
					if(questBase.qObject3 != null) {
						questBase.qTopic3 = EditorGUILayout.TextField("Topic Name", questBase.qTopic3);
						EditorGUILayout.LabelField("Topic Text");
						questBase.qTopic3Text = EditorGUILayout.TextField(" ", questBase.qTopic3Text, GUILayout.Height((questBase.qTopic3Text.Length / 3f) + 16.5f));
					}
				}
			}
			else if(questBase.qObjective3 == QuestObjective.ContinueQuest) {
				EditorGUILayout.HelpBox("This quest is set as finished and the next specific quest starts automaticly, making a nice quest chain. This is used only when we want for quest to continue. When this is selected Quest Log Desctiption should stay empty.", MessageType.None);
				if(questBase.questId.Contains("MQ")) {
					questBase.valorReward = EditorGUILayout.IntField ("Valor Rewarder", questBase.valorReward);
				}
				questBase.nextQuest = EditorGUILayout.TextField ("Quest To Start", questBase.nextQuest);
			}
			else if(questBase.qObjective3 == QuestObjective.FinishQuest) {
				EditorGUILayout.HelpBox("This quest is set as finished.", MessageType.None);
				EditorGUILayout.LabelField("Quest Log Description:");
                if (!string.IsNullOrEmpty(questBase.qObjectiveDesc3)) {
                    questBase.qObjectiveDesc3 = EditorGUILayout.TextField(" ", questBase.qObjectiveDesc3, GUILayout.Height((questBase.qObjectiveDesc3.Length / 3f) + 16.5f));
                }
                else {
                    questBase.qObjectiveDesc3 = EditorGUILayout.TextField(" ", questBase.qObjectiveDesc3);
                }
                EditorGUILayout.LabelField("Inform Player Of Quest State:");
				questBase.questStateUpdater3 = (QuestStateUpdater)EditorGUILayout.EnumPopup("", questBase.questStateUpdater3);
				GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
				EditorGUILayout.LabelField("Quest Rewards");
				questBase.itemReward = (GameObject)EditorGUILayout.ObjectField("Item Reward", questBase.itemReward, typeof(GameObject), true);
				questBase.goldReward = EditorGUILayout.IntField("Gold Reward", questBase.goldReward);
				questBase.valorReward = EditorGUILayout.IntField ("Valor Rewarder", questBase.valorReward);
			}
			else if(questBase.qObjective3 == QuestObjective.SearchArea) {
				EditorGUILayout.HelpBox("Not yet implemented.", MessageType.None);
			}
			else if(questBase.qObjective3 == QuestObjective.ReadBook) {
				EditorGUILayout.HelpBox("Not yet implemented.", MessageType.None);
			}
			else if(questBase.qObjective3 == QuestObjective.TravelTo) {
				EditorGUILayout.HelpBox("Player needs to enter the Location Zone specified.", MessageType.None);
				//questBase.travelToZone3 = (LocationZone)EditorGUILayout.ObjectField("Location Zone", questBase.travelToZone3, typeof(LocationZone), true);
			}
			EditorGUILayout.Space();
		}
		showObjectiveFour = EditorGUILayout.Foldout(showObjectiveFour, "Objective - 4", foldoutStyle);
		if(showObjectiveFour) {
			EditorGUILayout.Space ();
			questBase.qObjective4 = (QuestObjective)EditorGUILayout.EnumPopup ("", questBase.qObjective4);

			if(questBase.qObjective4 == QuestObjective.ContinueQuest) {
				EditorGUILayout.HelpBox("This quest is set as finished and the next specific quest starts automaticly, making a nice quest chain. This is used only when we want for quest to continue. When this is selected Quest Log Desctiption should stay empty.", MessageType.None);
				if(questBase.questId.Contains("MQ")) {
					questBase.valorReward = EditorGUILayout.IntField ("Valor Rewarder", questBase.valorReward);
				}
				questBase.nextQuest = EditorGUILayout.TextField ("Quest To Start", questBase.nextQuest);
			}
			else if(questBase.qObjective4 == QuestObjective.FinishQuest) {
				EditorGUILayout.HelpBox("This quest is set as finished.", MessageType.None);
				EditorGUILayout.LabelField("Quest Log Description:");
                if (!string.IsNullOrEmpty(questBase.qObjectiveDesc4)) {
                    questBase.qObjectiveDesc4 = EditorGUILayout.TextField(" ", questBase.qObjectiveDesc4, GUILayout.Height((questBase.qObjectiveDesc4.Length / 3f) + 16.5f));
                }
                else {
                    questBase.qObjectiveDesc4 = EditorGUILayout.TextField(" ", questBase.qObjectiveDesc4);
                }
                EditorGUILayout.LabelField("Inform Player Of Quest State:");
				questBase.questStateUpdater4 = (QuestStateUpdater)EditorGUILayout.EnumPopup("", questBase.questStateUpdater4);
				GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
				EditorGUILayout.LabelField("Quest Rewards");
				questBase.itemReward = (GameObject)EditorGUILayout.ObjectField("Item Reward", questBase.itemReward, typeof(GameObject), true);
				questBase.goldReward = EditorGUILayout.IntField("Gold Reward", questBase.goldReward);
				questBase.valorReward = EditorGUILayout.IntField ("Valor Rewarder", questBase.valorReward);
			}
			else if(questBase.qObjective4 != QuestObjective.None) {
				EditorGUILayout.HelpBox("This objective can only be set to continue or to finish the quest. If you have more objectives make it to Continue Quest and make new quest. It will automatically make a chain quest and in game it will look as it is one quest, not two.", MessageType.Error);
			}
			EditorGUILayout.Space();
		}
	}
}

