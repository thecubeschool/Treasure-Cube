using UnityEngine;
using System.Collections;

#pragma warning disable 0414

public class NPCTopicManager : MonoBehaviour {

	private ConversationTopicManager conversationTopicManager;
	private Transform topicksHolder;

	public bool downloadTopicsToNpc;

	private NPC npc;

	void Start() {
		conversationTopicManager = GameObject.Find("[GameManager]").GetComponent<ConversationTopicManager>();
		topicksHolder = transform;
		npc = GetComponentInParent<NPC>();

		if (!transform.parent.name.Contains ("Ghost")) {
			StartCoroutine (GetTheTopicsToNpcs ());
		}
	}

	IEnumerator GetTheTopicsToNpcs() {

        if (conversationTopicManager.rumorsTopics.Count > 0) {
            int randomRumorTopic = Random.Range(0, conversationTopicManager.rumorsTopics.Count);
            GameObject topicRumor = (GameObject)Instantiate(conversationTopicManager.rumorsTopics[randomRumorTopic]);
            topicRumor.transform.SetParent(topicksHolder);
        }

		yield return new WaitForSeconds(240f);
		downloadTopicsToNpc = false;
	}
}
