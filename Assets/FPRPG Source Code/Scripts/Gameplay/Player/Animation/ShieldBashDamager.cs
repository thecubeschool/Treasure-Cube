using UnityEngine;
using System.Collections;

public class ShieldBashDamager : MonoBehaviour {

	public WeaponMeleeAnimate wepAnim;
	public PlayerStats stats;

	void Start() {
		stats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
	}

	void OnTriggerEnter(Collider col) {
		if(col.gameObject.tag == "Npc" && col.gameObject.tag != "IgnoreTag") {
			if(wepAnim.bashing == true) {
//				print("bashing");
				
				if(col.GetComponent<NPC>().npcDisposition != NPCDisposition.Hostile) {
					if(col.GetComponent<NPC>().npcDisposition != NPCDisposition.Ally) {
						col.GetComponent<NPC>().npcDisposition = NPCDisposition.Hostile;
						col.GetComponent<NPCAiNavmesh>().engagedInCombat = true;
					}
					if(col.GetComponent<NPC>().weapon == NPCWeapon.None) {
						//col.GetComponent<NPCAi>().isCoward = true;
					}
					
					if(col.GetComponent<NPC>().npcFaction != NPCFaction.Bandits) {
						if(stats.crimeScore < 20) {
							stats.crimeScore += 20;
						}
					}
				}
				
				col.GetComponentInChildren<NPCDamageSender>().dazed = true;
				
				wepAnim.GetComponent<PlaySFXOnAnimation>().WeaponHitObjectFXPlay();
				Instantiate(wepAnim.dirtFx, wepAnim.particlePoint.position, Quaternion.identity);
			}
		}
	}

	public void BashingActivator() {
		wepAnim.bashing = true;
	}
}
