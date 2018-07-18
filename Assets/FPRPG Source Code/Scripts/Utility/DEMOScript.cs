using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DEMOScript : MonoBehaviour {
    
    public void DEMOLoadLevel(int demoIndex) {
        SceneManager.LoadScene(demoIndex);
    }

    public void DEMOExitApplication() {
        Application.Quit();
    }

    public void Update() {
        if(Input.GetKey(KeyCode.Escape) && Input.GetKey(KeyCode.Space)) {
            SceneManager.LoadScene(0);
        }
    }
}
