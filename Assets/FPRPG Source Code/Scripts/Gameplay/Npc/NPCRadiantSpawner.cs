using UnityEngine;
using System.Collections;

public class NPCRadiantSpawner : MonoBehaviour {

	public GameObject lowLvlNpc;
	public GameObject mediumLvlNpc;
	public GameObject highLvlNpc;
	public GameObject epicLvlNpc;
	public GameObject legendaryNpc;
	public GameObject currentNpc;

	private GameObject playerGo;
	private float playerDistance;
	private float timer = 120f;
	private float cooldown;

	private PlayerLevelManager playerLvlManager;

	void Start() {
		playerGo = GameObject.FindGameObjectWithTag("Player");
		playerLvlManager = playerGo.GetComponent<PlayerLevelManager>();

		SpawnRadiantNpc();
	}

	void Update() {
		playerDistance = Vector3.Distance(playerGo.transform.position, transform.position);

		if(cooldown <= 0) {
			if(playerDistance > 250f) {
				SpawnRadiantNpc();
			}

			cooldown = timer;
		}
	}

	void SpawnRadiantNpc() {
		if(playerLvlManager.playerLevel <= 10) {
			Destroy(currentNpc.gameObject);
			GameObject npcgo = Instantiate(lowLvlNpc, transform.position, Quaternion.identity) as GameObject;
			currentNpc = npcgo;
		}
		else if(playerLvlManager.playerLevel > 10 && playerLvlManager.playerLevel <= 20) {
			Destroy(currentNpc.gameObject);
			GameObject npcgo = Instantiate(mediumLvlNpc, transform.position, Quaternion.identity) as GameObject;
			currentNpc = npcgo;
		}
		else if(playerLvlManager.playerLevel > 20 && playerLvlManager.playerLevel <= 30) {
			Destroy(currentNpc.gameObject);
			GameObject npcgo = Instantiate(highLvlNpc, transform.position, Quaternion.identity) as GameObject;
			currentNpc = npcgo;
		}
		else if(playerLvlManager.playerLevel > 30 && playerLvlManager.playerLevel <= 40) {
			Destroy(currentNpc.gameObject);
			GameObject npcgo = Instantiate(epicLvlNpc, transform.position, Quaternion.identity) as GameObject;
			currentNpc = npcgo;
		}
		else if(playerLvlManager.playerLevel > 40 && playerLvlManager.playerLevel <= 50) {
			Destroy(currentNpc.gameObject);
			GameObject npcgo = Instantiate(legendaryNpc, transform.position, Quaternion.identity) as GameObject;
			currentNpc = npcgo;
		}
	}
}
