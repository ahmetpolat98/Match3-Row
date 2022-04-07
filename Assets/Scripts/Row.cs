using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Row : MonoBehaviour
{
    public GameObject tilePrefab;
    public List<Tile> tiles;

    void Awake(){
        // tiles = new List<Tile>();
    }

    public void addTile(){
        GameObject node = Instantiate(tilePrefab);
        node.transform.SetParent(this.transform);
        node.transform.localScale= new Vector3(1,1,1);
        tiles.Add(node.GetComponent<Tile>());
    }

}
