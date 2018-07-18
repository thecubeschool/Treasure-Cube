using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class NPCOutfiter : MonoBehaviour {

	public NPC npc;
	[Space(10f)]
	public Sprite elinian;
    public Sprite elinianF;
    public Sprite arian;
    public Sprite arianF;
	public Sprite sintarian;
    public Sprite sintarianF;
	public Sprite elevirian;
    public Sprite elevirianF;
    public Sprite koronian;
    public Sprite koronianF;
    public Sprite skeleton;
	public Sprite werewolf;
	public Sprite bauk;
	public Sprite wolf;
    public Sprite deer;
	[Space(10f)]
	public Sprite baldHair;
	public Sprite shortHair;
	public Sprite longHair;
	public Sprite cleantop;
	public Sprite topfuzz;
    public Sprite twistedHair;
    public Sprite bunHair;
    public Sprite stylizedHair;
    public Sprite ponytailHair;
	[Space(10f)]
	public Sprite goatee;
	public Sprite beard;
	public Sprite smallMoustache;
	public Sprite bigMoustache;
	public Sprite moustacheNSideburns;
	public Sprite longBeard;
    public Sprite sideburns;
    public Sprite lowerGoatee;
	[Space(10f)]
	public Sprite eyepatch;
	public Sprite hatGreen;
	public Sprite furCap;
	public Sprite featheraGreen;
	public Sprite featheraBlue;
	public Sprite hoodLungum;
	public Sprite hatPriest;
	public Sprite hoodPriest;
	public Sprite helmIron; //Iron
	public Sprite helmImperial; //Imperial
	public Sprite helmSteel;
	public Sprite helmSilver;
	public Sprite helmElven;
	public Sprite helmArden; //Arden
	public Sprite helmDarkSteel;
	public Sprite helmNorgarnian;
	public Sprite helmChronicler;
	[Space(10f)]
	public Sprite gambesonGreen;
	public Sprite gambesonRed;
	public Sprite coatDark;
	public Sprite inkeeperGarment;
	public Sprite furClothes;
	public Sprite robeBlue;
	public Sprite robeLungum;
	public Sprite robePriest;
	public Sprite robeSinta;
	public Sprite armorIron;
	public Sprite armorSteel;
	public Sprite armorSilver;
	public Sprite armorImperial;
	public Sprite armorElven;
	public Sprite armorArden;
	public Sprite armorNorgarian;
	public Sprite armorDarkSteel;
	public Sprite armorChronicler;
    public Sprite dressBrown;
    public Sprite dressBlue;
    public Sprite dressGreen;

	[Space(10f)]
	public Sprite[] weapons;
	public Sprite[] shields;
	[Space(10f)]
	public SpriteRenderer baseSr;
	public SpriteRenderer weaponSr;
	public SpriteRenderer shieldSr;
	public SpriteRenderer hairSr;
	public SpriteRenderer facialHairSr;
	public SpriteRenderer helmetSr;
	public SpriteRenderer outfirSr;
	
	public void Update () {

		if(npc.race == CharacterRace.Elinian) {
            if (npc.gender == CharacterGender.Male) {
                baseSr.sprite = elinian;
            }
            else if(npc.gender == CharacterGender.Female) {
                baseSr.sprite = elinianF;
            }
        }
		else if(npc.race == CharacterRace.Ariyan) {
            if (npc.gender == CharacterGender.Male) {
                baseSr.sprite = arian;
            }
            else if (npc.gender == CharacterGender.Female) {
                baseSr.sprite = arianF;
            }
        }
		else if(npc.race == CharacterRace.Sintarian) {
            if (npc.gender == CharacterGender.Male) {
                baseSr.sprite = sintarian;
            }
            else if (npc.gender == CharacterGender.Female) {
                baseSr.sprite = sintarianF;
            }
        }
		else if(npc.race == CharacterRace.Elevirian) {
            if (npc.gender == CharacterGender.Male) {
                baseSr.sprite = elevirian;
            }
            else if (npc.gender == CharacterGender.Female) {
                baseSr.sprite = elevirianF;
            }
        }
		else if(npc.race == CharacterRace.Koronian) {
            if (npc.gender == CharacterGender.Male) {
                baseSr.sprite = koronian;
            }
            else if (npc.gender == CharacterGender.Female) {
                baseSr.sprite = koronianF;
            }
        }
		else if(npc.race == CharacterRace.Skeleton) {
			baseSr.sprite = skeleton;
		}
		else if(npc.race == CharacterRace.Werewolf) {
			baseSr.sprite = werewolf;
		}
		else if(npc.race == CharacterRace.Bauk) {
			baseSr.sprite = bauk;
		}
		else if(npc.race == CharacterRace.BeastWolf) {
			baseSr.sprite = wolf;
        }
        else if (npc.race == CharacterRace.AnimalDeer) {
            baseSr.sprite = deer;
        }

        if (npc.hair == NPCHair.None) {
			hairSr.sprite = null;
		}
		else if(npc.hair == NPCHair.Bald) {
			hairSr.sprite = baldHair;
		}
		else if(npc.hair == NPCHair.ShortHair) {
			hairSr.sprite = shortHair;
		}
		else if(npc.hair == NPCHair.LongHair) {
			hairSr.sprite = longHair;
		}
		else if(npc.hair == NPCHair.Cleantop) {
			hairSr.sprite = cleantop;
		}
		else if(npc.hair == NPCHair.Topfuzz) {
			hairSr.sprite = topfuzz;
		}
        else if (npc.hair == NPCHair.TwistedHair) {
            hairSr.sprite = twistedHair;
        }
        else if (npc.hair == NPCHair.BunHair) {
            hairSr.sprite = bunHair;
        }
        else if (npc.hair == NPCHair.StylizedHair) {
            hairSr.sprite = stylizedHair;
        }
        else if (npc.hair == NPCHair.PonytailHair) {
            hairSr.sprite = ponytailHair;
        }

        if (npc.facialHair == NPCFacialHair.None) {
			facialHairSr.sprite = null;
		}
		else if(npc.facialHair == NPCFacialHair.Goatee) {
			facialHairSr.sprite = goatee;
		}
		else if(npc.facialHair == NPCFacialHair.Beard) {
			facialHairSr.sprite = beard;
		}
		else if(npc.facialHair == NPCFacialHair.ShortMoustache) {
			facialHairSr.sprite = smallMoustache;
		}
		else if(npc.facialHair == NPCFacialHair.BigMoustache) {
			facialHairSr.sprite = bigMoustache;
		}
		else if(npc.facialHair == NPCFacialHair.MoustacheNSideburns) {
			facialHairSr.sprite = moustacheNSideburns;
		}
		else if(npc.facialHair == NPCFacialHair.LongBeard) {
			facialHairSr.sprite = longBeard;
		}
        else if (npc.facialHair == NPCFacialHair.Sideburns) {
            facialHairSr.sprite = sideburns;
        }
        else if (npc.facialHair == NPCFacialHair.LowerGoatee) {
            facialHairSr.sprite = lowerGoatee;
        }

        if (npc.helmetHat == NPCHelmet.None) {
			helmetSr.sprite = null;
		}
		else if(npc.helmetHat == NPCHelmet.Eyepatch) {
			helmetSr.sprite = eyepatch;
		}
		else if(npc.helmetHat == NPCHelmet.HatGreen) {
			helmetSr.sprite = hatGreen;
		}
		else if(npc.helmetHat == NPCHelmet.FurCap) {
			helmetSr.sprite = furCap;
		}
		else if(npc.helmetHat == NPCHelmet.FeatheraGreen) {
			helmetSr.sprite = featheraGreen;
		}
		else if(npc.helmetHat == NPCHelmet.FeatheraBlue) {
			helmetSr.sprite = featheraBlue;
		}
		else if(npc.helmetHat == NPCHelmet.HoodLungum) {
			helmetSr.sprite = hoodLungum;
		}
		else if(npc.helmetHat == NPCHelmet.HelmIron) {
			helmetSr.sprite = helmIron;
		}
		else if(npc.helmetHat == NPCHelmet.HelmSteel) {
			helmetSr.sprite = helmSteel;
		}
		else if(npc.helmetHat == NPCHelmet.HelmSilver) {
			helmetSr.sprite = helmSilver;
		}
		else if(npc.helmetHat == NPCHelmet.HelmImperial) {
			helmetSr.sprite = helmImperial;
		}
		else if(npc.helmetHat == NPCHelmet.HelmElven) {
			helmetSr.sprite = helmElven;
		}
		else if(npc.helmetHat == NPCHelmet.HelmArden) {
			helmetSr.sprite = helmArden;
		}
		else if(npc.helmetHat == NPCHelmet.HelmDarkSteel) {
			helmetSr.sprite = helmDarkSteel;
		}
		else if(npc.helmetHat == NPCHelmet.HelmNorgarnian) {
			helmetSr.sprite = helmNorgarnian;
		}
		else if(npc.helmetHat == NPCHelmet.HelmChronicler) {
			helmetSr.sprite = helmChronicler;
		}
		else if(npc.helmetHat == NPCHelmet.HatPriest) {
			helmetSr.sprite = hatPriest;
		}
		else if(npc.helmetHat == NPCHelmet.HoodBlack) {
			helmetSr.sprite = hoodPriest;
		}

		if(npc.outfit == NPCOutfit.None) {
			outfirSr.sprite = null;
		}
		else if(npc.outfit == NPCOutfit.GambesonGreen) {
			outfirSr.sprite = gambesonGreen;
		}
		else if(npc.outfit == NPCOutfit.GambesonRed) {
			outfirSr.sprite = gambesonRed;
		}
		else if(npc.outfit == NPCOutfit.CoatDark) {
			outfirSr.sprite = coatDark;
		}
		else if(npc.outfit == NPCOutfit.InkeeperGarments) {
			outfirSr.sprite = inkeeperGarment;
		}
		else if(npc.outfit == NPCOutfit.FurClothes) {
			outfirSr.sprite = furClothes;
		}
		else if(npc.outfit == NPCOutfit.RobeBlue) {
			outfirSr.sprite = robeBlue;
		}
		else if(npc.outfit == NPCOutfit.RobeLungum) {
			outfirSr.sprite = robeLungum;
		}
		else if(npc.outfit == NPCOutfit.RobeSinta) {
			outfirSr.sprite = robeSinta;
		}
		else if(npc.outfit == NPCOutfit.ArmorIron) {
			outfirSr.sprite = armorIron;
		}
		else if(npc.outfit == NPCOutfit.ArmorSteel) {
			outfirSr.sprite = armorSteel;
		}
		else if(npc.outfit == NPCOutfit.ArmorSilver) {
			outfirSr.sprite = armorSilver;
		}
		else if(npc.outfit == NPCOutfit.ArmorImperial) {
			outfirSr.sprite = armorImperial;
		}
		else if(npc.outfit == NPCOutfit.ArmorElven) {
			outfirSr.sprite = armorElven;
		}
		else if(npc.outfit == NPCOutfit.ArmorArden) {
			outfirSr.sprite = armorArden;
		}
		else if(npc.outfit == NPCOutfit.ArmorNorgarnian) {
			outfirSr.sprite = armorNorgarian;
		}
		else if(npc.outfit == NPCOutfit.ArmorDarkSteel) {
			outfirSr.sprite = armorDarkSteel;
		}
		else if(npc.outfit == NPCOutfit.ArmorChronicler) {
			outfirSr.sprite = armorChronicler;
		}
		else if(npc.outfit == NPCOutfit.RobePriest) {
			outfirSr.sprite = robePriest;
		}
        else if (npc.outfit == NPCOutfit.DressBrown) {
            outfirSr.sprite = dressBrown;
        }
        else if (npc.outfit == NPCOutfit.DressBlue) {
            outfirSr.sprite = dressBlue;
        }
        else if (npc.outfit == NPCOutfit.DressGreen) {
            outfirSr.sprite = dressGreen;
        }

        if (npc.weapon == NPCWeapon.None) {
			weaponSr.sprite = null;
		}
		else {
			weaponSr.sprite = weapons[(int)npc.weapon];
		}

		if(npc.shield == NPCShield.None) {
			shieldSr.sprite = null;
		}
		else {
			shieldSr.sprite = shields[(int)npc.shield];
		}

		hairSr.color = npc.hairColor;
		facialHairSr.color = npc.hairColor;
	}
}