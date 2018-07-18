using UnityEngine;
using System.Collections;

public class BookManager : MonoBehaviour {

	public string bookTitle;
	public string bookAuthor;
	[TextArea(1, 10)]
	public string bookContent;
	[Space(10f)]
	public BookSkillUpgrade skillUpgrade;

	void Start() {
		if(bookTitle == "") {
			bookTitle = "Unknown volume";
		}
		if(bookAuthor == "") {
			bookAuthor = "Unknown author";
		}
		if(bookContent == "") {
			bookContent = "'The pages of the books has been ruined. You cant even read a word.'";
		}
	}
}
