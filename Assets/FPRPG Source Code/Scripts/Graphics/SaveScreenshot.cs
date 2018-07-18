using UnityEngine;
using System;
using System.IO;
using System.Collections;

public class SaveScreenshot : MonoBehaviour {

	public KeyCode shortcut = KeyCode.F10;

	private string directoryPath;

	private ShowMessage showMessage;

	void Start() {
		directoryPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Red Moose Games\The Fallen Chronicler\Screenshots\";

		if(!Directory.Exists(directoryPath)) {    
			Directory.CreateDirectory(directoryPath);			
		}

		if (GameObject.Find ("_UICanvasGame") != null) {
			showMessage = GameObject.Find ("_UICanvasGame").GetComponentInChildren<ShowMessage> ();
		}
	}
	
	void Update() {
		if (Input.GetKeyDown(shortcut)) {
			StartCoroutine(ScreenshotEncode());
		}
	}
	
	IEnumerator ScreenshotEncode() {

		// wait for graphics to render
		yield return new WaitForEndOfFrame();
		
		// create a texture to pass to encoding
		Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
		
		// put buffer into texture
		texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
		texture.Apply();
		
		// split the process up--ReadPixels() and the GetPixels() call inside of the encoder are both pretty heavy
		yield return 0;
		
		byte[] bytes = texture.EncodeToPNG();

		//string date = DateTime.Now.ToString("hh_mm_ss_ddmmmmyyyy");
		string date = DateTime.Now.ToString("yyyymmmmdd_hh_mm_ss");
		// save our test image (could also upload to WWW)
		File.WriteAllBytes(directoryPath + "screenshot_"  + date + ".png", bytes);

		showMessage.SendTheMessage("Screenshot saved!");

		// Added by Karl. - Tell unity to delete the texture, by default it seems to keep hold of it and memory crashes will occur after too many screenshots.
		DestroyObject( texture );
		
		//Debug.Log( Application.dataPath + "/../testscreen-" + count + ".png" );
	}
}
