using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ConversationBase : MonoBehaviour {

	public GameObject topicPrefab;

	public Canvas canvas;
	public RectTransform dialogRect;

	public Text dialogBox;

	private int numberOfTopicsToSet;
	private int topicsInstanitated;

	private List<GameObject> allTopics;

	private PlayerStats playerStats;

	void Awake() {
		playerStats = GameObject.Find("[Player]").GetComponent<PlayerStats>();
	}
	
	public void StartTheConversation(GameObject npc, string welcomingText) {

        Vector3 targetNpc = npc.transform.position;
        targetNpc.y = playerStats.transform.position.y;
        playerStats.transform.LookAt(targetNpc);

        allTopics = new List<GameObject>();

		dialogBox.text = string.Empty;
        dialogBox.text = welcomingText;
        dialogBox.rectTransform.position.Set (dialogBox.rectTransform.position.x,
		                                      0f,
		                                      dialogBox.rectTransform.position.z);

		numberOfTopicsToSet = npc.GetComponent<NPCTopicHolder>().topics.Count;

		if(numberOfTopicsToSet > 0) {
			if(topicsInstanitated < numberOfTopicsToSet) {
				foreach(NPCTopic t in npc.GetComponent<NPCTopicHolder>().topics) {
					GameObject newTopic = (GameObject)Instantiate(topicPrefab);
					RectTransform rectTopic = newTopic.GetComponent<RectTransform>();

					newTopic.GetComponent<ConversationTopic>().storeTopic = t.shopTopic;
					newTopic.GetComponent<ConversationTopic>().storeGold = t.shopGold;
					newTopic.GetComponent<ConversationTopic>().storeItems = t.itemsInShop;
					newTopic.GetComponent<ConversationTopic>().shopHolder = t.shopHolder;
					newTopic.GetComponent<ConversationTopic>().showOnce = t.showOnce;
					newTopic.GetComponent<ConversationTopic>().specialTopic = t.specialTopic;
					newTopic.GetComponent<ConversationTopic>().specialTopicClass = t.specialTopicClass;
					newTopic.GetComponent<ConversationTopic>().classValue = t.classValue;
					newTopic.GetComponent<ConversationTopic>().classObject = t.classObject;
					newTopic.GetComponent<ConversationTopic>().npcTopic = t;
					newTopic.GetComponent<ConversationTopic>().topic = t.topic;
					newTopic.GetComponent<Text>().text = t.topic;
					newTopic.GetComponent<ConversationTopic>().questId = t.questId;
					newTopic.GetComponent<ConversationTopic>().questPhase = t.questPhase;
					newTopic.GetComponent<ConversationTopic>().topicTrigger = t.topicTrigger;
					newTopic.GetComponent<ConversationTopic>().triggerObject = t.triggerObject;
                    newTopic.GetComponent<ConversationTopic>().thisTopicEvent = t.onTopicEvent;

					newTopic.GetComponent<ConversationTopic>().dialogLine = t.dialogLine[Random.Range (0, t.dialogLine.Count)];

					string dilLine = newTopic.GetComponent<ConversationTopic>().dialogLine;
					string tmpBroSis;
					string tmpName;
					string tmpCulture;
						
					if(dilLine.Contains("BRO/SIS")) {
						if(playerStats.playerGender == CharacterGender.Male) {
							tmpBroSis = dilLine.Replace("BRO/SIS", "brother");
							newTopic.GetComponent<ConversationTopic>().dialogLine = tmpBroSis;
						}
						else {
							tmpBroSis = dilLine.Replace("BRO/SIS", "sister");
							newTopic.GetComponent<ConversationTopic>().dialogLine = tmpBroSis;
						}
					}
					if(dilLine.Contains("PC_NAME")) {
						if(playerStats.playerName != "" && playerStats.playerName != " ") {
							tmpName = dilLine.Replace("PC_NAME", playerStats.playerName);
							newTopic.GetComponent<ConversationTopic>().dialogLine = tmpName;
						}
						else {
							tmpName = dilLine.Replace("PC_NAME", "Gamer");
							newTopic.GetComponent<ConversationTopic>().dialogLine = tmpName;
						}
					}
					if(dilLine.Contains("CULTURE_REF")) {
						tmpCulture = dilLine.Replace("CULTURE_REF", playerStats.playerCulture.ToString());
						newTopic.GetComponent<ConversationTopic>().dialogLine = tmpCulture;
					}

					newTopic.name = "Topic_" + t.topic;
					newTopic.transform.SetParent(transform.Find("Topics/TopicsHolder").transform);

					allTopics.Add(newTopic);

					if(topicsInstanitated == 0) {
						rectTopic.localPosition = new Vector3(0, 0);
					}
					else {
						rectTopic.localPosition = new Vector3(0, topicsInstanitated * -26);
					}
					rectTopic.localScale = new Vector3(1, 1, 1);

					transform.Find("Topics/TopicsHolder").transform.GetComponent<RectTransform>().sizeDelta = new Vector2(194, topicsInstanitated * 26);

					topicsInstanitated++;
				}
			}
		}
	}

	public void EndTheConversation() {
		dialogBox.text = string.Empty;

		dialogBox.rectTransform.position.Set (dialogBox.rectTransform.position.x,
		                                      0f,
		                                      dialogBox.rectTransform.position.z);

		numberOfTopicsToSet = 0;
		topicsInstanitated = 0;

		if(allTopics.Count > 0) {
			foreach(GameObject go in allTopics) {
				Destroy(go);
			}
		}
	}
}
