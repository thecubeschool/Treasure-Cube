using UnityEngine;
using System.Collections;

#pragma warning disable 0414

public class NPCDamageSender : MonoBehaviour {

	public bool dazed = false;

	public GameObject bloodFx;
	public AudioSource npcDamageAudio;
	public AudioSource npcFootstepsAudio;

    private int npcRealDamage;

	public AudioClip[] woodImpactSfx;
	public AudioClip[] metalImpactSfx;
	public AudioClip[] swingSfx;
	public AudioClip[] swing2Sfx;
	public AudioClip[] attackSfx;
	public AudioClip[] footstepsSfx;

	private bool inflictDamage;
	private bool step;

	private NPC npc;
	private NPCAiNavmesh npcAi;
	private EquipmentManager equipManager;
	private GameManager gameManager;
    private PlayerLevelManager pcLvlManager;
    private PlayerSkillsAndAttributes pcSkills;

	private Transform myTarget;
	private AudioSource shieldBashAs; //This one is on player for the sake of sfx

	private int randomDazeChance;

	void Awake() {
        pcLvlManager = GameObject.Find("[Player]").GetComponent<PlayerLevelManager>();
        pcSkills = GameObject.Find("[Player]").GetComponent<PlayerSkillsAndAttributes>();
        npc = GetComponentInParent<NPC>();
		npcAi = GetComponentInParent<NPCAiNavmesh>();
		if (GameObject.Find ("[Player]")) {
			equipManager = GameObject.Find ("[Player]").GetComponentInChildren<EquipmentManager> ();
		}
		if (GameObject.Find ("[GameManager]")) {
			gameManager = GameObject.Find ("[GameManager]").GetComponent<GameManager> ();
		}
	}

	void Start() {
		if (GameObject.Find ("[Player]")) {
			shieldBashAs = GameObject.Find ("[Player]").transform.Find ("Audio/ShieldBash").GetComponent<AudioSource> ();
		}
	}

	void Update () {
		myTarget = npcAi.target;

		npcAi.dazed = dazed;

        if(npc.npcDifficulity == NPCDifficultyLevel.VeryEasy) {
            float tmp = pcSkills.body * 0.2f;
            npcRealDamage = (int)tmp;
        }
        else if (npc.npcDifficulity == NPCDifficultyLevel.Easy) {
            float tmp = pcSkills.body * 0.25f;
            npcRealDamage = (int)tmp;
        }
        else if (npc.npcDifficulity == NPCDifficultyLevel.Medium) {
            float tmp = pcSkills.body * 0.4f;
            npcRealDamage = (int)tmp;
        }
        else if (npc.npcDifficulity == NPCDifficultyLevel.Hard) {
            float tmp = pcSkills.body * 0.65f;
            npcRealDamage = (int)tmp;
        }

        else if (npc.npcDifficulity == NPCDifficultyLevel.Deadly) {
            float tmp = pcSkills.body * 0.8f;
            npcRealDamage = (int)tmp;
        }

        if (inflictDamage == true) {
			if(myTarget != null && equipManager != null) {
				if(myTarget.tag == "Player") {
					if(myTarget.GetComponent<PlayerStats>().currentHealth <= 0) {
						myTarget = null;
					}

					float distance = Vector3.Distance(transform.parent.position, myTarget.position);

					PlaySoundDamege(swingSfx);
					if(distance <= 3f) {
						if(dazed == false) {
							if(myTarget.GetComponentInChildren<WeaponMeleeAnimate>().blocking == false) {
								float armorScoreTmp = myTarget.GetComponentInChildren<EquipmentManager>().armorScore;
								float calculatedDamage = 0f;
								if(armorScoreTmp == 0) {
									calculatedDamage = npcRealDamage;
								}
								else {
									calculatedDamage = npcRealDamage / (armorScoreTmp/10);
								}
								if(gameManager.playerImmortal == false) {
									if(myTarget.GetComponent<PlayerSkillsAndAttributes>().heavyArmor >= 80) {
										float tmp = calculatedDamage;
										float reflectedDamage = tmp * 0.1f;
										float tmp2 = tmp + reflectedDamage;
										npc.npcHealth -= (int)tmp2;
									}
									myTarget.GetComponent<PlayerStats>().currentHealth -= (int)calculatedDamage;
									myTarget.GetComponent<PlayerStats>().lastHealthDrain = Time.time;
								}
								gameManager.currentOpponentFighting = gameObject.transform.parent.gameObject;
								PlaySoundDamege(attackSfx);

								if(equipManager.headSlot != null && equipManager.chestSlot != null) {
									if(equipManager.headSlot.GetComponent<Item>().armorType == ArmorType.LightArmor &&
									   equipManager.chestSlot.GetComponent<Item>().armorType == ArmorType.LightArmor) {
										myTarget.GetComponent<PlayerSkillsAndAttributes>().lightArmorAdvancement +=  npcRealDamage * 0.025f;
									}
									else if(equipManager.headSlot.GetComponent<Item>().armorType == ArmorType.HeavyArmor &&
									        equipManager.chestSlot.GetComponent<Item>().armorType == ArmorType.HeavyArmor) {
										myTarget.GetComponent<PlayerSkillsAndAttributes>().heavyArmorAdvancement +=  npcRealDamage * 0.025f;
									}
									else if(equipManager.headSlot.GetComponent<Item>().armorType == ArmorType.LightArmor &&
									        equipManager.chestSlot.GetComponent<Item>().armorType == ArmorType.HeavyArmor) {
										myTarget.GetComponent<PlayerSkillsAndAttributes>().lightArmorAdvancement +=  npcRealDamage * 0.005f;
										myTarget.GetComponent<PlayerSkillsAndAttributes>().heavyArmorAdvancement +=  npcRealDamage * 0.010f;
									}
									else if(equipManager.headSlot.GetComponent<Item>().armorType == ArmorType.HeavyArmor &&
									        equipManager.chestSlot.GetComponent<Item>().armorType == ArmorType.LightArmor) {
										myTarget.GetComponent<PlayerSkillsAndAttributes>().lightArmorAdvancement +=  npcRealDamage * 0.010f;
										myTarget.GetComponent<PlayerSkillsAndAttributes>().heavyArmorAdvancement +=  npcRealDamage * 0.005f;
									}
								}
								else if(equipManager.headSlot != null && equipManager.chestSlot == null) {
									if(equipManager.headSlot.GetComponent<Item>().armorType == ArmorType.LightArmor) {
										myTarget.GetComponent<PlayerSkillsAndAttributes>().lightArmorAdvancement +=  npcRealDamage * 0.015f;
									}
									if(equipManager.headSlot.GetComponent<Item>().armorType == ArmorType.HeavyArmor) {
										myTarget.GetComponent<PlayerSkillsAndAttributes>().heavyArmorAdvancement +=  npcRealDamage * 0.015f;
									}
								}
								else if(equipManager.headSlot == null && equipManager.chestSlot != null) {
									if(equipManager.chestSlot.GetComponent<Item>().armorType == ArmorType.LightArmor) {
										myTarget.GetComponent<PlayerSkillsAndAttributes>().lightArmorAdvancement +=  npcRealDamage * 0.015f;
									}
									if(equipManager.chestSlot.GetComponent<Item>().armorType == ArmorType.HeavyArmor) {
										myTarget.GetComponent<PlayerSkillsAndAttributes>().heavyArmorAdvancement +=  npcRealDamage * 0.015f;
									}
								}

								npcAi.attackCooldown = Random.Range(0.1f, 2.25f);
								npcAi.attackingTimer = npcAi.attackCooldown;
								npcAi.castSphere = false;

								Instantiate(bloodFx, new Vector3(myTarget.position.x, myTarget.position.y+0.5f, myTarget.position.z+0.5f), Quaternion.identity);
							
								randomDazeChance = Random.Range(1, 100);

								if(randomDazeChance >= 35 && randomDazeChance <= 75) {
									myTarget.GetComponentInChildren<WeaponMeleeAnimate>().dazed = true;
								}
							}
							else {
								myTarget.GetComponent<PlayerSkillsAndAttributes>().blockAdvancement += npcRealDamage * 0.015f;
								myTarget.GetComponentInChildren<EquipmentManager>().shieldSlot.GetComponent<Item>().health -= npcRealDamage * 0.25f;
								myTarget.GetComponent<PlayerStats>().lastStaminaDrain = Time.time;

								randomDazeChance = Random.Range(1, 100);

								if(myTarget.GetComponent<PlayerSkillsAndAttributes>().block < 25) {
									float blocked = (equipManager.shieldScore/2f) + (myTarget.GetComponent<PlayerSkillsAndAttributes>().body * 0.1f);

									float armorScoreTmp = equipManager.armorScore;
									float calculatedDamage = 0f;

									calculatedDamage = (npcRealDamage / (armorScoreTmp/10)) + blocked;

									if(calculatedDamage < npcRealDamage) {
										if(gameManager.playerImmortal == false) {
											if(myTarget.GetComponent<PlayerSkillsAndAttributes>().heavyArmor >= 80) {
												float tmp = calculatedDamage;
												float reflectedDamage = tmp * 0.1f;
												float tmp2 = tmp + reflectedDamage;
												npc.npcHealth -= (int)tmp2;
											}
											myTarget.GetComponent<PlayerStats>().currentHealth -= (int)(npcRealDamage - calculatedDamage);
											myTarget.GetComponent<PlayerStats>().lastHealthDrain = Time.time;

											myTarget.GetComponent<PlayerStats>().currentStamina -= npcRealDamage;
											myTarget.GetComponent<PlayerStats>().lastStaminaDrain = Time.time;
										}
									}

									if(randomDazeChance >= 15 && randomDazeChance <= 85) {
										myTarget.GetComponentInChildren<WeaponMeleeAnimate>().dazed = true;
									}
								}
								else if(myTarget.GetComponent<PlayerSkillsAndAttributes>().block >= 25 &&
								        myTarget.GetComponent<PlayerSkillsAndAttributes>().block < 50) {
									float blocked = (equipManager.shieldScore/2f) + (myTarget.GetComponent<PlayerSkillsAndAttributes>().body * 0.1f);
									
									float armorScoreTmp = equipManager.armorScore;
									float calculatedDamage = 0f;
									
									calculatedDamage = (npcRealDamage / (armorScoreTmp/10)) + blocked;
									
									if(calculatedDamage < npcRealDamage) {
										if(gameManager.playerImmortal == false) {
											if(myTarget.GetComponent<PlayerSkillsAndAttributes>().heavyArmor >= 80) {
												float tmp = calculatedDamage;
												float reflectedDamage = tmp * 0.1f;
												float tmp2 = tmp + reflectedDamage;
												npc.npcHealth -= (int)tmp2;
											}
											myTarget.GetComponent<PlayerStats>().currentHealth -= (int)(npcRealDamage - calculatedDamage);
											myTarget.GetComponent<PlayerStats>().lastHealthDrain = Time.time;
											
											myTarget.GetComponent<PlayerStats>().currentStamina -= npcRealDamage;
											myTarget.GetComponent<PlayerStats>().lastStaminaDrain = Time.time;
										}
									}

									if(randomDazeChance >= 25 && randomDazeChance <= 75) {
										myTarget.GetComponentInChildren<WeaponMeleeAnimate>().dazed = true;
									}
								}
								else if(myTarget.GetComponent<PlayerSkillsAndAttributes>().block >= 50 &&
								        myTarget.GetComponent<PlayerSkillsAndAttributes>().block < 80) {
									float blocked = (equipManager.shieldScore/2f) + (myTarget.GetComponent<PlayerSkillsAndAttributes>().body * 0.1f);
									
									float armorScoreTmp = equipManager.armorScore;
									float calculatedDamage = 0f;
									
									calculatedDamage = (npcRealDamage / (armorScoreTmp/10)) + blocked;
									
									if(calculatedDamage < npcRealDamage) {
										if(gameManager.playerImmortal == false) {
											if(myTarget.GetComponent<PlayerSkillsAndAttributes>().heavyArmor >= 80) {
												float tmp = calculatedDamage;
												float reflectedDamage = tmp * 0.1f;
												float tmp2 = tmp + reflectedDamage;
												npc.npcHealth -= (int)tmp2;
											}
											myTarget.GetComponent<PlayerStats>().currentHealth -= (int)(npcRealDamage - calculatedDamage);
											myTarget.GetComponent<PlayerStats>().lastHealthDrain = Time.time;
											
											myTarget.GetComponent<PlayerStats>().currentStamina -= npcRealDamage;
											myTarget.GetComponent<PlayerStats>().lastStaminaDrain = Time.time;
										}
									}

									if(randomDazeChance >= 35 && randomDazeChance <= 75) {
										myTarget.GetComponentInChildren<WeaponMeleeAnimate>().dazed = true;
									}
								}
								else if(myTarget.GetComponent<PlayerSkillsAndAttributes>().block >= 80 &&
								        myTarget.GetComponent<PlayerSkillsAndAttributes>().block < 100) {
									float blocked = (equipManager.shieldScore/2f) + (myTarget.GetComponent<PlayerSkillsAndAttributes>().body * 0.1f);
									
									float armorScoreTmp = equipManager.armorScore;
									float calculatedDamage = 0f;
									
									calculatedDamage = (npcRealDamage / (armorScoreTmp/10)) + blocked;
									
									if(calculatedDamage < npcRealDamage) {
										if(gameManager.playerImmortal == false) {
											if(myTarget.GetComponent<PlayerSkillsAndAttributes>().heavyArmor >= 80) {
												float tmp = calculatedDamage;
												float reflectedDamage = tmp * 0.1f;
												float tmp2 = tmp + reflectedDamage;
												npc.npcHealth -= (int)tmp2;
											}
											myTarget.GetComponent<PlayerStats>().currentHealth -= (int)(int)(npcRealDamage - calculatedDamage);
											myTarget.GetComponent<PlayerStats>().lastHealthDrain = Time.time;
											
											myTarget.GetComponent<PlayerStats>().currentStamina -= npcRealDamage;
											myTarget.GetComponent<PlayerStats>().lastStaminaDrain = Time.time;
										}
									}

									if(randomDazeChance >= 45 && randomDazeChance <= 55) {
										myTarget.GetComponentInChildren<WeaponMeleeAnimate>().dazed = true;
									}
								}

								if(myTarget.GetComponentInChildren<EquipmentManager>().shieldSlot.GetComponent<Item>().itemName.Contains("Wood") ||
								   myTarget.GetComponentInChildren<EquipmentManager>().shieldSlot.GetComponent<Item>().itemName.Contains("Iron")) {
									PlaySoundOnPlayer(woodImpactSfx);
								}
								else {
									PlaySoundOnPlayer(metalImpactSfx);
								}

								if(equipManager.headSlot != null && equipManager.chestSlot != null) {
									if(equipManager.headSlot.GetComponent<Item>().armorType == ArmorType.LightArmor &&
									   equipManager.chestSlot.GetComponent<Item>().armorType == ArmorType.LightArmor) {
										myTarget.GetComponent<PlayerSkillsAndAttributes>().lightArmorAdvancement +=  npcRealDamage * 0.01f;
									}
									else if(equipManager.headSlot.GetComponent<Item>().armorType == ArmorType.HeavyArmor &&
									        equipManager.chestSlot.GetComponent<Item>().armorType == ArmorType.HeavyArmor) {
										myTarget.GetComponent<PlayerSkillsAndAttributes>().heavyArmorAdvancement +=  npcRealDamage * 0.01f;
									}
									else if(equipManager.headSlot.GetComponent<Item>().armorType == ArmorType.LightArmor &&
									        equipManager.chestSlot.GetComponent<Item>().armorType == ArmorType.HeavyArmor) {
										myTarget.GetComponent<PlayerSkillsAndAttributes>().lightArmorAdvancement +=  npcRealDamage * 0.002f;
										myTarget.GetComponent<PlayerSkillsAndAttributes>().heavyArmorAdvancement +=  npcRealDamage * 0.005f;
									}
									else if(equipManager.headSlot.GetComponent<Item>().armorType == ArmorType.HeavyArmor &&
									        equipManager.chestSlot.GetComponent<Item>().armorType == ArmorType.LightArmor) {
										myTarget.GetComponent<PlayerSkillsAndAttributes>().lightArmorAdvancement +=  npcRealDamage * 0.005f;
										myTarget.GetComponent<PlayerSkillsAndAttributes>().heavyArmorAdvancement +=  npcRealDamage * 0.002f;
									}
								}
								else if(equipManager.headSlot != null && equipManager.chestSlot == null) {
									if(equipManager.headSlot.GetComponent<Item>().armorType == ArmorType.LightArmor) {
										myTarget.GetComponent<PlayerSkillsAndAttributes>().lightArmorAdvancement +=  npcRealDamage * 0.008f;
									}
									if(equipManager.headSlot.GetComponent<Item>().armorType == ArmorType.HeavyArmor) {
										myTarget.GetComponent<PlayerSkillsAndAttributes>().heavyArmorAdvancement +=  npcRealDamage * 0.008f;
									}
								}
								else if(equipManager.headSlot == null && equipManager.chestSlot != null) {
									if(equipManager.chestSlot.GetComponent<Item>().armorType == ArmorType.LightArmor) {
										myTarget.GetComponent<PlayerSkillsAndAttributes>().lightArmorAdvancement +=  npcRealDamage * 0.008f;
									}
									if(equipManager.chestSlot.GetComponent<Item>().armorType == ArmorType.HeavyArmor) {
										myTarget.GetComponent<PlayerSkillsAndAttributes>().heavyArmorAdvancement +=  npcRealDamage * 0.008f;
									}
								}

								transform.parent.GetComponent<Rigidbody>().AddForce(myTarget.forward * 300f);
								dazed = true;
							}
						}
					}
					inflictDamage = false;
				}
				else if(myTarget.tag == "Npc") {
					float distance = Vector3.Distance(transform.parent.position, myTarget.position);

					PlaySoundDamege(swingSfx);
					if(distance <= 3f) {
						myTarget.GetComponent<NPC>().npcHealth -= npcRealDamage;
						myTarget.GetComponent<NPCAiNavmesh>().target = this.npc.transform;
						PlaySoundDamege(attackSfx);
						Instantiate(bloodFx, new Vector3(myTarget.position.x, myTarget.position.y+0.5f, myTarget.position.z+0.5f), Quaternion.identity);
					}
					inflictDamage = false;
				}
			}
		}

		if(step == true) {
			PlaySoundFootsteps(footstepsSfx);
			step = false;
		}

		if(dazed == true) {
			StartCoroutine(DazeTheNpc());
		}
	}

	void CanInflictDamage() {
		inflictDamage = true;
	}

	void CanFootstep() {
		step = true;
	}

	void PlaySoundDamege(AudioClip[] name) {
		if(npcDamageAudio != null) {
			npcDamageAudio.clip = name[Random.Range(0, name.Length)];
			npcDamageAudio.Play();
		}
	}

	void PlaySoundFootsteps(AudioClip[] name) {
		if(npcFootstepsAudio != null) {
			npcFootstepsAudio.clip = name[Random.Range(0, name.Length)];
			npcFootstepsAudio.Play();
		}
	}

	void PlaySoundOnPlayer(AudioClip[] name) {
		if(shieldBashAs != null) {
			shieldBashAs.clip = name[Random.Range(0, name.Length)];
			shieldBashAs.Play();
		}
	}

	IEnumerator DazeTheNpc() {
		if(dazed) {
			yield return new WaitForSeconds(Random.Range(1, 3));
			dazed = false;
		}
	}
}
