using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    public LevelData levelData;

    public Button playButton;

    public Sprite unlockedSprite;
    public Sprite lockedSprite;
    public Text levelHeaderText;
    public Text highScoreText;


    public Level(){
        levelData = new LevelData();
    }
     private void Start(){        
         //level datadan okunacak, filedan
        //  levelData.grid_height = 8;
        //  levelData.grid_width = 8;
        //  levelData.move_count = 50;
        //  levelData.locked = false;
         checkLocked();
         if(!levelData.locked)
            playButton.onClick.AddListener(call:() => ScenesManager.LoadGame(levelData));
     }

    public void updateText(){
        levelHeaderText.text = "Level " + levelData.level_number + " - " + levelData.move_count + " Moves";
        highScoreText.text = "Highest Score: " + levelData.high_score;
    }


    public void checkLocked(){
        GameObject child_play_button = gameObject.transform.Find("PlayButton").gameObject;
        if(levelData.locked){           
             child_play_button.GetComponent<Image>().sprite = lockedSprite;
             child_play_button.GetComponentInChildren<Text>().text = "Locked";
             GetComponentInChildren<Button>().enabled = false;
        }
        else
        {
            child_play_button.GetComponent<Image>().sprite = unlockedSprite;
             child_play_button.GetComponentInChildren<Text>().text = "Play";
             GetComponentInChildren<Button>().enabled = true;
        }

    }

}
