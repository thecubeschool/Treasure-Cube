using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PauseMenuFunctions : MonoBehaviour {

	public void PMFQuitTheGame() {
        SceneManager.LoadScene(1);
	}

    public void PMFExitGame() {
        Application.Quit();
    }
}
