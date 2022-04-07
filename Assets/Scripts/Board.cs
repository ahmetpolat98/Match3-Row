using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public GameObject rowPrefab;

    // public static Board Instance {get; private set;} //Singleton Pattern
    public List<Row> rows;
    public Tile[,] tiles {get; private set;}

    // public int width => tiles.GetLength(dimension:0);
    // public int height => tiles.GetLength(dimension:1);
    public int width;
    public int height;

    private List<Tile> _selection = new List<Tile>();

    // private void Awake() => Instance = this;

    private void Start(){   
        height = 9;
        width = 4;

        createBoard(height,width);
        initTiles(height, width);       

    }


    //lvlin height ve widthine göre boardu oluşturuyor. default 4x4 board hazır(en az board büyüklüğü olduığu için) kalan width ve height a göre boarda row eklenir. row a da tile eklenir.
    public void createBoard(int lvl_height, int lvl_width){
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

    //boardın rows listesine row ekleme, prefabı alınan rows'u oyun objesi olarak boardın childı olarak ekliyip, listeye ekliyorum.
    public void addRow(){
        GameObject new_row = Instantiate(rowPrefab);
        new_row.transform.SetParent(this.transform);
        new_row.transform.localScale= new Vector3(1,1,1);
        rows.Add(new_row.GetComponent<Row>());
    }

    //tiles arrayini oluşturup, içerisine itemlerı yerleştirme
    public void initTiles(int lvl_height, int lvl_width){
        tiles = new Tile[lvl_height, lvl_width];

        for (int i = 0; i < lvl_height; i++)
        {
            for (int j = 0; j < lvl_width; j++)
            {
                var tile = rows[i].tiles[j];

                tile.x = j;
                tile.y = i;

                // tile.Item = ItemDatabase.Items[Random.Range(0, ItemDatabase.Items.Length)];
                tile.Item = ItemDatabase.Items[2]; //find item.id

                tiles[i, j] = tile;
            }
            
        }
    }

    public void Select(Tile tile){
        if(_selection.Contains(tile))
        _selection.Add(tile);

    }


}
