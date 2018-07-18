#pragma warning disable 618 

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Interact : MonoBehaviour {

	public LayerMask layersToCastOn;

	public float interactDistance = 1.5f;
	public GameObject objectPointedAt;

	public AudioClip[] fireplaceSFX;
	public AudioClip[] alchemySFX;
	public AudioClip[] flowerPickSFX;
	public AudioClip[] doorSFX;
	public AudioClip[] doorLockedSFX;
	public AudioClip[] itemSFX;
	public AudioClip[] coinsSFX;
	public AudioClip[] mountingSFX;
	public AudioClip[] bookSfx;
	public AudioClip[] containerSFX;
    public AudioClip[] sleepingSFX;

	public AudioSource interactionAS;

	public Camera camMain;

	private bool step = true;

	private PlayerStats playerStats;
	public HorseRiding horseRiding;
	private UIManager uiManager;
	private UISkillsAndAttributes uiSA;
	private ConversationBase conversationBase;
	private ShowMessage showMessage;
	private UICrosshairChanger crosshairChanger;
	private GameManager gameManager;
	private TodClock todClock;
	//private SceneFadeInOut fader;

	private Text interactText;

	void Start() {

		playerStats = GetComponent<PlayerStats>();
		if (GameObject.Find ("_UICanvasGame") != null) {
			uiManager = GameObject.Find ("_UICanvasGame").GetComponent<UIManager> ();
			uiSA = uiManager.uISkillsAndAttributes;
			conversationBase = GameObject.Find("_UICanvasGame").GetComponent<UIManager>().dialogUI.GetComponent<ConversationBase>();
			showMessage = GameObject.Find("_UICanvasGame").GetComponentInChildren<ShowMessage>();
			crosshairChanger = GameObject.Find("_UICanvasGame").GetComponent<UIManager>().crosshairUI.GetComponent<UICrosshairChanger>();
			interactText = GameObject.Find("_UICanvasGame").transform.Find("InteractName").GetComponent<Text>();
			//fader = GameObject.Find("_UICanvasGame").GetComponentInChildren<SceneFadeInOut>();
		}
		gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
		todClock = gameManager.gameObject.GetComponent<TodClock> ();
		

		if (interactText != null) {
			interactText.text = "";
		}
	}

	void Update() {

        Vector3 cam = new Vector3(camMain.transform.position.x, camMain.transform.position.y, camMain.transform.position.z);
		Transform camTrans = camMain.transform;
		Ray ray = new Ray(cam, camTrans.forward);

		Debug.DrawRay(cam, camTrans.forward, Color.red);

		RaycastHit hit;
		if(Physics.Raycast (ray, out hit, interactDistance, layersToCastOn)) {
			if(hit.transform.tag == "Npc") {
				if(hit.transform.GetComponent<NPC>()) {
					if (interactText != null) {
						interactText.text = hit.transform.GetComponent<NPC> ().npcName;		
					}
					if (hit.transform.GetComponent<NPC> ().npcDisposition != NPCDisposition.Hostile) {
						objectPointedAt = hit.transform.gameObject;
						if (crosshairChanger != null) {
							crosshairChanger.crosshairState = CrosshairState.Talk;
							crosshairChanger.commitCrime = false;
                            if (uiManager.tutorialUI.GetComponentInChildren<TutorialUI>()) {
                                uiManager.tutorialUI.GetComponentInChildren<TutorialUI>().ShowInteractTutorial();
                            }
						}

						if(playerStats.crimeScore <= 100 && hit.transform.GetComponent<NPC>().npcFaction != NPCFaction.Bandits &&
                            hit.transform.GetComponent<NPC>().npcFaction != NPCFaction.Monster) {
							if (uiManager != null && uiManager.enabled == true) {
								if (uiManager.dialogUI.activeSelf == false) {
									if (Input.GetButtonDown("Use")) {
										if (uiManager.inventoryUI.activeSelf == false && uiManager.waiterUI.activeSelf == false) {
                                            uiManager.dialogUI.SetActive(true);
                                            hit.transform.GetComponent<NPCAiNavmesh>().WelcomingDialogSetup();
                                            conversationBase.StartTheConversation(hit.transform.gameObject, hit.transform.GetComponent<NPCAiNavmesh>().approachDialog);
                                            Transform[] t = uiManager.dialogUI.transform.Find ("Topics/TopicsHolder").GetComponentsInChildren<Transform> ();		
											uiManager.SetActiveUIElement (t [1].gameObject);
											hit.transform.GetComponent<NPCAiNavmesh> ().approachDialog = "";
											//hit.transform.GetComponent<NPCAiNavmesh> ().shownApproachDialog = true;
										}
									}
								}
							}
						}
						else {
                            if (hit.transform.GetComponentInChildren<NPCOverheadTalker>()) {
                                hit.transform.GetComponentInChildren<NPCOverheadTalker>().NpcTalk("I have nothing to say to you, criminal scum!");
                            }
						}
					}
				}
			}
			else if(hit.transform.tag == "Door") {
				if (interactText != null) {
                    if (hit.transform.GetComponentInParent<DoorManager>().locked == true) {
                        if (hit.transform.GetComponentInParent<DoorManager>().doorName.Contains("Rent")) {
                            if (hit.transform.name.Contains("Inside")) {
                                interactText.text = hit.transform.GetComponentInParent<DoorManager>().doorName;
                            }
                            else {
                                interactText.text = hit.transform.GetComponentInParent<DoorManager>().doorName + "\n (Locked)";
                            }
                        }
                        else {
                            interactText.text = hit.transform.GetComponentInParent<DoorManager>().doorName + "\n (Locked)";
                        }
                    }
                    else {
                        interactText.text = hit.transform.GetComponentInParent<DoorManager>().doorName;
                    }
				}
				objectPointedAt = hit.transform.gameObject;
				if (crosshairChanger != null) {
					crosshairChanger.crosshairState = CrosshairState.Open;
				}

				if(!hit.transform.GetComponentInParent<DoorManager>().owner.Count.Equals(0) && 
                   !hit.transform.GetComponentInParent<DoorManager>().doorName.Contains("Player")) { // Ako nije moje
					if (crosshairChanger != null) {
						crosshairChanger.commitCrime = true;
                        if (uiManager.tutorialUI.GetComponentInChildren<TutorialUI>()) {
                            uiManager.tutorialUI.GetComponentInChildren<TutorialUI>().ShowCrimeTutorial();
                        }
                    }
				}
				else {
					if (crosshairChanger != null) {
						crosshairChanger.commitCrime = false;
                        if (uiManager.tutorialUI.GetComponentInChildren<TutorialUI>()) {
                            uiManager.tutorialUI.GetComponentInChildren<TutorialUI>().ShowInteractTutorial();
                        }
                    }
				}

				if(Input.GetButtonDown("Use")) {
					if(step) {
                        if (hit.transform.GetComponentInParent<DoorManager>().locked == true) {
                            if (hit.transform.GetComponentInParent<DoorManager>().doorName.Contains("Rent")) {
                                if (hit.transform.name.Contains("Inside")) {
                                    StartCoroutine(PlaySound(doorSFX));
                                    uiManager.blackscreenUI.GetComponent<BlackscreenAnimator>().BSFadeInOut();
                                    hit.transform.GetComponentInParent<DoorManager>().UseTheDoor(hit.transform.gameObject);
                                }
                                else {
                                    StartCoroutine(PlaySound(doorLockedSFX));
                                }
                            }
                            else {
                                StartCoroutine(PlaySound(doorLockedSFX));
                            }
                        }
                        else {
                            StartCoroutine(PlaySound(doorSFX));
                            uiManager.blackscreenUI.GetComponent<BlackscreenAnimator>().BSFadeInOut();
                            hit.transform.GetComponentInParent<DoorManager>().UseTheDoor(hit.transform.gameObject);
                            Debug.Log(hit.transform.gameObject.name);
                        }
					}
					if(crosshairChanger.commitCrime == true) {
                        if (gameManager.whoSeePlayer.Count > 0) {
                            playerStats.crimeScore += 10;
                            if (playerStats.valorScore >= 5) {
                                playerStats.valorScore -= 5;
                            }
                            uiSA.crimeScore.text = "Crime score: " + playerStats.crimeScore.ToString();
                            uiSA.valorScore.text = "Valor score: " + playerStats.valorScore.ToString();
                        }
					}
				}
			}
			else if(hit.transform.tag == "Item") {
				if(hit.transform.GetComponent<ArrowQuiver>()) {
					ArrowQuiver tmp = hit.transform.GetComponent<ArrowQuiver>();
					if (interactText != null) {
						interactText.text = tmp.arrowPrefab.GetComponent<Item> ().itemName + " (" + tmp.countOfArrows + ")";
					}

					if(tmp.owner == null || //Ako nije nicija kucia ili sta vec
					   tmp.owner == gameObject) { //Ako je to moja kuca, ili sta vec
						if (crosshairChanger != null) {
							crosshairChanger.commitCrime = false;
                            if (uiManager.tutorialUI.GetComponentInChildren<TutorialUI>()) {
                                uiManager.tutorialUI.GetComponentInChildren<TutorialUI>().ShowInteractTutorial();
                            }
                        }
					}
					else {
						if (crosshairChanger != null) {
							crosshairChanger.commitCrime = true;
                            if (uiManager.tutorialUI.GetComponentInChildren<TutorialUI>()) {
                                uiManager.tutorialUI.GetComponentInChildren<TutorialUI>().ShowCrimeTutorial();
                            }
                        }
					}
				}
				else {
					if (interactText != null) {
						interactText.text = hit.transform.GetComponent<Item> ().itemName;
					}
					if(hit.transform.GetComponent<Item>().owner == null || //Ako nije nicija kucia ili sta vec
					   hit.transform.GetComponent<Item>().owner == gameObject) { //Ako je to moja kuca, ili sta vec
						if (crosshairChanger != null) {
							crosshairChanger.commitCrime = false;
                            if (uiManager.tutorialUI.GetComponentInChildren<TutorialUI>()) {
                                uiManager.tutorialUI.GetComponentInChildren<TutorialUI>().ShowInteractTutorial();
                            }
                        }
					}
					else {
						if (crosshairChanger != null) {
							crosshairChanger.commitCrime = true;
                            if (uiManager.tutorialUI.GetComponentInChildren<TutorialUI>()) {
                                uiManager.tutorialUI.GetComponentInChildren<TutorialUI>().ShowCrimeTutorial();
                            }
                        }
					}
				}
				objectPointedAt = hit.transform.gameObject;
				if (crosshairChanger != null) {
					crosshairChanger.crosshairState = CrosshairState.Loot;
				}

				if(Input.GetButtonDown("Use")) {
					if(step) {
						StartCoroutine(PlaySound(itemSFX));
					}
					hit.transform.GetComponent<PickupItem>().PickupThisItem();
					if(crosshairChanger.commitCrime == true) {
                        if (gameManager.whoSeePlayer.Count > 0) {
                            playerStats.crimeScore += 10;
                            if (playerStats.valorScore >= 5) {
                                playerStats.valorScore -= 5;
                            }
                            uiSA.crimeScore.text = "Crime score: " + playerStats.crimeScore.ToString();
                            uiSA.valorScore.text = "Valor score: " + playerStats.valorScore.ToString();
                        }
					}
				}
			}
			else if(hit.transform.tag == "CoinBag") {
				if (interactText != null) {
					interactText.text = "Coin Bag";
				}
				objectPointedAt = hit.transform.gameObject;
				if (crosshairChanger != null) {
					crosshairChanger.crosshairState = CrosshairState.Loot;
					crosshairChanger.commitCrime = false;
                    if (uiManager.tutorialUI.GetComponentInChildren<TutorialUI>()) {
                        uiManager.tutorialUI.GetComponentInChildren<TutorialUI>().ShowInteractTutorial();
                    }
                }
				
				if(Input.GetButtonDown("Use")) {
					if(step) {
						StartCoroutine(PlaySound(coinsSFX));
					}
					hit.transform.GetComponent<CoinsBag>().PickupCoinBag();
				}
			}
			else if(hit.transform.tag == "Horse") {
				if(hit.transform.GetComponent<HorseManager>().owner == null || //Ako nije niciji konj ili sta vec
				   hit.transform.GetComponent<HorseManager>().owner == gameObject) { //Ako je to moj konj, ili sta vec
					if (interactText != null) {
						interactText.text = hit.transform.GetComponent<HorseManager> ().horseName;
					}
					if (crosshairChanger != null) {
						crosshairChanger.crosshairState = CrosshairState.Mount;
						crosshairChanger.commitCrime = false;
                        if (uiManager.tutorialUI.GetComponentInChildren<TutorialUI>()) {
                            uiManager.tutorialUI.GetComponentInChildren<TutorialUI>().ShowInteractTutorial();
                        }
                    }

					if(Input.GetButtonUp("Use")) {
						if(hit.transform.GetComponent<HorseManager>().hasEquipment) {
							if(step) {
								StartCoroutine(PlaySound(mountingSFX));
							}
							horseRiding.playerMounted = true;
							horseRiding.horseType = hit.transform.GetComponent<HorseManager>().horseType;
							horseRiding.speed = hit.transform.GetComponent<HorseManager>().speed;
							horseRiding.health = hit.transform.GetComponent<HorseManager>().health;
							horseRiding.horseRidenNpcGo = hit.transform.gameObject;
							horseRiding.horseRidenNpcGo.SetActive(false);
							horseRiding.gameObject.SetActive(true);
							horseRiding.horseRidenNpcGo.GetComponent<HorseManager>().stabled = false;

							if(gameManager.activeHorse != hit.transform.gameObject) {
								gameManager.activeHorse = null;
								gameManager.activeHorse = hit.transform.gameObject;
							}
						}
						else {
							showMessage.SendTheMessage("I can not mount this horse until I equip him with saddle.");
						}
					}
				}
				else {
					if (interactText != null) {
						interactText.text = "Steal " + "\n" + " " + hit.transform.GetComponent<HorseManager> ().horseName;
					}
					if (crosshairChanger != null) {
						crosshairChanger.crosshairState = CrosshairState.Mount;
						crosshairChanger.commitCrime = true;
                        if (uiManager.tutorialUI.GetComponentInChildren<TutorialUI>()) {
                            uiManager.tutorialUI.GetComponentInChildren<TutorialUI>().ShowCrimeTutorial();
                        }
                    }

					if(Input.GetButtonUp("Use")) {
						if(hit.transform.GetComponent<HorseManager>().hasEquipment) {
							if(step) {
								StartCoroutine(PlaySound(mountingSFX));
							}

							horseRiding.playerMounted = true;
							horseRiding.horseType = hit.transform.GetComponent<HorseManager>().horseType;
							horseRiding.speed = hit.transform.GetComponent<HorseManager>().speed;
							horseRiding.health = hit.transform.GetComponent<HorseManager>().health;
							horseRiding.horseRidenNpcGo = hit.transform.gameObject;
							horseRiding.horseRidenNpcGo.SetActive(false);
							horseRiding.gameObject.SetActive(true);
							horseRiding.horseRidenNpcGo.GetComponent<HorseManager>().stabled = false;

							if(crosshairChanger.commitCrime == true) {
                                if (gameManager.whoSeePlayer.Count > 0) {

                                    playerStats.crimeScore += 30;
                                    if (playerStats.valorScore >= 15) {
                                        playerStats.valorScore -= 15;
                                    }
                                    else {
                                        playerStats.valorScore = 0;
                                    }
                                    uiSA.crimeScore.text = "Crime score: " + playerStats.crimeScore.ToString();
                                    uiSA.valorScore.text = "Valor score: " + playerStats.valorScore.ToString();
                                }
							}
						}
						else {
							showMessage.SendTheMessage("I can not steal this horse without a saddle on him. I may fall down and get caught.");
						}
					}
				}
			}
			else if(hit.transform.tag == "Ingredient") {
				if (interactText != null) {
					interactText.text = "Pick up " + "\n" + " " + hit.transform.GetComponent<Ingredient> ().ingredientName;
				}
				if (crosshairChanger != null) {
					crosshairChanger.crosshairState = CrosshairState.Loot;
					crosshairChanger.commitCrime = false;
                    if (uiManager.tutorialUI.GetComponentInChildren<TutorialUI>()) {
                        uiManager.tutorialUI.GetComponentInChildren<TutorialUI>().ShowInteractTutorial();
                    }
                }

				if(Input.GetButtonDown("Use")) {
					if(step) {
						StartCoroutine(PlaySound(flowerPickSFX));
						hit.transform.GetComponent<Ingredient>().PickupIngredient();
					}
				}
			}
			else if(hit.transform.tag == "Book") {
				if (interactText != null) {
					interactText.text = hit.transform.GetComponent<BookManager> ().bookTitle;
				}
				if (crosshairChanger != null) {
					crosshairChanger.crosshairState = CrosshairState.Read;
					crosshairChanger.commitCrime = false;
                    if (uiManager.tutorialUI.GetComponentInChildren<TutorialUI>()) {
                        uiManager.tutorialUI.GetComponentInChildren<TutorialUI>().ShowInteractTutorial();
                    }
                }

				if(Input.GetButtonDown("Use")) {
					if(step) {
						StartCoroutine(PlaySound(bookSfx));
						if (uiManager != null && uiManager.enabled != false) {
							uiManager.bookUI.SetActive (true);
							BookManager bm = hit.transform.GetComponent<BookManager> ();
							uiManager.bookUI.GetComponent<BookUi> ().BookUpdateContent (bm);
						}
					}

					if(crosshairChanger.commitCrime == true) {
                        if (gameManager.whoSeePlayer.Count > 0) {

                            playerStats.crimeScore += 10;
                            if (playerStats.valorScore >= 5) {
                                playerStats.valorScore -= 5;
                            }
                            uiSA.crimeScore.text = "Crime score: " + playerStats.crimeScore.ToString();
                            uiSA.valorScore.text = "Valor score: " + playerStats.valorScore.ToString();
                        }
					}
				}
			}
			else if(hit.transform.tag == "Fireplace") {
				if (interactText != null) {
					interactText.text = "Fireplace";
				}
				if (crosshairChanger != null) {
					crosshairChanger.crosshairState = CrosshairState.Loot;
					crosshairChanger.commitCrime = false;
                    if (uiManager.tutorialUI.GetComponentInChildren<TutorialUI>()) {
                        uiManager.tutorialUI.GetComponentInChildren<TutorialUI>().ShowInteractTutorial();
                    }
                }
				
				if(Input.GetButtonDown("Use")) {
					if(step) {
						FireplaceFireController fire = hit.transform.GetComponentInChildren<FireplaceFireController>();
						if(fire.fire.GetComponentInChildren<ParticleSystem>().emissionRate > 0f) {
							//StartCoroutine(PlaySound(alchemySFX));
							if (uiManager != null && uiManager.enabled != false &&
							    gameManager.playerState != PlayerState.Fighting) {
								//uiManager.alchemyUI.SetActive (true);	
								//uiManager.SetActiveUIElement (uiManager.alchemyUI.transform.Find ("CraftBtn").gameObject);
								//uiManager.alchemyUI.GetComponent<AlchemyUi> ().SetupAlchemyWindow ();

								uiManager.waiterUI.SetActive (true);	
								uiManager.SetActiveUIElement (uiManager.waiterUI.transform.Find ("WaitBtn").gameObject);
							}
						}
						else {
							StartCoroutine(PlaySound(fireplaceSFX));
							fire.LightTheFire();
						}
					}
				}
			}
            else if (hit.transform.tag == "Bed") {
                if (interactText != null) {
                    interactText.text = "Bed";
                }
                if (crosshairChanger != null) {
                    crosshairChanger.crosshairState = CrosshairState.Loot;
                    crosshairChanger.commitCrime = false;
                    if (uiManager.tutorialUI.GetComponentInChildren<TutorialUI>()) {
                        uiManager.tutorialUI.GetComponentInChildren<TutorialUI>().ShowInteractTutorial();
                    }
                }

                if (Input.GetButtonDown("Use")) {
                    if (step) {
                        StartCoroutine(PlaySound(sleepingSFX));
                        if (uiManager != null && uiManager.enabled != false &&
                            gameManager.playerState != PlayerState.Fighting) {

                            uiManager.waiterUI.SetActive(true);
                            uiManager.SetActiveUIElement(uiManager.waiterUI.transform.Find("WaitBtn").gameObject);
                        }
                    }
                }
            }
            else if(hit.transform.tag == "Inn") {
				if (interactText != null) {

					int currentHour = 0;
					int.TryParse(todClock.currentHour, out currentHour);

					InnManager innManager = hit.transform.GetComponent<InnManager>();

					interactText.text = innManager.innName;
				}
				if (crosshairChanger != null) {
					crosshairChanger.crosshairState = CrosshairState.Info;
					crosshairChanger.commitCrime = false;
				}
			}
			else if(hit.transform.tag == "WorldBarrier") {
				if (crosshairChanger != null) {
					crosshairChanger.crosshairState = CrosshairState.Normal;
					crosshairChanger.commitCrime = false;
				}
				if (interactText != null) {
					interactText.text = "I can not go that way!";
				}
			}
            else if (hit.transform.tag == "PropertyOnSale") {
                if (interactText != null) {
                    interactText.text = "Sale Sign";
                }
                objectPointedAt = hit.transform.gameObject;
                if (crosshairChanger != null) {
                    crosshairChanger.crosshairState = CrosshairState.Info;
                    crosshairChanger.commitCrime = false;
                    if (uiManager.tutorialUI.GetComponentInChildren<TutorialUI>()) {
                        uiManager.tutorialUI.GetComponentInChildren<TutorialUI>().ShowInteractTutorial();
                    }
                }

                if (Input.GetButtonDown("Use")) {
                    if (step) {
                        StartCoroutine(PlaySound(bookSfx));
                    }
                    hit.transform.GetComponent<BuyProperty>().AskToBuyProperty();
                }
            }
            else {
				if (interactText != null) {
					interactText.text = "";
				}
				objectPointedAt = null;
				if (crosshairChanger != null) {
					crosshairChanger.crosshairState = CrosshairState.Normal;
					crosshairChanger.commitCrime = false;
				}
			}
		}
		else {
			if (interactText != null) {
				interactText.text = "";
			}
			objectPointedAt = null;
			if (crosshairChanger != null) {
				crosshairChanger.crosshairState = CrosshairState.Normal;
				crosshairChanger.commitCrime = false;
			}
		}
	}

	public IEnumerator PlaySound(AudioClip[] name) {
		if(interactionAS != null) {
			step = false;
			interactionAS.clip = name[Random.Range(0, name.Length)];
			interactionAS.Play();
			yield return new WaitForSeconds(0.25f);
			step = true;
		}
	} 
}
