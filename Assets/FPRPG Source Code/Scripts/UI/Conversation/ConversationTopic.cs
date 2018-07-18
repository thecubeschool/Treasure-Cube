using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;

public class ConversationTopic : MonoBehaviour {

	public bool storeTopic = false;
	public int storeGold;
	public List<GameObject> storeItems;
	public Transform shopHolder;

	public SpecialTopicClass specialTopicClass;
	public int classValue;
	public GameObject classObject;
	public bool specialTopic = false;
	public bool showOnce = false;
	public NPCTopic npcTopic;

	[Tooltip("Here we put the name of the topic, what will be seen inside dialog window.")]
	public string topic;
	[Tooltip("If topic activates/updates a quest, here we put name of the quest so the engine can find it inside the Quests GameObject.")]
	public string questId;
	[Tooltip("If topic activates/updates a quest, here we set phase for that quest.")]
	public QuestPhase questPhase;

	[Tooltip("Here we write down text that will be shown when this topic is active.")]
	[TextArea(2, 40)]
	public string dialogLine;
	public int dialogPicked;

	[HideInInspector]
	public ConversationBase conversationBase;
	[HideInInspector]
	public Transform questsGo;

	public TopicTrigger topicTrigger;
	public GameObject triggerObject;
	
	private QuestBase questBase;

	private bool gotQuest;
	private GameObject specialTopicer;
	private GameObject storeUi;
	private Text stTxt;
	private SpecialTopicYesNoActivator stActivator;
	private UIManager uiManager;
	private ShowMessage showMessage;

    public UnityEvent thisTopicEvent;

	void Start() {

		conversationBase = GameObject.Find("_UICanvasGame").GetComponentInChildren<ConversationBase>();
		specialTopicer = GameObject.Find("_UICanvasGame").transform.Find("SpecialTopicer").gameObject;
		storeUi = GameObject.Find("_UICanvasGame").transform.Find("StoreWindow").gameObject;
		uiManager = GameObject.Find("_UICanvasGame").GetComponent<UIManager>();
		showMessage = GameObject.Find("_UICanvasGame").GetComponentInChildren<ShowMessage>();

		if(specialTopic == true) {
			stTxt = specialTopicer.transform.Find("TopicTxt").GetComponent<Text>();
			stActivator = specialTopicer.GetComponent<SpecialTopicYesNoActivator>();

			stActivator.specialTopicClass = specialTopicClass;
			stActivator.classValue = classValue;
			stActivator.classObject = classObject;
			stTxt.text = dialogLine;

			stActivator.conversationTopic = gameObject;
			stActivator.npcTopic = npcTopic.gameObject;
		}

		if(questId != "" && GameObject.Find("[Quests]")) {
			questsGo = GameObject.Find("[Quests]").transform;
			questBase = questsGo.Find(questId).GetComponent<QuestBase>();
		}
	}

	public void ActivateTheTopic() {

		if(specialTopic) {
			specialTopicer.SetActive(true);
		}

		if(storeTopic) {
			uiManager.dialogUI.SetActive(false);
			storeUi.SetActive(true);
			npcTopic.GetTheItemsFromTheShopTransform();
			storeUi.GetComponentInChildren<StoreMerchantsInventory>().merchantsGold = storeGold;
			storeUi.GetComponentInChildren<StoreMerchantsInventory>().merchantItems = storeItems;
			storeUi.GetComponentInChildren<StoreMerchantsInventory>().merchantStoreHolder = shopHolder;
			storeUi.GetComponentInChildren<StoreMerchantsInventory>().RESETMerchantItems();
			storeUi.GetComponentInChildren<StoreMerchantsInventory>().STOREGetMerchantItems();
			storeUi.GetComponentInChildren<StorePlayerInventory>().RESETPlayerStore();
			storeUi.GetComponentInChildren<StorePlayerInventory>().STOREGetPlayerItems();
		}

		conversationBase.dialogBox.text = string.Empty;
        conversationBase.dialogBox.text = dialogLine;
        conversationBase.dialogBox.rectTransform.anchoredPosition.Set(conversationBase.dialogBox.rectTransform.position.x,
                                                                      0f);

        thisTopicEvent.Invoke();

        if (questId != "" && questBase != null) {
			questBase.questPhase = questPhase;
		}

		if(topicTrigger == TopicTrigger.NpcChangeToHostile) {
			triggerObject.GetComponent<NPC>().npcDisposition = NPCDisposition.Hostile;
		}
		else if(topicTrigger == TopicTrigger.DeliverItemToNpc) {
			uiManager.inventoryUI.GetComponent<WeightedInventory>().RemoveItemFromInventory(triggerObject);
			showMessage.SendTheMessage("Item " + triggerObject.GetComponent<Item>().itemName + " removed from inventory.");
			Destroy(triggerObject);
		}
		else if(topicTrigger == TopicTrigger.NpcGiveItem) {
			uiManager.inventoryUI.GetComponent<WeightedInventory>().AddItemToInventory(triggerObject);
			showMessage.SendTheMessage("Item " + triggerObject.GetComponent<Item>().itemName + " added to inventory.");
		}

		if(showOnce == true) {
			if(npcTopic.gameObject != null) {
				Destroy(npcTopic.gameObject);
                showOnce = false;
            }
		}
	}
}
