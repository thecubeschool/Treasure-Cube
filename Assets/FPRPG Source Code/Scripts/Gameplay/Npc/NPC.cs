using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.AI;

public enum NPCDifficultyLevel {
    VeryEasy = 0,
    Easy = 1,
    Medium = 2,
    Hard = 3,
    Deadly = 4,
}

public enum NPCFaction {
	None = 0,
	Commoners = 1,
	Bandits = 2,
	Guards = 3,
	Merchants = 4,
    Monster = 5,
    Animal = 6,
}

public enum NPCClass {
	None = 0,
	Peasant = 1,
	Commoner = 2,
	Blacksmith = 3,
	Hunter = 4,
	Merchant = 5,
	Bard = 6,
	Priest = 7,
	Knight = 8,
	Warrior = 9,
	Sorcerer = 10,
	Thief = 11,
	Barbarian = 12,
	Essential = 13,
}

public enum NPCDisposition {
	Neutral = 0,
	Hostile = 1,
	Friendly = 2,
	Ally = 3,
}

public enum NPCHair {
	None = 0,
	Bald = 1,
	ShortHair = 2,
	LongHair = 3,
	Cleantop = 4,
	Topfuzz = 5,
    TwistedHair = 6,
    BunHair = 7,
    StylizedHair = 8,
    PonytailHair = 9,
}

public enum NPCFacialHair {
	None = 0,
	Goatee = 1,
	Beard = 2,
	LongBeard = 3,
	ShortMoustache = 4,
	BigMoustache = 5,
	MoustacheNSideburns = 6,
    Sideburns = 7,
    LowerGoatee = 8,
}

public enum NPCOutfit {
	None = 0,
	GambesonGreen = 1,
	GambesonRed = 2,
	FurClothes = 3,
	RobeBlue = 5,
	RobeLungum = 6,
	RobeSinta = 7,
	ArmorIron = 8,
	ArmorSteel = 9,
	ArmorSilver = 10,
	ArmorImperial = 11,
	ArmorElven = 12,
	ArmorArden = 13,
	ArmorNorgarnian = 14,
	ArmorDarkSteel = 15,
	ArmorChronicler = 16,
	CoatDark = 17,
	RobePriest = 18,
	InkeeperGarments = 19,
    DressBrown = 20,
    DressBlue = 21,
    DressGreen = 22,
}

public enum NPCHelmet {
	None = 0,
	HatGreen = 1,
	FurCap = 2,
	FeatheraGreen = 3,
	FeatheraBlue = 4,
	HelmIron = 5,
	HelmSteel = 6,
	HelmSilver = 7,
	HelmImperial = 8,
	HelmElven = 9,
	HelmArden = 10,
	Eyepatch = 11,
	HelmDarkSteel = 12,
	HoodLungum = 13,
	HelmNorgarnian = 14,
	HelmChronicler = 15,
	HatPriest = 16,
	HoodBlack = 17,
}

public enum NPCWeapon {
	SwordIron = 0,
	SwordSilver = 1,
	SwordSteel = 2,
	SwordImperial = 3,
	SwordElven = 4,
	SwordArden = 5,
	MaceIron = 6,
	MaceSilver = 7,
	MaceSteel = 8,
	MaceImperial = 9,
	MaceElven = 10,
	MaceArden = 11,
	AxeIron = 12,
	AxeSilver = 13,
	AxeSteel = 14,
	AxeImperial = 15,
	AxeElven = 16,
	AxeArden = 17,
	BowWooden = 18,
	BowSilver = 19,
	BowSteel = 20,
	BowImperial = 21,
	BowElven = 22,
	BowArden = 23,
	SwordDarksteel = 24,
	MaceDarksteel = 25,
    AxeDarksteel = 26,
    SwordNorgarnian = 27,
    MaceNorgarnian = 28,
    AxeNorgarnian = 29,
	None = 30,
}

public enum NPCShield {
	ShieldWooden = 0,
	ShieldSilver = 1,
	ShieldSteel = 2,
	ShieldImperial = 3,
	ShieldElven = 4,
	ShieldArden = 5,
	ShieldDarksteel = 6,
	MiscTorch = 7,
	None = 8,
}

public enum NPCDirection {
	Right = 0,
	Left = 1,
}

public class NPC : MonoBehaviour {
    
    public int npcId = 0000;

	public GameObject[] lootBag;
	public GameObject bonesPrtcl;

	[Space(20f)]

	public Animator animBase;
	public Animator animHair;
	public Animator animFacialHair;
	public Animator animHelmHat;
	public Animator animOutfit;
	public Animator animWeapon;

	[Space(20f)]

	[TextArea(1,3)]
	public string npcName;

	[Tooltip("To what faction does this NPC belong to?")]
	public NPCFaction npcFaction;
	[Tooltip("How does this NPC feel about the player? Is he his enemy, friend, ally, or is he totaly neutral toward him?")]
	public NPCDisposition npcDisposition;

    [HideInInspector]
    public int npcHealth;
    [HideInInspector]
    public bool hasDied;
	[HideInInspector]
	public int npcMaxHealth;
    [Tooltip("How difficult to kill is this npc.")]
    public NPCDifficultyLevel npcDifficulity;
    //[HideInInspector]
    [Tooltip("Even though we set NPC Difficulity, we need to set the ammount of dammage the npc will deal to other npcs or player.")]
    public int npcDamage;
	
	[Space(20f)]
	
	public CharacterRace race;
    public CharacterGender gender;
	public Color hairColor = Color.gray;
	public NPCHair hair;
	public NPCFacialHair facialHair;
	public NPCHelmet helmetHat;
	public NPCOutfit outfit;
	public NPCWeapon weapon;
	public NPCShield shield;

	private PlayerStats playerStats;
    private PlayerSkillsAndAttributes pcSkills;
	private NPCAiNavmesh ai;
	[HideInInspector]
	public NPCOutfiter npcOutfiter;

    [Space(20f)]

    public bool setLooks = false;

	private GameManager gameManager;
	private GenericLootAssigner genLoot;

	private Vector3 localVelocity;
	private float rightOrLeft;
	private NPCDirection npcDirection;
	private UnityEngine.AI.NavMeshAgent nma;
	private float npcScale;

	public NPCTopic merchantTopic;
	public float merchantCounter;

    [Space(20f)]

    public bool willNpcRespawn = false;
    public int respawnAfterDays = 0;

    void Awake () {

        transform.localEulerAngles = Vector3.zero; // We always set our npc to zero rotation at start because if set otherwife it will not follow player as billboard 

		if (npcFaction == NPCFaction.Merchants) {
			if (merchantTopic == null) {
				Debug.LogError (gameObject.name + " merchant needs merchantTopic assigned.");
			}
		}

		if(GameObject.FindGameObjectWithTag("GameManager")) {
			gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
			genLoot = gameManager.gameObject.GetComponentInChildren<GenericLootAssigner>();
		}

		ai = GetComponent<NPCAiNavmesh>();
		npcOutfiter = GetComponent<NPCOutfiter>();

        if (GameObject.Find("[Player]")) {
            playerStats = GameObject.Find("[Player]").GetComponent<PlayerStats>();
            pcSkills = GameObject.Find("[Player]").GetComponent<PlayerSkillsAndAttributes>();
        }

		animBase.enabled = true;
		animHair.enabled = true;
		animFacialHair.enabled = true;
		animHelmHat.enabled = true;
		animOutfit.enabled = true;
		animWeapon.enabled = true;

		npcOutfiter.hairSr.material.color = hairColor;
		npcOutfiter.facialHairSr.material.color = hairColor;


        if (npcDifficulity == NPCDifficultyLevel.VeryEasy) {
            float tmp = pcSkills.body * 0.7f;
            npcHealth = (int)tmp;
        }
        else if (npcDifficulity == NPCDifficultyLevel.Easy) {
            float tmp = pcSkills.body * 0.95f;
            npcHealth = (int)tmp;
        }
        else if (npcDifficulity == NPCDifficultyLevel.Medium) {
            float tmp = pcSkills.body * 1.5f;
            npcHealth = (int)tmp;
        }
        else if (npcDifficulity == NPCDifficultyLevel.Hard) { //Very hard
            float tmp = pcSkills.body * 1.8f;
            npcHealth = (int)tmp;
        }
        else if (npcDifficulity == NPCDifficultyLevel.Deadly) { //Very very hard
            float tmp = pcSkills.body * 2.8f;
            npcHealth = (int)tmp;
        }

        if (race == CharacterRace.Werewolf) {
			transform.localScale = new Vector3(2.2f, 2.2f, 2.2f);
		}
        else if (race == CharacterRace.AnimalDeer) {
            transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
        }
        else {
			transform.localScale = new Vector3(2f, 2f, 2f);
		}

		npcMaxHealth = npcHealth;

		nma = GetComponent<UnityEngine.AI.NavMeshAgent> ();
		npcScale = npcOutfiter.baseSr.transform.localScale.x;

		if(!gameObject.name.Contains("Beast") && lootBag.Length <= 0) {
            if (npcDifficulity == NPCDifficultyLevel.VeryEasy) {
                if (genLoot.lowLvlLoot.Length > 0) {
                    float randFloat = Random.Range(0f, 1f);

                    if (randFloat >= 0f && randFloat <= 0.5f) {
                        List<GameObject> tmpList = lootBag.ToList();
                        tmpList.Add(genLoot.lowLvlLoot[Random.Range(0, genLoot.lowLvlLoot.Length)]);
                        lootBag = tmpList.ToArray();
                    }
                }
            }
            else if (npcDifficulity == NPCDifficultyLevel.Easy) {
                if (genLoot.mediumLvlLoot.Length > 0) {
                    float randFloat = Random.Range(0f, 1f);

                    if (randFloat >= 0f && randFloat <= 0.5f) {
                        List<GameObject> tmpList = lootBag.ToList();
                        tmpList.Add(genLoot.mediumLvlLoot[Random.Range(0, genLoot.mediumLvlLoot.Length)]);
                        lootBag = tmpList.ToArray();
                    }
                }
            }
            else if (npcDifficulity == NPCDifficultyLevel.Medium) {
                if (genLoot.highLvlLoot.Length > 0) {
                    float randFloat = Random.Range(0f, 1f);

                    if (randFloat >= 0f && randFloat <= 0.5f) {
                        List<GameObject> tmpList = lootBag.ToList();
                        tmpList.Add(genLoot.highLvlLoot[Random.Range(0, genLoot.highLvlLoot.Length)]);
                        lootBag = tmpList.ToArray();
                    }
                }
            }
            else if (npcDifficulity == NPCDifficultyLevel.Hard) {
                if (genLoot.extraLvlLoot.Length > 0) {
                    float randFloat = Random.Range(0f, 1f);

                    if (randFloat >= 0f && randFloat <= 0.5f) {
                        List<GameObject> tmpList = lootBag.ToList();
                        tmpList.Add(genLoot.extraLvlLoot[Random.Range(0, genLoot.extraLvlLoot.Length)]);
                        lootBag = tmpList.ToArray();
                    }
                }
            }
            else if (npcDifficulity == NPCDifficultyLevel.Deadly) {
                float randFloat = Random.Range(0f, 1f);

                if (randFloat >= 0f && randFloat <= 0.5f) {
                    List<GameObject> tmpList = lootBag.ToList();

                    foreach(GameObject go in tmpList) {
                        if(go.name == "CoinsBagRich") {
                            tmpList.Add(genLoot.extraLvlLoot[Random.Range(0, genLoot.extraLvlLoot.Length)]);
                            lootBag = tmpList.ToArray();
                        }
                    }
                }
            }
        }

        if(willNpcRespawn == false) {
            Destroy(GetComponent<NPCRespawner>());
        }
        else {
            if(respawnAfterDays <= 0) {
                respawnAfterDays = Random.Range(2, 14);
            }
            GetComponent<NPCRespawner>().daysUntilRespawn = respawnAfterDays;
        }
	}

    void Update() {
        if (npcHealth > 0) {
            if (npcFaction == NPCFaction.Merchants && !gameObject.name.Contains("STABLER")) {
                merchantCounter++;
                if (merchantCounter > 2400f) {
                    merchantTopic.shopGold += 40;
                    merchantCounter = 0;
                }
            }

            localVelocity = transform.InverseTransformDirection(nma.velocity);
            rightOrLeft = localVelocity.x;

            if (rightOrLeft > 0.5f) {
                if (npcDirection == NPCDirection.Left) {
                    npcDirection = NPCDirection.Right;
                }
            }
            else if (rightOrLeft < 0.5f) {
                if (npcDirection == NPCDirection.Right) {
                    npcDirection = NPCDirection.Left;
                }
            }

            if (gameObject.name.Contains("BEAST")) {
                if (npcDirection == NPCDirection.Left) {
                    npcOutfiter.baseSr.transform.localScale = new Vector3(npcScale, npcScale, npcScale);
                }
                else {
                    npcOutfiter.baseSr.transform.localScale = new Vector3(-npcScale, npcScale, npcScale);
                }
            }
            else if (gameObject.name.Contains("ANIMAL")) {
                if (npcDirection == NPCDirection.Left) {
                    npcOutfiter.baseSr.transform.localScale = new Vector3(npcScale, npcScale, npcScale);
                }
                else {
                    npcOutfiter.baseSr.transform.localScale = new Vector3(-npcScale, npcScale, npcScale);
                }
            }

            if (setLooks == false) {
                SetUpLooks();
                setLooks = true;
            }

            if (ai.engagedInCombat == true && npcFaction != NPCFaction.Guards) { //Ovo stavljamo ovako da bi strazari uvek imali isukano oruzje
                if (weapon == NPCWeapon.None) {
                    animWeapon.SetBool("equiped", false);
                }
                else {
                    animWeapon.SetBool("equiped", true);
                }
            }
            else if (npcFaction == NPCFaction.Guards) {
                animWeapon.SetBool("equiped", true);
            }
            else {
                animWeapon.SetBool("equiped", false);
            }

            //ANIMATIONS SECTION
            if (ai.movingState == NPCMovingState.Idle && ai.attackingState == NPCAttackingState.Idle) {
                //anim.SetBool("walk", false);
                animBase.SetBool("walk", false);
                animHair.SetBool("walk", false);
                animFacialHair.SetBool("walk", false);
                animHelmHat.SetBool("walk", false);
                animOutfit.SetBool("walk", false);
                animWeapon.SetBool("walk", false);
                animBase.SetBool("attack", false);
                animHair.SetBool("attack", false);
                animFacialHair.SetBool("attack", false);
                animHelmHat.SetBool("attack", false);
                animOutfit.SetBool("attack", false);
                animWeapon.SetBool("attack", false);
            }
            else if (ai.movingState == NPCMovingState.Moving && ai.attackingState == NPCAttackingState.Idle) {
                //anim.SetBool("walk", true);
                animBase.SetBool("walk", true);
                animHair.SetBool("walk", true);
                animFacialHair.SetBool("walk", true);
                animHelmHat.SetBool("walk", true);
                animOutfit.SetBool("walk", true);
                animWeapon.SetBool("walk", true);
                animBase.SetBool("attack", false);
                animHair.SetBool("attack", false);
                animFacialHair.SetBool("attack", false);
                animHelmHat.SetBool("attack", false);
                animOutfit.SetBool("attack", false);
                animWeapon.SetBool("attack", false);
            }
            else if (ai.attackingState == NPCAttackingState.Melee && ai.movingState == NPCMovingState.Idle) {
                //anim.SetBool("attack", true);
                animBase.SetBool("attack", true);
                animHair.SetBool("attack", true);
                animFacialHair.SetBool("attack", true);
                animHelmHat.SetBool("attack", true);
                animOutfit.SetBool("attack", true);
                animWeapon.SetBool("attack", true);
                animBase.SetBool("walk", false);
                animHair.SetBool("walk", false);
                animFacialHair.SetBool("walk", false);
                animHelmHat.SetBool("walk", false);
                animOutfit.SetBool("walk", false);
                animWeapon.SetBool("walk", false);
            }
            else if (ai.attackingState == NPCAttackingState.Idle) {
                //anim.SetBool("attack", false);
                animBase.SetBool("attack", false);
                animHair.SetBool("attack", false);
                animFacialHair.SetBool("attack", false);
                animHelmHat.SetBool("attack", false);
                animOutfit.SetBool("attack", false);
                animWeapon.SetBool("attack", false);
            }
            //ANIMATIONS SECTION

            if (npcFaction == NPCFaction.Bandits) {
                if (playerStats.crimeScore < 50) {
                    npcDisposition = NPCDisposition.Hostile;
                }
                else {
                    npcDisposition = NPCDisposition.Neutral;
                }
            }
            else if (npcFaction == NPCFaction.Guards) {
                if (playerStats.crimeScore >= 50) {
                    npcDisposition = NPCDisposition.Hostile;
                }
                else {
                    npcDisposition = NPCDisposition.Neutral;
                }
            }

            hasDied = false;
        }
        else {
            if (ai.addedToPlayerOpponents == true) {
                if (gameManager != null) {
                    gameManager.playerOpponents.Remove(gameObject);
                }
                ai.addedToPlayerOpponents = false;
            }

            if (hasDied == false) {
                Instantiate(bonesPrtcl, new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), Quaternion.identity);
                if (lootBag.Length > 0) {
                    DropObjectsFromLootBag();
                }
                if (gameManager != null) {
                    gameManager.playerState = PlayerState.Exploring;

                    if (gameObject.GetComponent<NPCRespawner>() != null) {
                        gameObject.GetComponent<NPCAiNavmesh>().target = null;
                        gameObject.GetComponent<NPCAiNavmesh>().targetInSight = false;

                        gameObject.GetComponent<NPCOutfiter>().enabled = false;
                        gameObject.GetComponent<CapsuleCollider>().enabled = false;
                        gameObject.GetComponent<BoxCollider>().enabled = false;
                        gameObject.GetComponent<NPCTopicHolder>().enabled = false;
                        gameObject.GetComponent<NavMeshAgent>().enabled = false;
                        gameObject.GetComponent<NPCAiNavmesh>().enabled = false;

                        animBase.gameObject.SetActive(false);
                        animHair.gameObject.SetActive(false);
                        animFacialHair.gameObject.SetActive(false);
                        animOutfit.gameObject.SetActive(false);
                        animHelmHat.gameObject.SetActive(false);
                        animWeapon.gameObject.SetActive(false);
                    }
                    else {
                        gameObject.SetActive(false);
                    }

                    hasDied = true;
                }
            }
        }
    }

	void DropObjectsFromLootBag() {
		if(lootBag.Length > 0) {
			foreach(GameObject o in lootBag) {
				Instantiate(o, new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), Quaternion.identity);
			}
		}
	}

	public void SetUpLooks() {		
		if(race == CharacterRace.Elinian) {
            if (gender == CharacterGender.Male) {
                animBase.SetBool("Elinian", true);
                animBase.SetBool("male", true);
            }
            else if(gender == CharacterGender.Female) {
                animBase.SetBool("Elinian", true);
                animBase.SetBool("female", true);
            }
        }
		else if(race == CharacterRace.Ariyan) {
            if (gender == CharacterGender.Male) {
                animBase.SetBool("Arian", true);
                animBase.SetBool("male", true);
            }
            else if (gender == CharacterGender.Female) {
                animBase.SetBool("Arian", true);
                animBase.SetBool("female", true);
            }
        }
		else if(race == CharacterRace.Sintarian) {
            if (gender == CharacterGender.Male) {
                animBase.SetBool("Sintarian", true);
                animBase.SetBool("male", true);
            }
            else if (gender == CharacterGender.Female) {
                animBase.SetBool("Sintarian", true);
                animBase.SetBool("female", true);
            }
        }
		else if(race == CharacterRace.Elevirian) {
            if (gender == CharacterGender.Male) {
                animBase.SetBool("Elevirian", true);
                animBase.SetBool("male", true);
            }
            else if (gender == CharacterGender.Female) {
                animBase.SetBool("Elevirian", true);
                animBase.SetBool("female", true);
            }
        }
		else if(race == CharacterRace.Koronian) {
            if (gender == CharacterGender.Male) {
                animBase.SetBool("Koronian", true);
                animBase.SetBool("male", true);
            }
            else if (gender == CharacterGender.Female) {
                animBase.SetBool("Koronian", true);
                animBase.SetBool("female", true);
            }
        }
		else if(race == CharacterRace.Skeleton) {
			animBase.SetBool("Skeleton", true);
		}
		else if(race == CharacterRace.Werewolf) {
			animBase.SetBool("Werewolf", true);
		}
		else if(race == CharacterRace.Bauk) {
			animBase.SetBool("Bauk", true);
		}
		else if(race == CharacterRace.BeastWolf) {
			animBase.SetBool("BeastWolf", true);
		}
        else if (race == CharacterRace.AnimalDeer) {
            animBase.SetBool("AnimalDeer", true);
        }

        if (hair == NPCHair.None) {
			animHair.SetBool("Bald", false);
			animHair.SetBool("Short Hair", false);
			animHair.SetBool("Long Hair", false);
		}
		else if(hair == NPCHair.Bald) {
			animHair.SetBool("Bald", true);
		}
		else if(hair == NPCHair.ShortHair) {
			animHair.SetBool("Short Hair", true);
		}
		else if(hair == NPCHair.LongHair) {
			animHair.SetBool("Long Hair", true);
		}
		else if(hair == NPCHair.Cleantop) {
			animHair.SetBool("Cleantop", true);
		}
		else if(hair == NPCHair.Topfuzz) {
			animHair.SetBool("Topfuzz", true);
		}
        else if (hair == NPCHair.TwistedHair) {
            animHair.SetBool("TwistedHair", true);
        }
        else if (hair == NPCHair.BunHair) {
            animHair.SetBool("BunHair", true);
        }
        else if (hair == NPCHair.StylizedHair) {
            animHair.SetBool("StylizedHair", true);
        }
        else if (hair == NPCHair.PonytailHair) {
            animHair.SetBool("PonytailHair", true);
        }

        if (facialHair == NPCFacialHair.None) {
			animFacialHair.SetBool("None", true);
		}
		else if(facialHair == NPCFacialHair.Goatee) {
			animFacialHair.SetBool("Goatee", true);
		}
		else if(facialHair == NPCFacialHair.Beard) {
			animFacialHair.SetBool("Beard", true);
		}
		else if(facialHair == NPCFacialHair.ShortMoustache) {
			animFacialHair.SetBool("ShortMoustache", true);
		}
		else if(facialHair == NPCFacialHair.BigMoustache) {
			animFacialHair.SetBool("BigMoustache", true);
		}
		else if(facialHair == NPCFacialHair.MoustacheNSideburns) {
			animFacialHair.SetBool("MoustacheNSideburns", true);
		}
		else if(facialHair == NPCFacialHair.LongBeard) {
			animFacialHair.SetBool("LongBeard", true);
		}
        else if (facialHair == NPCFacialHair.Sideburns) {
            animFacialHair.SetBool("Sideburns", true);
        }
        else if (facialHair == NPCFacialHair.LowerGoatee) {
            animFacialHair.SetBool("LowerGoatee", true);
        }


        if (helmetHat == NPCHelmet.None) {
			animHelmHat.SetBool("None", true);
		}
		else if(helmetHat == NPCHelmet.HatGreen) {
			animHelmHat.SetBool("HatGreen", true);
		}
		else if(helmetHat == NPCHelmet.FurCap) {
			animHelmHat.SetBool("FurCap", true);
		}
		else if(helmetHat == NPCHelmet.FeatheraGreen) {
			animHelmHat.SetBool("FeatheraGreen", true);
		}
		else if(helmetHat == NPCHelmet.FeatheraBlue) {
			animHelmHat.SetBool("FeatheraBlue", true);
		}
		else if(helmetHat == NPCHelmet.HelmIron) {
			animHelmHat.SetBool("HelmIron", true);
		}
		else if(helmetHat == NPCHelmet.HelmSteel) {
			animHelmHat.SetBool("HelmSteel", true);
		}
		else if(helmetHat == NPCHelmet.HelmSilver) {
			animHelmHat.SetBool("HelmSilver", true);
		}
		else if(helmetHat == NPCHelmet.HelmImperial) {
			animHelmHat.SetBool("HelmImperial", true);
		}
		else if(helmetHat == NPCHelmet.HelmElven) {
			animHelmHat.SetBool("HelmElven", true);
		}
		else if(helmetHat == NPCHelmet.HelmArden) {
			animHelmHat.SetBool("HelmArden", true);
		}
		else if(helmetHat == NPCHelmet.Eyepatch) {
			animHelmHat.SetBool("Eyepatch", true);
		}
		else if(helmetHat == NPCHelmet.HelmDarkSteel) {
			animHelmHat.SetBool("HelmDarkSteel", true);
		}
		else if(helmetHat == NPCHelmet.HelmChronicler) {
			animHelmHat.SetBool("HelmChronicler", true);
		}
		else if(helmetHat == NPCHelmet.HatPriest) {
			animHelmHat.SetBool("HatPriest", true);
		}
		else if(helmetHat == NPCHelmet.HoodBlack) {
			animHelmHat.SetBool("HoodBlack", true);
		}
		
		if(outfit == NPCOutfit.None) {
			animOutfit.SetBool("Nude", true);
		}
		else if(outfit == NPCOutfit.GambesonGreen) {
			animOutfit.SetBool("GambesonGreen", true);
		}
		else if(outfit == NPCOutfit.GambesonRed) {
			animOutfit.SetBool("GambesonRed", true);
		}
		else if(outfit == NPCOutfit.FurClothes) {
			animOutfit.SetBool("FurClothes", true);
		}
		else if(outfit == NPCOutfit.RobeBlue) {
			animOutfit.SetBool("RobeBlue", true);
		}
		else if(outfit == NPCOutfit.RobeLungum) {
			animOutfit.SetBool("RobeLungum", true);
		}
		else if(outfit == NPCOutfit.RobeSinta) {
			animOutfit.SetBool("RobeSinta", true);
		}
		else if(outfit == NPCOutfit.ArmorIron) {
			animOutfit.SetBool("ArmorIron", true);
		}
		else if(outfit == NPCOutfit.ArmorSteel) {
			animOutfit.SetBool("ArmorSteel", true);
		}
		else if(outfit == NPCOutfit.ArmorSilver) {
			animOutfit.SetBool("ArmorSilver", true);
		}
		else if(outfit == NPCOutfit.ArmorImperial) {
			animOutfit.SetBool("ArmorImperial", true);
		}
		else if(outfit == NPCOutfit.ArmorElven) {
			animOutfit.SetBool("ArmorElven", true);
		}
		else if(outfit == NPCOutfit.ArmorArden) {
			animOutfit.SetBool("ArmorArden", true);
		}
		else if(outfit == NPCOutfit.ArmorNorgarnian) {
			animOutfit.SetBool("ArmorNorgarnian", true);
		}
		else if(outfit == NPCOutfit.ArmorDarkSteel) {
			animOutfit.SetBool("ArmorDarkSteel", true);
		}
		else if(outfit == NPCOutfit.ArmorChronicler) {
			animOutfit.SetBool("ArmorChronicler", true);
		}
		else if(outfit == NPCOutfit.CoatDark) {
			animOutfit.SetBool("CoatDark", true);
		}
		else if(outfit == NPCOutfit.RobePriest) {
			animOutfit.SetBool("RobePriest", true);
		}
		else if(outfit == NPCOutfit.InkeeperGarments) {
			animOutfit.SetBool("InnkeeperGarments", true);
		}
        else if (outfit == NPCOutfit.DressBlue) {
            animOutfit.SetBool("DressBlue", true);
        }
        else if (outfit == NPCOutfit.DressBrown) {
            animOutfit.SetBool("DressBrown", true);
        }
        else if (outfit == NPCOutfit.DressGreen) {
            animOutfit.SetBool("DressGreen", true);
        }

        if (weapon == NPCWeapon.None) {
			animWeapon.SetBool("equiped", false);
		}
		else {
			animWeapon.SetBool("equiped", true);
		}
	}
}
