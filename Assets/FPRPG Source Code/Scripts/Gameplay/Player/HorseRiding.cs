using UnityEngine;
using System.Collections;

public class HorseRiding : MonoBehaviour {

	private FirstPersonPlayer fpsP;
	private CharacterController controller;
	private Animator anim;

	public bool playerMounted = false;
	
	public HorseType horseType;

	public int health;
	public int speed;

	public bool setTheHorse = false;

	public Transform horseDismountPoint;
	public GameObject horseRidenNpcGo;

	public bool stopAnimation = false;

	void Start () {
		fpsP = transform.parent.transform.parent.transform.parent.GetComponent<FirstPersonPlayer>();
		controller = fpsP.gameObject.GetComponent<CharacterController>();
		anim = GetComponentInChildren<Animator> ();
		
		anim.SetBool("chestnut", false);
		anim.SetBool("black", false);
		anim.SetBool("grulla", false);
		anim.SetBool("palomino", false);
		anim.SetBool("gray", false);
		anim.SetBool("undead", false);

		gameObject.SetActive(false);
	}
	

	void Update () {

		if(setTheHorse == false) {
			if(horseType == HorseType.Chestnut) {
				anim.SetBool("chestnut", true);
				fpsP.onFoot = false;
			}
			if(horseType == HorseType.Black) {
				anim.SetBool("black", true);
				fpsP.onFoot = false;
			}
			if(horseType == HorseType.Grulla) {
				anim.SetBool("grulla", true);
				fpsP.onFoot = false;
			}
			if(horseType == HorseType.Palomino) {
				anim.SetBool("palomino", true);
				fpsP.onFoot = false;
			}
			if(horseType == HorseType.Grey) {
				anim.SetBool("gray", true);
				fpsP.onFoot = false;
			}
			if(horseType == HorseType.Undead) {
				anim.SetBool("undead", true);
				fpsP.onFoot = false;
			}

			fpsP.horseSpeed = speed;
		}

		float axisPlus = Input.GetAxis("Vertical");

		if(stopAnimation == false) {
			if(axisPlus > 0f) {
				if(controller.isGrounded) {
					anim.SetBool("walking", true);
				}
				else {
					anim.SetBool("walking", false);
				}
			}
			else {
				anim.SetBool("walking", false);
			}

			if(axisPlus == 0f) {
				if(Input.GetButtonUp("Use")) {
					gameObject.SetActive(false);

					horseRidenNpcGo.SetActive(true);
					horseRidenNpcGo.GetComponent<UnityEngine.AI.NavMeshAgent>().Warp(horseDismountPoint.position);
					//horseRidenNpcGo.transform.position = horseDismountPoint.position;

					fpsP.onFoot = true;
					playerMounted = false;
				}
			}
		}
		else {
			anim.SetBool("walking", false);
		}
	}
}
