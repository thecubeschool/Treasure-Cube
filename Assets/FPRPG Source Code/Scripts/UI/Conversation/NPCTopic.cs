using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public enum TopicType {
	Standard = 0,
	MerchantShop = 1,
	PropertyBuying = 2,
}

public enum SpecialTopicClass {
	BuyHorse = 0,
	BuyBoat = 1,
	BuyShip = 2,
	BuyHouse = 3,
	BuyInn = 4,
	BuyShop = 5,
	BuyBlacksmith = 6,
	FastTravel = 7,
    RentRoom = 8,
}

public enum TopicTrigger {
	None = 0,
	NpcChangeToHostile = 1,
	DeliverItemToNpc = 2,
	NpcGiveItem = 3,
}

public class NPCTopic : MonoBehaviour {

	public TopicType topicType;

	public bool shopTopic = false;
	public int shopGold = 0;
	public Transform shopHolder;
	[HideInInspector]
	public List<GameObject> itemsInShop;
	private int itemsShopGot = -1;

	[Header("Special Topic Vars")]
	public SpecialTopicClass specialTopicClass;
	public int classValue;
	[Tooltip("If we are using fast travel, here we will put transform where player will 'trave'/spawn.")]
	public GameObject classObject;
	public bool specialTopic = false;

	[Header("Normal Topic Vars")]
	public bool showOnce = false;
	[Tooltip("Here we put the name of the topic, what will be seen inside dialog window.")]
	public string topic;
	[Tooltip("If topic activates/updates a quest, here we put name of the quest so the engine can find it inside the Quests GameObject.")]
	public string questId;
	[Tooltip("If topic activates/updates a quest, here we set phase for that quest.")]
	public QuestPhase questPhase;

	[Tooltip("Here we write down text that will be shown when this topic is active. Here we can write many different types of npc saying something different about this topic.")]
	[TextArea(2, 40)]
	public List<string> dialogLine;

	public TopicTrigger topicTrigger;
	public GameObject triggerObject;

    public UnityEvent onTopicEvent;

	public void GetTheItemsFromTheShopTransform() {

		itemsInShop.Clear();

		if(topicType == TopicType.MerchantShop) {
			foreach(Transform child in shopHolder) {
				itemsInShop.Add(child.gameObject);
				itemsShopGot++;
			}
		}
	}
}
