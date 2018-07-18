using UnityEngine;
using System.Collections;

public enum HorseType {
	Chestnut = 0,
	Black = 1,
	Grulla = 2,
	Palomino = 3,
	Grey = 4,
	Undead = 5,
}

public enum BardingType {
    White = 0,
    Black = 1,
    Blue = 2,
    Green = 3,
    Red = 4,
    Brown = 5,
}

public class HorseManager : MonoBehaviour {

	private Animator anim;
	[Space(10f)]
	public HorseType horseType;
	public int health;
	public int speed;
	[Space(10f)]
	public string horseName = "My Horse";
	[Space(10f)]
	public GameObject owner;
	public bool hasEquipment = false;

	private GameManager gameManager;
	private ShowMessage showMessage;
	private GameObject playerGo;
	private float distanceToPlayer;
	private GameObject[] stablesList;
	private GameObject nearestStable;
	private string nearestStableName;
	public bool stabled = false;

	void Start() {

		anim = GetComponentInChildren<Animator> ();
		playerGo = GameObject.Find("[Player]");
		stablesList = GameObject.FindGameObjectsWithTag("Stable");
		showMessage = GameObject.Find("_UICanvasGame").GetComponentInChildren<ShowMessage>();
		gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

		anim.SetBool("chestnut", false);
		anim.SetBool("black", false);
		anim.SetBool("grulla", false);
		anim.SetBool("palomino", false);
		anim.SetBool("gray", false);
		anim.SetBool("undead", false);

		if(horseType == HorseType.Chestnut) {
			anim.SetBool("chestnut", true);
		}
		if(horseType == HorseType.Black) {
			anim.SetBool("black", true);
		}
		if(horseType == HorseType.Grulla) {
			anim.SetBool("grulla", true);
		}
		if(horseType == HorseType.Palomino) {
			anim.SetBool("palomino", true);
		}
		if(horseType == HorseType.Grey) {
			anim.SetBool("gray", true);
		}
		if(horseType == HorseType.Undead) {
			anim.SetBool("undead", true);
		}

		if(hasEquipment == true) {
			anim.SetBool("equiped", true);
		}
		else {
			anim.SetBool("equiped", false);
		}

		GetComponent<UnityEngine.AI.NavMeshAgent> ().Stop ();
	}

	void Update() {
		if(horseType == HorseType.Chestnut) {
			anim.SetBool("chestnut", true);
			anim.SetBool("black", false);
			anim.SetBool("grulla", false);
			anim.SetBool("palomino", false);
			anim.SetBool("gray", false);
			anim.SetBool("undead", false);
		}
		if(horseType == HorseType.Black) {
			anim.SetBool("chestnut", false);
			anim.SetBool("black", true);
			anim.SetBool("grulla", false);
			anim.SetBool("palomino", false);
			anim.SetBool("gray", false);
			anim.SetBool("undead", false);
		}
		if(horseType == HorseType.Grulla) {
			anim.SetBool("chestnut", false);
			anim.SetBool("black", false);
			anim.SetBool("grulla", true);
			anim.SetBool("palomino", false);
			anim.SetBool("gray", false);
			anim.SetBool("undead", false);
		}
		if(horseType == HorseType.Palomino) {
			anim.SetBool("chestnut", false);
			anim.SetBool("black", false);
			anim.SetBool("grulla", false);
			anim.SetBool("palomino", true);
			anim.SetBool("gray", false);
			anim.SetBool("undead", false);
		}
		if(horseType == HorseType.Grey) {
			anim.SetBool("chestnut", false);
			anim.SetBool("black", false);
			anim.SetBool("grulla", false);
			anim.SetBool("palomino", false);
			anim.SetBool("gray", true);
			anim.SetBool("undead", false);
		}
		if(horseType == HorseType.Undead) {
			anim.SetBool("chestnut", false);
			anim.SetBool("black", false);
			anim.SetBool("grulla", false);
			anim.SetBool("palomino", false);
			anim.SetBool("gray", false);
			anim.SetBool("undead", true);
		}

		if(hasEquipment == true) {
			anim.SetBool("equiped", true);
		}
		else {
			anim.SetBool("equiped", false);
		}

		if(owner == playerGo) {
			if(gameManager.activeHorse == gameObject) {
				distanceToPlayer = Vector3.Distance(playerGo.transform.position, transform.position);

				if(distanceToPlayer > 180f) {
					if(stabled == false) {
						GetNearestStable();
						showMessage.SendTheMessage("Your horse is now stabled at " + nearestStableName + ".");
					}
				}
			}
			else {
				if(stabled == false) {
					GetNearestStable();
				}
			}
		}
	}

	void GetNearestStable () {
		Transform nearestStable = null;
		float closestDistanceSqr = Mathf.Infinity;
		Vector3 currentPosition = transform.position;

		foreach(GameObject potentialTarget in stablesList) {
			Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
			float dSqrToTarget = directionToTarget.sqrMagnitude;
			if(dSqrToTarget < closestDistanceSqr) {
				closestDistanceSqr = dSqrToTarget;
				nearestStable = potentialTarget.transform;
			}
		}

        if (nearestStable != null) {
            transform.position = new Vector3(nearestStable.transform.position.x + Random.Range(0f, 1f), nearestStable.transform.position.y + Random.Range(0f, 1f), nearestStable.transform.position.z + Random.Range(0f, 1f));
            nearestStableName = nearestStable.name;
        }
		stabled = true;
	}
}
