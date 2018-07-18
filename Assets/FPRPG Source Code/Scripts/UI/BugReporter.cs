#if UNITY_STANDALONE

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

public class BugReporter : MonoBehaviour{

    public GameObject brHolder;

	public InputField emailInput;
	public InputField bugInput;

	private UIManager uiManager;

	//email setup
	private string from = "developer@thefallenchronicler.com";
	private string password = "Predragbranka1972";
	private string smtp = "smtp.thefallenchronicler.com";
	private string to = "developer@thefallenchronicler.com";

	private string email = "";
	private string body = "";
	
	private string filepath = "";

	void Start() {
		uiManager = GameObject.Find ("_UICanvasGame").GetComponent<UIManager> ();
	}
	
	void Update(){
		if(Input.GetKeyUp(KeyCode.F1)){
			//take a screenshot when opening the form
			TakeScreenshot();
            Debug.Log("Taking screenshot.");
            brHolder.SetActive(true);
		}
	}

	public void SendBugReport() {
		email = emailInput.text;
		body = bugInput.text;
		SendEmail();
	}
	
	void SendEmail(){
		MailMessage mail = new MailMessage();
		
		//create the resource to be attached to the email
		LinkedResource inline = new LinkedResource(filepath);
		inline.ContentId = Guid.NewGuid().ToString();
		Attachment att = new Attachment(filepath);
		att.ContentDisposition.Inline = true;
		
		//create the content of the email
		mail.From = new MailAddress(from);
		mail.To.Add(to);
		mail.Subject = "Bug Report Mail";
		mail.Body = String.Format(
			"Report by: " + email + "<br/><br/>" + 
			"Bug report:<br/>" + body + "<br/><br/><hr>" + 
			GetHardwareInfo() + "<br/><hr>" + 
			@"<img src=""cid:{0}"" />", inline.ContentId);
		mail.IsBodyHtml = true;
		mail.Attachments.Add(att);
		
		//set up the smtp
		SmtpClient smtpServer = new SmtpClient(smtp);
		smtpServer.Port = 587;
		smtpServer.EnableSsl = true;
		smtpServer.Credentials = new System.Net.NetworkCredential(from, password) 
			as ICredentialsByHost;
		ServicePointManager.ServerCertificateValidationCallback = 
			delegate(object obj, X509Certificate cert, X509Chain chain, SslPolicyErrors sslerrors) 
		{ return true; };
		smtpServer.Send(mail);
		
		#if UNITY_EDITOR
		Debug.Log("email sent");
		#endif
		uiManager.bugReportUI.SetActive (false);
	}
	
	//return a string with some essential hardware info
	string GetHardwareInfo(){
		return    "Graphics Device Name: " + SystemInfo.graphicsDeviceName + "<br/>"
			+ "Graphics Device Type: " + SystemInfo.graphicsDeviceType.ToString() + "<br/>"
				+ "Graphics Device Version: " + SystemInfo.graphicsDeviceVersion + "<br/>"
				+ "Graphics Memory Size: " + MBtoGB(SystemInfo.graphicsMemorySize) + "<br/>"
				+ "Graphics Shader Level: " + ShaderLevel(SystemInfo.graphicsShaderLevel) + "<br/>"
				+ "Maximum Texture Size: " + MBtoGB(SystemInfo.maxTextureSize) + "<br/>"
				+ "Operating System: " + SystemInfo.operatingSystem + "<br/>"
				+ "Processor Type: " + SystemInfo.processorType + "<br/>"
				+ "Processor Count: " + SystemInfo.processorCount.ToString() + "<br/>"
				+ "System Memory Size: " + MBtoGB(SystemInfo.systemMemorySize) + "<br/>"
				+ "Screen Size: " + Screen.width.ToString() + "x" + Screen.height.ToString();
	}
	
	//make the shader level more readable
	string ShaderLevel(int level){
		switch(level){
		case 20: return "Shader Model 2.x";
		case 30: return "Shader Model 3.0";
		case 40: return "Shader Model 4.0 ( DX10.0 )";
		case 41: return "Shader Model 4.1 ( DX10.1 )";
		case 50: return "Shader Model 5.0 ( DX11.0 )";
		default: return "";
		}
	}
	
	//unity returns a weird amount of available MB, this unifies it
	string MBtoGB(int mb){
		return Mathf.Ceil(mb / 1024f).ToString() + "GB";
	}
	
	//take a screenshot before opening the GUI
	public void TakeScreenshot(){
		ScreenCapture.CaptureScreenshot("bugscreenshot.png");
		filepath = Application.dataPath + "/../bugscreenshot.png";
	}
}
#endif