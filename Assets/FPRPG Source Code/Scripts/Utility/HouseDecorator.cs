using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 0414

public class HouseDecorator : MonoBehaviour {

    public Transform decorationHolder;

    public List<GameObject> furnitureMountPoints;
    public List<GameObject> wallMountPoints;

    public GameObject carpetPrefab;
    public List<GameObject> furniturePrefabs;
    public List<GameObject> wallFurniturePrefabs;

    public int furnitureDecorated = 0;

    private int bedCount = 0;
    private int fireplaceCount = 0;
    private int tableCount = 0;

#if UNITY_EDITOR
    public void DecorateHouse() {
        if (decorationHolder != null) {

            if(decorationHolder.childCount > 0) {
                EraseDecoration();
            }

            if (furnitureMountPoints.Count > 0) {
                if (furnitureDecorated < furnitureMountPoints.Count) {
                    foreach (GameObject f in furnitureMountPoints) {
                        if (f.GetComponent<HouseDecoratorMountPoint>().hasFurniture == false) {
                            if (f.name == "(CarpetPlace)") {
                                GameObject g = UnityEditor.PrefabUtility.InstantiatePrefab(carpetPrefab) as GameObject;
                                g.transform.position = f.transform.position;
                                g.transform.rotation = f.transform.rotation;
                                g.transform.parent = decorationHolder;
                                f.GetComponent<HouseDecoratorMountPoint>().hasFurniture = true;
                            }
                            else {
                                GameObject g = UnityEditor.PrefabUtility.InstantiatePrefab(furniturePrefabs[Random.Range(0, furniturePrefabs.Count)]) as GameObject;
                                if ((bedCount == 1 && g.name.Contains("Bed")) || (fireplaceCount == 1 && g.name.Contains("Fireplace")) ||
                                    (tableCount == 1 && g.name.Contains("Table"))) {
                                    DestroyImmediate(g);
                                }
                                else {
                                    g.transform.position = f.transform.position;
                                    g.transform.rotation = f.transform.rotation;
                                    g.transform.parent = decorationHolder;
                                    furnitureDecorated++;
                                    if (g.name.Contains("Bed")) {
                                        bedCount++;
                                    }
                                    else if (g.name.Contains("Fireplace")) {
                                        fireplaceCount++;
                                    }
                                    else if (g.name.Contains("Table")) {
                                        tableCount++;
                                    }
                                    f.GetComponent<HouseDecoratorMountPoint>().hasFurniture = true;
                                }
                            }
                        }
                    }
                }
            }
            else {
                Debug.LogWarning("No floor furniture set!");
            }

            if (wallMountPoints.Count > 0) {
                foreach (GameObject w in wallMountPoints) {
                    if (w.GetComponent<HouseDecoratorMountPoint>().hasFurniture == false) {
                        GameObject g = UnityEditor.PrefabUtility.InstantiatePrefab(wallFurniturePrefabs[Random.Range(0, wallFurniturePrefabs.Count)]) as GameObject;
                        g.transform.position = w.transform.position;
                        g.transform.rotation = w.transform.rotation;
                        g.transform.parent = decorationHolder;
                        furnitureDecorated++;
                        w.GetComponent<HouseDecoratorMountPoint>().hasFurniture = true;
                    }
                }
            }
            else {
                Debug.LogWarning("No wall furniture set!");
            }
        }
        else {
            Debug.LogWarning("Decoration holder var not set.");
        }
    }

    public void EraseDecoration() {
        if(furnitureDecorated > 0) {
            while (decorationHolder.childCount != 0) {
                DestroyImmediate(decorationHolder.GetChild(0).gameObject);
            }

            foreach(GameObject f in furnitureMountPoints) {
                f.GetComponent<HouseDecoratorMountPoint>().hasFurniture = false;
            }
            foreach (GameObject w in wallMountPoints) {
                w.GetComponent<HouseDecoratorMountPoint>().hasFurniture = false;
            }

            bedCount = 0;
            fireplaceCount = 0;
            tableCount = 0;
            furnitureDecorated = 0;
        }
    }
#endif
}
