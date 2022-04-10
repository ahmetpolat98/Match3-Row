using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    public LevelData levelData;

    public Button playButton;

    public Sprite lockedSprite;


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

    private void checkLocked(){
        if(levelData.locked){
             GameObject child_play_button = gameObject.transform.Find("PlayButton").gameObject;
             child_play_button.GetComponent<Image>().sprite = lockedSprite;
             child_play_button.GetComponentInChildren<Text>().text = "Locked";
             GetComponentInChildren<Button>().enabled = false;
         }
    }

    

        

}
