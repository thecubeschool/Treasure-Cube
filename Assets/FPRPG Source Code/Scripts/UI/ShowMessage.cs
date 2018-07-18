using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShowMessage : MonoBehaviour {

	private Text txt;
	private float countdown = 5f;
	private string messageToShow;

	void Start () {

		txt = GetComponent<Text>();

		if(txt == null) {
			Debug.Log("You forgot to set Text variable for in game message system.");
		}
	}

	void Update() {
		if(messageToShow != "") {
			if(countdown > 0f) {
				txt.text = messageToShow;
				countdown -= Time.deltaTime;
			}
			else {
				messageToShow = "";
				txt.text = "";
				countdown = 5f;
			}
		}
		else {
			if(countdown != 5f) {
				countdown = 5f;
			}
		}
	}

	public void SendTheMessage(string theMessage) {
		messageToShow = theMessage;
	}
}
