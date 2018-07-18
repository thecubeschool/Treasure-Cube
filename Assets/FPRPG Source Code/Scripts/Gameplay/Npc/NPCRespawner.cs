using UnityEngine;
using System.Collections;

public class NPCRespawner : MonoBehaviour {

	public int daysUntilRespawn = 10;
	public int daysCountdown;

	private GameObject playerGo;
	private TodClock todClock;

	private int counter;

	public GameObject[] npcGo;
	private int npcHealth;

	void Awake() {
		npcHealth = gameObject.GetComponent<NPC> ().npcHealth;
	}

	void Start() {
		playerGo = GameObject.Find ("[Player]");
		todClock = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<TodClock> ();
	}

	void LateUpdate() {
		if(daysCountdown < daysUntilRespawn) {
			if(gameObject.GetComponent<NPC>().npcHealth <= 0) {
				InvokeRepeating ("StartTheTimer", 0, 10f);
			}
		}
		else if (daysCountdown >= daysUntilRespawn) {
			float dist = Vector3.Distance(playerGo.transform.position, gameObject.transform.position);

			if(dist > 100f) {
				RespawnTheNpc();
			}
		}
	}

	void StartTheTimer() {
		if(counter != todClock.dayCount) {
			daysCountdown++;
			counter = todClock.dayCount;
		}
	}

	void RespawnTheNpc() {
        gameObject.GetComponent<NPC> ().npcHealth = npcHealth;
        gameObject.GetComponent<NPC>().hasDied = false;
        gameObject.GetComponent<NPCOutfiter>().enabled = true;
        gameObject.GetComponent<CapsuleCollider>().enabled = true;
        gameObject.GetComponent<BoxCollider>().enabled = true;
        gameObject.GetComponent<NPCTopicHolder>().enabled = true;
        gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
        gameObject.GetComponent<NPCAiNavmesh>().enabled = true;

        gameObject.GetComponent<NPC>().animBase.gameObject.SetActive(true);
        gameObject.GetComponent<NPC>().animHair.gameObject.SetActive(true);
        gameObject.GetComponent<NPC>().animFacialHair.gameObject.SetActive(true);
        gameObject.GetComponent<NPC>().animOutfit.gameObject.SetActive(true);
        gameObject.GetComponent<NPC>().animHelmHat.gameObject.SetActive(true);
        gameObject.GetComponent<NPC>().animWeapon.gameObject.SetActive(true);

        gameObject.GetComponent<NPC>().setLooks = true;

		counter = 0;
		daysCountdown = 0;

        gameObject.GetComponent<NPCAiNavmesh>().target = null;
        gameObject.GetComponent<NPCAiNavmesh>().targetInSight = false;
    }
}
