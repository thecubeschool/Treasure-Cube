using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerSkillsAndAttributes : MonoBehaviour {

	[Header("Attributes")]
	public int body;
	public int agility;
	public int mind;

	[Space(10f)]
	[Header("Body Skills")]
	public int melee;
	[Tooltip("Melee increases when player is hitting someone with a melee weapon.")]
	public float meleeAdvancement;
	public int block;
	[Tooltip("Block increases when player is blocking someones hits.")]
	public float blockAdvancement;
	public int heavyArmor;
	[Tooltip("Heavy Armor increases when player is hit by someone with a melee weapon while wearing heavy armor.")]
	public float heavyArmorAdvancement;
	
	[Space(10f)]
	[Header("Agility Skills")]
	public int acrobatics;
	[Tooltip("Acrobatics increases when player is moving or jumping.")]
	public float acrobaticsAdvancement;
	public int marksman;
	[Tooltip("Marksman increases when players arrow lands on target.")]
	public float marksmanAdvancement;
	public int lightArmor;
	[Tooltip("Light Armor increases when player is hit by someone with a melee weapon while wearing light armor.")]
	public float lightArmorAdvancement;

	[Space(10f)]
	[Header("Mind Skills")]
	public int speechcraft;
	[Tooltip("Speechcraft increases when player finishes quest.")]
	public float speechcraftAdvancement;
	public int alchemy;
	[Tooltip("Alchemy increases when player brews potion.")]
	public float alchemyAdvancement;
	public int focus;
	[Tooltip("Focus increases when player cast spells.")]
	public float focusAdvancement;

	[Space(10f)]
	[Header("Skills Info")]
	public int skillsIncreases;
	public int masteredSkills;

	private PlayerStats stats;
	private PlayerLevelManager playerLvlMng;
	private ShowMessage showMessage;
	public UISkillsAndAttributes uiSA;

	void Start () {

		stats = GetComponent<PlayerStats>();
		playerLvlMng = GetComponent<PlayerLevelManager>();

		if (GameObject.Find ("_UICanvasGame") != null) {
			showMessage = GameObject.Find ("_UICanvasGame").GetComponentInChildren<ShowMessage> ();
		}
		
		if(stats.playerRace == CharacterRace.Elinian) {
			body = 35;
			agility = 30;
			mind = 35;

			melee = 20;
			block = 10;
			heavyArmor = 10;
			//
			acrobatics = 15;
			marksman = 10;
			lightArmor = 15;
			//
			speechcraft = 10;
			alchemy = 10;
			focus = 10;
		}
		else if(stats.playerRace == CharacterRace.Ariyan) {
			body = 40;
			agility = 40;
			mind = 20;

			melee = 15;
			block = 15;
			heavyArmor = 20;
			//
			acrobatics = 10;
			marksman = 10;
			lightArmor = 10;
			//
			speechcraft = 10;
			alchemy = 10;
			focus = 10;
		}
		else if(stats.playerRace == CharacterRace.Sintarian) {
			body = 25;
			agility = 40;
			mind = 35;

			melee = 10;
			block = 10;
			heavyArmor = 10;
			//
			acrobatics = 10;
			marksman = 15;
			lightArmor = 10;
			//
			speechcraft = 10;
			alchemy = 20;
			focus = 15;
		}
		else if(stats.playerRace == CharacterRace.Koronian) {
			body = 20;
			agility = 40;
			mind = 40;

			melee = 10;
			block = 15;
			heavyArmor = 15;
			//
			acrobatics = 10;
			marksman = 10;
			lightArmor = 10;
			//
			speechcraft = 10;
			alchemy = 10;
			focus = 20;
		}
		else if(stats.playerRace == CharacterRace.Elevirian) {
			body = 35;
			agility = 20;
			mind = 45;

			melee = 10;
			block = 10;
			heavyArmor = 10;
			//
			acrobatics = 15;
			marksman = 20;
			lightArmor = 15;
			//
			speechcraft = 10;
			alchemy = 10;
			focus = 10;
		}

		if(stats.playerCulture == CharacterCulture.Empire) {
			melee += 5;
		}
		else if(stats.playerCulture == CharacterCulture.Ariya) {
			block += 5;
		}
		else if(stats.playerCulture == CharacterCulture.Senika) {
			heavyArmor += 5;
		}
		else if(stats.playerCulture == CharacterCulture.Sintar) {
			lightArmor += 5;
		}
		else if(stats.playerCulture == CharacterCulture.Elevir) {
			marksman += 5;
		}
		else if(stats.playerCulture == CharacterCulture.Korona) {
			alchemy += 5;
		}

		if(stats.playerProfession == CharacterProfession.Warrior) {
			melee += 5;
			block += 10;
			heavyArmor += 5;
		}
		else if(stats.playerProfession == CharacterProfession.Knight) {
			melee += 10;
			heavyArmor += 5;
			speechcraft += 5;
		}
		if(stats.playerProfession == CharacterProfession.Cleric) {
			melee += 5;
			lightArmor += 5;
			alchemy += 10;
		}
		if(stats.playerProfession == CharacterProfession.Hunter) {
			marksman += 10;
			lightArmor += 5;
			alchemy += 5;
		}
		if(stats.playerProfession == CharacterProfession.Agent) {
			acrobatics += 5;
			marksman += 5;
			lightArmor += 5;
		}
		if(stats.playerProfession == CharacterProfession.Ranger) {
			melee += 5;
			marksman += 5;
			focus += 10;
		}
	}

	bool mastered1;
	bool mastered2;
	bool mastered3;
	bool mastered4;
	bool mastered5;
	bool mastered6;
	bool mastered7;
	bool mastered8;
	bool mastered9;

	void Update() {
        if (playerLvlMng.playerLevel < 50) {

            if (melee < 100) {
                if (meleeAdvancement > 1f) {
                    if (showMessage != null) {
                        showMessage.SendTheMessage("Melee improved by 1 point.");
                    }
                    skillsIncreases++;
                    uiSA.skillsIncreases.text = "Skills increases: " + skillsIncreases.ToString();
                    melee++;
                    playerLvlMng.skillsAdvanced++;
                    meleeAdvancement = 0f;
                }
            }
            else {
                if (mastered1 == false) {
                    masteredSkills++;
                    uiSA.masteredSkills.text = "Mastered skills: " + masteredSkills.ToString();
                    mastered1 = true;
                }
            }
            if (block < 100) {
                if (blockAdvancement > 1f) {
                    if (showMessage != null) {
                        showMessage.SendTheMessage("Block improved by 1 point.");
                    }
                    skillsIncreases++;
                    uiSA.skillsIncreases.text = "Skills increases: " + skillsIncreases.ToString();
                    block++;
                    playerLvlMng.skillsAdvanced++;
                    blockAdvancement = 0f;
                }
            }
            else {
                if (mastered2 == false) {
                    masteredSkills++;
                    uiSA.masteredSkills.text = "Mastered skills: " + masteredSkills.ToString();
                    mastered2 = true;
                }
            }
            if (heavyArmor < 100) {
                if (heavyArmorAdvancement > 1f) {
                    if (showMessage != null) {
                        showMessage.SendTheMessage("Heavy Armor improved by 1 point.");
                    }
                    skillsIncreases++;
                    uiSA.skillsIncreases.text = "Skills increases: " + skillsIncreases.ToString();
                    heavyArmor++;
                    playerLvlMng.skillsAdvanced++;
                    heavyArmorAdvancement = 0f;
                }
            }
            else {
                if (mastered3 == false) {
                    masteredSkills++;
                    uiSA.masteredSkills.text = "Mastered skills: " + masteredSkills.ToString();
                    mastered3 = true;
                }
            }
            if (acrobatics < 100) {
                if (acrobaticsAdvancement > 1f) {
                    if (showMessage != null) {
                        showMessage.SendTheMessage("Acrobatics improved by 1 point.");
                    }
                    skillsIncreases++;
                    uiSA.skillsIncreases.text = "Skills increases: " + skillsIncreases.ToString();
                    acrobatics++;
                    playerLvlMng.skillsAdvanced++;
                    acrobaticsAdvancement = 0f;
                }
            }
            else {
                if (mastered4 == false) {
                    masteredSkills++;
                    uiSA.masteredSkills.text = "Mastered skills: " + masteredSkills.ToString();
                    mastered4 = true;
                }
            }
            if (marksman < 100) {
                if (marksmanAdvancement > 1f) {
                    if (showMessage != null) {
                        showMessage.SendTheMessage("Marksman improved by 1 point.");
                    }
                    skillsIncreases++;
                    uiSA.skillsIncreases.text = "Skills increases: " + skillsIncreases.ToString();
                    marksman++;
                    playerLvlMng.skillsAdvanced++;
                    marksmanAdvancement = 0f;
                }
            }
            else {
                if (mastered5 == false) {
                    masteredSkills++;
                    uiSA.masteredSkills.text = "Mastered skills: " + masteredSkills.ToString();
                    mastered5 = true;
                }
            }
            if (lightArmor < 100) {
                if (lightArmorAdvancement > 1f) {
                    if (showMessage != null) {
                        showMessage.SendTheMessage("Light Armor improved by 1 point.");
                    }
                    skillsIncreases++;
                    uiSA.skillsIncreases.text = "Skills increases: " + skillsIncreases.ToString();
                    lightArmor++;
                    playerLvlMng.skillsAdvanced++;
                    lightArmorAdvancement = 0f;
                }
            }
            else {
                if (mastered6 == false) {
                    masteredSkills++;
                    uiSA.masteredSkills.text = "Mastered skills: " + masteredSkills.ToString();
                    mastered6 = true;
                }
            }
            if (speechcraft < 100) {
                if (speechcraftAdvancement > 1f) {
                    if (showMessage != null) {
                        showMessage.SendTheMessage("Speechcraft improved by 1 point.");
                    }
                    skillsIncreases++;
                    uiSA.skillsIncreases.text = "Skills increases: " + skillsIncreases.ToString();
                    speechcraft++;
                    playerLvlMng.skillsAdvanced++;
                    speechcraftAdvancement = 0f;
                }
            }
            else {
                if (mastered7 == false) {
                    masteredSkills++;
                    uiSA.masteredSkills.text = "Mastered skills: " + masteredSkills.ToString();
                    mastered7 = true;
                }
            }
            if (alchemy < 100) {
                if (alchemyAdvancement > 1f) {
                    if (showMessage != null) {
                        showMessage.SendTheMessage("Alchemy improved by 1 point.");
                    }
                    skillsIncreases++;
                    uiSA.skillsIncreases.text = "Skills increases: " + skillsIncreases.ToString();
                    alchemy++;
                    playerLvlMng.skillsAdvanced++;
                    alchemyAdvancement = 0f;
                }
            }
            else {
                if (mastered8 == false) {
                    masteredSkills++;
                    uiSA.masteredSkills.text = "Mastered skills: " + masteredSkills.ToString();
                    mastered8 = true;
                }
            }
            if (focus < 100) {
                if (focusAdvancement > 1f) {
                    if (showMessage != null) {
                        showMessage.SendTheMessage("Focus improved by 1 point.");
                    }
                    skillsIncreases++;
                    uiSA.skillsIncreases.text = "Skills increases: " + skillsIncreases.ToString();
                    focus++;
                    playerLvlMng.skillsAdvanced++;
                    focusAdvancement = 0f;
                }
            }
            else {
                if (mastered9 == false) {
                    masteredSkills++;
                    uiSA.masteredSkills.text = "Mastered skills: " + masteredSkills.ToString();
                    mastered9 = true;
                }
            }
        }
	}

	public void SpendLevelingPoints(int attributeIndex) {

		if(attributeIndex == 1) {
            body++;
            stats.maxHealth = (body * 2) + body / 10;
            stats.currentHealth = stats.maxHealth;
            playerLvlMng.pointsForLeveling--;
        }
		else if(attributeIndex == 2) {
            agility++;
            stats.maxStamina = (body + agility) + agility / 10;
            stats.currentStamina = stats.maxStamina;
            playerLvlMng.pointsForLeveling--;
        }
		else if(attributeIndex == 3) {
            mind++;
            stats.maxMana = (mind * 2) + mind / 10;
            stats.currentMana = stats.maxMana;
            playerLvlMng.pointsForLeveling--;
        }
	}
}
