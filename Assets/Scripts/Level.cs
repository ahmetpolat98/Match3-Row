using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    public LevelData level;

    public Button playButton;

    public Sprite lockedSprite;



     private void Start(){
         level = new LevelData();
         //level datadan okunacak, filedan
         level.grid_height = 8;
         level.grid_width = 8;
         level.move_count = 50;
         level.locked = false;
         checkLocked();
         if(!level.locked)
            playButton.onClick.AddListener(call:() => ScenesManager.LoadGame(level));
     }

    private void checkLocked(){
        if(level.locked){
             GameObject child_play_button = gameObject.transform.Find("PlayButton").gameObject;
             child_play_button.GetComponent<Image>().sprite = lockedSprite;
             child_play_button.GetComponentInChildren<Text>().text = "Locked";
             GetComponentInChildren<Button>().enabled = false;
         }
    }

        

}
