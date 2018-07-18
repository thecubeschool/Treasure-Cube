using UnityEngine;
using System.Collections;

public class NewCharacterSetup : MonoBehaviour {

	public string characterName;
	public CharacterGender characterGender;
	public CharacterRace characterRace;
	public CharacterProtector characterProtector;
	public CharacterCulture characterCulture;
	public CharacterProfession characterProfession;

	public int hairIndex;
	public int facialhairIndex;
	public Color hairColor;

	public bool characterTransfered = false;
	private bool inventoryDone = false;

    private StartingGear startingGear;

    private void Start() {
        DontDestroyOnLoad(this.gameObject);
    }

    void Update() {
		if (characterTransfered == true) {
			if(inventoryDone == false) {
                if (startingGear == null) {
                    startingGear = GameObject.FindObjectOfType<StartingGear>();
                    startingGear.firstStartup = true;
                }
                inventoryDone = true;
            }
		}
	}
}
