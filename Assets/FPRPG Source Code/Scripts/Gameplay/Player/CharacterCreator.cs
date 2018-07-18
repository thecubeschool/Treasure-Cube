using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public enum CharacterGender {
	Male = 0,
	Female = 1,
}
public enum CharacterRace {
	Elinian = 0,
	Ariyan = 1,
	Sintarian = 2,
	Elevirian = 3,
	Koronian = 4,
	Skeleton = 5,
	Werewolf = 6,
	Bauk = 7,
	BeastWolf = 8,
    AnimalDeer = 9,
}

public enum CharacterProtector {
	Ilir = 0,
	Yaves = 1,
	Uthar = 2,
	Wachilass = 3,
	Niminia = 4,
	Aranatha = 5,
}

public enum CharacterCulture {
	Empire = 0,
	Ariya = 1,
	Senika = 2,
	Sintar = 3,
	Korona = 4,
	Elevir = 5,
}

public enum CharacterProfession {
	Warrior = 0,
	Knight = 1,
	Cleric = 2,
	Hunter = 3,
	Agent = 4,
	Ranger = 5,
}

public class CharacterCreator : MonoBehaviour {

	public string characterName;
	public CharacterGender characterGender;
	public CharacterRace characterRace;
	public CharacterProtector characterProtector;
	public CharacterCulture characterCulture;
	public CharacterProfession characterProfession;

	public int raceIndex;
	private int oldRaceIndex;
	private string currRaceState;
    public int genderIndex;
    private int oldGenderIndex;
	public int outfitIndex;
	private int oldOutfitIndex;
	private string currOutfitState;
	public int hairIndex;
	private int oldHairIndex;
	private string currHairState;
	public int facialhairIndex;
	private int oldFacialhairIndex;
	private string currFaciahairState;
	public Color hairColor;

	[Space(20f)]
	public Animator outfitAnim;
	public Animator bodyAnim;
	public Animator hairAnim;
	private SpriteRenderer hairSR;
	public Animator facialhairAnim;
	private SpriteRenderer facialhairSR;
	private Material colorMaterial;

	[Space(20f)]
	public InputField nameInput;
	public Slider hairSlider;
	public Slider facialhairSlider;
	public Text raceDescBox;
	public Text protectorDescBox;	
	public Text cultureDescBox;	
	public Text appearanceDescBox;	
	public Text skillsDescBox;

	private Button genderSel;
	private Button raceSel;
	private Button protectorSel;
	private Button cultureSel;
	private Button skillsSel;

	public Sprite btnNormal;
	public Sprite btnSelected;

	private NewCharacterSetup ncs;

	[Space(10f)]
	public Image maleBtn;
	public Image femaleBtn;

	public Image elinianBtn;
	public Image ariyanBtn;
	public Image sintarianBtn;
	public Image koronianBtn;
	public Image elevitianBtn;

	public Image ilirBtn;
	public Image yavesBtn;
	public Image utharBtn;
	public Image wachilasBtn;
	public Image niminiaBtn;
	public Image aranathaBtn;

	public Image empireBtn;
	public Image ariyaBtn;
	public Image senikaBtn;
	public Image sintarBtn;
	public Image elevirBtn;
	public Image koronaBtn;
		
	public Image warriorBtn;
	public Image knightBtn;
	public Image clericBtn;
	public Image hunterBtn;
	public Image agentBtn;
	public Image rangerBtn;

	void Start() {

		ncs = GameObject.FindObjectOfType<NewCharacterSetup> ();

		hairSR = hairAnim.gameObject.GetComponent<SpriteRenderer> ();
		facialhairSR = facialhairAnim.gameObject.GetComponent<SpriteRenderer> ();

		colorMaterial = Instantiate(hairSR.material);

		hairSR.material = colorMaterial;
		facialhairSR.material = colorMaterial;

        /*
		characterRace = (CharacterRace)UnityEngine.Random.Range (0, Enum.GetValues (typeof(CharacterRace)).Length);
		characterProtector = (CharacterProtector)UnityEngine.Random.Range (0, Enum.GetValues (typeof(CharacterProtector)).Length);
		characterCulture = (CharacterCulture)UnityEngine.Random.Range (0, Enum.GetValues (typeof(CharacterCulture)).Length);
		characterProfession = (CharacterProfession)UnityEngine.Random.Range (0, Enum.GetValues (typeof(CharacterProfession)).Length);
        */

        characterGender = CharacterGender.Male;
        characterRace = CharacterRace.Elinian;
        characterProtector = CharacterProtector.Yaves;
        characterCulture = CharacterCulture.Empire;
        characterProfession = CharacterProfession.Knight;
        hairSlider.value = 3;
        facialhairSlider.value = 1;

        if (characterGender == CharacterGender.Male) {
			maleBtn.sprite = btnSelected;
			femaleBtn.sprite = btnNormal;
			genderSel = maleBtn.transform.GetComponent<Button> ();
		}
		else {
			maleBtn.sprite = btnNormal;
			femaleBtn.sprite = btnSelected;
			genderSel = femaleBtn.transform.GetComponent<Button> ();
		}

		if (characterRace == CharacterRace.Elinian) {
			elinianBtn.sprite = btnSelected;
			ariyanBtn.sprite = btnNormal;
			sintarianBtn.sprite = btnNormal;
			koronianBtn.sprite = btnNormal;
			elevitianBtn.sprite = btnNormal;
			raceSel = elinianBtn.transform.GetComponent<Button> ();
		} 
		else if (characterRace == CharacterRace.Ariyan) {
			elinianBtn.sprite = btnNormal;
			ariyanBtn.sprite = btnSelected;
			sintarianBtn.sprite = btnNormal;
			koronianBtn.sprite = btnNormal;
			elevitianBtn.sprite = btnNormal;
			raceSel = ariyanBtn.transform.GetComponent<Button> ();
		} 
		else if (characterRace == CharacterRace.Sintarian) {
			elinianBtn.sprite = btnNormal;
			ariyanBtn.sprite = btnNormal;
			sintarianBtn.sprite = btnSelected;
			koronianBtn.sprite = btnNormal;
			elevitianBtn.sprite = btnNormal;
			raceSel = sintarianBtn.transform.GetComponent<Button> ();
		} 
		else if (characterRace == CharacterRace.Koronian) {
			elinianBtn.sprite = btnNormal;
			ariyanBtn.sprite = btnNormal;
			sintarianBtn.sprite = btnNormal;
			koronianBtn.sprite = btnSelected;
			elevitianBtn.sprite = btnNormal;
			raceSel = koronianBtn.transform.GetComponent<Button> ();
		}
		else if (characterRace == CharacterRace.Elevirian) {
			elinianBtn.sprite = btnNormal;
			ariyanBtn.sprite = btnNormal;
			sintarianBtn.sprite = btnNormal;
			koronianBtn.sprite = btnNormal;
			elevitianBtn.sprite = btnSelected;
			raceSel = elevitianBtn.transform.GetComponent<Button> ();
		}

		if (characterProtector == CharacterProtector.Ilir) {
			ilirBtn.sprite = btnSelected;
			yavesBtn.sprite = btnNormal;
			utharBtn.sprite = btnNormal;
			wachilasBtn.sprite = btnNormal;
			niminiaBtn.sprite = btnNormal;
			aranathaBtn.sprite = btnNormal;
			protectorSel = ilirBtn.transform.GetComponent<Button> ();
		}
		else if (characterProtector == CharacterProtector.Yaves) {
			ilirBtn.sprite = btnNormal;
			yavesBtn.sprite = btnSelected;
			utharBtn.sprite = btnNormal;
			wachilasBtn.sprite = btnNormal;
			niminiaBtn.sprite = btnNormal;
			aranathaBtn.sprite = btnNormal;
			protectorSel = yavesBtn.transform.GetComponent<Button> ();
		}
		else if (characterProtector == CharacterProtector.Uthar) {
			ilirBtn.sprite = btnNormal;
			yavesBtn.sprite = btnNormal;
			utharBtn.sprite = btnSelected;
			wachilasBtn.sprite = btnNormal;
			niminiaBtn.sprite = btnNormal;
			aranathaBtn.sprite = btnNormal;
			protectorSel = utharBtn.transform.GetComponent<Button> ();
		}
		else if (characterProtector == CharacterProtector.Wachilass) {
			ilirBtn.sprite = btnNormal;
			yavesBtn.sprite = btnNormal;
			utharBtn.sprite = btnNormal;
			wachilasBtn.sprite = btnSelected;
			niminiaBtn.sprite = btnNormal;
			aranathaBtn.sprite = btnNormal;
			protectorSel = wachilasBtn.transform.GetComponent<Button> ();
		}
		else if (characterProtector == CharacterProtector.Niminia) {
			ilirBtn.sprite = btnNormal;
			yavesBtn.sprite = btnNormal;
			utharBtn.sprite = btnNormal;
			wachilasBtn.sprite = btnNormal;
			niminiaBtn.sprite = btnSelected;
			aranathaBtn.sprite = btnNormal;
			protectorSel = niminiaBtn.transform.GetComponent<Button> ();
		}
		else if (characterProtector == CharacterProtector.Aranatha) {
			ilirBtn.sprite = btnNormal;
			yavesBtn.sprite = btnNormal;
			utharBtn.sprite = btnNormal;
			wachilasBtn.sprite = btnNormal;
			niminiaBtn.sprite = btnNormal;
			aranathaBtn.sprite = btnSelected;
			protectorSel = aranathaBtn.transform.GetComponent<Button> ();
		}

		if (characterCulture == CharacterCulture.Empire) {
			empireBtn.sprite = btnSelected;
			ariyaBtn.sprite = btnNormal;
			senikaBtn.sprite = btnNormal;
			sintarBtn.sprite = btnNormal;
			koronaBtn.sprite = btnNormal;
			elevirBtn.sprite = btnNormal;
			cultureSel = empireBtn.transform.GetComponent<Button> ();
		}
		else if (characterCulture == CharacterCulture.Ariya) {
			empireBtn.sprite = btnNormal;
			ariyaBtn.sprite = btnSelected;
			senikaBtn.sprite = btnNormal;
			sintarBtn.sprite = btnNormal;
			koronaBtn.sprite = btnNormal;
			elevirBtn.sprite = btnNormal;
			cultureSel = ariyaBtn.transform.GetComponent<Button> ();
		}
		else if (characterCulture == CharacterCulture.Senika) {
			empireBtn.sprite = btnNormal;
			ariyaBtn.sprite = btnNormal;
			senikaBtn.sprite = btnSelected;
			sintarBtn.sprite = btnNormal;
			koronaBtn.sprite = btnNormal;
			elevirBtn.sprite = btnNormal;
			cultureSel = senikaBtn.transform.GetComponent<Button> ();
		}
		else if (characterCulture == CharacterCulture.Sintar) {
			empireBtn.sprite = btnNormal;
			ariyaBtn.sprite = btnNormal;
			senikaBtn.sprite = btnNormal;
			sintarBtn.sprite = btnSelected;
			koronaBtn.sprite = btnNormal;
			elevirBtn.sprite = btnNormal;
			cultureSel = sintarBtn.transform.GetComponent<Button> ();
		}
		else if (characterCulture == CharacterCulture.Korona) {
			empireBtn.sprite = btnNormal;
			ariyaBtn.sprite = btnNormal;
			senikaBtn.sprite = btnNormal;
			sintarBtn.sprite = btnNormal;
			koronaBtn.sprite = btnSelected;
			elevirBtn.sprite = btnNormal;
			cultureSel = koronaBtn.transform.GetComponent<Button> ();
		}
		else if (characterCulture == CharacterCulture.Elevir) {
			empireBtn.sprite = btnNormal;
			ariyaBtn.sprite = btnNormal;
			senikaBtn.sprite = btnNormal;
			sintarBtn.sprite = btnNormal;
			koronaBtn.sprite = btnNormal;
			elevirBtn.sprite = btnSelected;
			cultureSel = elevirBtn.transform.GetComponent<Button> ();
		}

		if (characterProfession == CharacterProfession.Warrior) {
			warriorBtn.sprite = btnSelected;
			knightBtn.sprite = btnNormal;
			clericBtn.sprite = btnNormal;
			hunterBtn.sprite = btnNormal;
			agentBtn.sprite = btnNormal;
			rangerBtn.sprite = btnNormal;
			skillsSel = warriorBtn.transform.GetComponent<Button> ();
		}
		else if (characterProfession == CharacterProfession.Knight) {
			warriorBtn.sprite = btnNormal;
			knightBtn.sprite = btnSelected;
			clericBtn.sprite = btnNormal;
			hunterBtn.sprite = btnNormal;
			agentBtn.sprite = btnNormal;
			rangerBtn.sprite = btnNormal;
			skillsSel = knightBtn.transform.GetComponent<Button> ();
		}
		else if (characterProfession == CharacterProfession.Cleric) {
			warriorBtn.sprite = btnNormal;
			knightBtn.sprite = btnNormal;
			clericBtn.sprite = btnSelected;
			hunterBtn.sprite = btnNormal;
			agentBtn.sprite = btnNormal;
			rangerBtn.sprite = btnNormal;
			skillsSel = clericBtn.transform.GetComponent<Button> ();
		}
		else if (characterProfession == CharacterProfession.Hunter) {
			warriorBtn.sprite = btnNormal;
			knightBtn.sprite = btnNormal;
			clericBtn.sprite = btnNormal;
			hunterBtn.sprite = btnSelected;
			agentBtn.sprite = btnNormal;
			rangerBtn.sprite = btnNormal;
			skillsSel = hunterBtn.transform.GetComponent<Button> ();
		}
		else if (characterProfession == CharacterProfession.Agent) {
			warriorBtn.sprite = btnNormal;
			knightBtn.sprite = btnNormal;
			clericBtn.sprite = btnNormal;
			hunterBtn.sprite = btnNormal;
			agentBtn.sprite = btnSelected;
			rangerBtn.sprite = btnNormal;
			skillsSel = agentBtn.transform.GetComponent<Button> ();
		}
		else if (characterProfession == CharacterProfession.Ranger) {
			warriorBtn.sprite = btnNormal;
			knightBtn.sprite = btnNormal;
			clericBtn.sprite = btnNormal;
			hunterBtn.sprite = btnNormal;
			agentBtn.sprite = btnNormal;
			rangerBtn.sprite = btnSelected;
			skillsSel = rangerBtn.transform.GetComponent<Button> ();
		}
	}

	void Update() {

		characterName = nameInput.text;

        if (characterGender == CharacterGender.Male) {
            bodyAnim.SetBool("male", true);
            bodyAnim.SetBool("female", false);
            genderIndex = 1;

            if (facialhairSlider.gameObject.activeSelf == false) {
                facialhairSlider.gameObject.SetActive(true);
            }
        }
        else if (characterGender == CharacterGender.Female) {
            bodyAnim.SetBool("male", false);
            bodyAnim.SetBool("female", true);
            genderIndex = 2;

            facialhairSlider.value = 0;
            if (facialhairSlider.gameObject.activeSelf == true) {
                facialhairSlider.gameObject.SetActive(false);
            }
        }

        if (characterRace == CharacterRace.Elinian) {
            bodyAnim.SetBool("Elinian", true);
			bodyAnim.SetBool("Arian", false);
			bodyAnim.SetBool("Sintarian", false);
			bodyAnim.SetBool("Elevirian", false);
			bodyAnim.SetBool("Koronian", false);
			raceIndex = 1;
		}
		else if(characterRace == CharacterRace.Ariyan) {
			bodyAnim.SetBool("Elinian", false);
			bodyAnim.SetBool("Arian", true);
			bodyAnim.SetBool("Sintarian", false);
			bodyAnim.SetBool("Elevirian", false);
			bodyAnim.SetBool("Koronian", false);
			raceIndex = 2;
		}
		else if(characterRace == CharacterRace.Sintarian) {
			bodyAnim.SetBool("Elinian", false);
			bodyAnim.SetBool("Arian", false);
			bodyAnim.SetBool("Sintarian", true);
			bodyAnim.SetBool("Elevirian", false);
			bodyAnim.SetBool("Koronian", false);
			raceIndex = 3;
		}
		else if(characterRace == CharacterRace.Elevirian) {
			bodyAnim.SetBool("Elinian", false);
			bodyAnim.SetBool("Arian", false);
			bodyAnim.SetBool("Sintarian", false);
			bodyAnim.SetBool("Elevirian", true);
			bodyAnim.SetBool("Koronian", false);
			raceIndex = 4;
		}
		else if(characterRace == CharacterRace.Koronian) {
			bodyAnim.SetBool("Elinian", false);
			bodyAnim.SetBool("Arian", false);
			bodyAnim.SetBool("Sintarian", false);
			bodyAnim.SetBool("Elevirian", false);
			bodyAnim.SetBool("Koronian", true);
			raceIndex = 5;
		}

		if(characterProfession == CharacterProfession.Warrior) {
			outfitAnim.SetBool("GambesonRed", false);
			outfitAnim.SetBool("GambesonGreen", false);
			outfitAnim.SetBool("ArmorIron", true);
			outfitAnim.SetBool("CoatDark", false);
			outfitAnim.SetBool("RobePriest", false);
			outfitAnim.SetBool("RobeLungum", false);
			outfitIndex = 1;
		}
		else if(characterProfession == CharacterProfession.Knight) {
			outfitAnim.SetBool("GambesonRed", false);
			outfitAnim.SetBool("GambesonGreen", false);
			outfitAnim.SetBool("ArmorIron", false);
			outfitAnim.SetBool("CoatDark", true);
			outfitAnim.SetBool("RobePriest", false);
			outfitAnim.SetBool("RobeLungum", false);
			outfitIndex = 2;
		}
		else if(characterProfession == CharacterProfession.Cleric) {
			outfitAnim.SetBool("GambesonRed", false);
			outfitAnim.SetBool("GambesonGreen", false);
			outfitAnim.SetBool("ArmorIron", false);
			outfitAnim.SetBool("CoatDark", false);
			outfitAnim.SetBool("RobePriest", true);
			outfitAnim.SetBool("RobeLungum", false);
			outfitIndex = 3;
		}
		else if(characterProfession == CharacterProfession.Hunter) {
			outfitAnim.SetBool("GambesonRed", false);
			outfitAnim.SetBool("GambesonGreen", true);
			outfitAnim.SetBool("ArmorIron", false);
			outfitAnim.SetBool("CoatDark", false);
			outfitAnim.SetBool("RobePriest", false);
			outfitAnim.SetBool("RobeLungum", false);
			outfitIndex = 4;
		}
		else if(characterProfession == CharacterProfession.Agent) {
			outfitAnim.SetBool("GambesonRed", false);
			outfitAnim.SetBool("GambesonGreen", false);
			outfitAnim.SetBool("ArmorIron", false);
			outfitAnim.SetBool("CoatDark", true);
			outfitAnim.SetBool("RobePriest", false);
			outfitAnim.SetBool("RobeLungum", false);
			outfitIndex = 5;
		}
		else if(characterProfession == CharacterProfession.Ranger) {
			outfitAnim.SetBool("GambesonRed", true);
			outfitAnim.SetBool("GambesonGreen", false);
			outfitAnim.SetBool("ArmorIron", false);
			outfitAnim.SetBool("CoatDark", false);
			outfitAnim.SetBool("RobePriest", false);
			outfitAnim.SetBool("RobeLungum", false);
			outfitIndex = 6;
		}

		if(hairSlider.value == 1) {
			hairAnim.SetBool("Bald", true);
			hairAnim.SetBool("Short Hair", false);
			hairAnim.SetBool("Long Hair", false);
			hairAnim.SetBool("Cleantop", false);
			hairAnim.SetBool("Topfuzz", false);
            hairAnim.SetBool("TwistedHair", false);
            hairAnim.SetBool("BunHair", false);
            hairAnim.SetBool("StylizedHair", false);
            hairAnim.SetBool("PonytailHair", false);
            hairIndex = 1;
		}
		else if(hairSlider.value == 2) {
			hairAnim.SetBool("Bald", false);
			hairAnim.SetBool("Short Hair", true);
			hairAnim.SetBool("Long Hair", false);
			hairAnim.SetBool("Cleantop", false);
			hairAnim.SetBool("Topfuzz", false);
            hairAnim.SetBool("TwistedHair", false);
            hairAnim.SetBool("BunHair", false);
            hairAnim.SetBool("StylizedHair", false);
            hairAnim.SetBool("PonytailHair", false);
            hairIndex = 2;
		}
		else if(hairSlider.value == 3) {
			hairAnim.SetBool("Bald", false);
			hairAnim.SetBool("Short Hair", false);
			hairAnim.SetBool("Long Hair", true);
			hairAnim.SetBool("Cleantop", false);
			hairAnim.SetBool("Topfuzz", false);
            hairAnim.SetBool("TwistedHair", false);
            hairAnim.SetBool("BunHair", false);
            hairAnim.SetBool("StylizedHair", false);
            hairAnim.SetBool("PonytailHair", false);
            hairIndex = 3;
		}
		else if(hairSlider.value == 4) {
			hairAnim.SetBool("Bald", false);
			hairAnim.SetBool("Short Hair", false);
			hairAnim.SetBool("Long Hair", false);
			hairAnim.SetBool("Cleantop", true);
			hairAnim.SetBool("Topfuzz", false);
            hairAnim.SetBool("TwistedHair", false);
            hairAnim.SetBool("BunHair", false);
            hairAnim.SetBool("StylizedHair", false);
            hairAnim.SetBool("PonytailHair", false);
            hairIndex = 4;
		}
		else if(hairSlider.value == 5) {
			hairAnim.SetBool("Bald", false);
			hairAnim.SetBool("Short Hair", false);
			hairAnim.SetBool("Long Hair", false);
			hairAnim.SetBool("Cleantop", false);
			hairAnim.SetBool("Topfuzz", true);
            hairAnim.SetBool("TwistedHair", false);
            hairAnim.SetBool("BunHair", false);
            hairAnim.SetBool("StylizedHair", false);
            hairAnim.SetBool("PonytailHair", false);
            hairIndex = 5;
		}
        else if (hairSlider.value == 6) {
            hairAnim.SetBool("Bald", false);
            hairAnim.SetBool("Short Hair", false);
            hairAnim.SetBool("Long Hair", false);
            hairAnim.SetBool("Cleantop", false);
            hairAnim.SetBool("Topfuzz", false);
            hairAnim.SetBool("TwistedHair", true);
            hairAnim.SetBool("BunHair", false);
            hairAnim.SetBool("StylizedHair", false);
            hairAnim.SetBool("PonytailHair", false);
            hairIndex = 6;
        }
        else if (hairSlider.value == 7) {
            hairAnim.SetBool("Bald", false);
            hairAnim.SetBool("Short Hair", false);
            hairAnim.SetBool("Long Hair", false);
            hairAnim.SetBool("Cleantop", false);
            hairAnim.SetBool("Topfuzz", false);
            hairAnim.SetBool("TwistedHair", false);
            hairAnim.SetBool("BunHair", true);
            hairAnim.SetBool("StylizedHair", false);
            hairAnim.SetBool("PonytailHair", false);
            hairIndex = 7;
        }
        else if (hairSlider.value == 8) {
            hairAnim.SetBool("Bald", false);
            hairAnim.SetBool("Short Hair", false);
            hairAnim.SetBool("Long Hair", false);
            hairAnim.SetBool("Cleantop", false);
            hairAnim.SetBool("Topfuzz", false);
            hairAnim.SetBool("TwistedHair", false);
            hairAnim.SetBool("BunHair", false);
            hairAnim.SetBool("StylizedHair", true);
            hairAnim.SetBool("PonytailHair", false);
            hairIndex = 8;
        }
        else if (hairSlider.value == 9) {
            hairAnim.SetBool("Bald", false);
            hairAnim.SetBool("Short Hair", false);
            hairAnim.SetBool("Long Hair", false);
            hairAnim.SetBool("Cleantop", false);
            hairAnim.SetBool("Topfuzz", false);
            hairAnim.SetBool("TwistedHair", false);
            hairAnim.SetBool("BunHair", false);
            hairAnim.SetBool("StylizedHair", false);
            hairAnim.SetBool("PonytailHair", true);
            hairIndex = 9;
        }

        if (facialhairSlider.value == 0) { //Facial hair must begin with 0 upwards, while hair should begin with 2 upward
			facialhairAnim.SetBool("None", true);
			facialhairAnim.SetBool("Goatee", false);
			facialhairAnim.SetBool("Beard", false);
			facialhairAnim.SetBool("LongBeard", false);
			facialhairAnim.SetBool("ShortMoustache", false);
			facialhairAnim.SetBool("BigMoustache", false);
            facialhairAnim.SetBool("MoustacheNSideburns", false);
            facialhairAnim.SetBool("Sideburns", false);
            facialhairAnim.SetBool("LowerGoatee", false);
            facialhairIndex = 0;
		}
		else if(facialhairSlider.value == 1) {
			facialhairAnim.SetBool("None", false);
			facialhairAnim.SetBool("Goatee", true);
			facialhairAnim.SetBool("Beard", false);
			facialhairAnim.SetBool("LongBeard", false);
			facialhairAnim.SetBool("ShortMoustache", false);
			facialhairAnim.SetBool("BigMoustache", false);
			facialhairAnim.SetBool("MoustacheNSideburns", false);
            facialhairAnim.SetBool("Sideburns", false);
            facialhairAnim.SetBool("LowerGoatee", false);
            facialhairIndex = 1;
		}
		else if(facialhairSlider.value == 2) {
			facialhairAnim.SetBool("None", false);
			facialhairAnim.SetBool("Goatee", false);
			facialhairAnim.SetBool("Beard", true);
			facialhairAnim.SetBool("LongBeard", false);
			facialhairAnim.SetBool("ShortMoustache", false);
			facialhairAnim.SetBool("BigMoustache", false);
			facialhairAnim.SetBool("MoustacheNSideburns", false);
            facialhairAnim.SetBool("Sideburns", false);
            facialhairAnim.SetBool("LowerGoatee", false);
            facialhairIndex = 2;
		}
		else if(facialhairSlider.value == 3) {
			facialhairAnim.SetBool("None", false);
			facialhairAnim.SetBool("Goatee", false);
			facialhairAnim.SetBool("Beard", false);
			facialhairAnim.SetBool("LongBeard", true);
			facialhairAnim.SetBool("ShortMoustache", false);
			facialhairAnim.SetBool("BigMoustache", false);
			facialhairAnim.SetBool("MoustacheNSideburns", false);
            facialhairAnim.SetBool("Sideburns", false);
            facialhairAnim.SetBool("LowerGoatee", false);
            facialhairIndex = 3;
		}
		else if(facialhairSlider.value == 4) {
			facialhairAnim.SetBool("None", false);
			facialhairAnim.SetBool("Goatee", false);
			facialhairAnim.SetBool("Beard", false);
			facialhairAnim.SetBool("LongBeard", false);
			facialhairAnim.SetBool("ShortMoustache", true);
			facialhairAnim.SetBool("BigMoustache", false);
			facialhairAnim.SetBool("MoustacheNSideburns", false);
            facialhairAnim.SetBool("Sideburns", false);
            facialhairAnim.SetBool("LowerGoatee", false);
            facialhairIndex = 4;
		}
		else if(facialhairSlider.value == 5) {
			facialhairAnim.SetBool("None", false);
			facialhairAnim.SetBool("Goatee", false);
			facialhairAnim.SetBool("Beard", false);
			facialhairAnim.SetBool("LongBeard", false);
			facialhairAnim.SetBool("ShortMoustache", false);
			facialhairAnim.SetBool("BigMoustache", true);
			facialhairAnim.SetBool("MoustacheNSideburns", false);
            facialhairAnim.SetBool("Sideburns", false);
            facialhairAnim.SetBool("LowerGoatee", false);
            facialhairIndex = 5;
		}
		else if(facialhairSlider.value == 6) {
			facialhairAnim.SetBool("None", false);
			facialhairAnim.SetBool("Goatee", false);
			facialhairAnim.SetBool("Beard", false);
			facialhairAnim.SetBool("LongBeard", false);
			facialhairAnim.SetBool("ShortMoustache", false);
			facialhairAnim.SetBool("BigMoustache", false);
			facialhairAnim.SetBool("MoustacheNSideburns", true);
            facialhairAnim.SetBool("Sideburns", false);
            facialhairAnim.SetBool("LowerGoatee", false);
            facialhairIndex = 6;
        }
        else if (facialhairSlider.value == 7) {
            facialhairAnim.SetBool("None", false);
            facialhairAnim.SetBool("Goatee", false);
            facialhairAnim.SetBool("Beard", false);
            facialhairAnim.SetBool("LongBeard", false);
            facialhairAnim.SetBool("ShortMoustache", false);
            facialhairAnim.SetBool("BigMoustache", false);
            facialhairAnim.SetBool("MoustacheNSideburns", false);
            facialhairAnim.SetBool("Sideburns", true);
            facialhairAnim.SetBool("LowerGoatee", false);
            facialhairIndex = 7;
        }
        else if (facialhairSlider.value == 8) {
            facialhairAnim.SetBool("None", false);
            facialhairAnim.SetBool("Goatee", false);
            facialhairAnim.SetBool("Beard", false);
            facialhairAnim.SetBool("LongBeard", false);
            facialhairAnim.SetBool("ShortMoustache", false);
            facialhairAnim.SetBool("BigMoustache", false);
            facialhairAnim.SetBool("MoustacheNSideburns", false);
            facialhairAnim.SetBool("Sideburns", false);
            facialhairAnim.SetBool("LowerGoatee", true);
            facialhairIndex = 8;
        }

        if (genderIndex != oldGenderIndex || outfitIndex != oldOutfitIndex || raceIndex != oldRaceIndex || hairIndex != oldHairIndex || facialhairIndex != oldFacialhairIndex) {
			bodyAnim.SetBool("Elinian", false);
			bodyAnim.SetBool("Arian", false);
			bodyAnim.SetBool("Sintarian", false);
			bodyAnim.SetBool("Elevirian", false);
			bodyAnim.SetBool("Koronian", false);

			outfitAnim.SetBool("GambesonRed", false);
			outfitAnim.SetBool("GambesonGreen", false);
			outfitAnim.SetBool("ArmorIron", false);
			outfitAnim.SetBool("CoatDark", false);
			outfitAnim.SetBool("RobePriest", false);
			outfitAnim.SetBool("RobeLungum", false);

			hairAnim.SetBool("Bald", false);
			hairAnim.SetBool("Short Hair", false);
			hairAnim.SetBool("Long Hair", false);
			hairAnim.SetBool("Cleantop", false);
			hairAnim.SetBool("Topfuzz", false);
            hairAnim.SetBool("TwistedHair", false);
            hairAnim.SetBool("BunHair", false);
            hairAnim.SetBool("StylizedHair", false);
            hairAnim.SetBool("PonytailHair", false);

            facialhairAnim.SetBool("None", false);
			facialhairAnim.SetBool("Goatee", false);
			facialhairAnim.SetBool("Beard", false);
			facialhairAnim.SetBool("LongBeard", false);
			facialhairAnim.SetBool("ShortMoustache", false);
			facialhairAnim.SetBool("BigMoustache", false);
			facialhairAnim.SetBool("MoustacheNSideburns", false);
            facialhairAnim.SetBool("Sideburns", false);
            facialhairAnim.SetBool("LowerGoatee", false);
            /*
			if(characterRace == CharacterRace.Elinian) {
				bodyAnim.SetBool("Elinian", true);
			}
			else if(characterRace == CharacterRace.Ariyan) {
				bodyAnim.SetBool("Arian", true);
			}
			else if(characterRace == CharacterRace.Sintarian) {
				bodyAnim.SetBool("Sintarian", true);
			}
			else if(characterRace == CharacterRace.Koronian) {
				bodyAnim.SetBool("Koronian", true);
			}
			else if(characterRace == CharacterRace.Elevirian) {
				bodyAnim.SetBool("Elevirian", true);
			}

			if(characterProfession == CharacterProfession.Warrior) {
				outfitAnim.SetBool("ArmorIron", true);
			}
			else if(characterProfession == CharacterProfession.Knight) {
				outfitAnim.SetBool("CoatDark", true);
			}
			else if(characterProfession == CharacterProfession.Cleric) {
				outfitAnim.SetBool("RobePriest", true);
			}
			else if(characterProfession == CharacterProfession.Hunter) {
				outfitAnim.SetBool("GambesonGreen", true);
			}
			else if(characterProfession == CharacterProfession.Agent) {
				outfitAnim.SetBool("CoatDark", true);
			}
			else if(characterProfession == CharacterProfession.Ranger) {
				outfitAnim.SetBool("GambesonRed", true);
			}

			if(hairSlider.value == 1) {
				hairAnim.SetBool("Bald", true);
			}
			else if(hairSlider.value == 2) {
				hairAnim.SetBool("Short Hair", true);
			}
			else if(hairSlider.value == 3) {
				hairAnim.SetBool("Long Hair", true);
			}
			else if(hairSlider.value == 4) {
				hairAnim.SetBool("Cleantop", true);
			}
			else if(hairSlider.value == 5) {
				hairAnim.SetBool("Topfuzz", true);
			}

			if(facialhairSlider.value == 1) {
				facialhairAnim.SetBool("None", true);
			}
			else if(facialhairSlider.value == 2) {
				facialhairAnim.SetBool("Goatee", true);
			}
			else if(facialhairSlider.value == 3) {
				facialhairAnim.SetBool("Beard", true);
			}
			else if(facialhairSlider.value == 4) {
				facialhairAnim.SetBool("LongBeard", true);
			}
			else if(facialhairSlider.value == 5) {
				facialhairAnim.SetBool("ShortMoustache", true);
			}
			else if(facialhairSlider.value == 6) {
				facialhairAnim.SetBool("BigMoustache", true);
			}
			else if(facialhairSlider.value == 7) {
				facialhairAnim.SetBool("MoustacheNSideburns", true);
			}
*/
            oldGenderIndex = genderIndex;
			oldOutfitIndex = outfitIndex;
			oldRaceIndex = raceIndex;
			oldHairIndex = hairIndex;
			oldFacialhairIndex = facialhairIndex;
		}
	}

	public void GenderBtnSelected(Image img) {
		if (genderSel != img.transform.GetComponent<Button> ()) {
			img.sprite = btnSelected;
			if(genderSel != null) {
				genderSel.GetComponent<Image>().sprite = btnNormal;
			}
			genderSel = img.transform.GetComponent<Button> ();
		}
	}

	public void GenderSelected(string gender) {
		if(gender == "male") {
			characterGender = CharacterGender.Male;
		}
		if(gender == "female") {
			characterGender = CharacterGender.Female;
		}
	}

	public void RaceBtnSelected(Image img) {
		if (raceSel != img.transform.GetComponent<Button> ()) {
			img.sprite = btnSelected;
			if(img.transform.name.Contains("Elinian")) {
				raceDescBox.text = "Proud citizens of Elin are one of the finest people of Talimira. In the past a people inflamed by conquest and war, now they are enjoying their normal everyday lifes, finding joy in even the smallest of things.";
			}
			else if(img.transform.name.Contains("Ariyan")) {
				raceDescBox.text = "Descendants of the first man that settled in Talimira, these strong warriors are the personification of the bravest of heroes. They are customed to long and dark winter nights, prepeared for every battle that life can get them into.";
			}
			else if(img.transform.name.Contains("Sintarian")) {
				raceDescBox.text = "Thousand of years ago some of the Elinian folk migrated east, at the doorstep of Korona. In time they will build great free state of Sinta and the burning rays of the desert sun darkened their skin. They are proud followers and protectors of the Holy Church.";
			}
			else if(img.transform.name.Contains("Elevirian")) {
				raceDescBox.text = "The Golden Ones as they prefer to call themselves, are relatives to Koronian elves, but in no manner can one see similarity be it in body or mind. They represent themselves as highest among all races and the one and trues citizens of the Talimira. They are most hated among other races.";
			}
			else if(img.transform.name.Contains("Koronian")) {
				raceDescBox.text = "A long lost relatives of the High Elven race, Koronians or Dark Elves are people filled with magic and mysticism. They are praised for their knowledge, but hated for their secrets. They have shown as allias to the humands in the past and so have earn their friendship.";
			}
			if(raceSel != null) {
				raceSel.GetComponent<Image>().sprite = btnNormal;
			}
			raceSel = img.transform.GetComponent<Button> ();
		}
	}

	public void RaceSelected(string race) {
		if(race == "elinian") {
			characterRace = CharacterRace.Elinian;
		}
		if(race == "ariyan") {
			characterRace = CharacterRace.Ariyan;
		}
		if(race == "sintarian") {
			characterRace = CharacterRace.Sintarian;
		}
		if(race == "elevirian") {
			characterRace = CharacterRace.Elevirian;
		}
		if(race == "koronian") {
			characterRace = CharacterRace.Koronian;
		}
	}

	public void ProtectorBtnSelected(Image img) {
		if (protectorSel != img.transform.GetComponent<Button> ()) {
			img.sprite = btnSelected;
			if(protectorSel != null) {
				protectorSel.GetComponent<Image>().sprite = btnNormal;
			}
			protectorSel = img.transform.GetComponent<Button> ();
		}
	}

	public void CultureBtnSelected(Image img) {
		if (cultureSel != img.transform.GetComponent<Button> ()) {
			img.sprite = btnSelected;
			if(img.transform.name.Contains("Empire")) {
				cultureDescBox.text = "You hail from the wide green plains of the Empire, a vast and open country that lay in the lands of Elin. The people of the Empire are known as good swordmasters and orators, while holding to their firm and strong faith in the Holy Church and Lauragast, God of everything.";
			}
			else if(img.transform.name.Contains("Ariya")) {
				cultureDescBox.text = "Cold high peaks of Ariya are your home. You, as your kinsman, are accustomed to the harsh winter weather of the North. The feel of blood on your hands and the rage of battle is not unkown to you. You grew up among the most noble of all.";
			}
			else if(img.transform.name.Contains("Senika")) {
				cultureDescBox.text = "Basin of Senika, weast of Ariya, is your homeland. You grew up in the family devoted to the cult of Dark Lord Utalir. All your life you spent learning about your dark religion, but know you changed course and started the journey of cleansing yourself.";
			}
			else if(img.transform.name.Contains("Sintar")) {
				cultureDescBox.text = "The dark skinned warriors of Sintar are the most fine and noble warriors in the whole world. You grew up as one of them, far beyond the Great Pass surrounded by the hot sands of Korona to the east and high and sharp mountains of the South.";
			}
			else if(img.transform.name.Contains("Elevir")) {
				cultureDescBox.text = "Dense forests of Elevir were your home. You grew up surrounded by the goldenskinned elves that praise themselves to the heavens. All your life you were surrounded by those who thought to be in higher status than you, even though you were totally equal in every way.";
			}
			else if(img.transform.name.Contains("Korona")) {
				cultureDescBox.text = "Hot sands of Korona feeded you, while the wild magic of the dark elves made you a man. The first word you mutter as a child was a incantation stronger than any other can say, for the people of Koronian free cities are most gifted in the arts of alchemy and spells.";
			}
			if(cultureSel != null) {
				cultureSel.GetComponent<Image>().sprite = btnNormal;
			}
			cultureSel = img.transform.GetComponent<Button> ();
		}
	}

	public void SkillsBtnSelected(Image img) {
		if (skillsSel != img.transform.GetComponent<Button> ()) {
			img.sprite = btnSelected;
			if(img.transform.name.Contains("Warrior")) {
				skillsDescBox.text = "Fearing not for the injury, warriors charge their enemies from the front. \n\n Melee skill +5 \n Block skill +10 \n Heavy Armor skill +5";
				characterProfession = CharacterProfession.Warrior;
			}
			else if(img.transform.name.Contains("Knight")) {
				skillsDescBox.text = "The most noble of all combatants, knights are a real embodiment of chivalry. \n\n Melee skill +10 \n Heavy Armor skill +5 \n Speechcraft skill +5";
				characterProfession = CharacterProfession.Knight;
			}
			else if(img.transform.name.Contains("Cleric")) {
				skillsDescBox.text = "Weilding the power to heal himself, cleric is ready to fight darkest of enemies. \n\n Melee skill +5 \n Light Armor skill +5 \n Alchemy skill +10";
				characterProfession = CharacterProfession.Cleric;
			}
			else if(img.transform.name.Contains("Hunter")) {
				skillsDescBox.text = "A marksman, adept at combat from the distance. \n\n Marksman skill +10 \n Light Armor skill +5 \n Alchemy skill +5";
				characterProfession = CharacterProfession.Hunter;
			}
			else if(img.transform.name.Contains("Agent")) {
				skillsDescBox.text = "Nearly invinsible in the shadow, but surely deadly in face to face. \n\n Acrobatics skill +10 \n Marksman skill +5 \n Light Armor skill +5";
				characterProfession = CharacterProfession.Agent;
			}
			else if(img.transform.name.Contains("Ranger")) {
				skillsDescBox.text = "Prefering wilderness over towns, they are nimble and able to protect themselves. \n\n Melee skill +5 \n Marksman skill +5 \n Focus skill +10";
				characterProfession = CharacterProfession.Ranger;
			}
			if(skillsSel != null) {
				skillsSel.GetComponent<Image>().sprite = btnNormal;			
			}
			skillsSel = img.transform.GetComponent<Button> ();
		}
	}

	public void NewCharacterSetupTransfer() {
		ncs.characterName = characterName;
		ncs.characterGender = characterGender;
		ncs.characterRace = characterRace;
		ncs.characterProtector = characterProtector;
		ncs.characterCulture = characterCulture;
		ncs.characterProfession = characterProfession;
		
		ncs.hairIndex = hairIndex;
		ncs.facialhairIndex = facialhairIndex;
		ncs.hairColor = hairColor;
	}

	public void StartNewGame(int sceneIndex) {
		StartCoroutine(StatGameDelay(sceneIndex));
	}

	IEnumerator StatGameDelay(int sceneIndex) {
		yield return new WaitForSeconds(3.0f);
        SceneManager.LoadSceneAsync(sceneIndex);
	}
}
