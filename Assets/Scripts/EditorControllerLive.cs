using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EditorControllerLive : MonoBehaviour {
    private GameObject preview;
    public GameObject tileToPlace;
    private Vector3 cursor;
    private GameObject tileCollection;

    void Start() {
        cursor = Vector3.zero;
        preview = GameObject.Find("Tile Preview");
        tileCollection = GameObject.Find("Tiles");
    }

    public void UpdateLive(Event e) {
        if(e.modifiers.ToString() == "Shift"){
            if(e.keyCode.ToString() == "W"){
                cursor.y += 0.5f;
            }
            if(e.keyCode.ToString() == "S"){
                cursor.y -= 0.5f;
            }
        }
        else if(e.keyCode.ToString() == "W"){
            cursor.z += 1;
        }
        else if(e.keyCode.ToString() == "A"){
            cursor.x -= 1;
        }
        else if(e.keyCode.ToString() == "S"){
            cursor.z -= 1;
        }
        else if(e.keyCode.ToString() == "D"){
            cursor.x += 1;
        }

        preview.transform.position = cursor;

        if(e.keyCode.ToString() == "Space"){
            GameObject thisTile = Instantiate(tileToPlace, cursor, Quaternion.identity);
            thisTile.transform.parent = tileCollection.transform;
        }
    }
}
