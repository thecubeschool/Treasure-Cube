using UnityEngine;
using System.Collections;

public class CoinsBag : MonoBehaviour {

	public int lowAmount, highAmount;
	[Space(10f)]
	public int coinAmount;

	private PlayerStats playerStats;
	
	void Start () {
		coinAmount = Random.Range(lowAmount, highAmount);
		playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
	}

	public void PickupCoinBag() {
		playerStats.playerMoney += coinAmount;
		Destroy(gameObject);
	}
}
