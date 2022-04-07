using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public GameObject rowPrefab;

    // public static Board Instance {get; private set;} //Singleton Pattern
    public List<Row> rows;
    public Tile[,] tiles {get; private set;}

    public int width => tiles.GetLength(dimension:0);
    public int height => tiles.GetLength(dimension:1);

    // private void Awake() => Instance = this;

    private void Start(){   
        // tiles = new Tile[5, 5];//Tile[rows.Max(selector:row => row.tiles.Length), rows.Length]

        // for (int i = 0; i < height; i++)
        // {
        //     for (int j = 0; j < width; j++)
        //     {
        //         var tile = rows[i].tiles[j];

        //         tile.x = j;
        //         tile.y = i;
            
        //         tile.Item = ItemDatabase.Items[Random.Range(0, ItemDatabase.Items.Length)];

        //         tiles[j, i] = tile;
        //     }
        // }
        // GameObject node = Instantiate(rowPrefab);
        // node.transform.SetParent(this.transform);
        // node.transform.localScale= new Vector3(1,1,1);

        createBoard(5,5);

    }


    public void createBoard(int lvl_width, int lvl_height){
        for (int i = 0; i < lvl_height-4; i++)
        {
            addRow();
        }
        for (int i = 0; i < lvl_height; i++)
        {
            for (int j = 0; j < lvl_width-4; j++)
            {
                rows[i].addTile();
            }
        }


    }

    public void addRow(){
        GameObject new_row = Instantiate(rowPrefab);
        new_row.transform.SetParent(this.transform);
        new_row.transform.localScale= new Vector3(1,1,1);
        rows.Add(new_row.GetComponent<Row>());
    }


}
