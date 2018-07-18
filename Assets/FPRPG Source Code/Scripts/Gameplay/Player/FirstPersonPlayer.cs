using UnityEngine;
using System.Collections;

public enum MovingSpeedState {
	Standing = 0,
	Jumping = 1,
	Walking = 2,
	Running = 3,
	OnHorse = 4,
	Swimming = 5,
	Dazed = 6,
}

public class FirstPersonPlayer : MonoBehaviour {

	public float timeSpeedFactor = 1f;

	public MovingSpeedState movingState;

	private CharacterController controller;
	private PlayerStats stats;
	private PlayerSkillsAndAttributes skillsAttributes;
	private WeaponMeleeAnimate wepMAnim;
	private EquipmentManager equipManager;

	public LayerMask waterLayer;
	private Transform waterCheckPoint;

	public bool onFoot = true;
	public bool inWater = false;

	public float baseSpeed = 0f;
	public float speedMultiplier = 0f;
	public float swimingFactor = 1f;
	public float walkingSpeed = 3.0f;
	public float runningSpeed = 5.0f;
	public float jumpingSpeed = 8.0f;
	public float horseSpeed = 8.0f;
	public float slowedSpeed = 1f;

	public float gravity = 20.0f;
	private Vector3 moveDirection = Vector3.zero;

	private float hor;
	private float ver;

	public float playerVelocity;

	private float speedIncreaser;
	private float staminaTimer;

    void Start() {
		controller = GetComponent<CharacterController>();
		stats = GetComponent<PlayerStats>();
		skillsAttributes = GetComponent<PlayerSkillsAndAttributes>();
		waterCheckPoint = transform.Find("WaterCheckPoint").transform;
		wepMAnim = GetComponentInChildren<WeaponMeleeAnimate>();
		equipManager = GetComponentInChildren<EquipmentManager>();
	}

	void Update() {

        CheckForWater();

		playerVelocity = controller.velocity.magnitude;

		if (speedMultiplier == 0f) {
			// ARMOR and HELMET
			if(equipManager.head == true && equipManager.chest == true) {
				if(equipManager.headSlot.GetComponent<Item>().armorType == ArmorType.LightArmor &&
				   equipManager.chestSlot.GetComponent<Item>().armorType == ArmorType.LightArmor) {
					if(skillsAttributes.lightArmor < 50) {
						baseSpeed = ((0.015f * skillsAttributes.acrobatics) + (0.01f * skillsAttributes.agility)) * 0.5f;
					}
					else {
						baseSpeed = (0.015f * skillsAttributes.acrobatics) + (0.01f * skillsAttributes.agility);
					}
				}
				else if(equipManager.headSlot.GetComponent<Item>().armorType == ArmorType.HeavyArmor &&
				        equipManager.chestSlot.GetComponent<Item>().armorType == ArmorType.HeavyArmor) {
					if(skillsAttributes.heavyArmor < 50) {
						baseSpeed = ((0.015f * skillsAttributes.acrobatics) + (0.01f * skillsAttributes.agility)) * 0.3f;
					}
					else {
						baseSpeed = (0.015f * skillsAttributes.acrobatics) + (0.01f * skillsAttributes.agility);
					}
				}
				else {
					baseSpeed = ((0.015f * skillsAttributes.acrobatics) + (0.01f * skillsAttributes.agility)) * 0.6f;
				}
			}
			// ARMOR
			else if(equipManager.head == false && equipManager.chest == true) {
				if(equipManager.chestSlot.GetComponent<Item>().armorType == ArmorType.LightArmor) {
					if(skillsAttributes.lightArmor < 50) {
						baseSpeed = ((0.015f * skillsAttributes.acrobatics) + (0.01f * skillsAttributes.agility)) * 0.65f;
					}
					else {
						baseSpeed = ((0.015f * skillsAttributes.acrobatics) + (0.01f * skillsAttributes.agility));
					}
				}
				else if(equipManager.chestSlot.GetComponent<Item>().armorType == ArmorType.HeavyArmor) {
					if(skillsAttributes.heavyArmor < 50) {
						baseSpeed = ((0.015f * skillsAttributes.acrobatics) + (0.01f * skillsAttributes.agility)) * 0.5f;
					}
					else {
						baseSpeed = ((0.015f * skillsAttributes.acrobatics) + (0.01f * skillsAttributes.agility));
					}
				}
			}
			// HELMET
			else if(equipManager.head == true && equipManager.chest == false) {
				if(equipManager.headSlot.GetComponent<Item>().armorType == ArmorType.LightArmor) {
					if(skillsAttributes.lightArmor < 50) {
						baseSpeed = ((0.015f * skillsAttributes.acrobatics) + (0.01f * skillsAttributes.agility)) * 0.85f;
					}
					else {
						baseSpeed = ((0.015f * skillsAttributes.acrobatics) + (0.01f * skillsAttributes.agility));
					}
				}
				else if(equipManager.headSlot.GetComponent<Item>().armorType == ArmorType.HeavyArmor) {
					if(skillsAttributes.heavyArmor < 50) {
						baseSpeed = ((0.015f * skillsAttributes.acrobatics) + (0.01f * skillsAttributes.agility)) * 0.8f;
					}
					else {
						baseSpeed = ((0.015f * skillsAttributes.acrobatics) + (0.01f * skillsAttributes.agility));
					}
				}
			}
			// NONE
			else if(equipManager.head == false && equipManager.chest == false) {
				baseSpeed = (0.015f * skillsAttributes.acrobatics) + (0.01f * skillsAttributes.agility);
			}
		}
		else {
			// HEAD and ARMOR
			if(equipManager.head == true && equipManager.chest == true) {
				if(equipManager.headSlot.GetComponent<Item>().armorType == ArmorType.LightArmor &&
				   equipManager.chestSlot.GetComponent<Item>().armorType == ArmorType.LightArmor) {
					if(skillsAttributes.lightArmor < 50) {
						float tmp = (0.015f * skillsAttributes.acrobatics) + (0.01f * skillsAttributes.agility);
						float tmp2 = tmp * speedMultiplier;
						baseSpeed = (tmp + tmp2) * 0.5f;
					}
					else {
						float tmp = (0.015f * skillsAttributes.acrobatics) + (0.01f * skillsAttributes.agility);
						float tmp2 = tmp * speedMultiplier;
						baseSpeed = tmp + tmp2;
					}
				}
				else if(equipManager.headSlot.GetComponent<Item>().armorType == ArmorType.HeavyArmor &&
				        equipManager.chestSlot.GetComponent<Item>().armorType == ArmorType.HeavyArmor) {
					if(skillsAttributes.heavyArmor < 50) {
						float tmp = (0.015f * skillsAttributes.acrobatics) + (0.01f * skillsAttributes.agility);
						float tmp2 = tmp * speedMultiplier;
						baseSpeed = (tmp + tmp2) * 0.5f;
					}
					else {
						float tmp = (0.015f * skillsAttributes.acrobatics) + (0.01f * skillsAttributes.agility);
						float tmp2 = tmp * speedMultiplier;
						baseSpeed = tmp + tmp2;
					}
				}
			}
			// ARMOR
			else if(equipManager.head == false && equipManager.chest == true) {
				if(equipManager.chestSlot.GetComponent<Item>().armorType == ArmorType.LightArmor) {
					if(skillsAttributes.lightArmor < 50) {
						float tmp = (0.015f * skillsAttributes.acrobatics) + (0.01f * skillsAttributes.agility);
						float tmp2 = tmp * speedMultiplier;
						baseSpeed = (tmp + tmp2) * 0.5f;
					}
					else {
						float tmp = (0.015f * skillsAttributes.acrobatics) + (0.01f * skillsAttributes.agility);
						float tmp2 = tmp * speedMultiplier;
						baseSpeed = tmp + tmp2;
					}
				}
				if(equipManager.chestSlot.GetComponent<Item>().armorType == ArmorType.HeavyArmor) {
					if(skillsAttributes.heavyArmor < 50) {
						float tmp = (0.015f * skillsAttributes.acrobatics) + (0.01f * skillsAttributes.agility);
						float tmp2 = tmp * speedMultiplier;
						baseSpeed = (tmp + tmp2) * 0.65f;
					}
					else {
						float tmp = (0.015f * skillsAttributes.acrobatics) + (0.01f * skillsAttributes.agility);
						float tmp2 = tmp * speedMultiplier;
						baseSpeed = tmp + tmp2;
					}
				}
			}
			// HEAD
			else if(equipManager.head == true && equipManager.chest == false) {
				if(equipManager.headSlot.GetComponent<Item>().armorType == ArmorType.LightArmor) {
					if(skillsAttributes.lightArmor < 50) {
						float tmp = (0.015f * skillsAttributes.acrobatics) + (0.01f * skillsAttributes.agility);
						float tmp2 = tmp * speedMultiplier;
						baseSpeed = (tmp + tmp2) * 0.85f;
					}
					else {
						float tmp = (0.015f * skillsAttributes.acrobatics) + (0.01f * skillsAttributes.agility);
						float tmp2 = tmp * speedMultiplier;
						baseSpeed = tmp + tmp2;
					}
				}
				if(equipManager.headSlot.GetComponent<Item>().armorType == ArmorType.HeavyArmor) {
					if(skillsAttributes.heavyArmor < 50) {
						float tmp = (0.015f * skillsAttributes.acrobatics) + (0.01f * skillsAttributes.agility);
						float tmp2 = tmp * speedMultiplier;
						baseSpeed = (tmp + tmp2) * 0.8f;
					}
					else {
						float tmp = (0.015f * skillsAttributes.acrobatics) + (0.01f * skillsAttributes.agility);
						float tmp2 = tmp * speedMultiplier;
						baseSpeed = tmp + tmp2;
					}
				}
			}
			// NONE
			else if(equipManager.head == false && equipManager.chest == false) {
				float tmp = (0.015f * skillsAttributes.acrobatics) + (0.01f * skillsAttributes.agility);
				float tmp2 = tmp * speedMultiplier;
				baseSpeed = tmp + tmp2;
			}
		}

		if(controller.isGrounded) {
			if(inWater == false) {
				hor = Input.GetAxis("Horizontal");
				ver = Input.GetAxis("Vertical");

                if (onFoot == true) {
					controller.height = 1.5f;
					moveDirection = new Vector3(hor, 0, ver);
				}
				else {
					controller.height = 2.1f;
					moveDirection = new Vector3(0, 0, ver);
				}
				moveDirection = transform.TransformDirection(moveDirection);

				if(onFoot == false) {
					if(ver > 0f) {
						if(speedIncreaser < horseSpeed) {
							speedIncreaser += Time.deltaTime*horseSpeed;
						}

						movingState = MovingSpeedState.OnHorse;
						moveDirection *= speedIncreaser;
					}
					else {
						movingState = MovingSpeedState.Standing;
						speedIncreaser = 0f;
					}
				}
				else {
					if(wepMAnim.dazed == false) {
						if(ver == 0f && hor == 0f) {
							movingState = MovingSpeedState.Standing;
						}
						else if(Input.GetButton("Sprint") && playerVelocity > 0f) {
							if(stats.currentStamina > 0 && ver > 0f) {
								movingState = MovingSpeedState.Running;
								moveDirection *= (baseSpeed + runningSpeed);
								if(ver != 0f || hor != 0f) {
									skillsAttributes.acrobaticsAdvancement += 0.005f * Time.deltaTime;
									stats.lastStaminaDrain = Time.time;

									if(movingState == MovingSpeedState.Running) {
										staminaTimer += Time.deltaTime;
										if(staminaTimer > 0.15f) {
											if(skillsAttributes.acrobatics >= 80) {
												staminaTimer += Time.deltaTime;
												if(staminaTimer > 0.3f) {
													stats.currentStamina--;
													staminaTimer = 0f;
												}
											}
											else if(skillsAttributes.acrobatics < 80) {
												staminaTimer += Time.deltaTime;
												if(staminaTimer > 0.15f) {
													stats.currentStamina--;
													staminaTimer = 0f;
												}
											}
										}
									}
								}
							}
							else {
								movingState = MovingSpeedState.Walking;
								moveDirection *= (baseSpeed + walkingSpeed);
								if(playerVelocity > 0f) {
									skillsAttributes.acrobaticsAdvancement += 0.0025f * Time.deltaTime;
								}
							}
						}
						else if(wepMAnim.blocking == true && playerVelocity > 0f) {
							if(skillsAttributes.block < 80) {
								movingState = MovingSpeedState.Walking;
								moveDirection *= (baseSpeed + slowedSpeed);
							}
							else {
								movingState = MovingSpeedState.Walking;
								moveDirection *= (baseSpeed + walkingSpeed);
							}
						}
						else {
							movingState = MovingSpeedState.Walking;
							moveDirection *= (baseSpeed + walkingSpeed);
							if(playerVelocity > 0f) {
								skillsAttributes.acrobaticsAdvancement += 0.0025f * Time.deltaTime;
							}
						}
					}
					else {
						movingState = MovingSpeedState.Dazed;
						moveDirection *= (baseSpeed + slowedSpeed);
					}
				}

				if(Input.GetButtonDown("Jump")) {
					if(onFoot == false) {
						if(playerVelocity > 0f) {
							moveDirection.y = jumpingSpeed;
						}
					}
					else {
						movingState = MovingSpeedState.Jumping;
						moveDirection.y = jumpingSpeed;
						skillsAttributes.acrobaticsAdvancement += 0.005f;

						if(skillsAttributes.acrobatics < 50) {
							stats.currentStamina -= 6;
							stats.lastStaminaDrain = Time.time;
						}
						else if(skillsAttributes.acrobatics >= 50 && skillsAttributes.acrobatics < 80) {
							stats.currentStamina -= 3;
							stats.lastStaminaDrain = Time.time;
						}
					}
				}
			}
			else {
                hor = Input.GetAxis("Horizontal");
                ver = Input.GetAxis("Vertical");

                moveDirection = new Vector3(hor, 0, ver);
				moveDirection = transform.TransformDirection(moveDirection);

				if(ver == 0 && hor == 0) {
					movingState = MovingSpeedState.Standing;
				}
				else {
					if(Input.GetButton("Sprint")) {
						movingState = MovingSpeedState.Swimming;
						moveDirection *= (baseSpeed + (runningSpeed/swimingFactor));
					}
					else {
						movingState = MovingSpeedState.Swimming;
						moveDirection *= (baseSpeed + (walkingSpeed/swimingFactor));
					}
				}

				if(Input.GetButtonDown("Jump")) {
					movingState = MovingSpeedState.Jumping;
					moveDirection.y = (baseSpeed + (jumpingSpeed/swimingFactor));
					skillsAttributes.acrobaticsAdvancement += 0.005f;
					stats.currentStamina -= 5;
					stats.lastStaminaDrain = Time.time;
				}
			}
		}

		moveDirection.y -= gravity * Time.deltaTime * timeSpeedFactor;
		controller.Move(moveDirection * Time.deltaTime * timeSpeedFactor);
	}

	void CheckForWater() {

		RaycastHit hit;

		Debug.DrawRay(waterCheckPoint.position, -Vector3.up, Color.green, 1f);
		if(Physics.Raycast(waterCheckPoint.position, -Vector3.up, out hit, 1f, waterLayer)) {
			if(hit.transform.tag == "Water") {
				inWater = true;
			}
			else {
				inWater = false;
			}
		}
		else {
			inWater = false;
		}
	}
}
