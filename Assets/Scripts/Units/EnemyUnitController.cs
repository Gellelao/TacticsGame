using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnitController : UnitController {
	private bool selected;
	private bool mouseOver;

	public virtual void Start() {
		AlignToTile();
		tile.GetComponent<Node>().occupant = this;
		selected = false;
		moveDistance = 3;
		attackDistance = 2;
		jumpDistance = 1;
		hitpoints = 20;
		damage = 5;
		defense = 2;
		speed = 3;

		Initialize();
	}

	protected override void SpecificUpdate () {
		// Moving
		if(selected){
			//StartPathingTo(gameObject);
     	}
		// Selecting
        if (mouseOver && Input.GetButtonDown("Fire1")){
            selected = true;
			print(EnemyAI.NumberOfTargetsInRange(tile.GetComponent<Node>(), attackDistance));
			StartPathingTo(EnemyAI.FindNodeToMoveTo(tile.GetComponent<Node>(), moveDistance, attackDistance));
        }
    }

    void OnMouseOver(){
        mouseOver = true;
    }

    void OnMouseExit(){
        mouseOver = false;
    }

	public void Deselect(){
		selected = false;
	}
}
