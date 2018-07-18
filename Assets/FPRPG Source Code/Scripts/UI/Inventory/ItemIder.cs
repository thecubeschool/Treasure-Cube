using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ItemIder : MonoBehaviour {

    void Update() {
#if UNITY_EDITOR
        Item[] allItems = GameObject.FindObjectsOfType<Item>();
        Item myItem = GetComponent<Item>();

        for (int i = 0; i < allItems.Length; i++) {
            myItem.itemId = Random.Range(0, 1000);
            if (allItems[i].itemId == myItem.itemId) {
                myItem.itemId = Random.Range(0, 1000);
            }
        }

        DestroyImmediate(this);
#endif
    }
}
