using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCTopicHolder : MonoBehaviour {

	[HideInInspector]
	public List<NPCTopic> topics;
	[HideInInspector]
	public Transform topicsTransform;
	[HideInInspector]
	private bool fetchNewTopics;

	void Start() {

		topics = new List<NPCTopic>();

		topicsTransform = transform.Find("topics").transform;

		topics.Clear();
		foreach(Transform t in topicsTransform) {
			topics.Add(t.GetComponent<NPCTopic>());
		}
	}

	void Update() {
		if(fetchNewTopics == false) {
			topics.Clear();
			foreach(Transform t in topicsTransform) {
				topics.Add(t.GetComponent<NPCTopic>());
			}
			fetchNewTopics = true;
		}
		else {
			StartCoroutine(CountDown101());
		}
	}

	IEnumerator CountDown101() {
		yield return new WaitForSeconds(2f);

		fetchNewTopics = false;
	}
}
