using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum BookSkillUpgrade {
	None = 0,
	LightArmor = 1,
	HeavyArmor = 2,
	Melee = 3,
	Marskman = 4,
	Block = 5,
	Focus = 6,
	Alchemy = 7,
	Acrobatics = 8,
	Speechcraft = 9,
}

public class BookUi : MonoBehaviour {

	public Text bookTitle;
	public Text bookAuthor;
	public Text bookContent;
	public ScrollRect bookScrollrect;
	[Space(10f)]
	public BookSkillUpgrade skillUpgrade;
	[Space(10f)]
	public PlayerSkillsAndAttributes skillsAttributes;
	public ShowMessage showMessage;

	void Start() {
		skillsAttributes = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSkillsAndAttributes>();
		showMessage = GameObject.Find("_UICanvasGame").GetComponentInChildren<ShowMessage>();
	}

	public void BookClearContent() {
		bookTitle.text = string.Empty;
		bookAuthor.text = string.Empty;
		bookContent.text = string.Empty;
		bookScrollrect.verticalNormalizedPosition = 1;
		skillUpgrade = BookSkillUpgrade.None;
	}

	public void BookUpdateContent(BookManager bookManager) {
		bookTitle.text = bookManager.bookTitle;
		bookAuthor.text =  "by " + bookManager.bookAuthor;
		bookContent.text = bookManager.bookContent;
		skillUpgrade = bookManager.skillUpgrade;

		BookUpgradeSkill();
	}

	public void BookUpgradeSkill() {
		if(skillUpgrade == BookSkillUpgrade.LightArmor) {
			skillsAttributes.lightArmor++;
			showMessage.SendTheMessage("Light Armor improved by 1 point.");
		}
		else if(skillUpgrade == BookSkillUpgrade.HeavyArmor) {
			skillsAttributes.heavyArmor++;
			showMessage.SendTheMessage("Heavy Armor improved by 1 point.");
		}
		else if(skillUpgrade == BookSkillUpgrade.Melee) {
			skillsAttributes.melee++;
			showMessage.SendTheMessage("Melee improved by 1 point.");
		}
		else if(skillUpgrade == BookSkillUpgrade.Marskman) {
			skillsAttributes.marksman++;
			showMessage.SendTheMessage("Marskman improved by 1 point.");
		}
		else if(skillUpgrade == BookSkillUpgrade.Block) {
			skillsAttributes.block++;
			showMessage.SendTheMessage("Block improved by 1 point.");
		}
		else if(skillUpgrade == BookSkillUpgrade.Focus) {
			skillsAttributes.heavyArmor++;
			showMessage.SendTheMessage("Focus improved by 1 point.");
		}
		else if(skillUpgrade == BookSkillUpgrade.Alchemy) {
			skillsAttributes.alchemy++;
			showMessage.SendTheMessage("Alchemy improved by 1 point.");
		}
		else if(skillUpgrade == BookSkillUpgrade.Acrobatics) {
			skillsAttributes.acrobatics++;
			showMessage.SendTheMessage("Acrobatics improved by 1 point.");
		}
		else if(skillUpgrade == BookSkillUpgrade.Speechcraft) {
			skillsAttributes.speechcraft++;
			showMessage.SendTheMessage("Speechcraft improved by 1 point.");
		}
	}
}
