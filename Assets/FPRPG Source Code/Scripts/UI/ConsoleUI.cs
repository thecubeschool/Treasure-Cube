using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ConsoleUI : MonoBehaviour {

	public InputField inputField;
	public Text consoleOutputText;
	[Space(10f)]
	public GameManager gameManager;
	public UIManager uiManager;
	public TodClock todClock;
	public PlayerStats playerStats;
	public WeightedInventory inventory;
	[Space(10f)]
	public GameObject playerGo;
	public Transform crosshairHitPoint;
	public GameObject[] allLocations;
	public GameObject questHolder;
	private string lastCommandEntered;

	public void KeepActive() {
		inputField.ActivateInputField();
	}

	void Start() {
		playerGo = GameObject.Find("[Player]");
		inputField = GetComponent<InputField>();
		gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
		todClock = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TodClock>();
		playerStats = playerGo.GetComponent<PlayerStats>();
		allLocations = GameObject.FindGameObjectsWithTag("LocationZone");
		uiManager = GameObject.Find("_UICanvasGame").GetComponent<UIManager>();
		inventory = uiManager.inventoryUI.GetComponent<WeightedInventory>();


		KeepActive();
	}

	void Update() {
		if (lastCommandEntered != "" && lastCommandEntered != null) {
			if(inputField.isFocused && Input.GetKey(KeyCode.UpArrow)){
				inputField.text = lastCommandEntered;
			}
		}
		if(inputField.isFocused && inputField.text != "" && Input.GetKey(KeyCode.Return)) {
			if(inputField.text == "time") {
				string oldText = consoleOutputText.text;
				string commandText = "Time is " + todClock.currentHour + ":" + todClock.currentMinute + ".";
				consoleOutputText.text = oldText + "\n" + commandText;
			}
			else if(inputField.text.Contains("goto ")) {
				foreach(GameObject travelPoint in allLocations) {
					if(inputField.text.Contains(travelPoint.transform.parent.name)) {
						Vector3 p = travelPoint.transform.position;
						playerGo.transform.position = new Vector3(p.x, p.y + 2f, p.z);

						string oldText = consoleOutputText.text;
						string commandText = "Player teleported to " + travelPoint.transform.parent.name + ".";
						consoleOutputText.text = oldText + "\n" + commandText;
					}
				}
			}
			else if(inputField.text.Contains("gold ")) {

				string[] strArray = inputField.text.Split( );
				int goldI = 0;
				int.TryParse(strArray[1], out goldI);
				playerStats.playerMoney += goldI;

				string oldText = consoleOutputText.text;
				string commandText = "Player given " + goldI + " gold coins.";
				consoleOutputText.text = oldText + "\n" + commandText;
			}
            else if (inputField.text.Contains("pcSetLevel ")) {

                string[] strArray = inputField.text.Split();
                int levelI = 0;
                int.TryParse(strArray[1], out levelI);
                playerStats.gameObject.GetComponent<PlayerLevelManager>().playerLevel = levelI;

                string oldText = consoleOutputText.text;
                string commandText = "Player level set to " + levelI;
                consoleOutputText.text = oldText + "\n" + commandText;
            }
            else if(inputField.text == "help") {
				string oldText = consoleOutputText.text;
				string commandText =
						"debuging [Starts up debuging mod.]" + "\n" +
						"god (Make player immortal.)" + "\n" + 
						"fly (Break rules of physics and fly around.)" + "\n" + 
						"time (Display current in game time.)" + "\n" + 
						"goto [NameOfLocation] (Teleport player to desired location.)" + "\n" +
						"gold [GoldAmount] (Gives desired amount of gold to player.)" + "\n" +
						"item [ItemID] (Add desired item to players inventory.)" + "\n" +
						"freezeweather (Weather will not change by itself if OFF)" + "\n" +
						"setweather [weatherID] (Changes the current weather to the weather specified)" + "\n" +
						"spawnnpc [npcID] (Spawns desired npc where crosshair points on solid ground.)" + "\n" +
						"dayspeed [value] (Sets the day cycle speed. Default is 2400.)" + "\n" +
						"questStart [questID] (Starts the quest in question, if it is not already started.)" + "\n" +
						"coc (enable/disable main camera occlusion culling.)" + "\n" +
						"itemid (lists all acquirable game items IDs.)" + "\n" +
                        "pcSetLevel (Set players level)";

				consoleOutputText.text = oldText + "\n" + commandText;
			}
			else if(inputField.text == "fly") {
				if(gameManager.flyingMode == false) {
					string oldText = consoleOutputText.text;
					string commandText = "Flying mode activated.";
					consoleOutputText.text = oldText + "\n" + commandText;
					gameManager.flyingMode = true;
				}
				else if(gameManager.flyingMode == true) {
					string oldText = consoleOutputText.text;
					string commandText = "Flying mode deactivated.";
					consoleOutputText.text = oldText + "\n" + commandText;
					gameManager.flyingMode = false;
				}
			}
			else if(inputField.text == "god") {
				if(gameManager.playerImmortal == false) {
					string oldText = consoleOutputText.text;
					string commandText = "God mode activated.";
					consoleOutputText.text = oldText + "\n" + commandText;
					gameManager.playerImmortal = true;
				}
				else if(gameManager.playerImmortal == true) {
					string oldText = consoleOutputText.text;
					string commandText = "God mode deactivated.";
					consoleOutputText.text = oldText + "\n" + commandText;
					gameManager.playerImmortal = false;
				}
			}
			else if(inputField.text.Contains("item ")) {
				foreach(GameObject item in gameManager.allGameItems) {
					if(inputField.text.Contains(item.name)) {
						GameObject itemTmp = Instantiate(item, Vector3.zero, Quaternion.identity) as GameObject;
						itemTmp.GetComponent<Rigidbody>().useGravity = false;
						inventory.AddItemToInventory(itemTmp);
						
						string oldText = consoleOutputText.text;
						string commandText = "Item " + itemTmp.name + " added to inventory.";
						consoleOutputText.text = oldText + "\n" + commandText;
					}
				}
			}
			else if(inputField.text.Contains("setweather ")) {
				if(inputField.text.Contains("clear")) {
					gameManager.GetComponent<TodWeather>().weatherIs = WeatherIs.Clear;
					string oldText = consoleOutputText.text;
					string commandText = "Weather changed to CLEAR.";
					consoleOutputText.text = oldText + "\n" + commandText;
				}
				else if(inputField.text.Contains("cloudy")) {
					gameManager.GetComponent<TodWeather>().weatherIs = WeatherIs.Cloudy;
					string oldText = consoleOutputText.text;
					string commandText = "Weather changed to CLOUDY.";
					consoleOutputText.text = oldText + "\n" + commandText;
				}
				else if(inputField.text.Contains("lightrain")) {
					gameManager.GetComponent<TodWeather>().weatherIs = WeatherIs.LightRain;
					string oldText = consoleOutputText.text;
					string commandText = "Weather changed to LIGHT RAIN.";
					consoleOutputText.text = oldText + "\n" + commandText;
				}
				else if(inputField.text.Contains("heavyrain")) {
					gameManager.GetComponent<TodWeather>().weatherIs = WeatherIs.HeavyRain;
					string oldText = consoleOutputText.text;
					string commandText = "Weather changed to HEAVY RAIN.";
					consoleOutputText.text = oldText + "\n" + commandText;
				}
				else if(inputField.text.Contains("storm")) {
					gameManager.GetComponent<TodWeather>().weatherIs = WeatherIs.Storm;
					string oldText = consoleOutputText.text;
					string commandText = "Weather changed to STORM.";
					consoleOutputText.text = oldText + "\n" + commandText;
				}
				else if(inputField.text.Contains("fog")) {
					gameManager.GetComponent<TodWeather>().weatherIs = WeatherIs.Fog;
					string oldText = consoleOutputText.text;
					string commandText = "Weather changed to FOG.";
					consoleOutputText.text = oldText + "\n" + commandText;
				}
				else if(inputField.text.Contains("snow")) {
					gameManager.GetComponent<TodWeather>().weatherIs = WeatherIs.Snow;
					string oldText = consoleOutputText.text;
					string commandText = "Weather changed to SNOW.";
					consoleOutputText.text = oldText + "\n" + commandText;
				}
				else if(inputField.text.Contains("blizzard")) {
					gameManager.GetComponent<TodWeather>().weatherIs = WeatherIs.Blizard;
					string oldText = consoleOutputText.text;
					string commandText = "Weather changed to BLIZZARD.";
					consoleOutputText.text = oldText + "\n" + commandText;
				}
				else if(inputField.text.Contains("intro")) {
					gameManager.GetComponent<TodWeather>().weatherIs = WeatherIs.IntroFog;
					string oldText = consoleOutputText.text;
					string commandText = "Weather changed to INTRO FOG.";
					consoleOutputText.text = oldText + "\n" + commandText;
				}
				else {
					string oldText = consoleOutputText.text;
					string commandText = "The weather specified does not exist. Try using CLEAR, CLOUDY, LIGHT RAIN, HEAVY RAIN, STORM, FOG, SNOW, BLIZZARD instead.";
					consoleOutputText.text = oldText + "\n" + commandText;
				}
			}
			else if(inputField.text.Contains("spawnnpc ")) {
				foreach(GameObject npc in gameManager.allGameNpcs) {
					if(inputField.text.Contains(npc.name)) {
						GameObject npcTmp = Instantiate(npc, crosshairHitPoint.position, Quaternion.identity) as GameObject;

						if(npcTmp.GetComponent<HorseManager>() != null) {
							npcTmp.GetComponent<HorseManager>().owner = playerGo;
							npcTmp.GetComponent<HorseManager>().hasEquipment = true;
							//Vector3 newPos = new Vector3(crosshairHitPoint.position.x,
							//                             crosshairHitPoint.position.y + 2f,
							//                             crosshairHitPoint.position.z);
							//npcTmp.transform.position = newPos;
						}
						string oldText = consoleOutputText.text;
						string commandText = "Npc " + npcTmp.name + " spawned.";
						consoleOutputText.text = oldText + "\n" + commandText;
					}
				}
			}
			else if(inputField.text.Contains("debuging")) {
				if(gameManager.debugingMode == false) {
					gameManager.debugingMode = true;

					string oldText = consoleOutputText.text;
					string commandText = "Debuging mode ON.";
					consoleOutputText.text = oldText + "\n" + commandText;
				}
				else {
					gameManager.debugingMode = false;
					
					string oldText = consoleOutputText.text;
					string commandText = "Debuging mode OFF.";
					consoleOutputText.text = oldText + "\n" + commandText;
				}
			}
			else if(inputField.text.Contains("freezeweather")) {
				if(gameManager.GetComponent<TodWeather>().willWeatherChangeGeneric == false) {
					gameManager.GetComponent<TodWeather>().willWeatherChangeGeneric = true;
					
					string oldText = consoleOutputText.text;
					string commandText = "Weather changing generic ON.";
					consoleOutputText.text = oldText + "\n" + commandText;
				}
				else {
					gameManager.GetComponent<TodWeather>().willWeatherChangeGeneric = false;
					
					string oldText = consoleOutputText.text;
					string commandText = "Weather changing generic OFF.";
					consoleOutputText.text = oldText + "\n" + commandText;
				}
			}
			else if(inputField.text.Contains("dayspeed ")) {
				
				string[] strArray = inputField.text.Split();
				float valueF = 0f;
				float.TryParse(strArray[1], out valueF);
				gameManager.GetComponent<TodWeather>().secondsInFullDay = valueF;

				string oldText = consoleOutputText.text;
				string commandText = "Day cycle speed set to " + valueF +".";
				consoleOutputText.text = oldText + "\n" + commandText;
			}
			else if(inputField.text.Contains("questStart ")) {
				foreach(Transform qu in questHolder.transform) {
					QuestBase q = qu.GetComponent<QuestBase>();
					if(inputField.text.Contains(qu.name)) {
						q.questStarted = true;
						q.questPhase = QuestPhase.Phase1;

						string oldText = consoleOutputText.text;
						string commandText = "Quest " + q.name + " started.";
						consoleOutputText.text = oldText + "\n" + commandText;
					}
				}
			}
			else if(inputField.text == "coc") {
				Camera mainCam = playerGo.transform.Find("FPCameraGO/FPCamera").GetComponent<Camera>();
				if(mainCam.useOcclusionCulling == false) {
					string oldText = consoleOutputText.text;
					string commandText = "Occlusion Culling activated.";
					consoleOutputText.text = oldText + "\n" + commandText;
					mainCam.useOcclusionCulling = true;
				}
				else if(mainCam.useOcclusionCulling == true) {
					string oldText = consoleOutputText.text;
					string commandText = "Occlusion Culling deactivated.";
					consoleOutputText.text = oldText + "\n" + commandText;
					mainCam.useOcclusionCulling = false;
				}
			}
			else if(inputField.text == "itemid") {
				foreach(GameObject item in gameManager.allGameItems) {
					string oldText = consoleOutputText.text;
					string commandText = item.name;
					consoleOutputText.text = oldText + "\n" + commandText;
				}
			}
			else {
				string oldText = consoleOutputText.text;
				string commandText = "Unknown command. Type 'help' for list of commands.";
				consoleOutputText.text = oldText + "\n" + commandText;
			}

			lastCommandEntered = inputField.text;
			inputField.text = "";
		}
	}
}
