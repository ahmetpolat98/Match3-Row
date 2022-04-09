﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData
{
    public int level_number=0;
    public int grid_height=0;
    public int grid_width=0;
    public int move_count=0;
    public int high_score=0;
    public bool locked=false;
    public List<string> grid=null;
}

public class CurrentLevel
{
    public static LevelData currentLevel;

    public static void initCurrentLevel(){
        currentLevel = new LevelData();
    }
}
