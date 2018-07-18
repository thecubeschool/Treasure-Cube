using UnityEngine;
using System.Collections;

public class MissileDamager : MonoBehaviour {

	public GameObject dirtFx;
	public GameObject bloodFx;
	public int missileDamage;

	public GameObject spawner;
	public WeaponRangedAnimate wra;
	public GameManager gameManager;
	public PlayerSkillsAndAttributes skillsAttributes;
	public UISkillsAndAttributes uiSA;

	public bool playerArrow;
	private bool npcArrow;

    private void Awake() {
        if(skillsAttributes == null) {
            skillsAttributes = GameObject.FindObjectOfType<PlayerSkillsAndAttributes>();
        }
        if (uiSA == null) {
            uiSA = GameObject.FindObjectOfType<UISkillsAndAttributes>();
        }
    }

    void OnCollisionEnter(Collision other){
		if(other.transform.CompareTag("Npc")) {
			wra.GetComponent<PlaySFXOnAnimation>().WeaponHitFXPlay();

			if(playerArrow) {
				if(other.transform.GetComponent<NPC>().npcDisposition != NPCDisposition.Hostile) {
					if(other.transform.GetComponent<NPC>().npcDisposition != NPCDisposition.Ally) {
						other.transform.GetComponent<NPC>().npcDisposition = NPCDisposition.Hostile;
						other.transform.GetComponent<NPCAiNavmesh>().engagedInCombat = true;
					}
					if(other.transform.GetComponent<NPC>().weapon == NPCWeapon.None) {
						//other.transform.GetComponent<NPCAi>().isCoward = true;
					}
					
					if(other.transform.GetComponent<NPC>().npcFaction != NPCFaction.Bandits) {
						if(spawner.GetComponent<PlayerStats>().crimeScore < 100) {
							spawner.GetComponent<PlayerStats>().crimeScore += 50;
							if(spawner.GetComponent<PlayerStats>().valorScore >= 25) {
								spawner.GetComponent<PlayerStats>().valorScore -= 25;
							}
							else {
								spawner.GetComponent<PlayerStats>().valorScore = 0;
							}
							uiSA.crimeScore.text = "Crime score: " + spawner.GetComponent<PlayerStats>().crimeScore.ToString();
							uiSA.valorScore.text = "Valor score: " + spawner.GetComponent<PlayerStats>().valorScore.ToString();
						}
					}
				}
				other.transform.GetComponent<NPCAiNavmesh>().target = GameObject.FindGameObjectWithTag("Player").transform;
			}

			if(gameManager.currentOpponentFighting != other.gameObject) {
				gameManager.currentOpponentFighting = other.gameObject;
				gameManager.lastTimeOpponentHit = Time.time;
			}

			if(skillsAttributes.marksman > 25) {
				float tmp = missileDamage * 0.15f;
				float tmp2 = missileDamage + tmp;
				other.transform.GetComponent<NPC>().npcHealth -= (int)tmp2;
			}
			else {
				other.transform.GetComponent<NPC>().npcHealth -= missileDamage;
			}
			Instantiate(bloodFx, new Vector3(other.transform.position.x, other.transform.position.y+0.5f, other.transform.position.z+0.5f), Quaternion.identity);

			Destroy(gameObject);
		}
		else {
			Instantiate(dirtFx, new Vector3(other.transform.localPosition.x, other.transform.localPosition.y, other.transform.localPosition.z+0.5f), Quaternion.identity);
			Destroy(gameObject);
		}
	}
}
