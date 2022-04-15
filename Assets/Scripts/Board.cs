using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    public static Board Instance {get; private set;} //Singleton Pattern

    public GameObject rowPrefab;
    public Sprite succesSprite;
    public Text scoreText;
    public Text remainMovesText;
    public Text highestScoreText;

    public List<Row> rows;
    private List<int> lockedRows;
    public Tile[,] tiles {get; private set;}
    private List<Tile> _selection;
    private LevelData currentLevel;

    private int width;
    private int height;
    private int moves;
    private int remain_moves;
    private int score;

    private const float TweenDuration = 0.25f;


    private void Awake() => Instance = this;

    private void Start(){
        currentLevel = CurrentLevel.currentLevel; //Receiving the data of the level opened to be played.
        height = currentLevel.grid_height;
        width = currentLevel.grid_width;
        remain_moves = currentLevel.move_count;

        score = 0;
        moves = 0;
        _selection = new List<Tile>();
        lockedRows = new List<int>();

        createBoard(height,width);
        initTiles(height, width);       

        updateText();

    }


    //The board is created as 4x4 by default. Row and tile are added to the board according to the height and width of the level.
    private void createBoard(int lvl_height, int lvl_width){
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

    //adding row
    private void addRow(){
        GameObject new_row = Instantiate(rowPrefab);
        new_row.transform.SetParent(this.transform);
        new_row.transform.localScale= new Vector3(1,1,1);
        rows.Add(new_row.GetComponent<Row>());
    }

    
    //Creating an array of tiles and placing items in it
    private void initTiles(int lvl_height, int lvl_width){
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

    //Function to select tiles. Non-adjacent and tile selection conditions are checked. The swap function of tiles is called. The conditional at the end of the game is evaluated.
    public async void Select(Tile tile){
        if(!_selection.Contains(tile) && !isRowLocked(tile.y)){
            _selection.Add(tile);
        }
        
        if(_selection.Count < 2) return;

        Debug.Log(message:$"Selected tiles: ({_selection[0].y}, {_selection[0].x}) and ({_selection[1].y}, {_selection[1].x})");

        bool is_remain_move = isRemainMove();

        if(areTilesNeighbour(_selection[0], _selection[1]) && is_remain_move){
            await Swap(_selection[0], _selection[1]);
            checkRows(_selection[0], _selection[1]);
            remain_moves -= 1;
            moves += 1;
            updateText();
        }
        _selection.Clear();

        if (remain_moves == 0 || !checkPossibleMatch())
        {
            endGame();
        }

    }

    //Swap operation and animation of 2 tiles are performed.
    private async Task Swap(Tile tile1, Tile tile2){
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

    private bool areTilesNeighbour(Tile tile1, Tile tile2){
        if((tile1.x == tile2.x && Mathf.Abs(tile1.y-tile2.y) == 1) || (tile1.y == tile2.y && Mathf.Abs(tile1.x-tile2.x) == 1)) return true;
        return false;
    }

    //It is checked whether the row that comes as a parameter is match.
    private bool checkRowMatch(int row){
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

    //After the swap, the rows between the swapped tiles are checked. If the same row has been changed, it will not enter the match control.
    private void checkRows(Tile tile1, Tile tile2){
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

    //It is checked whether there is a row on the board that can be matched. If not, the game is over.
    private bool checkPossibleMatch(){
        int red_count = 0;
        int blue_count = 0;
        int yellow_count = 0;
        int green_count = 0;

        for (int i = 0; i < height; i++)
        {
            if(lockedRows.Contains(i)){
                red_count = 0;
                blue_count = 0;
                yellow_count = 0;
                green_count = 0;
                continue;
            }
            for (int j = 0; j < width; j++)
            {
                
                switch (tiles[i, j].Item.id)
                {
                    case 0:
                        {
                            blue_count += 1;
                            break;
                        }
                    case 1:
                        {
                            green_count += 1;
                            break;
                        }
                    case 2:
                        {
                            red_count += 1;
                            break;
                        }
                    case 3:
                        {
                            yellow_count += 1;
                            break;
                        }
                }
            }
            if(red_count >= width || blue_count >= width || yellow_count >= width || green_count >= width){
                    return true;
            }
        }
        Debug.Log("No possible Match!");
        return false;
    }


    private bool isRemainMove(){
        if (remain_moves <= 0)
        {
            return false;
        }
        return true;
    }

    //Locks the matched row
    private void lockRow(int row){
        for (int i = 0; i < width; i++)
        {
            tiles[row, i].icon.transform.DOScale(1.25f, TweenDuration).Play();
            tiles[row, i].icon.sprite = succesSprite;
        }
        lockedRows.Add(row);
    }

    private bool isRowLocked(int row){
        if(!lockedRows.Contains(row))
            return false;
        return true;
    }

    //Performs the scene transition when the game is over. Saves the game played.
    private void endGame(){
        PlayedLevel.playedLevelNo = currentLevel.level_number;
        PlayedLevel.score = score;
        ScenesManager.LoadLevels();
    }

    private void updateText(){
        highestScoreText.text = "Highest Score: " + currentLevel.high_score;
        scoreText.text = "Score: " + score;
        remainMovesText.text = "Remain Moves: " + remain_moves;
    }


}
