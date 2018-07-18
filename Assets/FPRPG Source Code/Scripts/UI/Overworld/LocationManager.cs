using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocationManager : MonoBehaviour {

    [Tooltip("Will this location be visible on the overworld map view before it is discovered by player?")]
    public bool visibleBeforeDiscovery = false;

    public Sprite locationFoundSprite;

    public GameObject nameTxtGo;

    private void OnMouseEnter() {
        if (visibleBeforeDiscovery == true) {
            if (nameTxtGo.activeSelf == false) {
                nameTxtGo.SetActive(true);
            }
        }

       // GetComponent<RectTransform>().sizeDelta = new Vector2(1.1f, 1.1f);
    }

    private void OnMouseExit() {
        if (visibleBeforeDiscovery == true) {
            if (nameTxtGo.activeSelf == true) {
                nameTxtGo.SetActive(false);
            }
        }

        //GetComponent<RectTransform>().sizeDelta = new Vector2(1f, 1f);
    }

    private void LateUpdate() {
        if (visibleBeforeDiscovery == true) {
            if (GetComponent<Image>().enabled == false) {
                GetComponent<Image>().enabled = true;
            }
        }
        else {
            if (GetComponent<Image>().enabled == true) {
                GetComponent<Image>().enabled = false;
            }
        }
    }
}
