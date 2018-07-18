using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class NPCIDer : MonoBehaviour {

    void Update() {
#if UNITY_EDITOR
        NPC[] allNpcs = GameObject.FindObjectsOfType<NPC>();
        NPC myNpc = GetComponent<NPC>();

        for (int i = 0; i < allNpcs.Length; i++) {
            myNpc.npcId = Random.Range(0, 1000);
            if (allNpcs[i].npcId == myNpc.npcId) {
                myNpc.npcId = Random.Range(0, 1000);
            }
        }

        DestroyImmediate(this);
#endif
    }
}