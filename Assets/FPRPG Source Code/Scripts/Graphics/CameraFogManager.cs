using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFogManager : MonoBehaviour {

    public bool toggleFog;

    public void OnPreRender() {
        toggleFog = RenderSettings.fog;
        RenderSettings.fog = enabled;
    }

    public void OnPostRender() {
        RenderSettings.fog = toggleFog;
    }

}
