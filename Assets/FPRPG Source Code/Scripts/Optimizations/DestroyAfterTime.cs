using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour {

    public float destroyTimer;

    void LateUpdate() {
        StartCoroutine(DestroyAfterSomeTime());
    }

    IEnumerator DestroyAfterSomeTime() {
        yield return new WaitForSeconds(destroyTimer);
        Destroy(gameObject);
    }
}
