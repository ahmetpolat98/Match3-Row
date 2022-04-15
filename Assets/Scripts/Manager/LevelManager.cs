using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.Networking;

public class LevelManager : MonoBehaviour
{
    public GameObject levelPrefab;
    private List<string> levelFiles;
    private List<string> levelData;
    private List<string> saveLevelData;
    private List<string> newScoreLvlData;
    private List<string> urls;
    private List<string> downloadlevelData;

    private string level_path;
    private string save_level_data_path;

    void Start(){
        levelData = new List<string>();
        saveLevelData = new List<string>();
        newScoreLvlData = new List<string>();
        levelFiles = new List<string>();
        downloadlevelData = new List<string>();
        urls = new List<string>();


        StartCoroutine(downloadLevel());

        readDownloadedFiles();


        if(CurrentLevel.currentLevel != null){
            if (PlayedLevel.score > CurrentLevel.currentLevel.high_score && PlayedLevel.playedLevelNo == CurrentLevel.currentLevel.level_number)
            {
                Debug.Log("High Score!");
                writeNewHighScore();
            }
        }
        
        createLevels();

        readFiles();       
     
    }
    private void readFiles(){
        int lvl_no = 1;

        //read data from lvl files and save files
        foreach (string item in levelFiles)
        {
            level_path = Application.streamingAssetsPath + "/Levels/" + item;
            save_level_data_path = Application.streamingAssetsPath + "/SaveLevelData/" + lvl_no;
            // Debug.Log(level_path);//
            readLevelData(level_path);
            initLevelData();

            readSaveLevelData(save_level_data_path);
            initSaveLevelData();
            lvl_no += 1;
        }
    }


    private void readDownloadedFiles(){
        levelFiles.Clear();
        string path = Application.streamingAssetsPath + "/downloaded_files";
        List<string> fileLines = File.ReadAllLines(path).ToList();
        foreach (string line in fileLines)
        {
           levelFiles.Add(line);
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


    private void writeLevelFile(string text){
        Debug.Log("Save Downloaded Level File");
        string[] download_file_values = text.Split(' ', '\n');

        string path = Application.streamingAssetsPath + "/Levels/" + download_file_values[1];
        File.WriteAllText(path, text);

        
        string downloaded_path = Application.streamingAssetsPath + "/downloaded_files";
        File.AppendAllText(downloaded_path, "\n");
        File.AppendAllText(downloaded_path, download_file_values[1]);
    }

    private IEnumerator downloadLevel(){
            string path = Application.streamingAssetsPath + "/level_urls";

            List<string> fileLines = File.ReadAllLines(path).ToList();
            bool connection = false;

            Debug.Log("Read lvl url file to download.");

            for (int i = 0; i < fileLines.Count; i++) // dosya bitene kadar
            {
                downloadlevelData.Clear();

                string line_first = File.ReadLines(path).First();
                List<string> lines_except_first = File.ReadLines(path).Skip(1).ToList();

                string[] values = line_first.Split(" "[0]);
                Debug.Log("Downloading Levels..");
                // Debug.Log(i+1);
                // Debug.Log(values[0]);
                // Debug.Log(values[1]);
                downloadlevelData.Add(values[0]);
                downloadlevelData.Add(values[1]);
                

                UnityWebRequest www = new UnityWebRequest(downloadlevelData[1]);
                www.downloadHandler = new DownloadHandlerBuffer();
                yield return www.SendWebRequest();

                if(www.isNetworkError || www.isHttpError) {
                    
                    Debug.Log(www.error);
                    break;//not try the remaining downloads so that the priority order of the level numbers does not change.
                }
                else
                {

                    writeLevelFile(www.downloadHandler.text);
                    File.WriteAllLines(path, lines_except_first);
                    connection = true;
                }
            }
            // TODO lvler indirildikten sonra lvl file okuma işlemlerini yap, lvler güncellensin
            if(fileLines.Count>=1 && connection){
                readDownloadedFiles();
                readFiles();
            }
            
            
    }


    private void readLevelData(string path){
        List<string> fileLines = File.ReadAllLines(path).ToList();
        levelData.Clear();
        foreach (string line in fileLines)
        {
            string[] values = line.Split(" "[0]);
            // Debug.Log(values[1]);
            levelData.Add(values[1]);         
        }
    }

    private void readSaveLevelData(string path){
        List<string> fileLines = File.ReadAllLines(save_level_data_path).ToList();
        saveLevelData.Clear();
        foreach (string line in fileLines)
        {
            string[] values = line.Split(" "[0]);
            // Debug.Log(values[1]);
            saveLevelData.Add(values[1]);         
        }
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

        // Debug.Log("-----");
        // Debug.Log(level.levelData.locked);
        // Debug.Log(level.levelData.high_score);
        level.updateText();
        level.checkLocked();
    }

    private void writeNewHighScore(){
        string path1 = Application.streamingAssetsPath + "/SaveLevelData/" + PlayedLevel.playedLevelNo;
        int nextLvl = PlayedLevel.playedLevelNo + 1;
        string path2 = Application.streamingAssetsPath + "/SaveLevelData/" + nextLvl;

        string write_data1 = "locked: 0";
        string write_data2 = "highest_score: " + PlayedLevel.score;

        List<string> write_data = new List<string>();
        write_data.Add(write_data1);
        write_data.Add(write_data2);    

        File.WriteAllLines(path1, write_data);
    
        try
        {
            List<string> fileLines = File.ReadAllLines(path2).ToList();
            newScoreLvlData.Clear();
            foreach (string line in fileLines)
            {
                string[] values = line.Split(" "[0]);
                newScoreLvlData.Add(values[1]);         
            }
            int high_score = int.Parse(newScoreLvlData[1]);

            write_data.Clear();

            write_data1 = "locked: 0";
            write_data2 = "highest_score: " + high_score;

            write_data.Add(write_data1);
            write_data.Add(write_data2);


            File.WriteAllLines(path2, write_data);
            }
        catch (System.Exception)
        {            
            throw;
        }
    }



}
