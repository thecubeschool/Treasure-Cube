using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WeaponRangedAnimate : MonoBehaviour {

	private Animator anim;

	public bool stopAnimation;
	public bool dazed;

	public GameObject bloodFx;

	public CharacterController controller;
	public EquipmentManager equipManager;
	public PlayerSkillsAndAttributes skillsAttributes;
	public ShowMessage showMessage;
	public WeightedInventory inventory;
	public GameManager gameManager;
	public PlayerStats playerStats;
	public UISkillsAndAttributes uiSA;
	private GameObject playerGo;

	[Space(10f)]

	public Transform arrowFirePoint;

	public GameObject arrowIronPrefab;
	public GameObject arrowSteelPrefab;
	public GameObject arrowSilverPrefab;
	public GameObject arrowImperialPrefab;
	public GameObject arrowElvenPrefab;
	public GameObject arrowArdenPrefab;
	
	public float baseDrawPower;
	private float drawPower;
	private float skillMod;
	private float drawPowerCalculated;

	private GameObject arrowHolder;

    public int fireArrow = 0;

	void Awake() {
        if (GameObject.Find("_UICanvasGame")) {
            showMessage = GameObject.Find("_UICanvasGame").GetComponentInChildren<ShowMessage>();
            inventory = GameObject.Find("_UICanvasGame").GetComponentInChildren<WeightedInventory>();
        }

        if (GameObject.Find("[GameManager]")) {
            gameManager = GameObject.Find("[GameManager]").GetComponent<GameManager>();
        }
	}

	void Start () {
		if(anim == null) {
			anim = GetComponent<Animator>();
		}

		playerGo = GameObject.Find ("[Player]");
	}

    void Update() {
        if (equipManager.weaponSlot != null) {
            if (equipManager.weaponSlot.GetComponent<Item>().itemName.Contains("Bow")) {
                if (stopAnimation == false) {
                    if (equipManager.weaponDrawn == true) {
                        anim.SetBool("equiped", true);
                    }
                    else {
                        anim.SetBool("equiped", false);
                    }

                    if (dazed == false) {
                        anim.SetFloat("walkSpeed", controller.velocity.magnitude);

                        if (equipManager.missileSlot != null) {
                            if (equipManager.missileSlot.GetComponent<Item>().currentStackSize > 0) {

                                arrowHolder.SetActive(true);

                                if (Input.GetButton("Block")) {
                                    anim.SetBool("aiming", true);

                                    if (Input.GetButton("Attack")) {
                                        skillMod = (skillsAttributes.marksman * 0.03f) * 10f;
                                        drawPowerCalculated = baseDrawPower * skillMod;

                                        if (drawPower < drawPowerCalculated) {
                                            drawPower += Time.deltaTime * 250f;
                                        }
                                    }
                                    else if (Input.GetButtonUp("Attack")) {
                                        anim.SetBool("fire", true);
                                    }
                                    else {
                                        anim.SetBool("fire", false);
                                    }
                                }
                                else {
                                    anim.SetBool("aiming", false);
                                    anim.SetBool("fire", false);
                                }
                            }
                            else {
                                if (arrowHolder != null) {
                                    arrowHolder.SetActive(false);
                                }

                                anim.SetBool("aiming", false);
                                anim.SetBool("fire", false);

                                if (equipManager.weaponSlot.GetComponent<Item>().itemName.Contains("Bow")) {
                                    if (Input.GetButtonDown("Block")) {
                                        if (showMessage != null) {
                                            showMessage.SendTheMessage("I do not have any arrows left.");
                                        }
                                    }
                                    else if (Input.GetButtonDown("Attack")) {
                                        if (showMessage != null) {
                                            showMessage.SendTheMessage("I do not have any arrows left.");
                                        }
                                    }
                                }
                            }
                        }
                        else {
                            if (arrowHolder != null) {
                                arrowHolder.SetActive(false);
                            }

                            anim.SetBool("aiming", false);
                            anim.SetBool("fire", false);

                            if (equipManager.weaponSlot.GetComponent<Item>().itemName.Contains("Bow")) {
                                if (Input.GetButtonDown("Block")) {
                                    if (showMessage != null) {
                                        showMessage.SendTheMessage("I do not have any arrows left.");
                                    }
                                }
                                else if (Input.GetButtonDown("Attack")) {
                                    if (showMessage != null) {
                                        showMessage.SendTheMessage("I do not have any arrows left.");
                                    }
                                }
                            }
                        }
                    }
                    else {
                        anim.SetBool("equiped", false);
                        anim.SetBool("aiming", false);
                        anim.SetBool("fire", false);
                        anim.SetFloat("walkSpeed", 0f);
                    }
                }
                else {
                    anim.SetBool("equiped", false);
                    anim.SetBool("aiming", false);
                    anim.SetBool("fire", false);
                    anim.SetFloat("walkSpeed", 0f);
                }
            }
            else {
                anim.SetBool("equiped", false);
                anim.SetBool("aiming", false);
                anim.SetBool("fire", false);
                anim.SetFloat("walkSpeed", 0f);
            }
        }
    }

    private void FixedUpdate() {
        if(fireArrow == 1) {
            FireTheArrow();
            fireArrow = 0;
        }
    }

    public void FireTheArrowAnimation() {
        if (fireArrow == 0) {
            fireArrow = 1;
        }
    }

    public void FireTheArrow() {
		if(equipManager.missileSlot != null) {
			if(equipManager.missileSlot.GetComponent<Item>().itemName.Contains("Iron")) {
				GameObject arrow = Instantiate(arrowIronPrefab, arrowFirePoint.position, arrowFirePoint.rotation) as GameObject;
				
				if(skillsAttributes.marksman < 50) {
					float tmp = (drawPower / 10) * 1.15f;
					playerStats.currentStamina -= (int)tmp;
					playerStats.lastStaminaDrain = Time.time;
				}
				else {
					float tmp1 = ((drawPower / 10) * 1.15f) * 0.25f;
					float tmp2 = (drawPower / 10) - tmp1;
					playerStats.currentStamina -= (int)tmp2;
					playerStats.lastStaminaDrain = Time.time;
				}

                arrow.GetComponent<MissileDamager>().gameManager = gameManager;
				arrow.GetComponent<MissileDamager>().skillsAttributes = skillsAttributes;
				arrow.GetComponent<MissileDamager>().wra = this;
				arrow.GetComponent<MissileDamager>().spawner = playerGo;
				arrow.GetComponent<MissileDamager>().missileDamage = equipManager.missileDamage;
				arrow.GetComponent<MissileDamager>().uiSA = uiSA;
				arrow.GetComponent<MissileDamager>().playerArrow = true;

				if(equipManager.missileSlot.GetComponent<Item>().currentStackSize <= 1) {
					GameObject sltTmp = equipManager.missileSlot;
					equipManager.missileSlot = null;
					equipManager.missile = false;
					inventory.RemoveItemFromInventory(sltTmp);
					Destroy(sltTmp);
				}
				else {
					equipManager.missileSlot.GetComponent<Item>().currentStackSize--;
				}


                arrow.GetComponent<Rigidbody>().AddForce(arrowFirePoint.forward * drawPower, ForceMode.Impulse);
                arrow.GetComponent<Rigidbody>().centerOfMass = new Vector3(0, 0, 0.25f);

                drawPower = 0;
            }
			else if(equipManager.missileSlot.GetComponent<Item>().itemName.Contains("Steel")) {
				GameObject arrow = Instantiate(arrowSteelPrefab, arrowFirePoint.position, arrowFirePoint.rotation) as GameObject;
				
				
				if(skillsAttributes.marksman < 50) {
					float tmp = (drawPower / 10) * 1.15f;
					playerStats.currentStamina -= (int)tmp;
					playerStats.lastStaminaDrain = Time.time;
				}
				else {
					float tmp1 = ((drawPower / 10) * 1.15f) * 0.25f;
					float tmp2 = (drawPower / 10) - tmp1;
					playerStats.currentStamina -= (int)tmp2;
					playerStats.lastStaminaDrain = Time.time;
				}

				if(skillsAttributes.marksman < 50) {
					float tmp = (drawPower / 10) * 1.15f;
					playerStats.currentStamina -= (int)tmp;
					playerStats.lastStaminaDrain = Time.time;
				}
				else {
					float tmp1 = ((drawPower / 10) * 1.15f) * 0.25f;
					float tmp2 = (drawPower / 10) - tmp1;
					playerStats.currentStamina -= (int)tmp2;
					playerStats.lastStaminaDrain = Time.time;
				}

				arrow.GetComponent<MissileDamager>().gameManager = gameManager;
				arrow.GetComponent<MissileDamager>().wra = this;
				arrow.GetComponent<MissileDamager>().spawner = playerGo;
				arrow.GetComponent<MissileDamager>().missileDamage = equipManager.missileDamage;
				arrow.GetComponent<MissileDamager>().uiSA = uiSA;
				arrow.GetComponent<MissileDamager>().playerArrow = true;
				if(equipManager.missileSlot.GetComponent<Item>().currentStackSize == 1) {
					GameObject sltTmp = equipManager.missileSlot;
					equipManager.missileSlot = null;
					equipManager.missile = false;
					inventory.RemoveItemFromInventory(sltTmp);
					Destroy(sltTmp);
				}
				else {
					equipManager.missileSlot.GetComponent<Item>().currentStackSize--;
				}

                arrow.GetComponent<Rigidbody>().AddForce(arrowFirePoint.forward * drawPower);
                arrow.GetComponent<Rigidbody>().centerOfMass = new Vector3(0, 0, 0.25f);
                
                drawPower = 0;
            }
			else if(equipManager.missileSlot.GetComponent<Item>().itemName.Contains("Silver")) {
				GameObject arrow = Instantiate(arrowSilverPrefab, arrowFirePoint.position, arrowFirePoint.rotation) as GameObject;
				
				if(skillsAttributes.marksman < 50) {
					float tmp = (drawPower / 10) * 1.15f;
					playerStats.currentStamina -= (int)tmp;
					playerStats.lastStaminaDrain = Time.time;
				}
				else {
					float tmp1 = ((drawPower / 10) * 1.15f) * 0.25f;
					float tmp2 = (drawPower / 10) - tmp1;
					playerStats.currentStamina -= (int)tmp2;
					playerStats.lastStaminaDrain = Time.time;
				}
				
				arrow.GetComponent<MissileDamager>().gameManager = gameManager;
				arrow.GetComponent<MissileDamager>().wra = this;
				arrow.GetComponent<MissileDamager>().spawner = playerGo;
				arrow.GetComponent<MissileDamager>().missileDamage = equipManager.missileDamage;
				arrow.GetComponent<MissileDamager>().uiSA = uiSA;
				arrow.GetComponent<MissileDamager>().playerArrow = true;
				if(equipManager.missileSlot.GetComponent<Item>().currentStackSize == 1) {
					GameObject sltTmp = equipManager.missileSlot;
					equipManager.missileSlot = null;
					equipManager.missile = false;
					inventory.RemoveItemFromInventory(sltTmp);
					Destroy(sltTmp);
				}
				else {
					equipManager.missileSlot.GetComponent<Item>().currentStackSize--;
				}

                arrow.GetComponent<Rigidbody>().AddForce(arrowFirePoint.forward * drawPower);
                arrow.GetComponent<Rigidbody>().centerOfMass = new Vector3(0, 0, 0.25f);

                drawPower = 0;
            }
			else if(equipManager.missileSlot.GetComponent<Item>().itemName.Contains("Imperial")) {
				GameObject arrow = Instantiate(arrowImperialPrefab, arrowFirePoint.position, arrowFirePoint.rotation) as GameObject;
				
				if(skillsAttributes.marksman < 50) {
					float tmp = (drawPower / 10) * 1.15f;
					playerStats.currentStamina -= (int)tmp;
					playerStats.lastStaminaDrain = Time.time;
				}
				else {
					float tmp1 = ((drawPower / 10) * 1.15f) * 0.25f;
					float tmp2 = (drawPower / 10) - tmp1;
					playerStats.currentStamina -= (int)tmp2;
					playerStats.lastStaminaDrain = Time.time;
				}
				
				arrow.GetComponent<MissileDamager>().gameManager = gameManager;
				arrow.GetComponent<MissileDamager>().wra = this;
				arrow.GetComponent<MissileDamager>().spawner = playerGo;
				arrow.GetComponent<MissileDamager>().missileDamage = equipManager.missileDamage;
				arrow.GetComponent<MissileDamager>().uiSA = uiSA;
				arrow.GetComponent<MissileDamager>().playerArrow = true;
				if(equipManager.missileSlot.GetComponent<Item>().currentStackSize == 1) {
					GameObject sltTmp = equipManager.missileSlot;
					equipManager.missileSlot = null;
					equipManager.missile = false;
					inventory.RemoveItemFromInventory(sltTmp);
					Destroy(sltTmp);
				}
				else {
					equipManager.missileSlot.GetComponent<Item>().currentStackSize--;
				}

                arrow.GetComponent<Rigidbody>().AddForce(arrowFirePoint.forward * drawPower);
                arrow.GetComponent<Rigidbody>().centerOfMass = new Vector3(0, 0, 0.25f);

                drawPower = 0;
            }
			else if(equipManager.missileSlot.GetComponent<Item>().itemName.Contains("Elven")) {
				GameObject arrow = Instantiate(arrowElvenPrefab, arrowFirePoint.position, arrowFirePoint.rotation) as GameObject;
				
				if(skillsAttributes.marksman < 50) {
					float tmp = (drawPower / 10) * 1.15f;
					playerStats.currentStamina -= (int)tmp;
					playerStats.lastStaminaDrain = Time.time;
				}
				else {
					float tmp1 = ((drawPower / 10) * 1.15f) * 0.25f;
					float tmp2 = (drawPower / 10) - tmp1;
					playerStats.currentStamina -= (int)tmp2;
					playerStats.lastStaminaDrain = Time.time;
				}
				
				arrow.GetComponent<MissileDamager>().gameManager = gameManager;
				arrow.GetComponent<MissileDamager>().wra = this;
				arrow.GetComponent<MissileDamager>().spawner = playerGo;
				arrow.GetComponent<MissileDamager>().missileDamage = equipManager.missileDamage;
				arrow.GetComponent<MissileDamager>().uiSA = uiSA;
				arrow.GetComponent<MissileDamager>().playerArrow = true;
				if(equipManager.missileSlot.GetComponent<Item>().currentStackSize == 1) {
					GameObject sltTmp = equipManager.missileSlot;
					equipManager.missileSlot = null;
					equipManager.missile = false;
					inventory.RemoveItemFromInventory(sltTmp);
					Destroy(sltTmp);
				}
				else {
					equipManager.missileSlot.GetComponent<Item>().currentStackSize--;
				}

                arrow.GetComponent<Rigidbody>().AddForce(arrowFirePoint.forward * drawPower);
                arrow.GetComponent<Rigidbody>().centerOfMass = new Vector3(0, 0, 0.25f);

                drawPower = 0;
            }
			else if(equipManager.missileSlot.GetComponent<Item>().itemName.Contains("Arden")) {
				GameObject arrow = Instantiate(arrowArdenPrefab, arrowFirePoint.position, arrowFirePoint.rotation) as GameObject;
				
				if(skillsAttributes.marksman < 50) {
					float tmp = (drawPower / 10) * 1.15f;
					playerStats.currentStamina -= (int)tmp;
					playerStats.lastStaminaDrain = Time.time;
				}
				else {
					float tmp1 = ((drawPower / 10) * 1.15f) * 0.25f;
					float tmp2 = (drawPower / 10) - tmp1;
					playerStats.currentStamina -= (int)tmp2;
					playerStats.lastStaminaDrain = Time.time;
				}
				
				arrow.GetComponent<MissileDamager>().gameManager = gameManager;
				arrow.GetComponent<MissileDamager>().wra = this;
				arrow.GetComponent<MissileDamager>().spawner = playerGo;
				arrow.GetComponent<MissileDamager>().missileDamage = equipManager.missileDamage;
				arrow.GetComponent<MissileDamager>().uiSA = uiSA;
				arrow.GetComponent<MissileDamager>().playerArrow = true;
				if(equipManager.missileSlot.GetComponent<Item>().currentStackSize == 1) {
					GameObject sltTmp = equipManager.missileSlot;
					equipManager.missileSlot = null;
					equipManager.missile = false;
					inventory.RemoveItemFromInventory(sltTmp);
					Destroy(sltTmp);
				}
				else {
					equipManager.missileSlot.GetComponent<Item>().currentStackSize--;
				}

                arrow.GetComponent<Rigidbody>().AddForce(arrowFirePoint.forward * drawPower);
                arrow.GetComponent<Rigidbody>().centerOfMass = new Vector3(0, 0, 0.25f);

                drawPower = 0;
            }
		}
	}

	public void FindTheArrowInUse() {
		if(equipManager.weaponSlot != null) {
			if(equipManager.weaponSlot.GetComponent<Item>().itemName.Contains("Bow")) {
				Transform tmpHolder = equipManager.weaponSlot.GetComponent<Item>().itemGO.transform.Find("ArrowHolder").transform;

				foreach(Transform a in tmpHolder) {
					a.gameObject.SetActive(false);
				}

				foreach(Transform tmpArrow in tmpHolder) {
					if(equipManager.missileSlot.GetComponent<Item>().itemName.Contains("Iron")) {
						if(tmpArrow.name.Contains("Iron")) {
							arrowHolder = tmpArrow.gameObject;
						}
					}
					else if(equipManager.missileSlot.GetComponent<Item>().itemName.Contains("Steel")) {
						if(tmpArrow.name.Contains("Steel")) {
							arrowHolder = tmpArrow.gameObject;
						}
					}
					else if(equipManager.missileSlot.GetComponent<Item>().itemName.Contains("Silver")) {
						if(tmpArrow.name.Contains("Silver")) {
							arrowHolder = tmpArrow.gameObject;
						}
					}
					else if(equipManager.missileSlot.GetComponent<Item>().itemName.Contains("Imperial")) {
						if(tmpArrow.name.Contains("Imperial")) {
							arrowHolder = tmpArrow.gameObject;
						}
					}
					else if(equipManager.missileSlot.GetComponent<Item>().itemName.Contains("Elven")) {
						if(tmpArrow.name.Contains("Elven")) {
							arrowHolder = tmpArrow.gameObject;
						}
					}
					else if(equipManager.missileSlot.GetComponent<Item>().itemName.Contains("Arden")) {
						if(tmpArrow.name.Contains("Arden")) {
							arrowHolder = tmpArrow.gameObject;
						}
					}
				}
			}
		}
	}
}
