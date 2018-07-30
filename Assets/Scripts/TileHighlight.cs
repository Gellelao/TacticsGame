using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHighlight : MonoBehaviour {

	// This shares a lot of code with SelectedIndicator and UnitController but couldn't see a way to reuse

	public Bounds GetBounds(GameObject g){
		Mesh mesh = g.GetComponent<MeshFilter>().mesh;
		mesh.RecalculateBounds();

		MeshFilter mf = g.GetComponent<MeshFilter>();
		return mf.sharedMesh.bounds;
	}
		
	public void AlignToObject(GameObject other){
		Bounds unitBounds = GetBounds(other);
		Vector3 halfOtherHeight = new Vector3(0, unitBounds.extents.y*other.transform.localScale.y, 0);

		//Bounds indicatorBounds = GetBounds(gameObject);
		//Vector3 halfIndicatorHeight = new Vector3(0, indicatorBounds.extents.y, 0);
	
		transform.position = other.transform.position+halfOtherHeight;
	}
}
