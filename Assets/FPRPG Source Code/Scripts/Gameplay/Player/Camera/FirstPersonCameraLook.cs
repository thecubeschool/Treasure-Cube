using UnityEngine;
using System.Collections;

#pragma warning disable 0414

public class FirstPersonCameraLook : MonoBehaviour {

	public float timeSpeedFactor = 1f;

	private Transform character;
	private Transform cam;

	public float XSensitivity = 2f;
	public float YSensitivity = 2f;
	public bool clampVerticalRotation = true;
	public float MinimumX = -90F;
	public float MaximumX = 90F;
	public bool smooth;
	public float smoothTime = 5f;
	
	
	private Quaternion m_CharacterTargetRot;
	private Quaternion m_CameraTargetRot;

	private FirstPersonPlayer fpPlayer;
	private GameManager gameManager;
	
	public void Start() {

		character = transform;
		cam = transform.Find("FPCameraGO/FPCamera").transform;

		m_CharacterTargetRot = character.localRotation;
		m_CameraTargetRot = cam.localRotation;
		fpPlayer = GetComponent<FirstPersonPlayer>();
		gameManager = GameObject.Find ("[GameManager]").GetComponent<GameManager> ();
	}

	void Update() {

        LookRotation(character, cam);
	}
	
	public void LookRotation(Transform character, Transform camera) {
		float yRot = Input.GetAxis("Mouse X") * XSensitivity;
		float xRot = Input.GetAxis("Mouse Y") * YSensitivity;

		m_CharacterTargetRot *= Quaternion.Euler (0f, yRot, 0f);
			
		if(fpPlayer.onFoot == true) {
			m_CameraTargetRot *= Quaternion.Euler (-xRot, 0f, 0f);
		}
		else {
			m_CameraTargetRot = Quaternion.identity;
		}
			
		if(clampVerticalRotation)
			m_CameraTargetRot = ClampRotationAroundXAxis (m_CameraTargetRot);
			
		if(smooth) {
			character.localRotation = Quaternion.Slerp (character.localRotation, m_CharacterTargetRot,
				                                        smoothTime * Time.deltaTime * timeSpeedFactor);
			camera.localRotation = Quaternion.Slerp (camera.localRotation, m_CameraTargetRot,
				                                        smoothTime * Time.deltaTime * timeSpeedFactor);
		}
		else {
			character.localRotation = m_CharacterTargetRot;
			camera.localRotation = m_CameraTargetRot;
		}
	}
	
	
	Quaternion ClampRotationAroundXAxis(Quaternion q) {
		q.x /= q.w;
		q.y /= q.w;
		q.z /= q.w;
		q.w = 1.0f;
		
		float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan (q.x);
		
		angleX = Mathf.Clamp (angleX, MinimumX, MaximumX);
		
		q.x = Mathf.Tan (0.5f * Mathf.Deg2Rad * angleX);
		
		return q;
	}
	
}