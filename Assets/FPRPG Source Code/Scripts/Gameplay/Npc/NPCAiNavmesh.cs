using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum NPCMovingState {
	Idle = 0,
	Moving = 1,
}

public enum NPCAttackingState {
	Idle = 0,
	Melee = 1,
	Ranged = 2,
	Magic = 3,
}

public enum NPCAICurrentState {
	Idle = 0,
	Wandering = 1,
	Following = 2,
	Patroling = 3,
	Guarding = 4,
	Resting = 5,
	Fighting = 6,
    Runningaway = 7,
    Sleeping = 8,
    GoingHome = 9,
    LeavingHome = 10,
}

public class NPCAiNavmesh : MonoBehaviour {

    //[HideInInspector]
    public NPCMovingState movingState;
    //[HideInInspector]
    public NPCAttackingState attackingState;
    [HideInInspector]
    public NPCAICurrentState aiState;
    ////////////////////////////////////////
    [Header("AI STATES")]
    [Space(10f)]
    [Header(" - Guarding AI - ")]
    public bool isGuarding = false;
	[Space(5f)]
    [Header(" - Patroling AI - ")]
    public bool isPatroling = false;
	public Transform[] patrolPoints;
	private int destPoint = 0;
	[Space(5f)]
    [Header(" - Wandering AI - ")]
    public bool isWanderer = false;
	public float wanderRadius;
	private float wanderTimer;
	[Space(5f)]
    [Header(" - Courier AI - ")]
    public bool isCourier = false;
	public GameObject itemToDeliver;
	[HideInInspector]
	public bool deliveredPackage = false;
	public bool startsQuest;
	public string questID;
	[Space(5f)]
    [Header(" - Coward AI - ")]
    public bool isCoward = false;
	[Space(5f)]
    //[HideInInspector]
    public Transform target;
    //[HideInInspector]
    public bool targetInSight;
    public Transform player;
    public bool playerInSight;
	////////////////////////////////////////
	[Space(5f)]
	[Tooltip("NPC will approach player and will start conversation if player is inside his vision.")]
	public bool approachPlayer = false;
	[TextArea(4, 10)]
	[Tooltip("In case that shownApproachDialog bool is not checked, aproachDialog will still be used as a startup dialog text.")]
	public string approachDialog;
	[HideInInspector]
	public bool shownApproachDialog;
	[TextArea(4, 10)]
	public List<string> customOverheadDialogs;
	////////////////////////////////////////
	[HideInInspector]
	public bool addedToPlayerOpponents;
	[HideInInspector]
	public bool engagedInCombat;
	////////////////////////////////////////
	private UnityEngine.AI.NavMeshAgent agent;
	private Transform playerTransform;
	private float timer;
    [HideInInspector]
	public Vector3 initialPosition;
	private float distanceToGuardPoint;
	////////////////////////////////////////
	private NPC npc;
	private UIManager uiManager;
	private ShowMessage showMessage;
	private WeightedInventory inventory;
	private GameManager gameManager;
	private NPCOverheadTalker npcOverheadTalker;
	private PlayerStats playerStats;
	private TodWeather todWeather;
    private Interact playerInteract;
    [HideInInspector]
    public DoorManager npcsHome; //Door script assigned here will be npcs home.
    private Vector3 lastPositionBeforeGoingHome;
    public AudioSource npcInteractionAs;
    private bool step = true;
    ////////////////////////////////////////
    [HideInInspector]
	public float attackCooldown;
	[HideInInspector]
	public float attackingTimer = 0f;
	[HideInInspector]
	public bool dazed;
	[HideInInspector]
	public bool castSphere = false;
	[HideInInspector]
	public Vector3 sphereHitPoint;
    [HideInInspector]
    private float distToStartAttacking = 2.25f;

    ////////////////////////////////////////
    private float talkerCooldown;
	private int randomTopicInt;
	////////////////////////////////////////
	private bool iSeePlayer;
    ////////////////////////////////////////

    void Awake() {

        if (isGuarding == false && isPatroling == false && isWanderer == false && isCourier == false && isCoward == false) {
            Debug.LogWarning("NPC " + transform.name + " has no AI set. By default he will be set to isGuarding state.!");
            isGuarding = true;
        }

        npc = GetComponent<NPC>();

        if (GameObject.Find("[Player]")) {
            player = GameObject.Find("[Player]").transform;
            playerTransform = GameObject.Find("[Player]").transform;
            playerStats = GameObject.Find("[Player]").GetComponent<PlayerStats>();
            playerInteract = playerTransform.GetComponent<Interact>();
        }
        if (GameObject.Find("_UICanvasGame")) {
            uiManager = GameObject.Find("_UICanvasGame").GetComponent<UIManager>();
            showMessage = GameObject.Find("_UICanvasGame").GetComponentInChildren<ShowMessage>();
            inventory = GameObject.Find("_UICanvasGame").GetComponentInChildren<WeightedInventory>();
        }
        if (GameObject.Find("[GameManager]")) {
            gameManager = GameObject.Find("[GameManager]").GetComponent<GameManager>();
            todWeather = GameObject.Find("[GameManager]").GetComponent<TodWeather>();
        }
		npcOverheadTalker = gameObject.GetComponentInChildren<NPCOverheadTalker> ();
		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		agent.updateRotation = false;
		agent.autoBraking = true;
		initialPosition = transform.position;

		//PLACE FOR WARNING MESSAGES
		if(isPatroling && approachPlayer) {
			Debug.LogWarning("Npc that has been set to patrol a certain area can not approach to the player.");
		}
		if(isCourier && approachPlayer) {
			Debug.LogWarning("Npc that has been set to bring message to player can not be set to approach the player. Disable approachPlayer for performance.");
		}

		if(isPatroling) {
			agent.stoppingDistance = 0f;
			agent.autoBraking = false;
		}
		if(isCourier) {
			agent.speed = 4.5f;
		}

		attackCooldown = Random.Range(0.1f, 2.25f);

		if (approachDialog != null && approachDialog != "") {
			WelcomingDialogSetup();
		}

        distToStartAttacking = Random.Range(2.15f, 2.5f);
    }

	void Update() {

        CanISeeTarget();

        if (playerInSight == true) {
            if (iSeePlayer == false) {
                gameManager.whoSeePlayer.Add(gameObject);
                iSeePlayer = true;
            }
        }
        else if (playerInSight == false) {
            if (iSeePlayer == true) {
                gameManager.whoSeePlayer.Remove(gameObject);
                iSeePlayer = false;
            }
        }

        if (isCourier == false) {
            if (aiState != NPCAICurrentState.Runningaway) {
                agent.speed = 3.5f;
            }
            else {
                agent.speed = 5.5f;
            }
        }

        if (isCoward == true) {
            //runaway
            float distToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            if (distToPlayer < 20f) {
                target = playerTransform;
            }
            else {
                target = null;
            }
        }

        if (target != null) {
			if(targetInSight == true && engagedInCombat == true && isCoward == false) {
                //if this npc is attacking npc that is attacking player, we want the other npc to attack this npc not player
                if(target.GetComponent<NPCAiNavmesh>() && target.GetComponent<NPCAiNavmesh>().target == player) {
                    target.GetComponent<NPCAiNavmesh>().target = transform;
                }
				NPCMoveToAndFight(target.position);
			}
			else if(isCourier == true && engagedInCombat == false && gameManager.playerState == PlayerState.Exploring && isCoward == false) {
				if(addedToPlayerOpponents == true) {
					gameManager.playerOpponents.Remove(gameObject);
					addedToPlayerOpponents = false;
				}
				if(deliveredPackage == false) {
					NPCMoveToPlayerAndTalk();
				}
				else {
					if(target == playerTransform) {
						target = null;
					}
					else {
						float distToCity = Vector3.Distance(transform.position, target.position);
						if(distToCity <= 10f) {
							if(gameManager.currentOpponentFighting == gameObject) {
								gameManager.currentOpponentFighting = null;
							}
							Destroy(gameObject);
						}
					}
				}
			}
			else if(isPatroling == true && engagedInCombat == false && isCoward == false) {
				if(addedToPlayerOpponents == true) {
					gameManager.playerOpponents.Remove(gameObject);
					addedToPlayerOpponents = false;
				}
				if(target.name.Contains("PatrolPoint")) {
					float distanceToPoint = Vector3.Distance(transform.position, target.position);
					if(distanceToPoint < 0.5f) {
						NPCPatrolGetNextPoint();
					}
				}
				else {
					NPCPatrolGetNextPoint();
				}
				NPCMoveTo(target.position);
			}
			else if(targetInSight == true && approachPlayer == true && shownApproachDialog == false && engagedInCombat == false && isCoward == false) {
				if(addedToPlayerOpponents == true) {
					gameManager.playerOpponents.Remove(gameObject);
					addedToPlayerOpponents = false;
				}
				NPCMoveToPlayerAndTalk();
			}
            else if (isCoward == true) {
                //runaway
                float distToPlayer = Vector3.Distance(transform.position, playerTransform.position);

                if(distToPlayer < 20f) {
                    NPCRunaway();
                }
            }
			else {
				if(targetInSight == true && approachPlayer == true && engagedInCombat == false) {
					if(addedToPlayerOpponents == true) {
						gameManager.playerOpponents.Remove(gameObject);
						addedToPlayerOpponents = false;
					}
					NPCMoveTo(target.position);
				}
			}
		}
		else {

            if(aiState == NPCAICurrentState.Fighting && targetInSight) {
                aiState = NPCAICurrentState.Idle;
                attackingState = NPCAttackingState.Idle;
                targetInSight = false;
            }

			if(npcOverheadTalker != null) {
				NPCTalker ();
			}
			if(isWanderer == true) {
				if(aiState != NPCAICurrentState.Fighting && aiState != NPCAICurrentState.Runningaway) {
                    if (npcsHome != null) {
                        if (todWeather.currHour >= 19 || todWeather.currHour < 7) {
                            NPCGoHome();
                           // Debug.Log("We are going home.");
                        }
                        else if(todWeather.currHour >= 7 && lastPositionBeforeGoingHome != Vector3.zero) { //if 
                            NPCLeaveHome(lastPositionBeforeGoingHome);
                           // Debug.Log("We are leaving home.");
                        }
                        else if (lastPositionBeforeGoingHome == Vector3.zero) {
                            NPCRandomWander();
                            //Debug.Log("We are wandering.");
                        }
                    }
                    else {
                        NPCRandomWander();
                    }
                }
			}
			else if(isGuarding == true) {
				if(aiState != NPCAICurrentState.Fighting) {
                    if (npcsHome != null) {
                        if (todWeather.currHour >= 19) {
                            NPCGoHome();
                            // Debug.Log("We are going home.");
                        }
                        else if (todWeather.currHour >= 7 && lastPositionBeforeGoingHome != Vector3.zero) { //if 
                            NPCLeaveHome(lastPositionBeforeGoingHome);
                            // Debug.Log("We are leaving home.");
                        }
                        else if (lastPositionBeforeGoingHome == Vector3.zero) {
                            distanceToGuardPoint = Vector3.Distance(transform.position, initialPosition);

                            if (distanceToGuardPoint <= 2.1f) {
                                agent.Stop();
                                agent.ResetPath();
                                movingState = NPCMovingState.Idle;
                                aiState = NPCAICurrentState.Guarding;
                            }
                            else {
                                NPCMoveTo(initialPosition);
                            }
                        }
                    }
                    else {
                        distanceToGuardPoint = Vector3.Distance(transform.position, initialPosition);

                        if (distanceToGuardPoint <= 2) {
                            agent.Stop();
                            agent.ResetPath();
                            aiState = NPCAICurrentState.Guarding;
                            movingState = NPCMovingState.Idle;
                        }
                        else {
                            NPCMoveTo(initialPosition);
                            aiState = NPCAICurrentState.Patroling;
                        }
                    }
				}
			}
			else if(isPatroling == true) {
				if(aiState != NPCAICurrentState.Fighting) {
					NPCPatrolGetNextPoint();
				}
			}
			else if(isCourier == true && approachPlayer == false) {
				if(deliveredPackage == false) {
					if(target != playerTransform) {
						target = playerTransform;
					}
				}
				else {
					GetNearestCity();
				}
			}
			else {
				if(approachPlayer == false && shownApproachDialog == true) {
					isGuarding = true;
				}
			}

			if(addedToPlayerOpponents == true) {
				gameManager.playerOpponents.Remove(gameObject);
				addedToPlayerOpponents = false;
			}
		}
	}

	void NPCMoveTo(Vector3 destination) {
		agent.SetDestination(destination);

		if(agent.remainingDistance <= agent.stoppingDistance && agent.velocity.sqrMagnitude == 0f) {
			movingState = NPCMovingState.Idle;
		}
		else {
			movingState = NPCMovingState.Moving;
		}
		aiState = NPCAICurrentState.Following;
	}

    void NPCRunaway() {

        Vector3 newPos = RandomNavSphere(transform.position, 50f, -1);

        if (agent.remainingDistance <= agent.stoppingDistance && agent.velocity.sqrMagnitude == 0f) {
            movingState = NPCMovingState.Idle;
            agent.SetDestination(newPos);
        }
        else {
            movingState = NPCMovingState.Moving;
        }
        aiState = NPCAICurrentState.Runningaway;
    }

    void NPCMoveToPlayerAndTalk() {
		if(target != playerTransform) {
			target = playerTransform;
		}

		agent.SetDestination(target.position);

		float distanceToPlayer = Vector3.Distance(transform.position, target.position);

		if(distanceToPlayer <= 2.25f) {
			movingState = NPCMovingState.Idle;

            if (uiManager != null) {
                uiManager.dialogUI.SetActive(true);
                uiManager.dialogUI.GetComponentInChildren<ConversationBase>().StartTheConversation(gameObject, approachDialog);
                Transform[] t = uiManager.dialogUI.transform.Find("Topics/TopicsHolder").GetComponentsInChildren<Transform>();
                uiManager.SetActiveUIElement(t[1].gameObject);
            }
            else {
                Debug.LogError("NPC: " + transform.name + " is trying to start conversation with Player but can not find main game UI containing ConversationBase.cs Component.");
            }
			if(npcOverheadTalker != null) {
				npcOverheadTalker.NpcTalk("");
			}

			WelcomingDialogSetup();
			approachPlayer = false;
			shownApproachDialog = true;
			target = null;
			//targetInSight = false;

			if(isCourier == true) {
				if(itemToDeliver != null) {
					GameObject itemTmp = Instantiate(itemToDeliver, Vector3.zero, Quaternion.identity) as GameObject;
					itemTmp.GetComponent<Rigidbody>().useGravity = false;
					inventory.AddItemToInventory(itemTmp);
					showMessage.SendTheMessage("Courier hands you the " + itemToDeliver.GetComponent<Item>().itemName + ".");
				}
				if(startsQuest == true) {
					GameObject.Find("[Quests]").transform.Find(questID).GetComponent<QuestBase>().questPhase = QuestPhase.Phase1;
				}
				deliveredPackage = true;
			}
		}
		else {
			movingState = NPCMovingState.Moving;
		}
		aiState = NPCAICurrentState.Following;
	}

	void NPCMoveToAndFight(Vector3 destination) {
		if(addedToPlayerOpponents == false && target == player) {
			gameManager.playerOpponents.Add(gameObject);
			addedToPlayerOpponents = true;
		}

		if(dazed == true) {
			agent.ResetPath();
			movingState = NPCMovingState.Idle;
			attackingState = NPCAttackingState.Idle;
		}
		else {
			if(attackingTimer > 0f) {
				if(castSphere == false) {
					sphereHitPoint = RandomNavSphere(transform.position, 6f, -1);
					castSphere = true;
				}
				agent.SetDestination(sphereHitPoint);
				attackingTimer -= Time.deltaTime;

				float distanceToPoint = Vector3.Distance(transform.position, sphereHitPoint);
				if(distanceToPoint > 0.05f) {
					movingState = NPCMovingState.Moving;
				}
				attackingState = NPCAttackingState.Idle;
			}
			else {

				agent.SetDestination(destination);

				float distanceToTarget = Vector3.Distance(transform.position, target.position);

				if(distanceToTarget <= distToStartAttacking) {
					movingState = NPCMovingState.Idle;
					if(npc.weapon.ToString().Contains("Sword") || npc.weapon.ToString().Contains("Axe") || npc.weapon.ToString().Contains("Mace")) {
						attackingState = NPCAttackingState.Melee;
					}
					else if(npc.weapon.ToString().Contains("Bow")) {
						attackingState = NPCAttackingState.Ranged;
					}
					else {
						attackingState = NPCAttackingState.Melee;
					}
				}
				else {
					movingState = NPCMovingState.Moving;
					attackingState = NPCAttackingState.Idle;
				}
			}
		}
		aiState = NPCAICurrentState.Fighting;
	}

	void NPCPatrolGetNextPoint() {
		if (patrolPoints.Length == 0)
			return;

		destPoint = (destPoint + 1) % patrolPoints.Length;

		target = patrolPoints[destPoint];
	}

	void NPCRandomWander() {
		timer += Time.deltaTime;
		
		if (timer >= wanderTimer) {
			Vector3 newPos = RandomNavSphere(initialPosition, wanderRadius, -1);
			agent.SetDestination(newPos);
			timer = 0;
			wanderTimer = Random.Range(6f, 20f);
		}

		if(agent.remainingDistance <= agent.stoppingDistance) {
			movingState = NPCMovingState.Idle;
		}
		else {
			movingState = NPCMovingState.Moving;
		}
		aiState = NPCAICurrentState.Wandering;
	}

    void NPCGoHome() {

        if (lastPositionBeforeGoingHome == Vector3.zero) {
            lastPositionBeforeGoingHome = transform.position;
        }

        Vector3 homePosDoorOutside = npcsHome.doorOutside.position;

        float distToHPDO = Vector3.Distance(transform.position, homePosDoorOutside);

        if (distToHPDO < 2.1f) { //If he is near outside door
            if (step) {
                StartCoroutine(PlaySound(playerInteract.doorSFX)); //play open door sfx
            }

            agent.enabled = false; //disable navmesh agent

            agent.transform.position = npcsHome.doorInside.position; //tranport npc model inside interior

            npcsHome.locked = true; //lock the house so pc cant disturb npcs
            if (step) {
                StartCoroutine(PlaySound(playerInteract.doorLockedSFX)); //play open door sfx
            }
            aiState = NPCAICurrentState.Sleeping;//sets npcs state to sleeping
        }
        else {
            if (agent.enabled == true) {
                agent.SetDestination(homePosDoorOutside); //go home

                if (agent.remainingDistance <= agent.stoppingDistance && agent.velocity.sqrMagnitude == 0f) {
                    movingState = NPCMovingState.Idle;
                }
                else {
                    movingState = NPCMovingState.Moving;
                }
                aiState = NPCAICurrentState.GoingHome;
            }
        }
    }

    void NPCLeaveHome(Vector3 destination) {

        float dist = Vector3.Distance(transform.position, destination);

        if(npcsHome.locked == true) {
            if (step) {
                StartCoroutine(PlaySound(playerInteract.doorSFX)); //play open door sfx
            }
            if (step) {
                StartCoroutine(PlaySound(playerInteract.doorLockedSFX)); //play open door sfx
            }
            agent.transform.position = npcsHome.doorOutside.position; //tranport npc model outside the house
            agent.enabled = true; //enable navmesh agent

            npcsHome.locked = false; //unlock the house

        }

        if(dist > 3f) {
            if (agent.destination != destination) {
                agent.SetDestination(destination);
            }

            if (agent.remainingDistance <= agent.stoppingDistance && agent.velocity.sqrMagnitude == 0f) {
                movingState = NPCMovingState.Idle;
            }
            else {
                movingState = NPCMovingState.Moving;
            }
            aiState = NPCAICurrentState.LeavingHome;
        }
        else {
            agent.ResetPath();

            if (lastPositionBeforeGoingHome != Vector3.zero) {
                lastPositionBeforeGoingHome = Vector3.zero;
            }
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask) {
		Vector3 randDirection = Random.insideUnitSphere * dist;
		
		randDirection += origin;
		
		UnityEngine.AI.NavMeshHit navHit;
		
		UnityEngine.AI.NavMesh.SamplePosition (randDirection, out navHit, dist, layermask);
		
		return navHit.position;
	}

	void GetNearestCity () {
		GameObject[] cities = GameObject.FindGameObjectsWithTag("LocationZone");
		List<GameObject> citiesList = cities.ToList();
		
		Transform nearestCity = null;
		float closestDistanceSqr = Mathf.Infinity;
		Vector3 currentPosition = transform.position;
		
		foreach(GameObject potentialTarget in citiesList.ToArray()) {
			Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
			float dSqrToTarget = directionToTarget.sqrMagnitude;
			if(dSqrToTarget < closestDistanceSqr) {
				closestDistanceSqr = dSqrToTarget;
				nearestCity = potentialTarget.transform;
				
				float distToNearestCity = Vector3.Distance(transform.position, nearestCity.position);
				if(distToNearestCity < 50f) {
					citiesList.Remove(nearestCity.gameObject);
				}
				else {
					agent.speed = 5.5f;
					movingState = NPCMovingState.Moving;
					target = nearestCity;
					agent.SetDestination(nearestCity.position);
				}
			}
		}
	}

	void OnTriggerEnter(Collider obj) {
		if(targetInSight == false) {
			if(obj.transform.CompareTag("Npc")) {
				NPCFaction otherNpcFaction = obj.transform.GetComponent<NPC>().npcFaction;
				if(npc.npcFaction == NPCFaction.Bandits && otherNpcFaction != NPCFaction.Bandits) {
					target = obj.transform;
					engagedInCombat = true;
				}
				else if(npc.npcFaction == NPCFaction.Guards && (otherNpcFaction == NPCFaction.Bandits || otherNpcFaction == NPCFaction.Monster)) {
					target = obj.transform;
					engagedInCombat = true;
				}
				else if(npc.npcFaction == NPCFaction.Monster && otherNpcFaction != NPCFaction.Monster) {
					target = obj.transform;
					engagedInCombat = true;
				}
			}
			else if(obj.transform.CompareTag("Player")) {
				if(npc.npcDisposition == NPCDisposition.Hostile && obj.transform.CompareTag("Player")) {
					target = obj.transform;
					engagedInCombat = true;
				}
				else if(approachPlayer == true && approachDialog != "" && shownApproachDialog == false) {
					target = obj.transform;
				}
			}
		}
	}

	void CanISeeTarget() {

        if (target != null) {
            RaycastHit hit;
            Vector3 rayDirection;

            rayDirection = target.position - transform.position;
            //rayDirection.y = 0f;

            LayerMask layerMask = ~(1 << 10);
            Vector3 rayPos = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);

            if (Physics.Raycast(rayPos, rayDirection, out hit, 30f, layerMask)) {
                Debug.DrawLine(rayPos, hit.point, Color.cyan);

                if (hit.transform == target) {
                    float distanceToTarget = Vector3.Distance(transform.position, target.position);
                    if (distanceToTarget > 20f) {
                        targetInSight = false;

                    }
                    else {
                        targetInSight = true;

                    }
                }
            }
        }

        RaycastHit hit1;
        Vector3 rayDirection1;

        rayDirection1 = player.position - transform.position;
        //rayDirection.y = 0f;

        LayerMask layerMask1 = ~(1 << 10);
        Vector3 rayPos1 = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);

        if (Physics.Raycast(rayPos1, rayDirection1, out hit1, 30f, layerMask1)) {
            Debug.DrawLine(rayPos1, hit1.point, Color.cyan);

            if (hit1.transform == player) {
                float distanceToTarget = Vector3.Distance(transform.position, player.position);
                if (distanceToTarget > 20f) {
                    playerInSight = false;

                }
                else {
                    playerInSight = true;

                }
            }
            else {
                playerInSight = false;
            }
        }
	}

	public void WelcomingDialogSetup() {
		if(approachDialog != "") {
			if(shownApproachDialog == false) {
				if(approachDialog.Contains("BRO/SIS")) {
					if(playerTransform.GetComponent<PlayerStats>().playerGender == CharacterGender.Male) {
						string welDial = approachDialog.Replace("BRO/SIS", "brother");
						approachDialog = welDial;
					}
					else {
						string welDial = approachDialog.Replace("BRO/SIS", "sister");
						approachDialog = welDial;
					}
				}
				else if(approachDialog.Contains("PC_NAME")) {
					if(playerTransform.GetComponent<PlayerStats>().playerName != "" || playerTransform.GetComponent<PlayerStats>().playerName != " ") {
						string welDial = approachDialog.Replace("PC_NAME", playerTransform.GetComponent<PlayerStats>().playerName);
						approachDialog = welDial;
					}
					else {
						string welDial = approachDialog.Replace(" PC_NAME", "");
						approachDialog = welDial;
					}
				}
			}
		}
		else {
			int rand = Random.Range(1, 60);

            if (playerTransform.GetComponent<PlayerStats>().playerRace == CharacterRace.Elinian) {
				if(gameObject.GetComponent<NPC>().race == CharacterRace.Elevirian) {
					if(rand >= 1 && rand <= 10) {
						approachDialog = "Speak quickly, elinachir.";
					}
					else if(rand >= 11 && rand <= 20) {
						approachDialog = "I have no time for this.";
					}
					else if(rand >= 21 && rand <= 30) {
						approachDialog = "Do not distrub me with this nonsense.";
					}
					else if(rand >= 31 && rand <= 40) {
						approachDialog = "If you have something to say, say it quickly.";
					}
					else if(rand >= 41 && rand <= 50) {
						approachDialog = "My time is valuable to spend it on blabering.";
					}
					else if(rand >= 51 && rand <= 60) {
						approachDialog = "Say what you want or we are done here elinachir.";
					}
				}
				else if(gameObject.GetComponent<NPC>().race == CharacterRace.Ariyan) {
					if(rand >= 1 && rand <= 10) {
						approachDialog = "Yes, child of the Empire?";
					}
					else if(rand >= 11 && rand <= 20) {
						approachDialog = "May I be of service, child of the Sun?";
					}
					else if(rand >= 21 && rand <= 30) {
						approachDialog = "How may I help you?";
					}
					else if(rand >= 31 && rand <= 40) {
						approachDialog = "Hmm?.";
					}
					else if(rand >= 41 && rand <= 50) {
						approachDialog = "May I be of assistance, Sun lover?";
					}
					else if(rand >= 51 && rand <= 60) {
						approachDialog = "Yes?";
					}
				}
				else {
					if(rand >= 1 && rand <= 10) {
						approachDialog = "How may I help you?";
					}
					else if(rand >= 11 && rand <= 20) {
						approachDialog = "Yes?";
					}
					else if(rand >= 21 && rand <= 30) {
						approachDialog = "May I be of assistance?";
					}
					else if(rand >= 31 && rand <= 40) {
						approachDialog = "May I be of service?";
					}
					else if(rand >= 41 && rand <= 50) {
						approachDialog = "Do you have something in mind?";
					}
					else if(rand >= 51 && rand <= 60) {
						approachDialog = "Do I know you?";
					}
				}
			}
			else if(playerTransform.GetComponent<PlayerStats>().playerRace == CharacterRace.Elevirian) {
				if(gameObject.GetComponent<NPC>().race == CharacterRace.Elinian) {
					if(rand >= 1 && rand <= 10) {
						approachDialog = "Speak.";
					}
					else if(rand >= 11 && rand <= 20) {
						approachDialog = "Your kind always have something stupid to say.";
					}
					else if(rand >= 21 && rand <= 30) {
						approachDialog = "Do not distrub me with this nonsense.";
					}
					else if(rand >= 31 && rand <= 40) {
						approachDialog = "If you have something to say, say it quickly.";
					}
					else if(rand >= 41 && rand <= 50) {
						approachDialog = "My time is valuable to spend it on blabering.";
					}
					else if(rand >= 51 && rand <= 60) {
						approachDialog = "Say what you want or we are done here.";
					}
				}
				else if(gameObject.GetComponent<NPC>().race == CharacterRace.Elevirian) {
					if(rand >= 1 && rand <= 10) {
						approachDialog = "Yes, son of the forest.";
					}
					else if(rand >= 11 && rand <= 20) {
						approachDialog = "You may speak with me, friend.";
					}
					else if(rand >= 21 && rand <= 30) {
						approachDialog = "Did you had something to say, friend?";
					}
					else if(rand >= 31 && rand <= 40) {
						approachDialog = "If you have something to say, take your time.";
					}
					else if(rand >= 41 && rand <= 50) {
						approachDialog = "My time is valuable, but I will make a moment or two for you friend.";
					}
					else if(rand >= 51 && rand <= 60) {
						approachDialog = "Did you wanted someting, my friend?";
					}
				}
				else {
					if(rand >= 1 && rand <= 10) {
						approachDialog = "How may I help you?";
					}
					else if(rand >= 11 && rand <= 20) {
						approachDialog = "Yes?";
					}
					else if(rand >= 21 && rand <= 30) {
						approachDialog = "May I be of assistance?";
					}
					else if(rand >= 31 && rand <= 40) {
						approachDialog = "May I be of service?";
					}
					else if(rand >= 41 && rand <= 50) {
						approachDialog = "Do you have something in mind?";
					}
					else if(rand >= 51 && rand <= 60) {
						approachDialog = "Do I know you?";
					}
				}
			}
			else if(playerTransform.GetComponent<PlayerStats>().playerRace == CharacterRace.Ariyan) {
				if(gameObject.GetComponent<NPC>().race == CharacterRace.Elevirian) {
					if(rand >= 1 && rand <= 10) {
						approachDialog = "How may I help you, arinachir?";
					}
					else if(rand >= 11 && rand <= 20) {
						approachDialog = "Yes, pale one?";
					}
					else if(rand >= 21 && rand <= 30) {
						approachDialog = "May I be of assistance, northerner?";
					}
					else if(rand >= 31 && rand <= 40) {
						approachDialog = "May I be of service, arinachir?";
					}
					else if(rand >= 41 && rand <= 50) {
						approachDialog = "Do you have something in mind, arinachir?";
					}
					else if(rand >= 51 && rand <= 60) {
						approachDialog = "Do I know you arinachir?";
					}
				}
				else {
					if(rand >= 1 && rand <= 10) {
						approachDialog = "How may I help you, child of the snow?";
					}
					else if(rand >= 11 && rand <= 20) {
						approachDialog = "Yes, pale one?";
					}
					else if(rand >= 21 && rand <= 30) {
						approachDialog = "May I be of assistance, northerner?";
					}
					else if(rand >= 31 && rand <= 40) {
						approachDialog = "May I be of service, Arian?";
					}
					else if(rand >= 41 && rand <= 50) {
						approachDialog = "Do you have something in mind, Arian?";
					}
					else if(rand >= 51 && rand <= 60) {
						approachDialog = "Do I know you outlander?";
					}
				}
			}
			else {
                print("S.");
				if(rand >= 1 && rand <= 10) {
					approachDialog = "How may I help you?";
				}
				else if(rand >= 11 && rand <= 20) {
					approachDialog = "Yes?";
				}
				else if(rand >= 21 && rand <= 30) {
					approachDialog = "May I be of assistance?";
				}
				else if(rand >= 31 && rand <= 40) {
					approachDialog = "May I be of service?";
				}
				else if(rand >= 41 && rand <= 50) {
					approachDialog = "Do you have something in mind?";
				}
				else if(rand >= 51 && rand <= 60) {
					approachDialog = "Do I know you?";
				}
			}
		}
	}

	void NPCTalker() {
		if(talkerCooldown <= 0f) {
			if(customOverheadDialogs.Count <= 0) {

				randomTopicInt = Random.Range(0, 20);
				/// 1 - 13 everything
				/// 13 - 15 weather
				/// 15 - 20 valor
				/// 20 - 30 quests

				if(npc.npcFaction == NPCFaction.Guards) {
					if(randomTopicInt == 2) {
						npcOverheadTalker.NpcTalk("Move along,\n citizen.");
					}
					else if(randomTopicInt == 3) {
						npcOverheadTalker.NpcTalk("Stay safe, \n traveler.");
					}
					else if(randomTopicInt == 4) {
						npcOverheadTalker.NpcTalk("You mess with law, \n you mess with me.");
					}
					else if(randomTopicInt == 5) {
						npcOverheadTalker.NpcTalk("May I be of \n assistance?");
					}
					else if(randomTopicInt == 6) {
						npcOverheadTalker.NpcTalk("How may I help \n you, citizen?");
					}
					else if(randomTopicInt == 7) {
						npcOverheadTalker.NpcTalk("Watch the roads, \n traveler.");
					}
					else if(randomTopicInt == 8) {
						npcOverheadTalker.NpcTalk("Everything's in order.");
					}
					else if(randomTopicInt == 9) {
						npcOverheadTalker.NpcTalk("What is it?");
					}
					else if(randomTopicInt == 10) {
						npcOverheadTalker.NpcTalk("*sniff*");
					}
					else if(randomTopicInt == 11) {
						npcOverheadTalker.NpcTalk("*snort*");
					}
					else if(randomTopicInt == 12) {
						npcOverheadTalker.NpcTalk("*cough*.");
					}
				}
				else if(npc.npcFaction == NPCFaction.Bandits && playerStats.crimeScore >= 20) {
					if(randomTopicInt == 2) {
						npcOverheadTalker.NpcTalk("What did you do, \n filth?");
					}
					else if(randomTopicInt == 3) {
						npcOverheadTalker.NpcTalk("Yes, brother in crime?");
					}
					else if(randomTopicInt == 4) {
						npcOverheadTalker.NpcTalk("We will show \n the world what \n crime is all about!");
					}
					else if(randomTopicInt == 5) {
						npcOverheadTalker.NpcTalk("Should we pick up \n some traveling merchants?");
					}
					else if(randomTopicInt == 6) {
						npcOverheadTalker.NpcTalk("I heard there are \n coins in town.");
					}
					else if(randomTopicInt == 7) {
						npcOverheadTalker.NpcTalk("*sniff*");
					}
					else if(randomTopicInt == 8) {
						npcOverheadTalker.NpcTalk("*snort*");
					}
					else if(randomTopicInt == 9) {
						npcOverheadTalker.NpcTalk("*cough*.");
					}
				}
				else if(npc.npcFaction == NPCFaction.Commoners) {
					if(randomTopicInt == 2) {
						npcOverheadTalker.NpcTalk("I heard there are some \n dangers lurking up north.");
					}
					else if(randomTopicInt == 3) {
						npcOverheadTalker.NpcTalk("The Imperial College \n declared the consensus with \n the Highschool of Korona.");
					}
					else if(randomTopicInt == 4) {
						npcOverheadTalker.NpcTalk("Hello there!");
					}
					else if(randomTopicInt == 5) {
						npcOverheadTalker.NpcTalk("Good to see you!");
					}
					else if(randomTopicInt == 6) {
						npcOverheadTalker.NpcTalk("How do you do?");
					}
					else if(randomTopicInt == 7) {
						npcOverheadTalker.NpcTalk("Heard of any news \n from the other regions?");
					}
					else if(randomTopicInt == 8) {
						npcOverheadTalker.NpcTalk("Greetings!");
					}
					else if(randomTopicInt == 9) {
						npcOverheadTalker.NpcTalk("I heard there were some \n bauks around here. \n Awful creatures!");
					}
					else if(randomTopicInt == 10) {
						npcOverheadTalker.NpcTalk("*sniff*");
					}
					else if(randomTopicInt == 11) {
						npcOverheadTalker.NpcTalk("*snort*");
					}
					else if(randomTopicInt == 12) {
						npcOverheadTalker.NpcTalk("*cough*.");
					}
				}
				////////
				if(npc.npcFaction != NPCFaction.Monster) {
					if(todWeather.weatherIs == WeatherIs.Clear) {
						if(randomTopicInt == 13) {
							npcOverheadTalker.NpcTalk("Sometimes I feel \n like sunlight is \n burning my skin.");
						}
						else if(randomTopicInt == 14) {
							npcOverheadTalker.NpcTalk("Ah, a nice \n sunny day!");
						}
						else if(randomTopicInt == 15) {
							npcOverheadTalker.NpcTalk("Sun in the back \n and wind in the hair!");
						}
					}
					else if(todWeather.weatherIs == WeatherIs.LightRain) {
						if(randomTopicInt == 13) {
							npcOverheadTalker.NpcTalk("Rain. I hate rain.");
						}
						else if(randomTopicInt == 14) {
							npcOverheadTalker.NpcTalk("It seems like \n it will rain in the next \n few days.");
						}
						else if(randomTopicInt == 15) {
							npcOverheadTalker.NpcTalk("Those dark clouds \n brings rain.");
						}
					}
					else if(todWeather.weatherIs == WeatherIs.Storm) {
						if(randomTopicInt == 13) {
							npcOverheadTalker.NpcTalk("Thunder. I hear thunders \n in the distance.");
						}
						else if(randomTopicInt == 14) {
							npcOverheadTalker.NpcTalk("I hate those \n glowing lighting!");
						}
						else if(randomTopicInt == 15) {
							npcOverheadTalker.NpcTalk("The night shall be cold \n because of the storm.");
						}
					}

					if(playerStats.valorScore >= 100 && playerStats.valorScore < 200) {
						if(randomTopicInt == 16) {
							npcOverheadTalker.NpcTalk("Have we met before?");
						}
						else if(randomTopicInt == 17) {
							npcOverheadTalker.NpcTalk("Hmm, I have a feeling \n I know you from somewhere.");
						}
						else if(randomTopicInt == 18) {
							npcOverheadTalker.NpcTalk("Are you the one... \n No, probably not.");
						}
						else if(randomTopicInt == 19) {
							npcOverheadTalker.NpcTalk("You must hear this a lot, \n but I have a feeling I know you.");
						}
						else if(randomTopicInt == 20) {
							npcOverheadTalker.NpcTalk("Are you the one I heard \n stories about last night?");
						}
					}
					else if(playerStats.valorScore >= 200) {
						if(randomTopicInt == 16) {
							npcOverheadTalker.NpcTalk("Hey, hero of the Empire!");
						}
						else if(randomTopicInt == 17) {
							npcOverheadTalker.NpcTalk("That is the face \n needs to see!");
						}
						else if(randomTopicInt == 18) {
							npcOverheadTalker.NpcTalk("Greeting, hero!");
						}
						else if(randomTopicInt == 19) {
							npcOverheadTalker.NpcTalk("Your deeds are sung in \n every tavern across the realm.");
						}
						else if(randomTopicInt == 20) {
							npcOverheadTalker.NpcTalk("Salutations, hero!");
						}
					}
				}
			}
			else {
				randomTopicInt = Random.Range(0, customOverheadDialogs.Count);

				if(randomTopicInt <= customOverheadDialogs.Count) {
					npcOverheadTalker.NpcTalk(customOverheadDialogs[randomTopicInt]);
				}
			}

			talkerCooldown = Random.Range(30f, 70f);
		}
		else {
			talkerCooldown -= Time.deltaTime;
		}
	}

    public IEnumerator PlaySound(AudioClip[] name) {
        if (npcInteractionAs != null) {
            step = false;
            npcInteractionAs.clip = name[Random.Range(0, name.Length)];
            npcInteractionAs.Play();
            yield return new WaitForSeconds(0.25f);
            step = true;
        }
    }
}
