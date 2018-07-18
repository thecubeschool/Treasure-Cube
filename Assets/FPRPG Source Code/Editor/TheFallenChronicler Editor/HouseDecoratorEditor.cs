using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(HouseDecorator))]
public class HouseDecoratorEditor : Editor {

    public override void OnInspectorGUI() {

        HouseDecorator houseDecorator = (HouseDecorator)target;

        if (GUILayout.Button("Decorate House", GUILayout.Width(150), GUILayout.Height(30))) {
            houseDecorator.DecorateHouse();
        }
        if (GUILayout.Button("Erase Decoration", GUILayout.Width(150), GUILayout.Height(30))) {
            houseDecorator.EraseDecoration();
        }
    }
}
