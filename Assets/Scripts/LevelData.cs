using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData
{
    public int level_number=0;
    public int grid_height=0;
    public int grid_width=0;
    public int move_count=0;
    public int high_score=0;
    public bool locked=true;
    public List<string> grid=null;

    public LevelData(){
        grid = new List<string>();
    }
}

public class CurrentLevel
{
    public static LevelData currentLevel;

    public static void initCurrentLevel(){
        currentLevel = new LevelData();
    }
}

public class PlayedLevel
{
    public static int playedLevelNo;
    public static int score;

}
