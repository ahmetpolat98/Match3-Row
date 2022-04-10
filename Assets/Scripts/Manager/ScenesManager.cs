using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    // public static ScenesManager Instance {get; private set;}

    // private void Awake() => Instance = this;
    public static void LoadGame(LevelData lvl){
        CurrentLevel.initCurrentLevel();
        CurrentLevel.currentLevel.level_number = lvl.level_number;
        CurrentLevel.currentLevel.grid_height = lvl.grid_height;
        CurrentLevel.currentLevel.grid_width = lvl.grid_width;
        CurrentLevel.currentLevel.move_count = lvl.move_count;
        CurrentLevel.currentLevel.high_score = lvl.high_score;
        CurrentLevel.currentLevel.grid = lvl.grid;
        
        SceneManager.LoadScene("Game");
    }

    
    
}
