using UnityEngine;
using System.Collections;

public enum CurrentMenu {
	MainMenu = 0,
	CharacterCreation = 1,
	Loading = 2,
}

public class MainMenu : MonoBehaviour {

	public CurrentMenu activeMenu; 
	public Animator uiAnim;
	public Transform mainCamera;
	public Vector3 camMMPosition;
	public Vector3 camCCPosition;

	void Start() {
		activeMenu = CurrentMenu.MainMenu;
		uiAnim.SetBool("mainMenu", true);
		uiAnim.SetBool("characterCreation", false);
		uiAnim.SetBool ("loading", false);
	}

	void Update() {
		if (activeMenu == CurrentMenu.MainMenu) {
			mainCamera.position = Vector3.Lerp (mainCamera.transform.position, camMMPosition, Time.deltaTime);
		} 
		else if (activeMenu == CurrentMenu.CharacterCreation) {
			mainCamera.position = Vector3.Lerp (mainCamera.transform.position, camCCPosition, Time.deltaTime);
		} 
		else if (activeMenu == CurrentMenu.Loading) {
			mainCamera.position = Vector3.Lerp (mainCamera.transform.position, camMMPosition, Time.deltaTime);
		}
	}

	public void SwitchScene(int isMain) {
		if(isMain == 1) {
			activeMenu = CurrentMenu.MainMenu;
			uiAnim.SetBool("mainMenu", true);
			uiAnim.SetBool("characterCreation", false);
			uiAnim.SetBool("loading", false);
		}
		else if(isMain == 2) {
			activeMenu = CurrentMenu.CharacterCreation;
			uiAnim.SetBool("mainMenu", false);
			uiAnim.SetBool("characterCreation", true);
			uiAnim.SetBool("loading", false);
		}
		else if(isMain == 3) {
			activeMenu = CurrentMenu.Loading;
			uiAnim.SetBool("mainMenu", false);
			uiAnim.SetBool("characterCreation", false);
			uiAnim.SetBool("loading", true);
		}
	}

	public void ExitTheApplication() {
		Application.Quit ();
	}
}
