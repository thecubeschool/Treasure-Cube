using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class QuestMarkerUI : MonoBehaviour {

	public Transform questTracker;

	public Transform playerTransform;
	public Transform questTarget;
	
	public float questMarker;
	public float markerPosition;
	
	private float degreeOffset;
	
	private RectTransform rectTransform;
	private GameManager gm;
	private Transform questsHolder;

    public GameObject overworldMapQuestTrackerGo;
	
	void Start() {
		gm = GameObject.Find("[GameManager]").GetComponent<GameManager>();
		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		rectTransform = GetComponent<RectTransform>();
        if (GameObject.Find("[Quests]")) {
            questsHolder = GameObject.Find("[Quests]").transform;
        }
	}

    void Update() {
        if (overworldMapQuestTrackerGo != null) {
            if (gm.trackingQuestId != "") {
                TrackQuestTarget();
            }
            else {
                if (questTarget != null) {
                    questTarget = null;
                }
            }

            if (questTarget != null) {
                if (GetComponent<Image>().enabled == false) {
                    GetComponent<Image>().enabled = true;
                }
                if (overworldMapQuestTrackerGo.activeSelf == false) {
                    overworldMapQuestTrackerGo.SetActive(true);
                }
            }
            else {
                if (GetComponent<Image>().enabled == true) {
                    GetComponent<Image>().enabled = false;
                }
                if (overworldMapQuestTrackerGo.activeSelf == true) {
                    overworldMapQuestTrackerGo.SetActive(false);
                }
            }
        }
	}

	void TrackQuestTarget() {

		if(questsHolder.Find(gm.trackingQuestId).GetComponent<QuestBase>().currentQuestTarget != null && 
		   questTarget != questsHolder.Find(gm.trackingQuestId).GetComponent<QuestBase>().currentQuestTarget) {
			questTarget = questsHolder.Find(gm.trackingQuestId).GetComponent<QuestBase>().currentQuestTarget.transform;
		}

		questTracker.LookAt(questTarget.transform.position);

		questMarker = questTracker.localEulerAngles.y;

		rectTransform.rotation = Quaternion.Euler(0f, 0f, -questMarker);

        overworldMapQuestTrackerGo.transform.position = new Vector3(questTarget.transform.position.x,
                                                                    questTarget.transform.position.y,
                                                                    questTarget.transform.position.z);
    }
}
