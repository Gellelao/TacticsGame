using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnitController : UnitController {
	private bool selected;
	private bool mouseOver;
	private PlayerPartyController parent;
	private GameObject selectedIndicator;

	public enum state {idle, moving, attacking};
	private state STATE;

	public virtual void Start() {
		selected = false;
		selectedIndicator = ResourceManager.GetIndicator();
		moveDistance = 5;
		attackDistance = 3;
		jumpDistance = 1;
		hitpoints = 20;
		damage = 5;
		defense = 2;
		speed = 3;

		Initialize();
		//STATE = state.idle;
	}

	protected override void SpecificUpdate () {
		// Update this to be state-based so that the units actions can be set via the PlayerPartyController using buttons on the interface

		// Moving
		if(selected){
			selectedIndicator.GetComponent<SelectedIndicator>().AlignToObject(gameObject);
			if(Input.GetMouseButtonDown(0)){
				Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
				RaycastHit hit;
				// Probably convert these STATE ifs into a switch
				if(STATE == state.moving){
					if( Physics.Raycast( ray, out hit, 100 ) ){
						if(hit.transform.gameObject.tag.Contains("Traversable") && hit.transform.gameObject.GetComponent<Node>().highlit){
							ResourceManager.ClearHighlights();
							StartPathingTo(hit.transform.gameObject.GetComponent<Node>());
							STATE = state.idle;
						}
					}
				}
				if(STATE == state.attacking){
					if( Physics.Raycast( ray, out hit, 100 ) ){
						GameObject o = hit.transform.gameObject;
						if(o.GetComponent<Node>() && o.GetComponent<Node>().highlit){
							if(o.GetComponent<Node>().occupant && o.GetComponent<Node>().occupant.tag.Contains("Enemy")){
							ResourceManager.ClearHighlights();
							// Attack the object at the tile that has been clicked on
							Attack(o.GetComponent<Node>().occupant);
							STATE = state.idle;
							}
						}
						else if(o.tag.Contains("Enemy") && o.GetComponent<UnitController>().GetTile().GetComponent<Node>().highlit){
							ResourceManager.ClearHighlights();
							// Attack the object that has been clicked on
							Attack(o.GetComponent<UnitController>());
							STATE = state.idle;
						}
					}
				}
			
			}
			// I'm assuming we don't want to continue because the unit is already selected
     	}
		// Selecting
        if (mouseOver && Input.GetButtonDown("Fire1")){
			ResourceManager.ClearHighlights();
			ResourceManager.DeselectPlayerUnits();
			Select();
        }
    }

	public void SetParent(PlayerPartyController p){
		this.parent = p;
	}

	public void move(){
		STATE = state.moving;
		ResourceManager.ClearHighlights();
		tile.GetComponent<Node>().MoveHighlightNeighbours(0, moveDistance, jumpDistance, tile.GetComponent<Node>());
	}

	public void attack(){
		STATE = state.attacking;
		ResourceManager.ClearHighlights();
		tile.GetComponent<Node>().AttackHighlightNeighbours(0, attackDistance, tile.GetComponent<Node>());
	}

    void OnMouseOver(){
        mouseOver = true;
    }

    void OnMouseExit(){
        mouseOver = false;
    }

	public void Select(){
        selected = true;
		parent.SetSelected(this);
	}

	public void Deselect(){
		selected = false;
		parent.SetSelected(null);
	}
}
