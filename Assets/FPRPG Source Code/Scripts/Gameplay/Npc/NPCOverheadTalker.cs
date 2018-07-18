using UnityEngine;
using System.Collections;

public class NPCOverheadTalker : MonoBehaviour {

	private TextMesh overheadTxtMesh;
	private string textToShow;
	private float textTimer;

	private TextOutline textOutline;
	private TodWeather tw;
	private GameObject playerGo;

	private float distance;
	public bool outlineTurner = false;

    void Start() {
        if (transform.parent.transform.parent.name.Contains("ANIMAL") || transform.parent.transform.parent.name.Contains("Beast")) {
            gameObject.SetActive(false);
        }

        outlineTurner = true;
        overheadTxtMesh = GetComponent<TextMesh>();
        textOutline = GetComponentInChildren<TextOutline>();
        tw = GameObject.Find("[GameManager]").GetComponent<TodWeather>();
        playerGo = GameObject.Find("[Player]");

    }

        void Update() {
		if (textTimer > 0f && textToShow != "") {
			textTimer -= Time.deltaTime / (tw.secondsInFullDay / 1200f);

			if (overheadTxtMesh.text != textToShow) {
				overheadTxtMesh.text = textToShow;
			}
		} 
		else if (textTimer <= 0f) {
			overheadTxtMesh.text = "";
			textToShow = "";
		}

		distance = Vector3.Distance (transform.position, playerGo.transform.position);

		if (distance < 10f) {
			GetComponent<MeshRenderer> ().enabled = true;
			if(outlineTurner == false && textOutline.outlines.Count > 0) {
				foreach(GameObject go in textOutline.outlines) {
					go.GetComponent<MeshRenderer>().enabled = true;
				}
				outlineTurner = true;
			}
		} 
		else {
			GetComponent<MeshRenderer> ().enabled = false;
			if(outlineTurner == true && textOutline.outlines.Count > 0) {
				foreach(GameObject go in textOutline.outlines) {
					go.GetComponent<MeshRenderer>().enabled = false;
				}
				outlineTurner = false;
			}
		}
	}

	public void NpcTalk(string text) {
		textTimer = 5f;
        text = text.Replace("\\n", "\n");
		textToShow = text;
	}
}
