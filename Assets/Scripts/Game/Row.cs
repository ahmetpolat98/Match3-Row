using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//row on the board
public class Row : MonoBehaviour
{
    public GameObject tilePrefab;
    public List<Tile> tiles;

    void Awake(){
        // tiles = new List<Tile>();
    }

    //function adding column to row
    public void addTile(){
        GameObject node = Instantiate(tilePrefab);
        node.transform.SetParent(this.transform);
        node.transform.localScale= new Vector3(1,1,1);
        tiles.Add(node.GetComponent<Tile>());
    }

}
