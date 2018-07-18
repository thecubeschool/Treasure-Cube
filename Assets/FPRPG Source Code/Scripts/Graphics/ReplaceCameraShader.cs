using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplaceCameraShader : MonoBehaviour {

	public Shader shaderReplacement;

	void Start() {
		//unlitShader = Shader.Find("Unlit/Texture");
		GetComponent<Camera>().SetReplacementShader(shaderReplacement,"");
	}
}
