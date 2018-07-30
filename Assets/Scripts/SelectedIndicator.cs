using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedIndicator : MonoBehaviour {

	void Start () {
		gameObject.GetComponent<Renderer>().enabled = false;
	}

	public Bounds GetBounds(GameObject g){
		Mesh mesh = g.GetComponent<MeshFilter>().mesh;
		mesh.RecalculateBounds();

		MeshFilter mf = g.GetComponent<MeshFilter>();
		return mf.sharedMesh.bounds;
	}
		
	public void AlignToObject(GameObject other){
		Bounds unitBounds = GetBounds(other);
		Vector3 halfOtherHeight = new Vector3(0, unitBounds.extents.y*other.transform.localScale.y, 0);

		Bounds indicatorBounds = GetBounds(gameObject);
		Vector3 halfIndicatorHeight = new Vector3(0, indicatorBounds.extents.y, 0);
	
		transform.position = other.transform.position+halfOtherHeight+halfIndicatorHeight;

		gameObject.GetComponent<Renderer>().enabled = true;
	}
}
