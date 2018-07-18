using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class PlayerLooks : MonoBehaviour {

	[Header("RENDERERS")]
	public Image baseSr;
	public Image hairSr;
	public Image beardSr;
	public Image armorSr;
	public Image helmSr;
	public Image weaponSr;
	public Image shieldSr;
	[Header("BODY SPRITES")]
	[Space(20f)]
	[Header("Race Sprites")]
	public Sprite elinianBase;
    public Sprite elinianFBase;
	public Sprite arianBase;
    public Sprite arianFBase;
    public Sprite sintarianBase;
    public Sprite sintarianFBase;
    public Sprite koronianBase;
    public Sprite koronianFBase;
    public Sprite elevirianBase;
    public Sprite elevirianFBase;
    [Header("Hair Sprites")]
	public Sprite baldHair;
	public Sprite shortHair;
	public Sprite longHair;
	public Sprite cleantop;
	public Sprite topfuzz;
    public Sprite twistedHair;
    public Sprite bunHair;
    public Sprite stylizedHair;
    public Sprite ponytailHair;
	[Header("Beard Sprites")]
	public Sprite goateeBeard;
	public Sprite beard;
	public Sprite longBeard;
	public Sprite moustache;
	public Sprite bigMoustache;
	public Sprite moustacheSideburns;
    public Sprite sideburns;
    public Sprite lowerGoatee;
    [Header("ITEM SPRITES")]
	[Space(20f)]
	[Header("Outfits")]
	public Sprite greenOutfit;
	public Sprite redOutfit;
	public Sprite coatDark;
	public Sprite robeBlack;
	public Sprite robeBlue;
	public Sprite lungumRobe;
	public Sprite ironArmor;
	public Sprite steelArmor;
	public Sprite silverArmor;
	public Sprite imperialArmor;
	public Sprite elvenArmor;
	public Sprite ardenArmor;
    public Sprite darksteelArmor;
    [Header("Headwear")]
	public Sprite greenHat;
	public Sprite greenFeathera;
	public Sprite blureFeathera;
	public Sprite ironHelm;
	public Sprite steelHelm;
	public Sprite silverHelm;
	public Sprite imperialHelm;
	public Sprite elvenHelm;
	public Sprite ardenHelm;
    public Sprite darksteelHelm;
	[Header("Weapons")]
	public NPCWeapon weaponEquiped;
	public int weaponInt;
	public Sprite[] weapons;
	[Header("Shields")]
	public NPCShield shieldEquiped;
	public int shieldInt;
	public Sprite[] shields;
	[Space(20f)]
	public Sprite emptySprite;

	private PlayerStats playerStats;
	private EquipmentManager equipManager;

    void Start() {
        playerStats = GameObject.Find("[Player]").GetComponent<PlayerStats>();
        equipManager = GameObject.Find("[Player]").GetComponentInChildren<EquipmentManager>();
    }

    public void Update() {
		if(playerStats.playerRace == CharacterRace.Elinian) {
            if (playerStats.playerGender == CharacterGender.Male) {
                baseSr.sprite = elinianBase;
            }
            else if(playerStats.playerGender == CharacterGender.Female) {
                baseSr.sprite = elinianFBase;
            }
        }
		else if(playerStats.playerRace == CharacterRace.Ariyan) {
            if (playerStats.playerGender == CharacterGender.Male) {
                baseSr.sprite = arianBase;
            }
            else if (playerStats.playerGender == CharacterGender.Female) {
                baseSr.sprite = arianFBase;
            }
        }
		else if(playerStats.playerRace == CharacterRace.Sintarian) {
            if (playerStats.playerGender == CharacterGender.Male) {
                baseSr.sprite = sintarianBase;
            }
            else if (playerStats.playerGender == CharacterGender.Female) {
                baseSr.sprite = sintarianFBase;
            }
        }
		else if(playerStats.playerRace == CharacterRace.Koronian) {
            if (playerStats.playerGender == CharacterGender.Male) {
                baseSr.sprite = koronianBase;
            }
            else if (playerStats.playerGender == CharacterGender.Female) {
                baseSr.sprite = koronianFBase;
            }
        }
		else if(playerStats.playerRace == CharacterRace.Elevirian) {
            if (playerStats.playerGender == CharacterGender.Male) {
                baseSr.sprite = elevirianBase;
            }
            else if (playerStats.playerGender == CharacterGender.Female) {
                baseSr.sprite = elevirianFBase;
            }
        }

		if(playerStats.playerHair == NPCHair.Bald) {
			hairSr.sprite = baldHair;
		}
		else if(playerStats.playerHair == NPCHair.ShortHair) {
			hairSr.sprite = shortHair;
		}
		else if(playerStats.playerHair == NPCHair.LongHair) {
			hairSr.sprite = longHair;
		}
		else if(playerStats.playerHair == NPCHair.Cleantop) {
			hairSr.sprite = cleantop;
		}
		else if(playerStats.playerHair == NPCHair.Topfuzz) {
			hairSr.sprite = topfuzz;
        }
        else if (playerStats.playerHair == NPCHair.TwistedHair) {
            hairSr.sprite = twistedHair;
        }
        else if (playerStats.playerHair == NPCHair.BunHair) {
            hairSr.sprite = bunHair;
        }
        else if (playerStats.playerHair == NPCHair.StylizedHair) {
            hairSr.sprite = stylizedHair;
        }
        else if (playerStats.playerHair == NPCHair.PonytailHair) {
            hairSr.sprite = ponytailHair;
        }

        if (playerStats.playerBeard == NPCFacialHair.None) {
			beardSr.sprite = emptySprite;
		}
		else if(playerStats.playerBeard == NPCFacialHair.Goatee) {
			beardSr.sprite = goateeBeard;
		}
		else if(playerStats.playerBeard == NPCFacialHair.Beard) {
			beardSr.sprite = beard;
		}
		else if(playerStats.playerBeard == NPCFacialHair.LongBeard) {
			beardSr.sprite = longBeard;
		}
		else if(playerStats.playerBeard == NPCFacialHair.ShortMoustache) {
			beardSr.sprite = moustache;
		}
		else if(playerStats.playerBeard == NPCFacialHair.BigMoustache) {
			beardSr.sprite = bigMoustache;
		}
		else if(playerStats.playerBeard == NPCFacialHair.MoustacheNSideburns) {
			beardSr.sprite = moustacheSideburns;
        }
        else if (playerStats.playerBeard == NPCFacialHair.Sideburns) {
            beardSr.sprite = sideburns;
        }
        else if (playerStats.playerBeard == NPCFacialHair.LowerGoatee) {
            beardSr.sprite = lowerGoatee;
        }

        hairSr.color = playerStats.playerHairColor;
		beardSr.color = playerStats.playerHairColor;

		if(equipManager.headSlot != null) {
			if(equipManager.headSlot.GetComponent<Item>().itemName.Contains("Green Cap")) {
				helmSr.sprite = greenHat;
			}
			else if(equipManager.headSlot.GetComponent<Item>().itemName.Contains("Green Feathera")) {
				helmSr.sprite = greenFeathera;
			}
			else if(equipManager.headSlot.GetComponent<Item>().itemName.Contains("Blue Feathera")) {
				helmSr.sprite = blureFeathera;
			}
			else if(equipManager.headSlot.GetComponent<Item>().itemName.Contains("Iron Helmet")) {
				helmSr.sprite = ironHelm;
			}
			else if(equipManager.headSlot.GetComponent<Item>().itemName.Contains("Steel Helmet")) {
				helmSr.sprite = steelHelm;
			}
			else if(equipManager.headSlot.GetComponent<Item>().itemName.Contains("Silver Helmet")) {
				helmSr.sprite = silverHelm;
			}
			else if(equipManager.headSlot.GetComponent<Item>().itemName.Contains("Imperial Helmet")) {
				helmSr.sprite = imperialHelm;
			}
			else if(equipManager.headSlot.GetComponent<Item>().itemName.Contains("Elven Helmet")) {
				helmSr.sprite = elvenHelm;
			}
			else if(equipManager.headSlot.GetComponent<Item>().itemName.Contains("Arden Helmet")) {
				helmSr.sprite = ardenHelm;
            }
            else if (equipManager.headSlot.GetComponent<Item>().itemName.Contains("Darksteel Helmet")) {
                helmSr.sprite = darksteelHelm;
            }
        }
		else {
			helmSr.sprite = emptySprite;
		}

		if(equipManager.chestSlot != null) {
			if(equipManager.chestSlot.GetComponent<Item>().itemName.Contains("Green Clothes")) {
				armorSr.sprite = greenOutfit;
			}
			else if(equipManager.chestSlot.GetComponent<Item>().itemName.Contains("Red Coat")) {
				armorSr.sprite = coatDark;
			}
			else if(equipManager.chestSlot.GetComponent<Item>().itemName.Contains("Blue Robe")) {
				armorSr.sprite = robeBlue;
			}
			else if(equipManager.chestSlot.GetComponent<Item>().itemName.Contains("Black Robe")) {
				armorSr.sprite = robeBlack;
			}
			else if(equipManager.chestSlot.GetComponent<Item>().itemName.Contains("Red Clothes")) {
				armorSr.sprite = redOutfit;
			}
			else if(equipManager.chestSlot.GetComponent<Item>().itemName.Contains("Monk Robe")) {
				armorSr.sprite = lungumRobe;
			}
			else if(equipManager.chestSlot.GetComponent<Item>().itemName.Contains("Iron Armor")) {
				armorSr.sprite = ironArmor;
			}
			else if(equipManager.chestSlot.GetComponent<Item>().itemName.Contains("Steel Armor")) {
				armorSr.sprite = steelArmor;
			}
			else if(equipManager.chestSlot.GetComponent<Item>().itemName.Contains("Silver Armor")) {
				armorSr.sprite = silverArmor;
			}
			else if(equipManager.chestSlot.GetComponent<Item>().itemName.Contains("Imperial Armor")) {
				armorSr.sprite = imperialArmor;
			}
			else if(equipManager.chestSlot.GetComponent<Item>().itemName.Contains("Elven Armor")) {
				armorSr.sprite = elvenArmor;
			}
			else if(equipManager.chestSlot.GetComponent<Item>().itemName.Contains("Arden Armor")) {
				armorSr.sprite = ardenArmor;
            }
            else if (equipManager.chestSlot.GetComponent<Item>().itemName.Contains("Darksteel Armor")) {
                armorSr.sprite = darksteelArmor;
            }
        }
		else {
			armorSr.sprite = emptySprite;
		}

		if(equipManager.weaponSlot != null) {

			if(equipManager.weaponSlot.GetComponent<Item>().itemGOName.Contains("Bow")) {
				weaponSr.rectTransform.anchoredPosition = new Vector2(80f, 72f);
				weaponSr.rectTransform.localRotation = Quaternion.Euler(0f, 0f, -15f);
			}
			else {
				weaponSr.rectTransform.anchoredPosition = new Vector2(99f, 60f);
				weaponSr.rectTransform.localRotation = Quaternion.Euler(0f, 0f, -45f);
			}

			string parsedName = equipManager.weaponSlot.GetComponent<Item>().itemGOName;

			if(parsedName.Contains("Ghost")) {
				string parsedName1 = parsedName.Replace("Ghost", "");

				NPCWeapon parsedWeapons = (NPCWeapon)System.Enum.Parse(typeof(NPCWeapon), parsedName1);
				weaponEquiped = parsedWeapons;
				weaponInt = (int)weaponEquiped;
				
				weaponSr.sprite = weapons[weaponInt];
			}
			else {
				NPCWeapon parsedWeapons = (NPCWeapon)System.Enum.Parse(typeof(NPCWeapon), parsedName);
				weaponEquiped = parsedWeapons;
				weaponInt = (int)weaponEquiped;
				
				weaponSr.sprite = weapons[weaponInt];
			}

		}
		else {
			weaponEquiped = NPCWeapon.None;
			weaponSr.sprite = emptySprite;
			weaponInt = 0;
		}

		if(equipManager.shieldSlot != null) {
			string parsedName = equipManager.shieldSlot.GetComponent<Item>().itemGOName;
			NPCShield parsedShields = (NPCShield)System.Enum.Parse(typeof(NPCShield), parsedName);
			shieldEquiped = parsedShields;
			shieldInt = (int)shieldEquiped;
			
			shieldSr.sprite = shields[shieldInt];
		}
		else {
			shieldEquiped = NPCShield.None;
			shieldSr.sprite = emptySprite;
			shieldInt = 0;
		}

		if (helmSr.sprite != emptySprite) {
			hairSr.enabled = false;
		} 
		else {
			hairSr.enabled = true;
		}
	}
}
