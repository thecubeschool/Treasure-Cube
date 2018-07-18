/// <summary>
/// Quests are scripts that are put on game objects insinde [Quest] gamobject in hierarchy. There are still some things one who makes quests to tak
/// in account. Every TalkToNpc dialog that is related to quest must always be put as a last choice inside NPCDialog script. Phase 4 is phase just for
/// finishing quest. Every quest that takes longer or grows into other quest must be put as a next quest that will start after Phase 4 ends.
/// Just like TalkToNpc, setting the phase 1 is used to start quest from dialog.
/// </summary>
/// 
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

public enum QuestTriggeredBy {
	TopicTrigger = 0,
	AreaTrigger = 1,
	ObjectTrigger = 2,
	TimeTrigger = 3,
	OtherQuest = 4,
	CourierTrigger = 5,
}

public enum QuestRequirement {
	None = 0,
	OtherQuest = 1,
	PlayerLevel = 2,
	OtherQuestAndPlayerLevel = 3,
}

public enum QuestObjective {
	None = 0,
	FindNpc = 1,
	KillNpc = 2,
	TalkToNpc = 3,
	SearchArea = 4,
	ReadBook = 5,
	TravelTo = 6,
	FinishQuest = 7,
	ContinueQuest = 8,
	GetItem = 9,
	DeliverItem = 10,
    Misc = 11,
}

public enum QuestPhase {
	Phase0 = 0,
	Phase1 = 1,
	Phase2 = 2,
	Phase3 = 3,
	Phase4 = 4,
}

public enum QuestStateUpdater {
	ByMessage = 0,
	ByQuestPopupDialog = 1,
}

public class QuestBase : MonoBehaviour {

    public bool questStarted;
    public bool questFailed;
    public QuestPhase questPhase;
    public string questId;
    public string questName;
    public int levelReq = 1;
    public string questReqId;
    public QuestRequirement questRequrement;
    public bool prerequestsMet = false;
    public string dateQuestStarted;

    public string currentDescription;
    public GameObject currentQuestTarget;

    public QuestTriggeredBy questTriggeredBy;
    public GameObject questTrigger;
    public string courierDialog;
    public string topicTriggerName;
    public string topicTriggerText = " ";

    public GameObject topicPrefab;

    public QuestObjective qObjective1;
    public float npcDistance1 = 7.5f;
    public GameObject qObject1;
    public GameObject qItem1;
    public string qTopic1;
    public string qTopic1Text;
    public UnityEvent qTopic1Event;
    public bool qTopic1ItemGive;
    public GameObject qTopic1Item;
    public string qObjectiveDesc1;
    public bool qObjective1Done;
    public List<GameObject> enableGoPhase1;
    public bool enable1;
    public List<GameObject> disalbeGoPhase1;
    public bool showPopupDesc1;
    public bool disable1;
    //public LocationZone travelToZone1;

    public QuestObjective qObjective2;
    public float npcDistance2 = 7.5f;
    public GameObject qObject2;
    public GameObject qItem2;
    public string qTopic2;
    public string qTopic2Text;
    public UnityEvent qTopic2Event;
    public bool qTopic2ItemGive;
    public GameObject qTopic2Item;
    public string qObjectiveDesc2;
    public bool qObjective2Done;
    public List<GameObject> enableGoPhase2;
    public bool enable2;
    public List<GameObject> disalbeGoPhase2;
    public bool showPopupDesc2;
    public bool disable2;
    //public LocationZone travelToZone2;

    public QuestObjective qObjective3;
    public float npcDistance3 = 7.5f;
    public GameObject qObject3;
    public GameObject qItem3;
    public string qTopic3;
    public string qTopic3Text;
    public UnityEvent qTopic3Event;
    public bool qTopic3ItemGive;
    public GameObject qTopic3Item;
    public string qObjectiveDesc3;
    public bool qObjective3Done;
    public List<GameObject> enableGoPhase3;
    public bool enable3;
    public List<GameObject> disalbeGoPhase3;
    public bool showPopupDesc3;
    public bool disable3;
    //public LocationZone travelToZone3;

    public QuestObjective qObjective4;
    public GameObject qObject4;
    public GameObject qItem4;
    public string qTopic4;
    public string qTopic4Text;
    public UnityEvent qTopic4Event;
    public string qObjectiveDesc4;
    public bool qObjective4Done;
    public List<GameObject> enableGoPhase4;
    public bool enable4;
    public List<GameObject> disalbeGoPhase4;
    public bool showPopupDesc4;
    public bool disable4;

    public QuestStateUpdater questStateUpdater1;
    public QuestStateUpdater questStateUpdater2;
    public QuestStateUpdater questStateUpdater3;
    public QuestStateUpdater questStateUpdater4;

    public int goldReward;
    public GameObject itemReward;
    public int valorReward;

    public bool questDone;

    public string nextQuest;

    private GameObject playerGo;
    private PlayerLevelManager lvlManager;
    private PlayerStats playerStats;
    private ShowMessage showMessage;
    private TodClock todClock;
    private QuestLogManager questLogManager;
    public UIManager uiManager;
    private GameManager gm;

    private bool messageQuest1;
    private bool messageQuest2;
    private bool messageQuest3;
    private bool messageQuestDone;

    private bool qTopic1B;
    private bool qTopic2B;
    private bool qTopic3B;
    private bool qTopic4B;

    public bool phaseOne;
    public bool phaseTwo;
    public bool phaseThree;
    public bool phaseFour;

    private UISkillsAndAttributes uiSA;

    public bool isQuestLoaded;

    void Awake() {
        playerGo = GameObject.Find("[Player]");
        lvlManager = playerGo.GetComponent<PlayerLevelManager>();
        playerStats = playerGo.GetComponent<PlayerStats>();
        showMessage = GameObject.Find("_UICanvasGame").GetComponentInChildren<ShowMessage>();
        todClock = GameObject.Find("[GameManager]").GetComponent<TodClock>();
        uiManager = GameObject.Find("_UICanvasGame").GetComponent<UIManager>();
        uiSA = uiManager.uISkillsAndAttributes;
        questLogManager = uiManager.GetComponentInChildren<QuestLogManager>();
        gm = GameObject.Find("[GameManager]").GetComponent<GameManager>();
    }

    void Start() {

        topicPrefab = Resources.Load("TopicNpc") as GameObject;

        InvokeRepeating("CheckIfRequirementAreMeet", 0, 2f);

        questId = transform.name;

        if (questStarted == false) {
            if (questTriggeredBy == QuestTriggeredBy.TopicTrigger) {
                if (questTrigger == null) {
                    Debug.LogWarning("Npc that starts the quest must be put inside this field!");
                }
                else if (questTrigger != null) {
                    Transform topicsTmp = questTrigger.transform.Find("topics").transform;
                    GameObject theTopic = Instantiate(topicPrefab as GameObject);
                    theTopic.GetComponent<NPCTopic>().topic = topicTriggerName;
                    theTopic.GetComponent<NPCTopic>().dialogLine[0] = topicTriggerText;
                    theTopic.GetComponent<NPCTopic>().showOnce = true;
                    theTopic.GetComponent<NPCTopic>().questId = questId;
                    theTopic.GetComponent<NPCTopic>().questPhase = QuestPhase.Phase1;

                    theTopic.transform.SetParent(topicsTmp);
                }
            }
            if (questTriggeredBy == QuestTriggeredBy.CourierTrigger) {
                if (questTrigger == null) {
                    Debug.LogWarning("Courier that starts the quest must be put inside this field!");
                }
                else {
                    if (!questDone && !questFailed) {
                        questTrigger.GetComponent<NPCAiNavmesh>().approachDialog = courierDialog;
                        questTrigger.GetComponent<NPCAiNavmesh>().shownApproachDialog = true;
                        questTrigger.GetComponent<NPCAiNavmesh>().deliveredPackage = false;
                        questTrigger.GetComponent<NPCAiNavmesh>().startsQuest = true;
                        questTrigger.GetComponent<NPCAiNavmesh>().questID = questId;
                    }
                    else {
                        Destroy(questTrigger);
                    }
                }
            }
        }
    }

    void CheckIfRequirementAreMeet() {
        if (lvlManager.playerLevel >= levelReq) {
            prerequestsMet = true;
        }
        else {
            prerequestsMet = false;
            questStarted = false;
        }
    }

    void Update() {

        if (questStarted == false) {
            if (questTriggeredBy == QuestTriggeredBy.AreaTrigger) {
                if (questTrigger.GetComponent<QuestAreaTrigger>().playerInTrigger == true) {
                    questPhase = QuestPhase.Phase1;
                    questStarted = true;
                }
            }
        }

        if (questStarted == false) {
            if (questPhase == QuestPhase.Phase1) {
                questStarted = true;
            }
        }

        if (questFailed == false) {
            if (questDone == false) { //If this quest is not done already
                if (questStarted == true) { //If this quest has started
                    if (prerequestsMet == true) { //If player meets the minimum lvl requirement
                                                  //////////////////////////////
                        if (questPhase == QuestPhase.Phase1) {
                            if (qObjective1Done == false) {
                                if (qObject1 != null) {
                                    if (qObject1.activeSelf == false) {
                                        qObject1.SetActive(true);
                                    }
                                }
                                currentDescription = qObjectiveDesc1;
                                currentQuestTarget = qObject1;
                                /////////////////////////////
                                if (enable1 == false) {
                                    foreach (GameObject go in enableGoPhase1) {
                                        go.SetActive(true);
                                    }
                                    enable1 = true;
                                }
                                if (disable1 == false) {
                                    foreach (GameObject go in disalbeGoPhase1) {
                                        go.SetActive(false);
                                    }
                                    disable1 = true;
                                }

                                if (messageQuest1 == false) {
                                    if (isQuestLoaded == false) {
                                        if (questTriggeredBy == QuestTriggeredBy.AreaTrigger || questTriggeredBy == QuestTriggeredBy.ObjectTrigger ||
                                           questTriggeredBy == QuestTriggeredBy.OtherQuest || questTriggeredBy == QuestTriggeredBy.TimeTrigger) {
                                            dateQuestStarted = todClock.dayCount + " " + todClock.currentMonth.ToString() + ", " + todClock.currentYear;
                                            if (questStateUpdater1 == QuestStateUpdater.ByQuestPopupDialog) {
                                                if (showPopupDesc1 == false) {
                                                    if (questTriggeredBy == QuestTriggeredBy.OtherQuest) {
                                                        uiManager.QuestInfoWindowPopup("Quest Updated", questName, dateQuestStarted, qObjectiveDesc1);
                                                        showPopupDesc1 = true;
                                                    }
                                                    else {
                                                        uiManager.QuestInfoWindowPopup("Quest Started", questName, dateQuestStarted, qObjectiveDesc1);
                                                        showPopupDesc1 = true;
                                                    }
                                                }
                                            }
                                            else if (questStateUpdater1 == QuestStateUpdater.ByMessage) {
                                                showMessage.SendTheMessage("Quest Updated: " + questName);
                                            }
                                        }
                                        else {
                                            if (questStateUpdater1 == QuestStateUpdater.ByQuestPopupDialog) {
                                                uiManager.QuestInfoWindowPopup("Quest Started", questName, dateQuestStarted, qObjectiveDesc1);
                                            }
                                            else if (questStateUpdater1 == QuestStateUpdater.ByMessage) {
                                                showMessage.SendTheMessage("Quest Started: " + questName);
                                            }
                                        }
                                    }
                                    questLogManager.AddQuestToLog(this);
                                    messageQuest1 = true;
                                }

                                if (qObjective1 == QuestObjective.FindNpc) {
                                    float distanceToNpc = Vector3.Distance(playerGo.transform.position, qObject1.transform.position);

                                    if (distanceToNpc < npcDistance1) {
                                        questPhase = QuestPhase.Phase2;
                                        messageQuest2 = false;
                                        qObjective1Done = true;
                                    }
                                }
                                if (qObjective1 == QuestObjective.TalkToNpc) {//If there is first
                                    if (qTopic1B == false) {
                                        GameObject questTopic1 = (GameObject)Instantiate(topicPrefab);
                                        questTopic1.transform.SetParent(qObject1.transform.Find("topics").transform);
                                        questTopic1.GetComponent<NPCTopic>().topic = qTopic1;
                                        questTopic1.GetComponent<NPCTopic>().dialogLine[0] = qTopic1Text;
                                        questTopic1.GetComponent<NPCTopic>().onTopicEvent = qTopic1Event;
                                        questTopic1.GetComponent<NPCTopic>().showOnce = false;
                                        questTopic1.GetComponent<NPCTopic>().questId = questId;
                                        if (qTopic1ItemGive) {
                                            questTopic1.GetComponent<NPCTopic>().topicTrigger = TopicTrigger.NpcGiveItem;
                                            questTopic1.GetComponent<NPCTopic>().triggerObject = qTopic1Item;
                                            showMessage.SendTheMessage("Item " + qTopic1Item.GetComponent<Item>().itemName + " added to invntory.");
                                        }
                                        questTopic1.GetComponent<NPCTopic>().questPhase = QuestPhase.Phase2;
                                        qTopic1B = true;
                                    }
                                }
                                if (qObjective1 == QuestObjective.KillNpc) {//If there is first
                                    if (qObject1 != null) {
                                        if (qObject1.GetComponent<NPC>().npcDisposition != NPCDisposition.Hostile) {
                                            qObject1.GetComponent<NPC>().npcFaction = NPCFaction.Bandits;
                                            qObject1.GetComponent<NPC>().npcDisposition = NPCDisposition.Hostile;
                                        }
                                        if (qObject1.GetComponent<NPC>().npcHealth <= 0) {
                                            questPhase = QuestPhase.Phase2;
                                            messageQuest2 = false;
                                            qObjective1Done = true;
                                        }
                                    }
                                    else if (qObject1 == null) {
                                        questPhase = QuestPhase.Phase2;
                                        messageQuest2 = false;
                                        qObjective1Done = true;
                                    }
                                }
                                if (qObjective1 == QuestObjective.TravelTo) {//If there is first
                                                                             //if(travelToZone1.playerIsHere == true) {
                                                                             //	questPhase = QuestPhase.Phase2;
                                                                             //	messageQuest2 = false;
                                                                             //	qObjective1Done = true;
                                                                             //}
                                }
                                if (qObjective1 == QuestObjective.SearchArea) {//If there is first
                                }
                                if (qObjective1 == QuestObjective.ReadBook) {//If there is first
                                }
                                if (qObjective1 == QuestObjective.FinishQuest) {
                                    if (goldReward > 0 && itemReward == null) {
                                        showMessage.SendTheMessage("Quest Completed: " + questName + "\n" +
                                                                   goldReward + " gold coins added to inventory.");
                                        playerGo.GetComponent<PlayerStats>().playerMoney += goldReward;
                                        goldReward = 0;
                                    }
                                    else if (goldReward == 0 && itemReward != null) {
                                        showMessage.SendTheMessage("Quest Completed: " + questName + "\n" +
                                                                   itemReward.GetComponent<Item>().itemName + " added to inventory.");
                                        GameObject itemRewardInst = GameObject.Instantiate(itemReward) as GameObject;
                                        uiManager.inventoryUI.GetComponent<WeightedInventory>().AddItemToInventory(itemRewardInst);
                                        itemReward = null;
                                    }
                                    else if (goldReward > 0 && itemReward != null) {
                                        showMessage.SendTheMessage("Quest Completed: " + questName + "\n" +
                                                                   itemReward.GetComponent<Item>().itemName + " and \n" +
                                                                   goldReward + " gold coins added to inventory.");
                                        playerGo.GetComponent<PlayerStats>().playerMoney += goldReward;
                                        goldReward = 0;
                                        GameObject itemRewardInst = GameObject.Instantiate(itemReward) as GameObject;
                                        uiManager.inventoryUI.GetComponent<WeightedInventory>().AddItemToInventory(itemRewardInst);
                                        itemReward = null;
                                    }
                                    if (showPopupDesc1 == false && qObjectiveDesc1 != "") {
                                        uiManager.QuestInfoWindowPopup("Quest Completed", questName, dateQuestStarted, qObjectiveDesc1);
                                        showPopupDesc1 = true;
                                    }
                                    playerStats.valorScore += valorReward;
                                    if (playerStats.crimeScore >= valorReward) {
                                        playerStats.crimeScore -= valorReward;
                                    }
                                    else {
                                        playerStats.crimeScore = 0;
                                    }
                                    uiSA.valorScore.text = "Valor score: " + playerStats.valorScore;
                                    uiSA.crimeScore.text = "Crime score: " + playerStats.crimeScore;
                                    playerStats.reputationScore += 5;
                                    uiSA.reputationScore.text = "Reputation score: " + playerStats.reputationScore;
                                    questLogManager.RemoveQuestFromLog(this);
                                    questStarted = false;
                                    questDone = true;
                                    //REWARDS

                                    questLogManager.questsFinished++;
                                }
                                if (qObjective1 == QuestObjective.ContinueQuest) {
                                    if (questId.Contains("MQ")) {
                                        playerStats.valorScore += valorReward;
                                        if (playerStats.crimeScore >= valorReward) {
                                            playerStats.crimeScore -= valorReward;
                                        }
                                        else {
                                            playerStats.crimeScore = 0;
                                        }
                                        uiSA.valorScore.text = "Valor score: " + playerStats.valorScore;
                                        uiSA.crimeScore.text = "Crime score: " + playerStats.crimeScore;
                                        playerStats.reputationScore += 10;
                                        uiSA.reputationScore.text = "Reputation score: " + playerStats.reputationScore;
                                    }
                                    GameObject.Find("[Quests]").transform.Find(nextQuest).GetComponent<QuestBase>().questStarted = true;
                                    GameObject.Find("[Quests]").transform.Find(nextQuest).GetComponent<QuestBase>().questPhase = QuestPhase.Phase1;
                                    questStarted = false;
                                    questDone = true;
                                }
                                if (qObjective1 == QuestObjective.GetItem) {
                                    foreach (GameObject i in uiManager.inventoryUI.GetComponent<WeightedInventory>().items) {
                                        if (i == qObject1) {
                                            showMessage.SendTheMessage("Quest Updated: " + questName);
                                            questPhase = QuestPhase.Phase2;
                                            messageQuest2 = false;
                                            qObjective1Done = true;
                                        }
                                    }
                                }
                                if (qObjective1 == QuestObjective.DeliverItem) {
                                    foreach (GameObject i in uiManager.inventoryUI.GetComponent<WeightedInventory>().items) {
                                        if (i == qItem1) {
                                            if (qTopic1B == false) {
                                                GameObject questTopic1 = (GameObject)Instantiate(topicPrefab);
                                                questTopic1.transform.SetParent(qObject1.transform.Find("topics").transform);
                                                questTopic1.GetComponent<NPCTopic>().topic = qTopic1;
                                                questTopic1.GetComponent<NPCTopic>().dialogLine[0] = qTopic1Text;
                                                questTopic1.GetComponent<NPCTopic>().showOnce = false;
                                                questTopic1.GetComponent<NPCTopic>().questId = questId;
                                                questTopic1.GetComponent<NPCTopic>().topicTrigger = TopicTrigger.DeliverItemToNpc;
                                                questTopic1.GetComponent<NPCTopic>().triggerObject = i;
                                                questTopic1.GetComponent<NPCTopic>().questPhase = QuestPhase.Phase2;
                                                qTopic1B = true;
                                            }
                                            messageQuest2 = false;
                                            qObjective1Done = true;
                                        }
                                    }
                                }
                            }
                        }
                        else if (questPhase == QuestPhase.Phase2) {
                            if (qObject2 != null) {
                                if (qObject2.activeSelf == false) {
                                    qObject2.SetActive(true);
                                }
                            }
                            currentDescription = qObjectiveDesc2;
                            currentQuestTarget = qObject2;

                            if (qObjective1Done == false) {
                                qObjective1Done = true;
                            }
                            if (qObjective2Done == false) {
                                //////////////////////////////

                                if (enable2 == false) {
                                    foreach (GameObject go in enableGoPhase2) {
                                        go.SetActive(true);
                                    }
                                    enable2 = true;
                                }
                                if (disable2 == false) {
                                    foreach (GameObject go in disalbeGoPhase2) {
                                        if (go.GetComponent<NPC>() != null) {
                                            go.SetActive(false);
                                        }
                                        disable2 = true;
                                    }

                                    if (qObjective2 != QuestObjective.FinishQuest) {
                                        if (messageQuest2 == false && isQuestLoaded == false) {
                                            if (questStateUpdater2 == QuestStateUpdater.ByQuestPopupDialog) {
                                                if (showPopupDesc2 == false) {
                                                    string dateTmp = todClock.dayCount + " " + todClock.currentMonth.ToString() + ", " + todClock.currentYear;
                                                    uiManager.QuestInfoWindowPopup("Quest Updated", questName, dateTmp, qObjectiveDesc2);
                                                    showPopupDesc2 = true;
                                                }
                                            }
                                            else if (questStateUpdater2 == QuestStateUpdater.ByMessage) {
                                                showMessage.SendTheMessage("Quest Updated: " + questName);
                                            }
                                            messageQuest2 = true;
                                        }
                                        else {
                                            if(isQuestLoaded) {
                                                questLogManager.AddQuestToLog(this);
                                            }
                                        }

                                        questLogManager.questsFinished++;
                                    }

                                    if (qObjective2 == QuestObjective.FindNpc) {
                                        float distanceToNpc = Vector3.Distance(playerGo.transform.position, qObject2.transform.position);

                                        if (distanceToNpc < npcDistance2) {
                                            questPhase = QuestPhase.Phase3;
                                            messageQuest3 = false;
                                            qObjective2Done = true;
                                        }
                                    }
                                    if (qObjective2 == QuestObjective.TalkToNpc) {//If there is first
                                        if (qTopic2B == false) {
                                            GameObject questTopic2 = (GameObject)Instantiate(topicPrefab);
                                            questTopic2.transform.SetParent(qObject2.transform.Find("topics").transform);
                                            questTopic2.GetComponent<NPCTopic>().topic = qTopic2;
                                            questTopic2.GetComponent<NPCTopic>().dialogLine[0] = qTopic2Text;
                                            questTopic2.GetComponent<NPCTopic>().onTopicEvent = qTopic2Event;
                                            questTopic2.GetComponent<NPCTopic>().showOnce = false;
                                            questTopic2.GetComponent<NPCTopic>().questId = questId;
                                            if (qTopic2ItemGive) {
                                                questTopic2.GetComponent<NPCTopic>().topicTrigger = TopicTrigger.NpcGiveItem;
                                                questTopic2.GetComponent<NPCTopic>().triggerObject = qTopic2Item;
                                                showMessage.SendTheMessage("Item " + qTopic2Item.GetComponent<Item>().itemName + " added to invntory.");
                                            }
                                            questTopic2.GetComponent<NPCTopic>().questPhase = QuestPhase.Phase3;
                                            qTopic2B = true;
                                        }
                                        messageQuest3 = false;
                                    }
                                    if (qObjective2 == QuestObjective.KillNpc) {//If there is first
                                        if (qObject2 != null) {
                                            if (qObject2.GetComponent<NPC>().npcDisposition != NPCDisposition.Hostile) {
                                                qObject2.GetComponent<NPC>().npcFaction = NPCFaction.Bandits;
                                                qObject2.GetComponent<NPC>().npcDisposition = NPCDisposition.Hostile;
                                            }
                                            if (qObject2.GetComponent<NPC>().npcHealth <= 0) {
                                                questPhase = QuestPhase.Phase3;
                                                messageQuest3 = false;
                                                qObjective2Done = true;
                                            }
                                        }
                                        else if (qObject2 == null) {
                                            questPhase = QuestPhase.Phase3;
                                            messageQuest3 = false;
                                            qObjective1Done = true;
                                        }
                                    }
                                    if (qObjective2 == QuestObjective.TravelTo) {//If there is first
                                                                                 //if(travelToZone2.playerIsHere == true) {
                                                                                 //	questPhase = QuestPhase.Phase3;
                                                                                 //	messageQuest3 = false;
                                                                                 //	qObjective2Done = true;
                                                                                 //}
                                    }
                                    if (qObjective2 == QuestObjective.SearchArea) {//If there is first
                                        qObjective2Done = true;
                                    }
                                    if (qObjective2 == QuestObjective.ReadBook) {//If there is first
                                        qObjective2Done = true;
                                    }
                                    if (qObjective2 == QuestObjective.FinishQuest) {
                                        if (goldReward > 0 && itemReward == null) {
                                            showMessage.SendTheMessage("Quest Completed: " + questName + "\n" +
                                                                       goldReward + " gold coins added to inventory.");
                                            playerGo.GetComponent<PlayerStats>().playerMoney += goldReward;
                                            goldReward = 0;
                                        }
                                        else if (goldReward == 0 && itemReward != null) {
                                            showMessage.SendTheMessage("Quest Completed: " + questName + "\n" +
                                                                       itemReward.GetComponent<Item>().itemName + " added to inventory.");
                                            GameObject itemRewardInst = GameObject.Instantiate(itemReward) as GameObject;
                                            uiManager.inventoryUI.GetComponent<WeightedInventory>().AddItemToInventory(itemRewardInst);
                                            itemReward = null;
                                        }
                                        else if (goldReward > 0 && itemReward != null) {
                                            showMessage.SendTheMessage("Quest Completed: " + questName + "\n" +
                                                                       itemReward.GetComponent<Item>().itemName + " and \n" +
                                                                       goldReward + " gold coins added to inventory.");
                                            playerGo.GetComponent<PlayerStats>().playerMoney += goldReward;
                                            goldReward = 0;
                                            GameObject itemRewardInst = GameObject.Instantiate(itemReward) as GameObject;
                                            uiManager.inventoryUI.GetComponent<WeightedInventory>().AddItemToInventory(itemRewardInst);
                                            itemReward = null;
                                        }
                                        if (showPopupDesc2 == false && qObjectiveDesc2 != "") {
                                            uiManager.QuestInfoWindowPopup("Quest Completed", questName, dateQuestStarted, qObjectiveDesc2);
                                            showPopupDesc2 = true;
                                        }
                                        playerStats.valorScore += valorReward;
                                        if (playerStats.crimeScore >= valorReward) {
                                            playerStats.crimeScore -= valorReward;
                                        }
                                        else {
                                            playerStats.crimeScore = 0;
                                        }
                                        uiSA.valorScore.text = "Valor score: " + playerStats.valorScore;
                                        uiSA.crimeScore.text = "Crime score: " + playerStats.crimeScore;
                                        playerStats.reputationScore += 5;
                                        uiSA.reputationScore.text = "Reputation score: " + playerStats.reputationScore;
                                        questLogManager.RemoveQuestFromLog(this);
                                        questStarted = false;
                                        questDone = true;
                                        //REWARDS

                                        questLogManager.questsFinished++;
                                    }
                                    if (qObjective2 == QuestObjective.ContinueQuest) {
                                        if (questId.Contains("MQ")) {
                                            playerStats.valorScore += valorReward;
                                            if (playerStats.crimeScore >= valorReward) {
                                                playerStats.crimeScore -= valorReward;
                                            }
                                            else {
                                                playerStats.crimeScore = 0;
                                            }
                                            uiSA.valorScore.text = "Valor score: " + playerStats.valorScore;
                                            uiSA.crimeScore.text = "Crime score: " + playerStats.crimeScore;
                                            playerStats.reputationScore += 10;
                                            uiSA.reputationScore.text = "Reputation score: " + playerStats.reputationScore;
                                        }
                                        GameObject.Find("[Quests]").transform.Find(nextQuest).GetComponent<QuestBase>().questStarted = true;
                                        GameObject.Find("[Quests]").transform.Find(nextQuest).GetComponent<QuestBase>().questPhase = QuestPhase.Phase1;
                                        questStarted = false;
                                        questDone = true;
                                    }
                                    if (qObjective2 == QuestObjective.GetItem) {
                                        foreach (GameObject i in uiManager.inventoryUI.GetComponent<WeightedInventory>().items) {
                                            if (i == qObject2) {
                                                showMessage.SendTheMessage("Quest Updated: " + questName);
                                                questPhase = QuestPhase.Phase3;
                                                messageQuest3 = false;
                                                qObjective1Done = true;
                                            }
                                        }
                                    }
                                    if (qObjective2 == QuestObjective.DeliverItem) {
                                        foreach (GameObject i in uiManager.inventoryUI.GetComponent<WeightedInventory>().items) {
                                            if (i == qItem2) {
                                                if (qTopic2B == false) {
                                                    GameObject questTopic2 = (GameObject)Instantiate(topicPrefab);
                                                    questTopic2.transform.SetParent(qObject2.transform.Find("topics").transform);
                                                    questTopic2.GetComponent<NPCTopic>().topic = qTopic2;
                                                    questTopic2.GetComponent<NPCTopic>().dialogLine[0] = qTopic2Text;
                                                    questTopic2.GetComponent<NPCTopic>().showOnce = false;
                                                    questTopic2.GetComponent<NPCTopic>().questId = questId;
                                                    questTopic2.GetComponent<NPCTopic>().topicTrigger = TopicTrigger.DeliverItemToNpc;
                                                    questTopic2.GetComponent<NPCTopic>().triggerObject = i;
                                                    questTopic2.GetComponent<NPCTopic>().questPhase = QuestPhase.Phase3;
                                                    qTopic2B = true;
                                                }
                                                messageQuest3 = false;
                                                qObjective2Done = true;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else if (questPhase == QuestPhase.Phase3) {
                            if (qObject3 != null) {
                                if (qObject3.activeSelf == false) {
                                    qObject3.SetActive(true);
                                }
                            }
                            currentDescription = qObjectiveDesc3;
                            currentQuestTarget = qObject3;

                            if (qObjective2Done == false) {
                                qObjective2Done = true;
                            }
                            if (qObjective3Done == false) {
                                //////////////////////////////

                                if (enable3 == false) {
                                    foreach (GameObject go in enableGoPhase3) {
                                        if (go.GetComponent<NPC>() != null) {
                                            go.SetActive(true);
                                        }
                                        enable3 = true;
                                    }
                                    if (disable3 == false) {
                                        foreach (GameObject go in disalbeGoPhase3) {
                                            if (go.GetComponent<NPC>() != null) {
                                                go.SetActive(false);
                                            }
                                            disable3 = true;
                                        }

                                        if (qObjective3 == QuestObjective.FindNpc) {
                                            float distanceToNpc = Vector3.Distance(playerGo.transform.position, qObject3.transform.position);

                                            if (distanceToNpc < npcDistance3) {
                                                questPhase = QuestPhase.Phase4;
                                                qObjective3Done = true;
                                            }
                                        }
                                        if (qObjective3 == QuestObjective.TalkToNpc) {//If there is first
                                            if (qTopic3B == false) {
                                                GameObject questTopic3 = (GameObject)Instantiate(topicPrefab);
                                                questTopic3.transform.SetParent(qObject3.transform.Find("topics").transform);
                                                questTopic3.GetComponent<NPCTopic>().topic = qTopic3;
                                                questTopic3.GetComponent<NPCTopic>().dialogLine[0] = qTopic3Text;
                                                questTopic3.GetComponent<NPCTopic>().onTopicEvent = qTopic3Event;
                                                questTopic3.GetComponent<NPCTopic>().showOnce = false;
                                                questTopic3.GetComponent<NPCTopic>().questId = questId;
                                                if (qTopic3ItemGive) {
                                                    questTopic3.GetComponent<NPCTopic>().topicTrigger = TopicTrigger.NpcGiveItem;
                                                    questTopic3.GetComponent<NPCTopic>().triggerObject = qTopic3Item;
                                                    showMessage.SendTheMessage("Item " + qTopic3Item.GetComponent<Item>().itemName + " added to invntory.");
                                                }
                                                questTopic3.GetComponent<NPCTopic>().questPhase = QuestPhase.Phase4;
                                                qTopic3B = true;
                                            }
                                        }
                                        if (qObjective3 == QuestObjective.KillNpc) {//If there is first
                                            if (qObject3 != null) {
                                                if (qObject3.GetComponent<NPC>().npcDisposition != NPCDisposition.Hostile) {
                                                    qObject3.GetComponent<NPC>().npcFaction = NPCFaction.Bandits;
                                                    qObject3.GetComponent<NPC>().npcDisposition = NPCDisposition.Hostile;
                                                }
                                                if (qObject3.GetComponent<NPC>().npcHealth <= 0) {
                                                    questPhase = QuestPhase.Phase4;
                                                    qObjective3Done = true;
                                                }
                                            }
                                            else if (qObject3 == null) {
                                                questPhase = QuestPhase.Phase4;
                                                messageQuest3 = false;
                                                qObjective1Done = true;
                                            }
                                        }
                                        if (qObjective3 == QuestObjective.TravelTo) {//If there is first
                                                                                     //if(travelToZone3.playerIsHere == true) {
                                                                                     //	questPhase = QuestPhase.Phase4;
                                                                                     //	messageQuest3 = false;
                                                                                     //	qObjective3Done = true;
                                                                                     //}
                                        }
                                        if (qObjective3 == QuestObjective.SearchArea) {//If there is first
                                        }
                                        if (qObjective3 == QuestObjective.ReadBook) {//If there is first
                                        }
                                        if (qObjective3 == QuestObjective.FinishQuest) {
                                            if (goldReward > 0 && itemReward == null) {
                                                showMessage.SendTheMessage("Quest Completed: " + questName + "\n" +
                                                                           goldReward + " gold coins added to inventory.");
                                                playerGo.GetComponent<PlayerStats>().playerMoney += goldReward;
                                                goldReward = 0;
                                            }
                                            else if (goldReward == 0 && itemReward != null) {
                                                showMessage.SendTheMessage("Quest Completed: " + questName + "\n" +
                                                                           itemReward.GetComponent<Item>().itemName + " added to inventory.");
                                                GameObject itemRewardInst = GameObject.Instantiate(itemReward) as GameObject;
                                                uiManager.inventoryUI.GetComponent<WeightedInventory>().AddItemToInventory(itemRewardInst);
                                                itemReward = null;
                                            }
                                            else if (goldReward > 0 && itemReward != null) {
                                                showMessage.SendTheMessage("Quest Completed: " + questName + "\n" +
                                                                           itemReward.GetComponent<Item>().itemName + " and \n" +
                                                                           goldReward + " gold coins added to inventory.");
                                                playerGo.GetComponent<PlayerStats>().playerMoney += goldReward;
                                                goldReward = 0;
                                                GameObject itemRewardInst = GameObject.Instantiate(itemReward) as GameObject;
                                                uiManager.inventoryUI.GetComponent<WeightedInventory>().AddItemToInventory(itemRewardInst);
                                                itemReward = null;
                                            }
                                            if (showPopupDesc3 == false && qObjectiveDesc3 != "") {
                                                if (isQuestLoaded == false) {
                                                    if (questStateUpdater3 == QuestStateUpdater.ByQuestPopupDialog) {
                                                        if (showPopupDesc3 == false) {
                                                            uiManager.QuestInfoWindowPopup("Quest Completed", questName, dateQuestStarted, qObjectiveDesc3);
                                                            showPopupDesc3 = true;
                                                        }
                                                    }
                                                    else if (questStateUpdater3 == QuestStateUpdater.ByMessage) {
                                                        showMessage.SendTheMessage("Quest Completed: " + questName);
                                                    }
                                                }
                                            }
                                            playerStats.valorScore += valorReward;
                                            if (playerStats.crimeScore >= valorReward) {
                                                playerStats.crimeScore -= valorReward;
                                            }
                                            else {
                                                playerStats.crimeScore = 0;
                                            }
                                            uiSA.valorScore.text = "Valor score: " + playerStats.valorScore;
                                            uiSA.crimeScore.text = "Crime score: " + playerStats.crimeScore;
                                            playerStats.reputationScore += 5;
                                            uiSA.reputationScore.text = "Reputation score: " + playerStats.reputationScore;
                                            questLogManager.RemoveQuestFromLog(this);
                                            questStarted = false;
                                            questDone = true;
                                            //REWARDS

                                            questLogManager.questsFinished++;
                                        }
                                        else {
                                            if (messageQuest3 == false && isQuestLoaded == false) {
                                                if (questStateUpdater3 == QuestStateUpdater.ByQuestPopupDialog) {
                                                    if (showPopupDesc3 == false) {
                                                        string dateTmp = todClock.dayCount + " " + todClock.currentMonth.ToString() + ", " + todClock.currentYear;
                                                        uiManager.QuestInfoWindowPopup("Quest Updated", questName, dateTmp, qObjectiveDesc3);
                                                        showPopupDesc3 = true;
                                                    }
                                                }
                                                else if (questStateUpdater3 == QuestStateUpdater.ByMessage) {
                                                    showMessage.SendTheMessage("Quest Updated: " + questName);
                                                }
                                                messageQuest3 = true;
                                            }
                                            else {
                                                if(isQuestLoaded) {
                                                    questLogManager.AddQuestToLog(this);
                                                }
                                            }
                                            questLogManager.questsFinished++;
                                        }
                                        if (qObjective3 == QuestObjective.ContinueQuest) {
                                            if (questId.Contains("MQ")) {
                                                playerStats.valorScore += valorReward;
                                                if (playerStats.crimeScore >= valorReward) {
                                                    playerStats.crimeScore -= valorReward;
                                                }
                                                else {
                                                    playerStats.crimeScore = 0;
                                                }
                                                uiSA.valorScore.text = "Valor score: " + playerStats.valorScore;
                                                uiSA.crimeScore.text = "Crime score: " + playerStats.crimeScore;
                                                playerStats.reputationScore += 10;
                                                uiSA.reputationScore.text = "Reputation score: " + playerStats.reputationScore;
                                            }
                                            GameObject.Find("[Quests]").transform.Find(nextQuest).GetComponent<QuestBase>().questStarted = true;
                                            GameObject.Find("[Quests]").transform.Find(nextQuest).GetComponent<QuestBase>().questPhase = QuestPhase.Phase1;
                                            questDone = true;
                                        }
                                        if (qObjective3 == QuestObjective.GetItem) {
                                            foreach (GameObject i in uiManager.inventoryUI.GetComponent<WeightedInventory>().items) {
                                                if (i == qObject3) {
                                                    showMessage.SendTheMessage("Quest Updated: " + questName);
                                                    questPhase = QuestPhase.Phase4;
                                                    qObjective3Done = true;
                                                }
                                            }
                                        }
                                        if (qObjective3 == QuestObjective.DeliverItem) {
                                            foreach (GameObject i in uiManager.inventoryUI.GetComponent<WeightedInventory>().items) {
                                                if (i == qItem3) {
                                                    if (qTopic3B == false) {
                                                        GameObject questTopic3 = (GameObject)Instantiate(topicPrefab);
                                                        questTopic3.transform.SetParent(qObject3.transform.Find("topics").transform);
                                                        questTopic3.GetComponent<NPCTopic>().topic = qTopic3;
                                                        questTopic3.GetComponent<NPCTopic>().dialogLine[0] = qTopic3Text;
                                                        questTopic3.GetComponent<NPCTopic>().showOnce = false;
                                                        questTopic3.GetComponent<NPCTopic>().questId = questId;
                                                        questTopic3.GetComponent<NPCTopic>().topicTrigger = TopicTrigger.DeliverItemToNpc;
                                                        questTopic3.GetComponent<NPCTopic>().triggerObject = i;
                                                        questTopic3.GetComponent<NPCTopic>().questPhase = QuestPhase.Phase4;
                                                        qTopic3B = true;
                                                    }
                                                    qObjective3Done = true;
                                                }
                                            }
                                        }
                                    }
                                }
                                else if (questPhase == QuestPhase.Phase4) {
                                    if (qObject4 != null) {
                                        if (qObject4.activeSelf == false) {
                                            qObject4.SetActive(true);
                                        }
                                    }
                                    if (qObjective3Done == false) {
                                        qObjective3Done = true;
                                    }
                                    if (qObjective4Done == false) {
                                        //////////////////////////////
                                        /// 
                                        currentDescription = qObjectiveDesc4;
                                        currentQuestTarget = qObject4;

                                        if (enable4 == false) {
                                            foreach (GameObject go in enableGoPhase4) {
                                                if (go.GetComponent<NPC>() != null) {
                                                    go.SetActive(true);
                                                }
                                                enable4 = true;
                                            }
                                            if (disable4 == false) {
                                                foreach (GameObject go in disalbeGoPhase4) {
                                                    if (go.GetComponent<NPC>() != null) {
                                                        go.SetActive(false);
                                                    }
                                                    disable4 = true;
                                                }

                                                if (qObjective4 == QuestObjective.FinishQuest) {
                                                    if (goldReward > 0 && itemReward == null) {
                                                        showMessage.SendTheMessage("Quest Completed: " + questName + "\n" +
                                                                                   goldReward + " gold coins added to inventory.");
                                                        playerGo.GetComponent<PlayerStats>().playerMoney += goldReward;
                                                        goldReward = 0;
                                                    }
                                                    else if (goldReward == 0 && itemReward != null) {
                                                        showMessage.SendTheMessage("Quest Completed: " + questName + "\n" +
                                                                                   itemReward.GetComponent<Item>().itemName + " added to inventory.");
                                                        GameObject itemRewardInst = GameObject.Instantiate(itemReward) as GameObject;
                                                        uiManager.inventoryUI.GetComponent<WeightedInventory>().AddItemToInventory(itemRewardInst);
                                                        itemReward = null;
                                                    }
                                                    else if (goldReward > 0 && itemReward != null) {
                                                        showMessage.SendTheMessage("Quest Completed: " + questName + "\n" +
                                                                                   itemReward.GetComponent<Item>().itemName + " and \n" +
                                                                                   goldReward + " gold coins added to inventory.");
                                                        playerGo.GetComponent<PlayerStats>().playerMoney += goldReward;
                                                        goldReward = 0;
                                                        GameObject itemRewardInst = GameObject.Instantiate(itemReward) as GameObject;
                                                        uiManager.inventoryUI.GetComponent<WeightedInventory>().AddItemToInventory(itemRewardInst);
                                                        itemReward = null;
                                                    }
                                                    if (showPopupDesc4 == false && qObjectiveDesc4 != "") {
                                                        uiManager.QuestInfoWindowPopup("Quest Completed", questName, dateQuestStarted, qObjectiveDesc4);
                                                        showPopupDesc4 = true;
                                                    }
                                                    playerStats.valorScore += valorReward;
                                                    if (playerStats.crimeScore >= valorReward) {
                                                        playerStats.crimeScore -= valorReward;
                                                    }
                                                    else {
                                                        playerStats.crimeScore = 0;
                                                    }
                                                    uiSA.valorScore.text = "Valor score: " + playerStats.valorScore;
                                                    uiSA.crimeScore.text = "Crime score: " + playerStats.crimeScore;
                                                    playerStats.reputationScore += 5;
                                                    uiSA.reputationScore.text = "Reputation score: " + playerStats.reputationScore;
                                                    questLogManager.RemoveQuestFromLog(this);
                                                    questStarted = false;
                                                    questDone = true;
                                                    //REWARDS

                                                    questLogManager.questsFinished++;
                                                }
                                                if (qObjective4 == QuestObjective.ContinueQuest) {
                                                    if (questId.Contains("MQ")) {
                                                        playerStats.valorScore += valorReward;
                                                        if (playerStats.crimeScore >= valorReward) {
                                                            playerStats.crimeScore -= valorReward;
                                                        }
                                                        else {
                                                            playerStats.crimeScore = 0;
                                                        }
                                                        uiSA.valorScore.text = "Valor score: " + playerStats.valorScore;
                                                        uiSA.crimeScore.text = "Crime score: " + playerStats.crimeScore;
                                                        playerStats.reputationScore += 10;
                                                        uiSA.reputationScore.text = "Reputation score: " + playerStats.reputationScore;
                                                    }
                                                    GameObject.Find("[Quests]").transform.Find(nextQuest).GetComponent<QuestBase>().questStarted = true;
                                                    GameObject.Find("[Quests]").transform.Find(nextQuest).GetComponent<QuestBase>().questPhase = QuestPhase.Phase1;
                                                    questDone = true;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else {
                                if (qObjective1 != QuestObjective.ContinueQuest && qObjective2 != QuestObjective.ContinueQuest &&
                                   qObjective3 != QuestObjective.ContinueQuest && qObjective4 != QuestObjective.ContinueQuest) {
                                    if (messageQuestDone == false) {
                                        //showMessage.SendTheMessage("Quest Completed: " + questName);
                                        currentDescription = string.Empty;
                                        currentQuestTarget = null;
                                        messageQuestDone = true;
                                    }
                                }
                                if (gm.trackingQuestId == questId) {
                                    gm.trackingQuestId = "";
                                }
                            }
                        }
                        else {
                            if (questStarted == true) {
                                if (messageQuestDone == false) {
                                    showMessage.SendTheMessage("Quest Failed: " + questName);
                                    questLogManager.RemoveQuestFromLog(this);
                                    currentDescription = string.Empty;
                                    currentQuestTarget = null;
                                    messageQuestDone = true;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}