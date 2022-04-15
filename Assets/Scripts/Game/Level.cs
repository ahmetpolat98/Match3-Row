using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//level row in the level list
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
        checkLocked();
        if(!levelData.locked)
            playButton.onClick.AddListener(call:() => ScenesManager.LoadGame(levelData)); // Adds listener to button if level is not locked
     }

    public void updateText(){
        levelHeaderText.text = "Level " + levelData.level_number + " - " + levelData.move_count + " Moves";
        if(levelData.locked){
            highScoreText.text = "Locked Level";
        }
        else{
            if(levelData.high_score == 0){
            highScoreText.text = "No Score";
            }
            else
            {
                highScoreText.text = "Highest Score: " + levelData.high_score;
            }
        }
    }

    //It is checked whether the level is locked or not. Sprites and access changes are made accordingly.
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
