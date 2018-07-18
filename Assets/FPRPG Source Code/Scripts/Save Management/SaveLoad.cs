using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FRPG.SMSaveManager;
using System;
using System.IO;
using UnityEngine.SceneManagement;
using System.Linq;
using Random = UnityEngine.Random;

public class SaveLoad : MonoBehaviour {
    public string saveFolderName = "MySave";

    private GameObject playerObject;

    private NPC[] npcsTmp; //Here we get our NPC components in the scene
    private List<int> npcsInScene; //Here we get IDs of all npcs in the scene when loading
    private List<int> npcsThatSouldNotBeInScene;
    private List<int> npcs; //Here we store our loaded (or open our saved) npc IDs
    private List<int> npcsHealth;
    private List<Vector3> npcsPosition;

    private Item[] itemsTmp;
    private List<int> itemsInScene;
    private List<int> itemsThatShouldNotBeInScene;
    private List<int> items;
    private List<Vector3> itemsPosition;
    private List<float> itemsHealth;
    private List<string> itemsName;

    private List<int> playerItems;
    private List<float> playerItemsHealth;
    private List<string> playerItemsName;
    private List<bool> playerItemsStolenOrNot;
    private List<bool> playerItemsEquippedOrNot;

    private List<string> quests;
    private List<QuestPhase> questsCurrentPhase;
    private List<bool> questsDoneStatus;
    private QuestBase[] allQuestsInScene;

    private GameManager gameManager;
    private TodWeather weatherManager;
    private TodClock calendarManager;
    private WeightedInventory inventory;

    public void Start() {
        playerObject = GameObject.FindObjectOfType<FirstPersonPlayer>().gameObject;
        gameManager = GameObject.FindObjectOfType<GameManager>();
        weatherManager = gameManager.GetComponent<TodWeather>();
        calendarManager = gameManager.GetComponent<TodClock>();
        inventory = GameObject.FindObjectOfType<UIManager>().inventoryUI.GetComponent<WeightedInventory>();
    }

    public void SaveMyGame() {

        npcsTmp = GameObject.FindObjectsOfType<NPC>();
        itemsTmp = GameObject.FindObjectsOfType<Item>();
        allQuestsInScene = GameObject.FindObjectsOfType<QuestBase>();
        npcs = new List<int>();
        npcsPosition = new List<Vector3>();
        npcsHealth = new List<int>();
        items = new List<int>();
        itemsPosition = new List<Vector3>();
        itemsHealth = new List<float>();
        itemsName = new List<string>();
        playerItems = new List<int>();
        playerItemsHealth = new List<float>();
        playerItemsName = new List<string>();
        playerItemsStolenOrNot = new List<bool>();
        playerItemsEquippedOrNot = new List<bool>();
        quests = new List<string>();
        questsCurrentPhase = new List<QuestPhase>();
        questsDoneStatus = new List<bool>();

        //Firstly we create/overwrite our save file

        SMSaveManager saveFile = new SMSaveManager(saveFolderName + "/saveGame.rpg", "123abc");
        saveFile.Open();
        
        //Firstly we are going to save players basic stats: name, gender, race etc...
        #region Player Stats
        PlayerStats pcs = playerObject.GetComponent<PlayerStats>();
        saveFile.SetValue("PC_Name", pcs.playerName);
        saveFile.SetValue("PC_Gender", (int)pcs.playerGender); //Here we cast enum to int so we can save gender as int, we will do this with every enum we want to save for easier use
        saveFile.SetValue("PC_Race", (int)pcs.playerRace);
        saveFile.SetValue("PC_Protector", (int)pcs.playerProtector);
        saveFile.SetValue("PC_Culture", (int)pcs.playerCulture);
        saveFile.SetValue("PC_Profession", (int)pcs.playerProfession);
        saveFile.SetValue("PC_Hair", (int)pcs.playerHair);
        saveFile.SetValue("PC_Beard", (int)pcs.playerBeard);
        saveFile.SetValue("PC_HairColor", pcs.playerHairColor);

        saveFile.SetValue("PC_Position", playerObject.transform.position);
        saveFile.SetValue("PC_Rotation", playerObject.transform.localEulerAngles);
        saveFile.SetValue("PC_MaxHealth", pcs.maxHealth);
        saveFile.SetValue("PC_CurrHealth", pcs.currentHealth);
        saveFile.SetValue("PC_MaxStamina", pcs.maxStamina);
        saveFile.SetValue("PC_CurrStamina", pcs.currentStamina);

        saveFile.SetValue("PC_CrimeScore", pcs.crimeScore);
        saveFile.SetValue("PC_ValorScore", pcs.valorScore);
        saveFile.SetValue("PC_ReputationScore", pcs.reputationScore);
        saveFile.SetValue("PC_PlacesFounded", pcs.placesFounded);

        saveFile.SetValue("PC_Money", pcs.playerMoney);
        saveFile.SetValue("PC_MaxWeight", pcs.maxCarryWeight);
        saveFile.SetValue("PC_CurrWeight", pcs.currentWeight);
        #endregion

        //Then player skills, attributes and level progression
        #region Player Skills and Attributes
        PlayerSkillsAndAttributes pcSkills = playerObject.GetComponent<PlayerSkillsAndAttributes>();
        saveFile.SetValue("PC_Body", pcSkills.body);
        saveFile.SetValue("PC_Agility", pcSkills.agility);
        saveFile.SetValue("PC_Mind", pcSkills.mind);
        saveFile.SetValue("PC_Melee", pcSkills.melee);
        saveFile.SetValue("PC_MeleeAdv", pcSkills.meleeAdvancement);
        saveFile.SetValue("PC_Block", pcSkills.block);
        saveFile.SetValue("PC_BlockAdv", pcSkills.blockAdvancement);
        saveFile.SetValue("PC_HeavyArmor", pcSkills.heavyArmor);
        saveFile.SetValue("PC_HeavyArmorAdv", pcSkills.heavyArmorAdvancement);
        saveFile.SetValue("PC_Acrobatics", pcSkills.acrobatics);
        saveFile.SetValue("PC_AcrobaticsAdv", pcSkills.acrobaticsAdvancement);
        saveFile.SetValue("PC_Marksman", pcSkills.marksman);
        saveFile.SetValue("PC_MarksmanAdv", pcSkills.marksmanAdvancement);
        saveFile.SetValue("PC_LightArmor", pcSkills.lightArmor);
        saveFile.SetValue("PC_LightArmorAdv", pcSkills.lightArmorAdvancement);
        saveFile.SetValue("PC_Speechcraft", pcSkills.speechcraft);
        saveFile.SetValue("PC_SpeechcraftAdv", pcSkills.speechcraftAdvancement);
        saveFile.SetValue("PC_Alchemy", pcSkills.alchemy);
        saveFile.SetValue("PC_AlchemyAdv", pcSkills.alchemyAdvancement);
        saveFile.SetValue("PC_Focus", pcSkills.focus);
        saveFile.SetValue("PC_FocusAdv", pcSkills.focusAdvancement);
        saveFile.SetValue("PC_SkillsIncreases", pcSkills.skillsIncreases);

        PlayerLevelManager pcLevelMan = playerObject.GetComponent<PlayerLevelManager>();

        saveFile.SetValue("PC_PointsForLeveling", pcLevelMan.pointsForLeveling);
        saveFile.SetValue("PC_PlayerLevel", pcLevelMan.playerLevel);
        saveFile.SetValue("PC_SkillsAdvanced", pcLevelMan.skillsAdvanced);
        #endregion

        //We would also like to save our npcs who are in the world, their position and health
        #region NPC Saving
        for (int npc = 0; npc < npcsTmp.Length; npc++) {
            npcs.Add(npcsTmp[npc].npcId);
            npcsPosition.Add(npcsTmp[npc].transform.position);
            npcsHealth.Add(npcsTmp[npc].npcHealth);
        }
        saveFile.SetValue("NPCs_id", npcs);
        saveFile.SetValue("NPCs_Positions", npcsPosition);
        saveFile.SetValue("NPCs_Health", npcsHealth);
        #endregion

        //We can also save items in our world and our inventory
        #region Items Saving
        for(int item = 0; item < itemsTmp.Length; item++) {
            items.Add(itemsTmp[item].itemId);
            itemsPosition.Add(itemsTmp[item].transform.position);
            itemsHealth.Add(itemsTmp[item].health);
            itemsName.Add(itemsTmp[item].itemName);
        }
        saveFile.SetValue("ITEMs_id", items);
        saveFile.SetValue("ITEMs_position", itemsPosition);
        saveFile.SetValue("ITEMs_health", itemsHealth);
        saveFile.SetValue("ITEMs_name", itemsName);

        #endregion

        //We will now save the time, day, weather etc...
        #region Time and Weather Saving
        saveFile.SetValue("WEATHER_is", weatherManager.weatherIs);
        saveFile.SetValue("TIME_is", weatherManager.currentTimeOfDay);
        saveFile.SetValue("DAY_count", calendarManager.dayCount);
        saveFile.SetValue("DAY_is", calendarManager.currentDay);
        saveFile.SetValue("MONTH_is", calendarManager.currentMonth);
        saveFile.SetValue("YEAR_is", calendarManager.currentYear);
        #endregion

        //We also want to save items that are currently stored in players inventory
        #region Inventory Saving
        for(int inventoryItem = 0; inventoryItem < inventory.items.Count; inventoryItem++) {
            playerItems.Add(inventory.items[inventoryItem].GetComponent<Item>().itemId);
            playerItemsHealth.Add(inventory.items[inventoryItem].GetComponent<Item>().health);
            playerItemsName.Add(inventory.items[inventoryItem].GetComponent<Item>().itemName);
            playerItemsStolenOrNot.Add(inventory.items[inventoryItem].GetComponent<Item>().stolen);
            playerItemsEquippedOrNot.Add(inventory.items[inventoryItem].GetComponent<Item>().equiped);
        }
        saveFile.SetValue("INVENTORY_Items", playerItems);
        saveFile.SetValue("INVENTORY_ItemsHealth", playerItemsHealth);
        saveFile.SetValue("INVENTORY_ItemsNames", playerItemsName);
        saveFile.SetValue("INVENTORY_ItemsIsStolen", playerItemsStolenOrNot);
        saveFile.SetValue("INVENTORY_ItemsIsEquipped", playerItemsEquippedOrNot);
        #endregion

        //Also we need to save our quests and their progress
        #region Quest Saving
        for (int quest = 0; quest < allQuestsInScene.Length; quest++) {
            quests.Add(allQuestsInScene[quest].questId);
            questsCurrentPhase.Add(allQuestsInScene[quest].questPhase);
            questsDoneStatus.Add(allQuestsInScene[quest].questDone);
        }
        saveFile.SetValue("QUESTS_Ids", quests);
        saveFile.SetValue("QUESTS_Phases", questsCurrentPhase);
        saveFile.SetValue("QUESTS_DoneStatus", questsDoneStatus);
        #endregion


        //And at last we want to save all of our data to our save file
        saveFile.Save();
    }

    public void LoadMyGame() {

        npcsTmp = GameObject.FindObjectsOfType<NPC>();
        itemsTmp = GameObject.FindObjectsOfType<Item>();
        allQuestsInScene = GameObject.FindObjectsOfType<QuestBase>();
        npcs = new List<int>();
        npcsInScene = new List<int>();
        npcsThatSouldNotBeInScene = new List<int>();
        npcsPosition = new List<Vector3>();
        npcsHealth = new List<int>();
        items = new List<int>();
        itemsInScene = new List<int>();
        itemsPosition = new List<Vector3>();
        itemsHealth = new List<float>();
        itemsName = new List<string>();
        playerItems = new List<int>();
        playerItemsHealth = new List<float>();
        playerItemsName = new List<string>();
        playerItemsStolenOrNot = new List<bool>();
        playerItemsEquippedOrNot = new List<bool>();
        quests = new List<string>();
        questsCurrentPhase = new List<QuestPhase>();
        questsDoneStatus = new List<bool>();

        for (int i = 0; i < npcsTmp.Length; i++) {
            npcsInScene.Add(npcsTmp[i].npcId);
        }

        for (int i = 0; i < itemsTmp.Length; i++) {
            itemsInScene.Add(itemsTmp[i].itemId);
        }

        //Firstly we create/open our save file

        SMSaveManager saveFile = new SMSaveManager(saveFolderName + "/saveGame.rpg", "123abc");
        saveFile.Open();

        #region Player Stats
        PlayerStats pcs = playerObject.GetComponent<PlayerStats>();
        pcs.playerName = saveFile.GetValue<string>("PC_Name", string.Empty);
        pcs.playerGender = (CharacterGender)saveFile.GetValue<int>("PC_Gender", 0); //Here we load int that we saved so we can find it in our enum
        pcs.playerRace = (CharacterRace)saveFile.GetValue<int>("PC_Race", 0);
        pcs.playerProtector = (CharacterProtector)saveFile.GetValue<int>("PC_Protector", 0);
        pcs.playerCulture = (CharacterCulture)saveFile.GetValue<int>("PC_Culture", 0);
        pcs.playerProfession = (CharacterProfession)saveFile.GetValue<int>("PC_Profession", 0);
        pcs.playerHair = (NPCHair)saveFile.GetValue<int>("PC_Hair", 0);
        pcs.playerBeard = (NPCFacialHair)saveFile.GetValue<int>("PC_Beard", 0);
        pcs.playerHairColor = saveFile.GetValue<Color>("PC_HairColor", Color.gray);

        playerObject.transform.position = saveFile.GetValue<Vector3>("PC_Position", Vector3.zero);
        playerObject.transform.localEulerAngles = saveFile.GetValue<Vector3>("PC_Rotation", Vector3.zero);
        pcs.maxHealth = saveFile.GetValue<int>("PC_MaxHealth", pcs.maxHealth);
        pcs.currentHealth = saveFile.GetValue<int>("PC_CurrHealth", pcs.currentHealth);
        pcs.maxStamina = saveFile.GetValue<int>("PC_MaxStamina", pcs.maxStamina);
        pcs.currentStamina = saveFile.GetValue<int>("PC_CurrStamina", pcs.currentStamina);

        pcs.crimeScore = saveFile.GetValue<int>("PC_CrimeScore", 0);
        pcs.valorScore = saveFile.GetValue<int>("PC_ValorScore", 0);
        pcs.reputationScore = saveFile.GetValue<int>("PC_ReputationScore", 0);
        pcs.placesFounded = saveFile.GetValue<int>("PC_PlacesFounded", 0);

        pcs.playerMoney = saveFile.GetValue<int>("PC_Money", 0);
        pcs.maxCarryWeight = saveFile.GetValue<int>("PC_MaxWeight", 0);
        pcs.currentWeight = saveFile.GetValue<int>("PC_CurrWeight", 0);
        #endregion

        #region Player Skills and Attributes
        PlayerSkillsAndAttributes pcSkills = playerObject.GetComponent<PlayerSkillsAndAttributes>();
        pcSkills.body = saveFile.GetValue<int>("PC_Body", pcSkills.body);
        pcSkills.agility = saveFile.GetValue<int>("PC_Agility", pcSkills.agility);
        pcSkills.mind = saveFile.GetValue<int>("PC_Mind", pcSkills.mind);
        pcSkills.melee = saveFile.GetValue<int>("PC_Melee", pcSkills.melee);
        pcSkills.meleeAdvancement = saveFile.GetValue<float>("PC_MeleeAdv", pcSkills.meleeAdvancement);
        pcSkills.block = saveFile.GetValue<int>("PC_Block", pcSkills.block);
        pcSkills.blockAdvancement = saveFile.GetValue<float>("PC_BlockAdv", pcSkills.blockAdvancement);
        pcSkills.heavyArmor = saveFile.GetValue<int>("PC_HeavyArmor", pcSkills.heavyArmor);
        pcSkills.heavyArmorAdvancement = saveFile.GetValue<float>("PC_HeavyArmorAdv", pcSkills.heavyArmorAdvancement);
        pcSkills.acrobatics = saveFile.GetValue<int>("PC_Acrobatics", pcSkills.acrobatics);
        pcSkills.acrobaticsAdvancement = saveFile.GetValue<float>("PC_AcrobaticsAdv", pcSkills.acrobaticsAdvancement);
        pcSkills.marksman = saveFile.GetValue<int>("PC_Marksman", pcSkills.marksman);
        pcSkills.marksmanAdvancement = saveFile.GetValue<float>("PC_MarksmanAdv", pcSkills.marksmanAdvancement);
        pcSkills.lightArmor = saveFile.GetValue<int>("PC_LightArmor", pcSkills.lightArmor);
        pcSkills.lightArmorAdvancement = saveFile.GetValue<float>("PC_LightArmorAdv", pcSkills.lightArmorAdvancement);
        pcSkills.speechcraft = saveFile.GetValue<int>("PC_Speechcraft", pcSkills.speechcraft);
        pcSkills.speechcraftAdvancement = saveFile.GetValue<float>("PC_SpeechcraftAdv", pcSkills.speechcraftAdvancement);
        pcSkills.alchemy = saveFile.GetValue<int>("PC_Alchemy", pcSkills.alchemy);
        pcSkills.alchemyAdvancement = saveFile.GetValue<float>("PC_AlchemyAdv", pcSkills.alchemyAdvancement);
        pcSkills.focus = saveFile.GetValue<int>("PC_Focus", pcSkills.focus);
        pcSkills.focusAdvancement = saveFile.GetValue<float>("PC_FocusAdv", pcSkills.focusAdvancement);
        pcSkills.skillsIncreases = saveFile.GetValue<int>("PC_SkillsIncreases", pcSkills.skillsIncreases);

        PlayerLevelManager pcLevelMan = playerObject.GetComponent<PlayerLevelManager>();

        pcLevelMan.pointsForLeveling = saveFile.GetValue<int>("PC_PointsForLeveling", pcLevelMan.pointsForLeveling);
        pcLevelMan.playerLevel = saveFile.GetValue<int>("PC_PlayerLevel", pcLevelMan.playerLevel);
        pcLevelMan.skillsAdvanced = saveFile.GetValue<int>("PC_SkillsAdvanced", pcLevelMan.skillsAdvanced);
        #endregion

        #region NPC Loading
        npcs = saveFile.GetValue<List<int>>("NPCs_id");
        npcsPosition = saveFile.GetValue<List<Vector3>>("NPCs_Positions");
        npcsHealth = saveFile.GetValue<List<int>>("NPCs_Health");

        if (npcs != null) {
            npcsThatSouldNotBeInScene = npcsInScene.Except(npcs).ToList();

            for (int presentNpcID = 0; presentNpcID < npcsInScene.Count; presentNpcID++) {
                for (int loadedNpcID = 0; loadedNpcID < npcs.Count; loadedNpcID++) {
                    if (npcsInScene[presentNpcID] == npcs[loadedNpcID]) {
                        npcsTmp[presentNpcID].transform.position = npcsPosition[loadedNpcID];
                        npcsTmp[presentNpcID].npcHealth = npcsHealth[loadedNpcID];
                    }
                }
            }

            for (int npcToDelete = 0; npcToDelete < npcsInScene.Count; npcToDelete++) {
                if (npcsThatSouldNotBeInScene.Contains(npcsInScene[npcToDelete])) {
                    npcsTmp[npcToDelete].gameObject.SetActive(false);
                }
            }
        }
        #endregion

        #region Item Loading

        items = saveFile.GetValue<List<int>>("ITEMs_id");
        itemsPosition = saveFile.GetValue<List<Vector3>>("ITEMs_position");
        itemsHealth = saveFile.GetValue<List<float>>("ITEMs_health");
        itemsName = saveFile.GetValue<List<string>>("ITEMs_name");

        if (items != null) {
            itemsThatShouldNotBeInScene = itemsInScene.Except(items).ToList();

            if (itemsTmp.Length > 0) {
                for (int itemPresent = 0; itemPresent < itemsTmp.Length; itemPresent++) {
                    for (int loadedItem = 0; loadedItem < items.Count; loadedItem++) {
                        if (itemsInScene[itemPresent] == items[loadedItem]) {
                            itemsTmp[itemPresent].transform.position = itemsPosition[loadedItem];
                            itemsTmp[itemPresent].health = itemsHealth[loadedItem];
                        }
                        if (!itemsInScene.Contains(items[loadedItem])) { //If we have item that is instantiated then spawned, we will need it to instantiante by finding it in GameManager script
                            for (int gmItem = 0; gmItem < gameManager.allGameItems.Count; gmItem++) {
                                if (itemsName[loadedItem] == gameManager.allGameItems[gmItem].GetComponent<Item>().itemName) {
                                    GameObject newItem = Instantiate(gameManager.allGameItems[gmItem]) as GameObject;
                                    newItem.GetComponent<Item>().itemId = items[loadedItem]; //Since this is new object we need to assign its id from the save for future saving
                                    newItem.transform.position = itemsPosition[loadedItem];
                                    newItem.GetComponent<Item>().health = itemsHealth[loadedItem];
                                }
                            }
                        }
                    }
                }

                for (int itemToDelete = 0; itemToDelete < itemsInScene.Count; itemToDelete++) {
                    if (itemsThatShouldNotBeInScene.Contains(itemsInScene[itemToDelete])) {
                        itemsTmp[itemToDelete].gameObject.SetActive(false);
                    }
                }
            }
            else {
                for (int loadedItem = 0; loadedItem < items.Count; loadedItem++) {
                    for (int gmItem = 0; gmItem < gameManager.allGameItems.Count; gmItem++) {
                        if (itemsName[loadedItem] == gameManager.allGameItems[gmItem].GetComponent<Item>().itemName) {
                            GameObject newItem = Instantiate(gameManager.allGameItems[gmItem]) as GameObject;
                            newItem.GetComponent<Item>().itemId = items[loadedItem]; //Since this is new object we need to assign its id from the save for future saving
                            newItem.transform.position = itemsPosition[loadedItem];
                            newItem.GetComponent<Item>().health = itemsHealth[loadedItem];
                        }
                    }
                }
            }
        }
        #endregion

        #region Time and Weather Loading
        pcs.playerName = saveFile.GetValue<string>("PC_Name", string.Empty);

        weatherManager.weatherIs = saveFile.GetValue<WeatherIs>("WEATHER_is");
        weatherManager.currentTimeOfDay = saveFile.GetValue<float>("TIME_is");
        calendarManager.dayCount = saveFile.GetValue<int>("DAY_count");
        calendarManager.currentDay = saveFile.GetValue<WeekDays>("DAY_is");
        calendarManager.currentMonth = saveFile.GetValue<Months>("MONTH_is");
        calendarManager.currentYear = saveFile.GetValue<int>("YEAR_is");
        #endregion

        #region Inventory Loading
        playerItems = saveFile.GetValue<List<int>>("INVENTORY_Items");
        playerItemsHealth = saveFile.GetValue<List<float>>("INVENTORY_ItemsHealth");
        playerItemsName = saveFile.GetValue<List<string>>("INVENTORY_ItemsNames");
        playerItemsStolenOrNot = saveFile.GetValue<List<bool>>("INVENTORY_ItemsIsStolen");
        playerItemsEquippedOrNot = saveFile.GetValue<List<bool>>("INVENTORY_ItemsIsEquipped");

        if (playerItems != null) {
            if(itemsTmp.Length > 0) {
                for (int itemPresent = 0; itemPresent < itemsTmp.Length; itemPresent++) {
                    for (int loadedItem = 0; loadedItem < playerItems.Count; loadedItem++) {
                        if (itemsInScene[itemPresent] == playerItems[loadedItem]) { //If item that is loaded is in scene but should be in inventory
                            itemsTmp[itemPresent].health = playerItemsHealth[loadedItem];
                            itemsTmp[itemPresent].stolen = playerItemsStolenOrNot[loadedItem];
                            itemsTmp[itemPresent].equiped = playerItemsEquippedOrNot[loadedItem];
                            inventory.AddItemToInventory(itemsTmp[itemPresent].gameObject);
                            if (playerItemsEquippedOrNot[loadedItem]) {
                                inventory.EquipItemFromInventory(itemsTmp[itemPresent]);
                            }
                        }
                        if (!itemsInScene.Contains(playerItems[loadedItem])) {//If item is not in scene
                            for (int gmItem = 0; gmItem < gameManager.allGameItems.Count; gmItem++) {
                                if (playerItemsName[loadedItem] == gameManager.allGameItems[gmItem].GetComponent<Item>().itemName) {
                                    GameObject newItem = Instantiate(gameManager.allGameItems[gmItem]) as GameObject;

                                    newItem.GetComponent<Item>().itemId = playerItems[loadedItem]; //Since this is new object we need to assign its id from the save for future saving
                                    newItem.GetComponent<Item>().health = playerItemsHealth[loadedItem];
                                    newItem.GetComponent<Item>().stolen = playerItemsStolenOrNot[loadedItem];
                                    newItem.GetComponent<Item>().equiped = playerItemsEquippedOrNot[loadedItem];
                                    if(playerItemsEquippedOrNot[loadedItem]) {
                                        inventory.EquipItemFromInventory(newItem.GetComponent<Item>());
                                    }
                                    inventory.AddItemToInventory(newItem);
                                }
                            }
                        }
                    }
                }
            }
            else {
                for (int loadedItem = 0; loadedItem < playerItems.Count; loadedItem++) {
                    for (int gmItem = 0; gmItem < gameManager.allGameItems.Count; gmItem++) {
                        if (playerItemsName[loadedItem] == gameManager.allGameItems[gmItem].GetComponent<Item>().itemName) {
                            GameObject newItem = Instantiate(gameManager.allGameItems[gmItem]) as GameObject;

                            newItem.GetComponent<Item>().itemId = playerItems[loadedItem]; //Since this is new object we need to assign its id from the save for future saving
                            newItem.GetComponent<Item>().health = playerItemsHealth[loadedItem];
                            newItem.GetComponent<Item>().stolen = playerItemsStolenOrNot[loadedItem];
                            newItem.GetComponent<Item>().equiped = playerItemsEquippedOrNot[loadedItem];
                            if (playerItemsEquippedOrNot[loadedItem]) {
                                inventory.EquipItemFromInventory(newItem.GetComponent<Item>());
                            }
                            inventory.AddItemToInventory(newItem);
                        }
                    }
                }
            }
        }
        #endregion

        #region Quests Loading
        quests = saveFile.GetValue<List<string>>("QUESTS_Ids");
        questsCurrentPhase = saveFile.GetValue<List<QuestPhase>>("QUESTS_Phases");
        questsDoneStatus = saveFile.GetValue<List<bool>>("QUESTS_DoneStatus");

        for(int quest = 0; quest < allQuestsInScene.Length; quest++) {
            for(int loadedQuest = 0; loadedQuest < quests.Count; loadedQuest++) {
                if(allQuestsInScene[quest].questId == quests[loadedQuest]) {
                    if (questsDoneStatus[loadedQuest] == false) {
                        allQuestsInScene[quest].questPhase = questsCurrentPhase[loadedQuest];
                        allQuestsInScene[quest].isQuestLoaded = true;
                    }
                    else {
                        allQuestsInScene[quest].questDone = questsDoneStatus[loadedQuest];
                    }
                }
            }
        }
        #endregion

        saveFile.Save();
    }
}
