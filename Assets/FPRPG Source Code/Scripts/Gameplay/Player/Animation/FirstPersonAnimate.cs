using UnityEngine;
using System.Collections;
//using XInputDotNetPure;

public class FirstPersonAnimate : MonoBehaviour {

    public Camera fpCam;
	private Animator anim;
	private FirstPersonPlayer fpPlayer;

    public int normalFov;
    public int sprintingFov;

	public bool stopAnimation;

	private bool playerIndexSet = false;
	//private PlayerIndex playerIndex;
	//private GamePadState state;
	//private GamePadState prevState;

	void Start() {
		anim = GetComponent<Animator>();
		fpPlayer = GetComponentInParent<FirstPersonPlayer>();
	}

	void Update() {

		/*if (!playerIndexSet || !prevState.IsConnected)
		{
			for (int i = 0; i < 4; ++i)
			{
				PlayerIndex testPlayerIndex = (PlayerIndex)i;
				GamePadState testState = GamePad.GetState(testPlayerIndex);
				if (testState.IsConnected) {
					playerIndex = testPlayerIndex;
					playerIndexSet = true;
				}
			}
		}
		
		prevState = state;
		state = GamePad.GetState(playerIndex);
		*/

		if(stopAnimation == false) {
			if(fpPlayer.movingState == MovingSpeedState.Standing) {
				anim.SetBool("walking", false);
				anim.SetBool("running", false);
				anim.SetBool("dazed", false);
				anim.SetBool("swimming", false);
				anim.SetBool("inAir", false);
				anim.SetBool("horseRunning", false);
			}
			else if(fpPlayer.movingState == MovingSpeedState.Walking) {
				anim.SetBool("walking", true);
				anim.SetBool("running", false);
				anim.SetBool("dazed", false);
				anim.SetBool("swimming", false);
				anim.SetBool("inAir", false);
				anim.SetBool("horseRunning", false);

				if(Input.GetAxis("Horizontal") > 0f) {
					float calculatedSpeed = Input.GetAxis("Horizontal");
					anim.SetFloat("walkingSpeed", calculatedSpeed);
				}
				else if(Input.GetAxis("Horizontal") < 0f) {
					float calculatedSpeed = Input.GetAxis("Horizontal") * -1f;
					anim.SetFloat("walkingSpeed", calculatedSpeed);
				}
				else if(Input.GetAxis("Vertical") > 0f) {
					float calculatedSpeed = Input.GetAxis("Vertical");
					anim.SetFloat("walkingSpeed", calculatedSpeed);
				}
				else if(Input.GetAxis("Vertical") < 0f) {
					float calculatedSpeed = Input.GetAxis("Horizontal") * -1f;
					anim.SetFloat("walkingSpeed", calculatedSpeed);
				}
			}
			else if(fpPlayer.movingState == MovingSpeedState.Running) {
				anim.SetBool("walking", false);
				anim.SetBool("running", true);
				anim.SetBool("dazed", false);
				anim.SetBool("swimming", false);
				anim.SetBool("inAir", false);
				anim.SetBool("horseRunning", false);
			}
			else if(fpPlayer.movingState == MovingSpeedState.Swimming) {
				anim.SetBool("walking", false);
				anim.SetBool("running", false);
				anim.SetBool("dazed", false);
				anim.SetBool("swimming", true);
				anim.SetBool("inAir", false);
				anim.SetBool("horseRunning", false);
			}
			else if(fpPlayer.movingState == MovingSpeedState.Jumping) {
				anim.SetBool("walking", false);
				anim.SetBool("running", false);
				anim.SetBool("dazed", false);
				anim.SetBool("swimming", false);
				anim.SetBool("inAir", true);
				anim.SetBool("horseRunning", false);
			}
			else if(fpPlayer.movingState == MovingSpeedState.Dazed) {
				anim.SetBool("walking", false);
				anim.SetBool("running", false);
				anim.SetBool("dazed", true);
				anim.SetBool("swimming", false);
				anim.SetBool("inAir", false);
				anim.SetBool("horseRunning", false);
			}
			else if(fpPlayer.movingState == MovingSpeedState.OnHorse) {
				anim.SetBool("walking", false);
				anim.SetBool("running", false);
				anim.SetBool("dazed", false);
				anim.SetBool("swimming", false);
				anim.SetBool("inAir", false);
				anim.SetBool("horseRunning", true);

				if(Input.GetAxis("Horizontal") > 0f) {
					float calculatedSpeed = Input.GetAxis("Horizontal");
					anim.SetFloat("walkingSpeed", calculatedSpeed);
				}
				else if(Input.GetAxis("Horizontal") < 0f) {
					float calculatedSpeed = Input.GetAxis("Horizontal") * -1f;
					anim.SetFloat("walkingSpeed", calculatedSpeed);
				}
				else if(Input.GetAxis("Vertical") > 0f) {
					float calculatedSpeed = Input.GetAxis("Vertical");
					anim.SetFloat("walkingSpeed", calculatedSpeed);
				}
				else if(Input.GetAxis("Vertical") < 0f) {
					float calculatedSpeed = Input.GetAxis("Horizontal") * -1f;
					anim.SetFloat("walkingSpeed", calculatedSpeed);
				}
			}
		}
		else {
			anim.SetBool("walking", false);
			anim.SetBool("running", false);
			anim.SetBool("dazed", false);
			anim.SetBool("swimming", false);
			anim.SetBool("inAir", false);
			anim.SetBool("horseRunning", false);

			if(playerIndexSet) {
				//GamePad.SetVibration(playerIndex, 0f, 0f);
			}
		}
	}

    public void SetFoV(int fov) {
        fpCam.fieldOfView = fov;
    }
}
