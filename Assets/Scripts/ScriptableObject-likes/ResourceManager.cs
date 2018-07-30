using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : ScriptableObject {
	private static GameObject selectionIndicator;
	private static Object moveHighlight;
	private static Object attackHighlight;
	private static Object attackUnitHighlight;
	private static GameObject[,,] tileArray;
	private static GameObject playerParty;
	private static Object healthBar;
	private static Object dummy;

	public static void Initialize () {

		Object indicator = Resources.Load("SelectedIndicator");
		selectionIndicator = Instantiate(indicator) as GameObject;

		moveHighlight = Resources.Load("HighlightMove");
		// attackHighlight = Resources.Load("HighlightAttack");
		// attackUnitHighlight = Resources.Load("HighlightAttackUnit");
		
		healthBar = Resources.Load("HealthBarHolder");
		dummy = Resources.Load("DummyTarget");

		playerParty = GameObject.Find("PlayerParty");

		// Hardcoding this feels awful but will do for now
		// 30x30x30 tile limit on maps for now
		tileArray = new GameObject[30,30,30];
		FillTileArray();
		SetTraversability();
	}

	public static GameObject GetIndicator(){
		return selectionIndicator;
	}

	public static GameObject GetMoveHighlight(){
		return Instantiate(moveHighlight) as GameObject;
	}
	public static GameObject GetAttackHighlight(){
		return Instantiate(moveHighlight) as GameObject;
	}
	public static GameObject GetAttackUnitHighlight(){
		return Instantiate(moveHighlight) as GameObject;
	}

	public static GameObject GetHealthBar(){
		return Instantiate(healthBar) as GameObject;
	}

	public static GameObject GetDummy(){
		return Instantiate(dummy) as GameObject;
	}

	private static void FillTileArray(){
		GameObject parent = GameObject.Find("Tiles");
		Transform[] children;
		children = parent.GetComponentsInChildren<Transform>();
		foreach (Transform child in children) {
			int x = (int)child.transform.position.x;
			int y = (int)(child.transform.position.y*2);
			int z = (int)child.transform.position.z;
			tileArray[x, y, z] = child.gameObject;
		}
	}

	// Goes through all the tiles in the array checking if they are 1 or two tiles below another tile
	// if so, they are set to be non-traversable
	private static void SetTraversability(){
		for(int x = 0; x < tileArray.GetLength(0); x++){
			for(int y = 0; y < tileArray.GetLength(1); y++){
				for(int z = 0; z < tileArray.GetLength(2); z++){
					if(tileArray[x,y,z]){
						if(tileArray[x,y+1,z] || tileArray[x,y+2,z]){
							tileArray[x,y,z].tag = "Untagged";
						}
					}
				}
			}
		}
	}

	public static GameObject[,,] GetTileArray(){
		return tileArray;
	}

	public static void ClearHighlights(){
		foreach (GameObject tile in tileArray) {
			if(tile){
				tile.GetComponent<Node>().SetHighlight(false);
			}
		}
	}

	public static void DeselectPlayerUnits(){
		PlayerUnitController[] children;
		children = playerParty.GetComponentsInChildren<PlayerUnitController>();
		foreach (PlayerUnitController child in children) {
			child.Deselect();
		}
	}
}
