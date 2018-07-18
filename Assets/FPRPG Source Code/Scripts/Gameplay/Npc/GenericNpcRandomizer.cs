using UnityEngine;
using System.Collections;

public class GenericNpcRandomizer : MonoBehaviour {

	public bool randomRace;
	public bool randomHair;
	public bool randomBeard;
	[Tooltip("Show/Hide npcs helm. It only works if there is already helm/hat set for this npc.")]
	public bool randomHelmOnOff;
	[Space(10f)]
	public bool leveledHelm;
	public bool leveledArmor;
	public bool leveledWeapon;
	public bool leveledShield;

	private NPC npc;
	private PlayerLevelManager playerLvl;

	void Awake() {
		npc = GetComponent<NPC>();
		playerLvl = GameObject.Find("[Player]").GetComponent<PlayerLevelManager>();
		GenerateNPCRandomStuff();
	}

	public void GenerateNPCRandomStuff() {
		if(randomRace == true) {
			int randRaceNum = Random.Range(0, 100);

			if(randRaceNum >= 0 && randRaceNum < 40) {
				npc.race = CharacterRace.Elinian;
			}
			else if(randRaceNum >= 40 && randRaceNum < 60) {
				npc.race = CharacterRace.Ariyan;
			}
			else if(randRaceNum >= 60 && randRaceNum < 80) {
				npc.race = CharacterRace.Sintarian;
			}
			else if(randRaceNum >= 80 && randRaceNum < 95) {
				npc.race = CharacterRace.Koronian;
			}
			else if(randRaceNum >= 95 && randRaceNum < 100) {
				npc.race = CharacterRace.Elevirian;
			}
		}
		if(randomHair == true) {
			int randHairNum = Random.Range(0, 100);
			
			if(randHairNum >= 0 && randHairNum < 20) {
				npc.hair = NPCHair.Bald;
			}
			else if(randHairNum >= 20 && randHairNum < 40) {
				npc.hair = NPCHair.ShortHair;
			}
			else if(randHairNum >= 40 && randHairNum < 60) {
				npc.hair = NPCHair.LongHair;
			}
			else if(randHairNum >= 60 && randHairNum < 80) {
				npc.hair = NPCHair.Cleantop;
			}
			else if(randHairNum >= 80 && randHairNum < 100) {
				npc.hair = NPCHair.Topfuzz;
			}
		}
		if(randomBeard == true) {
			int randBeardNum = Random.Range(0, 100);
			
			if(randBeardNum >= 0 && randBeardNum < 10) {
				npc.facialHair = NPCFacialHair.None;
			}
			else if(randBeardNum >= 10 && randBeardNum < 25) {
				npc.facialHair = NPCFacialHair.Beard;
			}
			else if(randBeardNum >= 25 && randBeardNum < 40) {
				npc.facialHair = NPCFacialHair.Goatee;
			}
			else if(randBeardNum >= 40 && randBeardNum < 55) {
				npc.facialHair = NPCFacialHair.ShortMoustache;
			}
			else if(randBeardNum >= 55 && randBeardNum < 70) {
				npc.facialHair = NPCFacialHair.BigMoustache;
			}
			else if(randBeardNum >= 75 && randBeardNum < 90) {
				npc.facialHair = NPCFacialHair.MoustacheNSideburns;
			}
			else if(randBeardNum >= 90 && randBeardNum < 100) {
				npc.facialHair = NPCFacialHair.LongBeard;
			}
		}

		if(leveledArmor == true) {
			if(playerLvl.playerLevel >= 1 && playerLvl.playerLevel < 3) {
				float randGambeson = Random.Range(0.1f, 1f);

				if(randGambeson > 0f && randGambeson <= 0.5f) {
					npc.outfit = NPCOutfit.GambesonRed;
				}
				else if(randGambeson > 0.5f && randGambeson <= 1f) {
					npc.outfit = NPCOutfit.GambesonGreen;
				}
			}
			else if(playerLvl.playerLevel >= 3 && playerLvl.playerLevel < 16) {
                float randArmor = Random.Range(0.1f, 1f);

                if (randArmor > 0f && randArmor <= 0.5f) {
                    npc.outfit = NPCOutfit.ArmorIron;
                }
                else if (randArmor > 0.5f && randArmor <= 1f) {
                    npc.outfit = NPCOutfit.ArmorSteel;
                }
			}
			else if(playerLvl.playerLevel >= 16 && playerLvl.playerLevel < 30) {
                float randArmor = Random.Range(0.1f, 1f);

                if (randArmor > 0f && randArmor <= 0.5f) {
                    npc.outfit = NPCOutfit.ArmorSilver;
                }
                else if (randArmor > 0.5f && randArmor <= 1f) {
                    npc.outfit = NPCOutfit.ArmorNorgarnian;
                }
            }
			else if(playerLvl.playerLevel >= 30 && playerLvl.playerLevel < 40) {
                float randArmor = Random.Range(0.1f, 1f);

                if (randArmor > 0f && randArmor <= 0.5f) {
                    npc.outfit = NPCOutfit.ArmorImperial;
                }
                else if (randArmor > 0.5f && randArmor <= 1f) {
                    npc.outfit = NPCOutfit.ArmorDarkSteel;
                }
            }
			else if(playerLvl.playerLevel >= 40 && playerLvl.playerLevel < 50) {
                float randArmor = Random.Range(0.1f, 1f);

                if (randArmor > 0f && randArmor <= 0.5f) {
                    npc.outfit = NPCOutfit.ArmorElven;
                }
                else if (randArmor > 0.5f && randArmor <= 1f) {
                    npc.outfit = NPCOutfit.ArmorArden;
                }
            }
		}
		if(leveledHelm == true) {
			int randomEyepatch = Random.Range(0, 100);

			if(randomEyepatch >= 0 && randomEyepatch < 20) {
				npc.helmetHat = NPCHelmet.Eyepatch;
			}
			else {
				if(playerLvl.playerLevel >= 1 && playerLvl.playerLevel < 3) {
					float randHat = Random.Range(0.1f, 1f);
					
					if(randHat > 0f && randHat <= 0.35f) {
						npc.helmetHat = NPCHelmet.FeatheraBlue;
					}
					else if(randHat > 0.35f && randHat <= 0.75f) {
						npc.helmetHat = NPCHelmet.FeatheraGreen;
					}
					else if(randHat > 0.75f && randHat <= 1f) {
						npc.helmetHat = NPCHelmet.HatGreen;
					}
				}
				else if(playerLvl.playerLevel >= 3 && playerLvl.playerLevel < 16) {
                    float randHelm = Random.Range(0.1f, 1f);

                    if (randHelm > 0f && randHelm <= 0.5f) {
                        npc.helmetHat = NPCHelmet.HelmIron;
                    }
                    else if (randHelm > 0.5f && randHelm <= 1f) {
                        npc.helmetHat = NPCHelmet.HelmSteel;
                    }
                }
				else if(playerLvl.playerLevel >= 16 && playerLvl.playerLevel < 30) {
                    float randHelm = Random.Range(0.1f, 1f);

                    if (randHelm > 0f && randHelm <= 0.5f) {
                        npc.helmetHat = NPCHelmet.HelmSilver;
                    }
                    else if (randHelm > 0.5f && randHelm <= 1f) {
                        npc.helmetHat = NPCHelmet.HelmNorgarnian;
                    }
                }
				else if(playerLvl.playerLevel >= 30 && playerLvl.playerLevel < 40) {
                    float randHelm = Random.Range(0.1f, 1f);

                    if (randHelm > 0f && randHelm <= 0.5f) {
                        npc.helmetHat = NPCHelmet.HelmImperial;
                    }
                    else if (randHelm > 0.5f && randHelm <= 1f) {
                        npc.helmetHat = NPCHelmet.HelmDarkSteel;
                    }
                }
				else if(playerLvl.playerLevel >= 40 && playerLvl.playerLevel < 50) {
                    float randHelm = Random.Range(0.1f, 1f);

                    if (randHelm > 0f && randHelm <= 0.5f) {
                        npc.helmetHat = NPCHelmet.HelmElven;
                    }
                    else if (randHelm > 0.5f && randHelm <= 1f) {
                        npc.helmetHat = NPCHelmet.HelmArden;
                    }
                }
			}
		}
		if(leveledWeapon == true) {
			int randWeaponType = Random.Range(1, 30);

			if(randWeaponType > 0 && randWeaponType <= 10) {
				if(playerLvl.playerLevel >= 1 && playerLvl.playerLevel < 16) {
					npc.weapon = NPCWeapon.SwordIron;
				}
				else if(playerLvl.playerLevel >= 16 && playerLvl.playerLevel < 30) {
					npc.weapon = NPCWeapon.SwordSteel;
				}
				else if(playerLvl.playerLevel >= 30 && playerLvl.playerLevel < 40) {
					npc.weapon = NPCWeapon.SwordDarksteel;
				}
				else if(playerLvl.playerLevel >= 40 && playerLvl.playerLevel < 50) {
					npc.weapon = NPCWeapon.SwordArden;
				}
			}
			else if(randWeaponType > 10 && randWeaponType <= 20) {
				if(playerLvl.playerLevel >= 1 && playerLvl.playerLevel < 16) {
					npc.weapon = NPCWeapon.AxeIron;
				}
				else if(playerLvl.playerLevel >= 16 && playerLvl.playerLevel < 30) {
					npc.weapon = NPCWeapon.AxeSteel;
				}
				else if(playerLvl.playerLevel >= 30 && playerLvl.playerLevel < 40) {
					npc.weapon = NPCWeapon.AxeImperial;
				}
				else if(playerLvl.playerLevel >= 40 && playerLvl.playerLevel < 50) {
					npc.weapon = NPCWeapon.AxeArden;
				}
			}
			else if(randWeaponType > 20 && randWeaponType <= 30) {
				if(playerLvl.playerLevel >= 1 && playerLvl.playerLevel < 16) {
					npc.weapon = NPCWeapon.MaceIron;
				}
				else if(playerLvl.playerLevel >= 16 && playerLvl.playerLevel < 30) {
					npc.weapon = NPCWeapon.MaceSteel;
				}
				else if(playerLvl.playerLevel >= 30 && playerLvl.playerLevel < 40) {
					npc.weapon = NPCWeapon.MaceDarksteel;
				}
				else if(playerLvl.playerLevel >= 40 && playerLvl.playerLevel < 50) {
					npc.weapon = NPCWeapon.MaceArden;
				}
			}
		}
		if(leveledShield == true) {
			if(playerLvl.playerLevel >= 1 && playerLvl.playerLevel < 16) {
				npc.shield = NPCShield.ShieldWooden;
			}
			else if(playerLvl.playerLevel >= 16 && playerLvl.playerLevel < 30) {
				npc.shield = NPCShield.ShieldSteel;
			}
			else if(playerLvl.playerLevel >= 30 && playerLvl.playerLevel < 40) {
				npc.shield = NPCShield.ShieldDarksteel;
			}
			else if(playerLvl.playerLevel >= 40 && playerLvl.playerLevel < 50) {
				npc.shield = NPCShield.ShieldArden;
			}
		}

		if(randomHelmOnOff == true) {
			int randHelmNum = Random.Range(0, 100);
			
			if(randHelmNum >= 75 && randHelmNum < 100) {
				npc.helmetHat = NPCHelmet.None;
			}
		}

		npc.SetUpLooks();
	}
}
