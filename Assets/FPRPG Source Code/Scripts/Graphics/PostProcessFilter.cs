using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[ExecuteInEditMode]
public class PostProcessFilter : MonoBehaviour {
	public Material PostProcessMaterial;
	
	public Camera BackgroundCamera;
	public Camera MainCamera;
	
	private RenderTexture mainRenderTexture;
	
	// Use this for initialization
	void Start () {
		mainRenderTexture = new RenderTexture(Screen.width, Screen.height, 16, RenderTextureFormat.ARGB32);
		mainRenderTexture.Create();
		
		BackgroundCamera.targetTexture = mainRenderTexture;
		MainCamera.targetTexture = mainRenderTexture;
	}
	
	void OnPostRender() {
		Graphics.Blit(mainRenderTexture, PostProcessMaterial);
	}
}