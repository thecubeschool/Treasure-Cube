using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public enum LungumMonasteryRanks {
	Novice = 0,
	Monk = 1,
	ErrandMonk = 2,
	FatherMonk = 3,
	PrimeMonk = 4,
	KnightOfTheSun = 5,
	GrandKnight = 6,
	GrandMaster = 7,
}

public enum Gender {
	Male = 0,
	Female = 1,
}

public class PlayerStats : MonoBehaviour {

	[Space(20f)]
	public Transform playerCurrentCam;
	public GameObject playerDeathCam;
	public GameObject playerDeathUi;
	[Space(20f)]

	[Tooltip("Did someone saw the player? If there is someone that sees players when he do any of the crimes, we want to add the crime score.")]
	public bool playerSeen = false;

	[Space(20f)]

	public string playerName;
	public CharacterGender playerGender;
	public CharacterRace playerRace;
	public CharacterProtector playerProtector;
	public CharacterCulture playerCulture;
	public CharacterProfession playerProfession;

	public NPCHair playerHair;
	public NPCFacialHair playerBeard;
	public Color playerHairColor = Color.grey;
	public int currentHealth;
	public float currentHealthF;
	public int maxHealth;
	public float healthRegenMulti;
	public bool atFire;
	public float lastHealthDrain;
	private float healthTimer;
	public int currentStamina;
	public float currentStaminaF;
	public int maxStamina;
	public float staminaRegenMulti;
	public float lastStaminaDrain;
	private float staminaTimer;
	public AudioClip staminaDrainedSFX;
	public AudioSource staminaAs;
	public int currentMana;
	public int maxMana;
	public float lastManaDrain;
	private float manaTimer;

	[Space(20f)]

	public int crimeScore = 0;
	public int valorScore = 0;
	public int reputationScore = 0;
	public int placesFounded = 0;

	[Space(20f)]

	public int playerMoney = 0;
	public int maxCarryWeight = 0;
	public int currentWeight;
	public float currentWeightF;

	private Image healthImage;
	private Image staminaImage;
	private Image manaImage;
	private Text playerMoneyTxt;

	private PlayerSkillsAndAttributes skillsAttributes;
	private bool fetchedStats = false;
	private GameManager gameManager;
	private NewCharacterSetup ncs;

	void Start() {

		ncs = GameObject.FindObjectOfType<NewCharacterSetup> ();

		if (ncs != null) { 
			playerName = ncs.characterName;
			playerGender = ncs.characterGender;
			playerRace = ncs.characterRace;
			playerProtector = ncs.characterProtector;
			playerCulture = ncs.characterCulture;
			playerProfession = ncs.characterProfession;

			playerHair = (NPCHair)ncs.hairIndex;
			playerBeard = (NPCFacialHair)ncs.facialhairIndex;
			playerHairColor = ncs.hairColor;

			ncs.characterTransfered = true;
		} 
		else {
			playerName = "Leuton";
			playerGender = CharacterGender.Male;
			playerRace = CharacterRace.Elinian;
			playerProtector = CharacterProtector.Ilir;
			playerCulture = CharacterCulture.Empire;
			playerProfession = CharacterProfession.Knight;
			
			playerHair = NPCHair.LongHair;
			playerBeard = NPCFacialHair.MoustacheNSideburns;
			playerHairColor = Color.gray;
		}

		skillsAttributes = GetComponent<PlayerSkillsAndAttributes>();
		gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

		if (GameObject.Find ("_UICanvasGame") != null) {
			healthImage = GameObject.Find ("_UICanvasGame").transform.Find ("HealthStaminaMana/Health/BarHealth").GetComponent<Image> ();
			staminaImage = GameObject.Find ("_UICanvasGame").transform.Find ("HealthStaminaMana/Stamina/BarStamina").GetComponent<Image> ();
			manaImage = GameObject.Find ("_UICanvasGame").transform.Find ("HealthStaminaMana/Mana/BarMana").GetComponent<Image> ();
			playerMoneyTxt = GameObject.Find ("_UICanvasGame").transform.Find ("InventorySkillsQuestLog/MoneyTxt").GetComponent<Text> ();
		}
	}

	void Update() {

		currentWeight = (int)currentWeightF;

		if(fetchedStats == false) {
			maxHealth = skillsAttributes.body * 2;
			maxStamina = skillsAttributes.body + skillsAttributes.agility;
			maxMana = skillsAttributes.mind * 2;
			
			currentHealth = maxHealth;
			currentStamina = maxStamina;
			currentMana = maxMana;

			fetchedStats = true;
		}

		if(currentHealth < 1) {
			currentHealth = 0;

			playerDeathCam.transform.parent = null;
			foreach(Transform child in playerDeathCam.transform) {
				Destroy(child.gameObject);
			}
			playerDeathCam.GetComponent<SphereCollider>().enabled = true;
			playerDeathCam.GetComponent<Rigidbody>().useGravity = true;
			playerDeathCam.GetComponent<PlayerDeathCamera>().enabled = true;
			playerDeathCam.GetComponent<Camera>().clearFlags = CameraClearFlags.Skybox;

			Instantiate(playerDeathUi, playerCurrentCam.position, playerCurrentCam.rotation);
			gameObject.SetActive(false);
			GameObject.Find("_UICanvasGame").gameObject.SetActive(false);

			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
		if(currentStamina < 1) {
			currentStamina = 0;
		}
		if(currentMana < 1) {
			currentMana = 0;
		}

		if(maxHealth < 1) {
			maxHealth = 0;
		}
		if(maxStamina < 1) {
			maxStamina = 0;
		}
		if(maxMana < 1) {
			maxMana = 0;
		}

		if(gameManager.playerState != PlayerState.Fighting) {
			if(Time.time - lastHealthDrain > 3f) {
				if(currentHealth < maxHealth) {
					healthTimer += Time.deltaTime;
					if(healthTimer > 0.15f) {
						if(healthRegenMulti > 0f) {
							if(atFire == false) {
								currentHealthF += 1f * healthRegenMulti;
								currentHealth = (int)currentHealthF;
							}
							else {
								currentHealthF += 2f * healthRegenMulti;
								currentHealth = (int)currentHealthF;
							}
						}
						else {
							if(atFire == false) {
								currentHealthF += 1f * Time.deltaTime;
								currentHealth = (int)currentHealthF;
							}
							else {
								currentHealthF += 2f * Time.deltaTime;
								currentHealth = (int)currentHealthF;
							}
						}
						healthTimer = 0f;
					}
					else {
						currentHealthF = currentHealth;
					}
					if(currentHealth >= maxHealth) {
						currentHealth = maxHealth;
					}
				}
			}
		}
		if(Time.time - lastStaminaDrain > 3f) {
			if(currentStamina < maxStamina) {
				staminaTimer += Time.deltaTime;
				if(staminaTimer > 0.15f) {
					if(staminaRegenMulti > 0f) {
						currentStaminaF += 1f * staminaRegenMulti;
						currentStamina = (int)currentStaminaF;
					}
					else {
						currentStamina++;
					}
					staminaTimer = 0f;
				}
				else {
					currentStaminaF = currentStamina;
				}
				if(currentStamina >= maxStamina) {
					currentStamina = maxStamina;
				}
			}
		}
		if(Time.time - lastManaDrain > 3f) {
			if(currentMana < maxMana) {
				manaTimer += Time.deltaTime;
				if(manaTimer > 0.15f) {
					currentMana++;
					manaTimer = 0f;
				}
				if(currentMana >= maxMana) {
					currentMana = maxMana;
				}
			}
		}

		if (healthImage != null && staminaImage != null && manaImage != null && playerMoneyTxt != null) {
			healthImage.fillAmount = (float)currentHealth / maxHealth;
			staminaImage.fillAmount = (float)currentStamina / maxStamina;
			manaImage.fillAmount = (float)currentMana / maxMana;
			playerMoneyTxt.text = playerMoney.ToString ();
		}
		float tmpWeight = (skillsAttributes.body * 2f) * (maxStamina * 0.02f);
		maxCarryWeight = (int)tmpWeight;

		if(currentWeight > maxCarryWeight) {
			currentWeight = maxCarryWeight;
		}

		if(currentHealth > maxHealth) {
			currentHealth = maxHealth;
		}
		if(currentStamina > maxStamina) {
			currentStamina = maxStamina;
		}

		if (currentStamina < 6f) {
			staminaAs.clip = staminaDrainedSFX;
			if(staminaAs.isPlaying == false) {
				staminaAs.Play ();
			}
		} 
		else {
			if(staminaAs.isPlaying == true) {
				staminaAs.Stop();
			}
		}
	}
}
