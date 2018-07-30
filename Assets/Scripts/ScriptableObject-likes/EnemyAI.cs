using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : ScriptableObject {

	public static Node FindNodeToMoveTo(Node origin, int moveRange, int attackRange){
		List<Node> toCheck = new List<Node>();
		List<Node> nodesThatCanAttackTargets = new List<Node>();
		toCheck.Add(origin);
		for(int i=0; i<moveRange; i++){
			List<Node> temp = new List<Node>();
			foreach(Node n in toCheck){
				temp.AddRange(n.GetNeighbours());
				
				if(NumberOfTargetsInRange(n, attackRange) > 0 && n.occupant==null && !nodesThatCanAttackTargets.Contains(n)){
					nodesThatCanAttackTargets.Add(n);
				}
			}
			toCheck = temp;
		}
		if(nodesThatCanAttackTargets.Count > 0){
			MonoBehaviour.print("Number of nodes that can attack targets (EnemyAI): " + nodesThatCanAttackTargets.Count);
			return nodesThatCanAttackTargets[Random.Range(0, nodesThatCanAttackTargets.Count)];
		}
		// Later extend it to find the furthest distance it can attack a unit from and pick that one
		// Maybe a "score" system which returns a number rating the tiles based on the stats and number of opponents that can be reached.
		// Score of zero if no opponents in range.

		// Hmm, actually need to score based on distance so that the ai can move towards targets even when they are not in range

		// Can't reach any targets, so return random tile
		return toCheck[Random.Range(0, toCheck.Count)];
	}

	public static int NumberOfTargetsInRange(Node origin, int attackRange){
		HashSet<Node> toCheck = new HashSet<Node>();
		HashSet<Node> targets = new HashSet<Node>();
		toCheck.Add(origin);
		for(int i=0; i<attackRange+1; i++){
			HashSet<Node> temp = new HashSet<Node>();
			foreach(Node n in toCheck){
				temp.UnionWith(n.GetNeighbours());
				
				if(n.occupant && n.occupant.tag.Contains("PlayerUnit")){
					targets.Add(n);
				}
			}
			toCheck = temp;
		}
		return targets.Count;
	}
}

