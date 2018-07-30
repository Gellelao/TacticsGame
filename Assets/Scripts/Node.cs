using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {
    private GameObject highlight;

    public float height;
    public Node parent;
    
    public float g;
    public float h;

    public float f{
        get{
            return g+h;
        }
    }

    public bool highlit;
    public UnitController occupant;


    void Start(){
        setHighlightToMove();
        highlight.GetComponent<Renderer>().enabled = false;
        highlight.GetComponent<TileHighlight>().AlignToObject(gameObject);
        height = transform.position.y*2;
    }

    private void setHighlightToMove(){
        Destroy(highlight);
        highlight = ResourceManager.GetMoveHighlight();
    }
    // private void setHighlightToAttack(){
    //     Destroy(highlight);
    //     highlight = ResourceManager.GetAttackHighlight();
    // }
    // private void setHighlightToAttackUnit(){
    //     Destroy(highlight);
    //     highlight = ResourceManager.GetAttackUnitHighlight();
    // }

    public List<Node> GetNeighbours(){
        List<Node> nbs = new List<Node>();
        
        int x = (int)transform.position.x;
        //int y = (int)(transform.position.y*2);
        int z = (int)transform.position.z;

        GameObject[,,] tileArray = ResourceManager.GetTileArray();

        // Check is x < tileArray(0) ????
        for(int i = 0; i < tileArray.GetLength(2); i++){
            if(CheckTile(tileArray[x+1, i, z])){
                nbs.Add(tileArray[x+1, i, z].GetComponent<Node>());
            }
        }
        if(x>0){
            for(int i = 0; i < tileArray.GetLength(2); i++){
                if(CheckTile(tileArray[x-1, i, z])){
                    nbs.Add(tileArray[x-1, i, z].GetComponent<Node>());
                }
            }
        }
        // for(int i = 0; i < tileArray.GetLength(2); i++){
        //     if(CheckTile(tileArray[x, i, z])){
        //         nbs.Add(tileArray[x, i, z].GetComponent<Node>());
        //     }
        // }
        // if(y>0){
        //     for(int i = 0; i < tileArray.GetLength(2); i++){
        //         if(CheckTile(tileArray[x, i, z])){
        //             nbs.Add(tileArray[x, i, z].GetComponent<Node>());
        //         }
        //     }
        // }
        for(int i = 0; i < tileArray.GetLength(2); i++){
            if(CheckTile(tileArray[x, i, z+1])){
                nbs.Add(tileArray[x, i, z+1].GetComponent<Node>());
            }
        }
        if(z>0){
            for(int i = 0; i < tileArray.GetLength(2); i++){
                if(CheckTile(tileArray[x, i, z-1])){
                    nbs.Add(tileArray[x, i, z-1].GetComponent<Node>());
                }
            }
        }

        return nbs;
    }

    public void MoveHighlight(int soFar, int moveLimit, int jumpLimit, Node origin){
        //setHighlightToMove();
        SetHighlight(true);
        MoveHighlightNeighbours(soFar, moveLimit, jumpLimit, origin);
        // foreach(Node neighbour in GetNeighbours()){
        //     // if neighbour doesn't have a tile above it(make a field in each Node for this), and is jumpable and within moving distance, highlight it
        //     // Maybe just turn traversable off when theres a tile within 3 tiles above this one
        //     if(neighbour.gameObject.tag.Contains("Traversable") && soFar < moveLimit && CheckJumpable(neighbour, jumpLimit)){
        //         if(neighbour.occupant == null){
        //             neighbour.Highlight(soFar+1, moveLimit, jumpLimit, origin);
        //         }
        //         else{
        //             if(neighbour.occupant is PlayerUnitController){
        //                 HighlightNeighbours(soFar+1, moveLimit, jumpLimit, origin);
        //             }
        //             // This way, you should be able to go past allies but not enemies
        //         }
        //     }
        // }
    }

    // Duplicate of highlight method above, but doesn't highlight itself
    public void MoveHighlightNeighbours(int soFar, int moveLimit, int jumpLimit, Node origin){
        //setHighlightToMove();
        foreach(Node neighbour in GetNeighbours()){
            // if neighbour doesn't have a tile above it(make a field in each Node for this), and is jumpable and within moving distance, highlight it
            // Maybe just turn traversable off when theres a tile within 3 tiles above this one
            if(neighbour.gameObject.tag.Contains("Traversable") && soFar < moveLimit && CheckJumpable(neighbour, jumpLimit)){
                if(neighbour.occupant == null){
                    neighbour.MoveHighlight(soFar+1, moveLimit, jumpLimit, origin);
                }
                else{
				    // Will have to change this when enemies start pathfinding, so they can walk past their allies but not player units
                    if(neighbour.occupant is PlayerUnitController){
                        neighbour.MoveHighlightNeighbours(soFar+1, moveLimit, jumpLimit, origin);
                    }
                    // This way, you should be able to go past allies but not enemies
                }
            }
        }
    }

    public void AttackHighlight(int soFar, int attackRange, Node origin){
        //setHighlightToAttack();
        SetHighlight(true);
        AttackHighlightNeighbours(soFar, attackRange, origin);
    }

    public void AttackHighlightNeighbours(int soFar, int attackRange, Node origin){
        //setHighlightToAttack();
        foreach(Node neighbour in GetNeighbours()){
            if(soFar < attackRange){
                if(neighbour.occupant == null){
                    neighbour.AttackHighlight(soFar+1, attackRange, origin);
                }
                else if(neighbour.occupant.tag.Contains("Enemy")){
                    neighbour.AttackHighlight(soFar+1, attackRange, origin);
                    //setHighlightToAttackUnit();
                }
                else if(neighbour.occupant is PlayerUnitController){
                    neighbour.AttackHighlightNeighbours(soFar+1, attackRange, origin);
                }
            }
        }
    }

    public void SetHighlight(bool state){
        highlight.GetComponent<Renderer>().enabled = state;
        highlit = state;
    }

    private bool CheckJumpable(Node n, int jumpLimit){
        // Trying out a mechanic where you can drop down farther than you can jump up
        float diff = n.height - this.height;
        if(diff > 0 && diff <= jumpLimit)return true;
        if(diff < 0 && diff >= -(jumpLimit+1))return true;
        if(diff == 0)return true;
        return false;
    }

    private bool CheckTile(GameObject tile){
        if(tile == null){
            return false;
        }
        if(tile.GetComponent<Node>() == null){
            return false;
        }
        // We could also maybe do other checks here like seeing if the tile is directly below a solid object
        // But those checks might be better in the places where the neighbours are used, as the GetNeighbour method should probably only get neighbours
        return true;
    }
}
