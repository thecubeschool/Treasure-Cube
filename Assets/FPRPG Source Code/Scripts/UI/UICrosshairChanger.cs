using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum CrosshairState {
	Normal = 0,
	Talk = 1,
	Open = 2,
	Loot = 3,
	Mount = 4,
	Read = 5,
	Info = 6,
}
public class UICrosshairChanger : MonoBehaviour {

	private Image img;

	[Space(5f)]
	public CrosshairState crosshairState;
	[Space(5f)]
	public Sprite normalSp;
	public Sprite talkSp;
	public Sprite openSp;
	public Sprite lootSp;
	public Sprite mountSp;
	public Sprite readSp;
	public Sprite infoSp;

	public bool commitCrime = false;

	void Start() {
		img = GetComponent<Image>();
	}

	void Update () {

		if(commitCrime == true) {
			img.color = Color.red;
		}
		else {
			img.color = Color.white;
		}

		if(crosshairState == CrosshairState.Talk) {
			img.sprite = talkSp;
		}
		else if(crosshairState == CrosshairState.Open) {
			img.sprite = openSp;
		}
		else if(crosshairState == CrosshairState.Loot) {
			img.sprite = lootSp;
		}
		else if(crosshairState == CrosshairState.Normal) {
			img.sprite = normalSp;
		}
		else if(crosshairState == CrosshairState.Mount) {
			img.sprite = mountSp;
		}
		else if(crosshairState == CrosshairState.Read) {
			img.sprite = readSp;
		}
		else if(crosshairState == CrosshairState.Info) {
			img.sprite = infoSp;
		}
	}
}
