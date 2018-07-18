using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UISkillsAndAttributes : MonoBehaviour {

	public GameObject skillsUI;
	public GameObject questLogUI;
	public GameObject questLogScrollUI;

	private PlayerStats stats;
	private PlayerSkillsAndAttributes skillsAttributes;
	private PlayerLevelManager lvlManager;
	[Space(10f)]
	public Text playerName;
	public Text playerLevelRace;
	[Space(10f)]
	public Text health;
	public Text stamina;
	public Text mana;
	[Space(10f)]
	public Text pointsLeft;
	public Text bodyPoints;
	public Button bodyAddPointsButton;
	public Text agilityPoints;
	public Button agilityAddPointsButton;
	public Text mindPoints;
	public Button mindAddPointsButton;
	[Space(10f)]	
	public Text melee;
	public Text block;
	public Text heavyArmor;
	public Text acrobatics;
	public Text marksman;
	public Text lightArmor;
	public Text speechcraft;
	public Text alchemy;
	public Text focus;
	[Space(5f)]
	public Image meleeProgress;
	public Image blockProgress;
	public Image heavyArmorProgress;
	public Image acrobaticsProgress;
	public Image marksmanProgress;
	public Image lightArmorProgress;
	public Image speechcraftProgress;
	public Image alchemyProgress;
	public Image focusProgress;
	[Space(5f)]
	public Text daysPassed;
	public Text activeQuests;
	public Text questsCompleted;
	public Text skillsIncreases;
	public Text masteredSkills;
	public Text crimeScore;
	public Text valorScore;
	public Text reputationScore;
	public Text placesFounded;
	public Text chroniclesRead;

	void Awake() {
		if(skillsUI.activeSelf == false) {
			skillsUI.SetActive(true);
		}
		if(questLogUI.activeSelf == false) {
			questLogUI.SetActive(true);
		}
		if(questLogScrollUI.activeSelf == false) {
			questLogScrollUI.SetActive(true);
		}
	}

	void Start() {
		stats = GameObject.Find("[Player]").GetComponent<PlayerStats>();
		skillsAttributes = GameObject.Find("[Player]").GetComponent<PlayerSkillsAndAttributes>();
		lvlManager = GameObject.Find("[Player]").GetComponent<PlayerLevelManager>();
		skillsUI.SetActive(true);
		questLogUI.SetActive(false);
		questLogScrollUI.SetActive(false);
	}

	void Update() {

		playerName.text = stats.playerName;
		playerLevelRace.text = "Level " + lvlManager.playerLevel.ToString() + ", " + stats.playerRace.ToString() + " " + stats.playerProfession.ToString();

		health.text = stats.currentHealth.ToString() + "/" + stats.maxHealth.ToString();
		stamina.text = stats.currentStamina.ToString() + "/" + stats.maxStamina.ToString();
		mana.text = stats.currentMana.ToString() + "/" + stats.maxMana.ToString();

		if(lvlManager.pointsForLeveling > 0) {
			pointsLeft.text = lvlManager.pointsForLeveling.ToString() + " points available";

			bodyAddPointsButton.interactable = true;
			agilityAddPointsButton.interactable = true;
			mindAddPointsButton.interactable = true;
		}
		else {
			pointsLeft.text = "0 points available";

			bodyAddPointsButton.interactable = false;
			agilityAddPointsButton.interactable = false;
			mindAddPointsButton.interactable = false;
		}

		bodyPoints.text = skillsAttributes.body.ToString();
		agilityPoints.text = skillsAttributes.agility.ToString();
		mindPoints.text = skillsAttributes.mind.ToString();

		melee.text = "Melee " + skillsAttributes.melee.ToString();
		block.text = "Block " + skillsAttributes.block.ToString();
		heavyArmor.text = "Heavy Armor " + skillsAttributes.heavyArmor.ToString();
		acrobatics.text = "Acrobatics " + skillsAttributes.acrobatics.ToString();
		marksman.text = "Marksman " + skillsAttributes.marksman.ToString();
		lightArmor.text = "Light Armor " + skillsAttributes.lightArmor.ToString();
		speechcraft.text = "Speechcraft " + skillsAttributes.speechcraft.ToString();
		alchemy.text = "Alchemy " + skillsAttributes.alchemy.ToString();
		focus.text = "Focus " + skillsAttributes.focus.ToString();

		meleeProgress.fillAmount = skillsAttributes.meleeAdvancement;
		blockProgress.fillAmount = skillsAttributes.blockAdvancement;
		heavyArmorProgress.fillAmount = skillsAttributes.heavyArmorAdvancement;
		acrobaticsProgress.fillAmount = skillsAttributes.acrobaticsAdvancement;
		marksmanProgress.fillAmount = skillsAttributes.marksmanAdvancement;
		lightArmorProgress.fillAmount = skillsAttributes.lightArmorAdvancement;
		speechcraftProgress.fillAmount = skillsAttributes.speechcraftAdvancement;
		alchemyProgress.fillAmount = skillsAttributes.alchemyAdvancement;
		focusProgress.fillAmount = skillsAttributes.focusAdvancement;
	}
}
