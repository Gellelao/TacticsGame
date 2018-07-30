using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : ScriptableObject {

	public static List<Node> FindPath(Node start, Node target, int jumpLimit){
		List<Node> open   = new List<Node>();
		HashSet<Node> closed = new HashSet<Node>();
		open.Add(start);

		while(open.Count > 0){
			// Sort by lowest f (I hope)
			//open.Sort((n1,n2)=>n1.f.CompareTo(n2.f));
			Node current = open[0];
			for (int i = 1; i < open.Count; i ++) {
				if (open[i].f < current.f || open[i].f == current.f) {
					if (open[i].h < current.h)
						current = open[i];
				}
			}

			open.Remove(current);
			closed.Add(current);

			if(current.Equals(target)){
				return Retrace(start, target);
			}

			List<Node> neighbours = current.GetNeighbours();
			foreach(Node neighbour in neighbours){
				if(closed.Contains(neighbour) || !Jumpable(current, neighbour, jumpLimit) || !neighbour.tag.Contains("Traversable") || 
				  // Can't walk past enemies
				  // Will have to change this when enemies start pathfinding, so they can walk past their allies but not player units
				  (neighbour.occupant != null && !(neighbour.occupant is PlayerUnitController))){
					continue;
				}
				// The real shit
				int newMovementCostToNeighbour = (int)(current.g + GetDistance(current, neighbour));
				if(newMovementCostToNeighbour < neighbour.g || !open.Contains(neighbour)){
					neighbour.g = newMovementCostToNeighbour;
					neighbour.h = GetDistance(neighbour, target);
					neighbour.parent = current;

					if(!open.Contains(neighbour)){
						open.Add(neighbour);
					}
				}
			}
		}
		return new List<Node>();
	}

	private static bool Jumpable(Node current, Node neighbour, int jumpLimit){
        // Trying out a mechanic where you can drop down farther than you can jump up
        float diff = neighbour.height - current.height;
        if(diff > 0 && diff <= jumpLimit)return true;
        if(diff < 0 && diff >= -(jumpLimit+1))return true;
        if(diff == 0)return true;
        return false;
    }

	private static List<Node> Retrace(Node start, Node target){
		List<Node> path = new List<Node>();
		Node current = target;

		while(current != start){
			path.Add(current);
			current = current.parent;
		}

		path.Reverse();
		return path;
	}

	private static int GetDistance(Node nodeA, Node nodeB){
		int dstX = (int)Mathf.Abs(nodeA.transform.position.x - nodeB.transform.position.x);
		int dstY = (int)Mathf.Abs(nodeA.transform.position.y - nodeB.transform.position.y);

		return dstX + dstY;
	}

}

