using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour {
	public GameObject tile;
	public GameObject targetTile;
	private GameObject dummyTarget;
	public GameObject healthBar;
	
	public int moveDistance;
	public int attackDistance;
	public int jumpDistance;
	public float hitpoints;
	public float damage;
	public float defense;
	public float speed;

	private List<Node> path;
	private int pathIndex;

	void Update(){
		if(targetTile){
			float step = speed * Time.deltaTime;
			// if((int)transform.position.y != (int)dummyTarget.transform.position.y){
			// 	Debug.Log("heights not equal");
			// 	// These next two lines only need to happen once per tile move but would have to make another field for it...
			// 	Vector3 targetHeight = new Vector3(0, 0, 0);
			// 	targetHeight.Set(transform.position.x, dummyTarget.transform.position.y, transform.position.z);

			// 	transform.position = Vector3.MoveTowards(transform.position, targetHeight, step);
			// }
			// else 
			if(transform.position.x != targetTile.transform.position.x || transform.position.z != targetTile.transform.position.z){
				transform.position = Vector3.MoveTowards(transform.position, dummyTarget.transform.position, step);
				UpdateHealthBarPos();
			}
			else{
				SetTile(targetTile);

				if(pathIndex < path.Count-1){
					pathIndex++;
					StartMovingToTile(path[pathIndex].gameObject);
				}
			}
		}

		SpecificUpdate();
	}

	protected virtual void SpecificUpdate(){

	}

	protected void Initialize(){
		// Dummy Target
		dummyTarget = ResourceManager.GetDummy();
		dummyTarget.GetComponent<Collider>().enabled = false;
		dummyTarget.GetComponent<Renderer>().enabled = false;

		// Health Bar initialization
		healthBar = ResourceManager.GetHealthBar();
		healthBar.GetComponent<HealthBarController>().SetMaxHP(hitpoints);
		healthBar.GetComponent<HealthBarController>().SetHP(hitpoints);
		UpdateHealthBarPos();
	}

	protected void UpdateHealthBarPos(){
		Vector3 screenPos = Camera.main.WorldToScreenPoint(this.gameObject.transform.position);
		screenPos.y-=20;
		healthBar.GetComponent<HealthBarController>().SetPos(screenPos);
	}

	public void Attack(UnitController target){
		target.Take(damage);
	}

	public void Take(float damage){
		float afterDefense = damage-defense;
		if(afterDefense > 0){
			this.hitpoints = Mathf.Max(0, hitpoints-(afterDefense));
			healthBar.GetComponent<HealthBarController>().SetHP(hitpoints);
		}
	}

	public void SetTile(GameObject t){
		this.tile = t;
		tile.GetComponent<Node>().occupant = this;
	}
	public GameObject GetTile(){
		return this.tile;
	}

	public void StartPathingTo(Node target){
		SetPath(Pathfinder.FindPath(tile.GetComponent<Node>(), target, jumpDistance));
	}

	public void SetPath(List<Node> pathP){
		this.path = pathP;
		pathIndex = 0;
		if(path.Count > 0){
			StartMovingToTile(path[0].gameObject);
		}
	}

	public void StartMovingToTile(GameObject target){
		AlignToTile(dummyTarget.transform, target);
		// We are no longer occupying our tile
		tile.GetComponent<Node>().occupant = null;
		targetTile = target;
	}

	public Bounds GetBounds(GameObject g){
		Mesh mesh = g.GetComponent<MeshFilter>().mesh;
		mesh.RecalculateBounds();

		MeshFilter mf = g.GetComponent<MeshFilter>();
		return mf.sharedMesh.bounds;
	}

	public void AlignToTile(Transform t, GameObject tileToAlignTo){
		Bounds unitBounds = GetBounds(t.gameObject);
		Vector3 halfUnitHeight = new Vector3(0, unitBounds.extents.y*t.localScale.y, 0);

		Bounds tileBounds = GetBounds(tileToAlignTo);
		Vector3 halfTileHeight = new Vector3(0, tileBounds.extents.y/2, 0);
		t.position = tileToAlignTo.transform.position+halfUnitHeight+halfTileHeight;
	}

	public void AlignToTile(){
		Bounds unitBounds = GetBounds(gameObject);
		Vector3 halfUnitHeight = new Vector3(0, unitBounds.extents.y*transform.localScale.y, 0);

		Bounds tileBounds = GetBounds(tile);
		Vector3 halfTileHeight = new Vector3(0, tileBounds.extents.y/2, 0);
		transform.position = tile.transform.position+halfUnitHeight+halfTileHeight;
	}
}
