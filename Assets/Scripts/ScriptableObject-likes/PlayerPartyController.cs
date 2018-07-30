using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPartyController : MonoBehaviour {
	public int maxPartySize;
	private string[] party;
	string placing;
	//private int index;

	PlayerUnitController selected;

	void Start () {
		party = new string[maxPartySize];
		placing = null;
		FillParty();
		DeployParty(1);
		//index = 0;
	}

	void Update(){
		if(placing != null){
			if(Input.GetMouseButtonDown(0)){
				Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
				RaycastHit hit;
			
				if( Physics.Raycast( ray, out hit, 100 ) ){
					if(hit.transform.gameObject.tag.Contains("SpawnArea")){
						GameObject unitInstantiated = Create(placing, hit.transform.gameObject);
						unitInstantiated.transform.parent = gameObject.transform;
						placing = null;
					}
				}
			}
		}
	}

	public void SetSelected(PlayerUnitController p){
		selected = p;
	}

	public void selectedMove(){
		selected.move();
	}
	public void selectedAttack(){
		selected.attack();
	}

	public void FillParty(){
		party[0] = "class0";
		party[1] = "class1";
		party[2] = "class2";
	}

	// public void AddUnit(int classId){
	// 	party[index] = "class" + classId;
	// 	if(index < maxPartySize-1){
	// 		index++;
	// 	}
	// 	Debug.Log("index: " + index + ", slot: " + party[index]);
	// }

	public void DeployParty(int index){
		placing = party[index];
	}

	public GameObject Create(string className, GameObject tile){
		Object prefab = Resources.Load(className);
		GameObject playerUnit = Instantiate(prefab) as GameObject;
   		UnitController pcControl = playerUnit.GetComponent<UnitController>();
		pcControl.SetTile(tile);
   		pcControl.AlignToTile();
		
   		PlayerUnitController puControl = playerUnit.GetComponent<PlayerUnitController>();
		puControl.SetParent(this);

   		return playerUnit;
	}
}
