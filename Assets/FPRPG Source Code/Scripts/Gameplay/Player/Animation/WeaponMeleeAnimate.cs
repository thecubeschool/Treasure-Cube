using UnityEngine;
using System.Collections;

public class WeaponMeleeAnimate : MonoBehaviour {

	private Animator anim;
	private Animator animShield;
	private GameObject playerGo;

	public float meleeMultiplier;
	public bool stopAnimation;
	public bool dazed;

	public Transform particlePoint;
	public GameObject[] bloodFx;
	public GameObject dirtFx;

	public CharacterController controller;
	public Interact interact;
	public EquipmentManager equipManager;
	public FirstPersonPlayer fps;
	public PlayerStats stats;
	public PlayerSkillsAndAttributes skillsAttributes;
	public GameManager gameManager;
	public UISkillsAndAttributes uiSA;

	public bool canInflictDamage = false;
	public bool canDropParticle = false;
	public bool drainStamina = false;
	public int calculatedDamage;

	private bool holdBool = false;
	public bool powerAttack = false;
	private float timerForHold = 0f;

	public bool blocking = false;
	public bool bashing = false;

	private GameObject npcHit;
	private bool landedHit;

    void Start() {
        playerGo = GameObject.Find("[Player]");
        fps = playerGo.GetComponent<FirstPersonPlayer>();
        skillsAttributes = playerGo.GetComponent<PlayerSkillsAndAttributes>();
        stats = playerGo.GetComponent<PlayerStats>();
        if (anim == null) {
            anim = GetComponent<Animator>();
        }
        if (animShield == null) {
            animShield = transform.parent.Find("ShieldsGO").GetComponent<Animator>();
        }
    }

    void Update() {

        if (dazed) {
			StartCoroutine(DazeThePlayer());
		}

		if(stopAnimation == false) {
			if(equipManager.torch == true && equipManager.torchDrawn == true && equipManager.shield == false) {
				animShield.SetBool("equipeed", true);
				if(dazed == false) {
					animShield.SetFloat("speed", controller.velocity.magnitude);
				}
				else {
					StartCoroutine(DazeThePlayer());

					animShield.SetBool("hold", false);
					animShield.SetBool("attack", false);
					animShield.SetBool("block", false);
				}
			}
			else {
				//animShield.SetBool("equipeed", false);
			}

			if(equipManager.weaponSlot != null) {
				if(!equipManager.weaponSlot.GetComponent<Item>().itemGOName.Contains("Bow")) { //if we are not having bow equipped
					if(equipManager.weaponDrawn == false) {
						anim.SetBool("equipeed", false);
						if(equipManager.torchDrawn == false && equipManager.shield == true) {
							animShield.SetBool("equipeed", false);
						}
						blocking = false;
					}
					else if(equipManager.weaponDrawn == true) {
						anim.SetBool("equipeed", true);
						animShield.SetBool("equipeed", true);

						if(skillsAttributes.block >= 80) {
							anim.SetBool("canCharge", true);
							animShield.SetBool("canCharge", true);
						}
						else {
							anim.SetBool("canCharge", false);
							animShield.SetBool("canCharge", false);
						}

						if(equipManager.weaponSlot.GetComponent<Item>().itemGOName.Contains("Sword")) {
							if(meleeMultiplier == 0f) {
								anim.SetFloat("attackSpeed", 1f);
							}
							else {
								float tmp = 1f * meleeMultiplier;
								float tmp2 = 1f + tmp;
								anim.SetFloat("attackSpeed", tmp2);
							}
						}
						else if(equipManager.weaponSlot.GetComponent<Item>().itemGOName.Contains("Axe")) {
							if(meleeMultiplier == 0f) {
								anim.SetFloat("attackSpeed", 0.85f);
							}
							else {
								float tmp = 0.85f * meleeMultiplier;
								float tmp2 = 0.85f + tmp;
								anim.SetFloat("attackSpeed", tmp2);
							}
						}
						else if(equipManager.weaponSlot.GetComponent<Item>().itemGOName.Contains("Mace")) {
							if(meleeMultiplier == 0f) {
								anim.SetFloat("attackSpeed", 0.7f);
							}
							else {
								float tmp = 0.7f * meleeMultiplier;
								float tmp2 = 0.7f + tmp;
								anim.SetFloat("attackSpeed", tmp2);
							}
						}
                        // HERE COMES SPECIAL WEAPONS
                        else if (equipManager.weaponSlot.GetComponent<Item>().itemGOName.Contains("BladeOfTheFallen")) {
                            if (meleeMultiplier == 0f) {
                                anim.SetFloat("attackSpeed", 1.5f);
                            }
                            else {
                                float tmp = 1.5f * meleeMultiplier;
                                float tmp2 = 1.5f + tmp;
                                anim.SetFloat("attackSpeed", tmp2);
                            }
                        }

                        blocking = false;
					}

					if(dazed == false) {
						anim.SetFloat("walkSpeed", controller.velocity.magnitude);
						animShield.SetFloat("speed", controller.velocity.magnitude);

						if(skillsAttributes.acrobatics < 25) {
							if(fps.movingState != MovingSpeedState.Jumping) {
								if(Input.GetButton("Attack") && !Input.GetButton("Block")) {
									if(stats.currentStamina > 6) {
										anim.SetBool("hold", true);
										animShield.SetBool("hold", true);
										holdBool = true;
										blocking = false;
									}
								}
								else if(Input.GetButtonUp("Attack") && !Input.GetButton("Block")) {
									anim.SetBool("attack", true);
									animShield.SetBool("hold", false);
									anim.SetBool("block", false);
									animShield.SetBool("block", false);
									holdBool = false;
									blocking = false;
								}
								else if(Input.GetButton("Block") && !Input.GetButton("Attack")) {
									if(equipManager.shield == true) {
										animShield.SetBool("attack", false);
										anim.SetBool("block", true);
										animShield.SetBool("block", true);
										blocking = true;
									}
								}
								else if(Input.GetButton("Block") && Input.GetButtonDown("Attack")) {
									if(equipManager.shield == true && skillsAttributes.block >= 50) {
										animShield.SetBool("attack", false);
										anim.SetBool("block", true);
										animShield.SetBool("block", true);
										animShield.SetBool("attack", true);
										blocking = true;
									}
								}
								else if(Input.GetButton("Block") && Input.GetButton("Attack")) {
									if(equipManager.shield == true) {
										animShield.SetBool("attack", false);
										anim.SetBool("block", true);
										animShield.SetBool("block", true);
										blocking = true;
									}
								}
								else {
									anim.SetBool("hold", false);
									anim.SetBool("attack", false);
									anim.SetBool("block", false);
									animShield.SetBool("hold", false);
									animShield.SetBool("attack", false);
									animShield.SetBool("block", false);
									holdBool = false;
									blocking = false;
								}
							}
						}
						else {
							if(Input.GetButton("Attack") && !Input.GetButton("Block")) {
								if(stats.currentStamina > 6) {
									anim.SetBool("hold", true);
									animShield.SetBool("hold", true);
									holdBool = true;
									blocking = false;
								}
							}
							else if(Input.GetButtonUp("Attack") && !Input.GetButton("Block")) {
								anim.SetBool("attack", true);
								animShield.SetBool("hold", false);
								anim.SetBool("block", false);
								animShield.SetBool("block", false);
								holdBool = false;
								blocking = false;
							}
							else if(Input.GetButton("Block") && !Input.GetButton("Attack")) {
								if(equipManager.shield == true) {
									animShield.SetBool("attack", false);
									anim.SetBool("block", true);
									animShield.SetBool("block", true);
									blocking = true;
								}
							}
							else if(Input.GetButton("Block") && Input.GetButtonDown("Attack")) {
								if(equipManager.shield == true && skillsAttributes.block >= 50) {
									animShield.SetBool("attack", false);
									anim.SetBool("block", true);
									animShield.SetBool("block", true);
									animShield.SetBool("attack", true);
									blocking = true;
								}
							}
							else if(Input.GetButton("Block") && Input.GetButton("Attack")) {
								if(equipManager.shield == true) {
									animShield.SetBool("attack", false);
									anim.SetBool("block", true);
									animShield.SetBool("block", true);
									blocking = true;
								}
							}
							else {
								anim.SetBool("hold", false);
								anim.SetBool("attack", false);
								anim.SetBool("block", false);
								animShield.SetBool("hold", false);
								animShield.SetBool("attack", false);
								animShield.SetBool("block", false);
								holdBool = false;
								blocking = false;
							}
						}
					}
					else {
						StartCoroutine(DazeThePlayer());

						anim.SetBool("hold", false);
						anim.SetBool("attack", false);
						anim.SetBool("block", false);
						animShield.SetBool("hold", false);
						animShield.SetBool("attack", false);
						animShield.SetBool("block", false);
						holdBool = false;
						blocking = false;
					}

					if(canInflictDamage == true || bashing == true) {
						StartCoroutine(ResetDamageBoolean());
					}
					if(canDropParticle == true) {
						StartCoroutine(ResetParticleBoolean());
					}

					if(holdBool == true) {
						if(timerForHold < equipManager.weaponDamage) {
							timerForHold += Time.deltaTime * 6f;
							calculatedDamage = (int)timerForHold;
						}
						else {
							powerAttack = true;
						}
					}
					else {
						timerForHold = equipManager.weaponDamage / 2;
					}
				}
				else {
					anim.SetBool("equipeed", false);
					animShield.SetBool("equipeed", false);

					anim.SetFloat("walkSpeed", 0f);
					animShield.SetFloat("speed", 0f);

					anim.SetBool("hold", false);
					anim.SetBool("attack", false);
					anim.SetBool("block", false);
					animShield.SetBool("hold", false);
					animShield.SetBool("attack", false);
					animShield.SetBool("block", false);
				}
			}
			else {
				anim.SetBool("equipeed", false);
				if(equipManager.torchDrawn == false && equipManager.shield == true) {
					animShield.SetBool("equipeed", false);
				}
				
				anim.SetFloat("walkSpeed", 0f);
				animShield.SetFloat("speed", 0f);
				
				anim.SetBool("hold", false);
				anim.SetBool("attack", false);
				anim.SetBool("block", false);
				animShield.SetBool("hold", false);
				animShield.SetBool("attack", false);
				animShield.SetBool("block", false);
			}
		}
		else {
			if(equipManager.weaponDrawn == false && equipManager.torchDrawn == false) {
				anim.SetBool("equipeed", false);
				if(equipManager.torchDrawn == false && equipManager.shield == true) {
					animShield.SetBool("equipeed", false);
				}
			}

			anim.SetFloat("walkSpeed", 0f);
			animShield.SetFloat("speed", 0f);
			
			anim.SetBool("hold", false);
			anim.SetBool("attack", false);
			anim.SetBool("block", false);
			animShield.SetBool("hold", false);
			animShield.SetBool("attack", false);
			animShield.SetBool("block", false);
		}

		if(drainStamina == true) {
			if(skillsAttributes.melee < 20) {
				if(calculatedDamage == equipManager.weaponDamage) {
					stats.currentStamina -= 10;
				}
			}
			else if(skillsAttributes.melee >= 20 && skillsAttributes.melee < 40) {
				if(calculatedDamage == equipManager.weaponDamage) {
					stats.currentStamina -= 8;
				}
			}
			else if(skillsAttributes.melee >= 40 && skillsAttributes.melee < 60) {
				if(calculatedDamage == equipManager.weaponDamage) {
					stats.currentStamina -= 6;
				}
			}
			else if(skillsAttributes.melee >= 60 && skillsAttributes.melee < 80) {
				if(calculatedDamage == equipManager.weaponDamage) {
					stats.currentStamina -= 5;
				}
			}
			else if(skillsAttributes.melee >= 80 && skillsAttributes.melee < 100) {
				if(calculatedDamage == equipManager.weaponDamage) {
					stats.currentStamina -= 4;
				}
			}
			else if(skillsAttributes.melee == 100) {
				if(calculatedDamage == equipManager.weaponDamage) {
					stats.currentStamina -= 2;
				}
			}
			stats.lastStaminaDrain = Time.time;

			drainStamina = false;
		}

		if(canDropParticle == true && landedHit == true) {
			if(npcHit != null) {
				canDropParticle = false;
				npcHit = null;
				bashing = false;
			}
			else {
				canDropParticle = false;
				landedHit = false;
				bashing = false;
			}
		}
	}

	public void DealTheDamageToNpc(int damage) {
		if(damage == 1) {
			canInflictDamage = true;
			drainStamina = true;
			bashing = false;
		}
	}

	public void CanShowParticle() {
		canDropParticle = true;
	}

	void OnTriggerEnter(Collider col) {
		if(canInflictDamage == true) {
			if(col.gameObject.tag == "Npc" && col.gameObject.tag != "IgnoreTag") {
				if(bashing == false) {
					landedHit = true;
					npcHit = col.gameObject;
					if(col.GetComponent<NPC>().npcDisposition != NPCDisposition.Hostile) {
						if(col.GetComponent<NPC>().npcDisposition != NPCDisposition.Ally) {
							col.GetComponent<NPC>().npcDisposition = NPCDisposition.Hostile;
							col.GetComponent<NPCAiNavmesh>().engagedInCombat = true;
						}
						if(col.GetComponent<NPC>().weapon == NPCWeapon.None) {
							//col.GetComponent<NPCAi>().isCoward = true;
						}

						if(col.GetComponent<NPC>().npcFaction != NPCFaction.Bandits) {
							if(stats.crimeScore < 100) {
								stats.crimeScore += 50;
								if(stats.valorScore >= 25) {
									stats.valorScore -= 25;
								}
								else {
									stats.valorScore = 0;
								}
							}
							uiSA.crimeScore.text = "Crime score: " + stats.crimeScore.ToString();
							uiSA.valorScore.text = "Valor score: " + stats.valorScore.ToString();
						}
					}
					GetComponent<PlaySFXOnAnimation>().WeaponHitFXPlay();
					//col.GetComponent<Rigidbody>().AddForce(transform.parent.transform.parent.transform.parent.forward * 300f);

					if(gameManager.currentOpponentFighting != col.gameObject) {
						gameManager.currentOpponentFighting = col.gameObject;
						gameManager.lastTimeOpponentHit = Time.time;
					}

					if(equipManager.weaponSlot.GetComponent<Item>().itemGOName.Contains("Silver") &&
					   col.GetComponent<NPC>().npcFaction == NPCFaction.Monster) {
						float finalDamage = calculatedDamage * 1.15f;
						col.GetComponent<NPC>().npcHealth -= (int)finalDamage;
					}
					else {
						col.GetComponent<NPC>().npcHealth -= calculatedDamage;
					}
					col.GetComponent<NPCAiNavmesh>().target = playerGo.transform;
                    if (equipManager.weaponSlot.GetComponent<Item>().health > 0f) {
                        equipManager.weaponSlot.GetComponent<Item>().health -= (0.03f * equipManager.weaponDamage);
                    }

					if(skillsAttributes.melee >= 80) {
						if(powerAttack == true) {
							col.GetComponentInChildren<NPCDamageSender>().dazed = true;
						}
					}

					skillsAttributes.meleeAdvancement += equipManager.weaponDamage * 0.004f;
					Instantiate(bloodFx[Random.Range(0, bloodFx.Length)], particlePoint.position, Quaternion.identity);
					calculatedDamage = 0;
					landedHit = false;
					canInflictDamage = false;
				}
			}
			else if(col.gameObject.tag != "Npc" && col.gameObject.tag != "IgnoreTag") {
				GetComponent<PlaySFXOnAnimation>().WeaponHitObjectFXPlay();
				Instantiate(dirtFx, particlePoint.position, Quaternion.identity);
				landedHit = true;
			}
		}
	}

	IEnumerator ResetDamageBoolean() {
		yield return new WaitForSeconds(0.2f);
		powerAttack = false;
		canInflictDamage = false;
		bashing = false;
	}

	IEnumerator ResetParticleBoolean() {
		yield return new WaitForSeconds(0.2f);
		canDropParticle = false;
	}

	IEnumerator DazeThePlayer() {
		yield return new WaitForSeconds(Random.Range(0.5f, 3.0f));
		dazed = false;
	}
}
