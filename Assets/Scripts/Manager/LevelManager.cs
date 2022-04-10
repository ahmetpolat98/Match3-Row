using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class LevelManager : MonoBehaviour
{
    public GameObject levelPrefab;
    private string[] levelFiles = {"RM_A1", "RM_A2", "RM_A3", "RM_A4", "RM_A5", "RM_A6", "RM_A7", "RM_A8", "RM_A9", "RM_A10"};
    private List<string> levelData;
    private List<string> saveLevelData;

    private string level_path;
    private string save_level_data_path;

    void Start(){
        levelData = new List<string>();
        saveLevelData = new List<string>();

        createLevels();

        foreach (string item in levelFiles)
        {
            level_path = Application.streamingAssetsPath + "/Levels/" + item;
            save_level_data_path = Application.streamingAssetsPath + "/SaveLevelData/" + item;
            Debug.Log(level_path);//
            readLevelData();
            readSaveLevelData();
        }
     
    }

    private void createLevels(){
        for(int i = 1; i <= 25; i++){
            GameObject newLevel = Instantiate(levelPrefab);
            newLevel.transform.SetParent(this.gameObject.transform);
            newLevel.name = "Level " +i;
            newLevel.transform.localScale = new Vector3(1,1,1);
        }
    }

    private void readLevelData(){
        List<string> fileLines = File.ReadAllLines(level_path).ToList();
        levelData.Clear();
        foreach (string line in fileLines)
        {
            string[] values = line.Split(" "[0]);
            // Debug.Log(values[1]);
            levelData.Add(values[1]);         
        }
        initLevelData();
    }

    private void readSaveLevelData(){
        List<string> fileLines = File.ReadAllLines(save_level_data_path).ToList();
        saveLevelData.Clear();
        foreach (string line in fileLines)
        {
            string[] values = line.Split(" "[0]);
            Debug.Log(values[1]);
            saveLevelData.Add(values[1]);         
        }
        initSaveLevelData();

    }

    private void initLevelData(){
        GameObject levelGameObject = gameObject.transform.Find("Level "+levelData[0]).gameObject;

        Level level = levelGameObject.GetComponent<Level>();
        level.levelData.level_number = int.Parse(levelData[0]);
        level.levelData.grid_width = int.Parse(levelData[1]);
        level.levelData.grid_height = int.Parse(levelData[2]);
        level.levelData.move_count = int.Parse(levelData[3]);

        string[] values = levelData[4].Split(","[0]);
        foreach (string item in values)
        {
            level.levelData.grid.Add(item);
        }

       


        // level.levelData.grid = levelData[4]();
        // levelGameObject.SetActive(false);
    }
    private void initSaveLevelData(){
        GameObject levelGameObject = gameObject.transform.Find("Level "+levelData[0]).gameObject;
        Level level = levelGameObject.GetComponent<Level>();
        int locked = int.Parse(saveLevelData[0]);
        level.levelData.high_score = int.Parse(saveLevelData[1]);
       
        if (locked == 0)
            level.levelData.locked = false;
        else
            level.levelData.locked = true;   

        Debug.Log("-----");
        Debug.Log(level.levelData.locked);
        Debug.Log(level.levelData.high_score);
        level.updateText();
        level.checkLocked();
    }




    // writeToFile
}
