using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class Board : MonoBehaviour
{
    public static Board Instance {get; private set;} //Singleton Pattern
    public GameObject rowPrefab;
    public Sprite succesSprite;


    public List<Row> rows;
    public List<int> lockedRows;
    public Tile[,] tiles {get; private set;}
    private List<Tile> _selection;
    public LevelData currentLevel;

    // public int width => tiles.GetLength(dimension:0);
    // public int height => tiles.GetLength(dimension:1);
    public int width;
    public int height;
    public int moves;
    public int remain_moves;
    // public int high_score;
    public int score;

    
    private const float TweenDuration = 0.25f;


    private void Awake() => Instance = this;

    private void Start(){
        currentLevel = CurrentLevel.currentLevel;
        height = currentLevel.grid_height;
        width = currentLevel.grid_width;
        remain_moves = currentLevel.move_count;
        // high_score = currentLevel.high_score;
        score = 0;
        moves = 0;
        _selection = new List<Tile>();
        lockedRows = new List<int>();

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

        int gridNo = 0;

        for (int i = 0; i < lvl_height; i++)
        {
            for (int j = 0; j < lvl_width; j++)
            {
                var tile = rows[i].tiles[j];

                tile.x = j;
                tile.y = i;

                tile.Item = ItemDatabase.Items[findItem(currentLevel.grid[gridNo])];
                gridNo += 1;
                // tile.Item = ItemDatabase.Items[2]; // TODO find item.id

                tiles[i, j] = tile;
            }
            
        }
    }

    private int findItem(string colour){
        switch (colour)
        {
            case "b":
                {
                    return 0;
                }
            case "g":
                {
                    return 1;
                }
            case "r":
                {
                    return 2;
                }
            case "y":
                {
                    return 3;
                }
            default:
                {
                    return 0;
                }
        }
    }

    public async void Select(Tile tile){
        if(!_selection.Contains(tile) && !isRowLocked(tile.y)){
            _selection.Add(tile);
        }
        
        if(_selection.Count < 2) return;

        Debug.Log(message:$"Selected tiles: ({_selection[0].y}, {_selection[0].x}) and ({_selection[1].y}, {_selection[1].x})");

        if(areTilesNeighbour(_selection[0], _selection[1])){
            await Swap(_selection[0], _selection[1]);
            checkRows(_selection[0], _selection[1]);
            moves += 1;
        }
            
        
        
        _selection.Clear();

    }

    public async Task Swap(Tile tile1, Tile tile2){
        var icon1 = tile1.icon;
        var icon2 = tile2.icon;

        var icon1Transform = icon1.transform;
        var icon2Transform = icon2.transform;

        var sequence = DOTween.Sequence();

        sequence.Join(icon1Transform.DOMove(icon2Transform.position, TweenDuration))
                .Join(icon2Transform.DOMove(icon1Transform.position, TweenDuration));

        await sequence.Play().AsyncWaitForCompletion();


        icon1Transform.SetParent(tile2.transform);
        icon2Transform.SetParent(tile1.transform);

        tile1.icon = icon2;
        tile2.icon = icon1;

        var tile1Item = tile1.Item;

        tile1.Item = tile2.Item;
        tile2.Item = tile1Item;

    }

    public bool areTilesNeighbour(Tile tile1, Tile tile2){
        if((tile1.x == tile2.x && Mathf.Abs(tile1.y-tile2.y) == 1) || (tile1.y == tile2.y && Mathf.Abs(tile1.x-tile2.x) == 1)) return true;
        return false;
    }

    public bool checkRowMatch(int row){
        Item temp = tiles[row,0].Item;
        bool is_match = true;
        for (int i = 1; i < width; i++)
        {
            if (tiles[row,i].Item != temp)
            {
                is_match = false;
                break;
            }
        }
        return is_match;
    }
    public void checkRows(Tile tile1, Tile tile2){
        int row1 = tile1.y;
        int row2 = tile2.y;
        bool is_match1;
        bool is_match2;
        if (row1 == row2)
        {
            return;
        }
        
        is_match1 = checkRowMatch(row1);
        is_match2 = checkRowMatch(row2);

        if (is_match1){
            score += tile1.Item.value;
            lockRow(row1);
        }
        if (is_match2)
        {
            score += tile2.Item.value;
            lockRow(row2);
        }

    }

    

    public void lockRow(int row){
        for (int i = 0; i < width; i++)
        {
            tiles[row, i].icon.transform.DOScale(1.25f, TweenDuration).Play();
            tiles[row, i].icon.sprite = succesSprite;
            // tiles[row, i].Item.sprite = succesSprite;
            //TODO icon success değiştir
        }
        lockedRows.Add(row);
    }

    public bool isRowLocked(int row){
        if(!lockedRows.Contains(row))
            return false;
        return true;
    }

}
