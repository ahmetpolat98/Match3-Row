using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject LevelsPopup, LevelsButton;

    public Text highScoreText;

    void Start(){
        if(CurrentLevel.currentLevel != null){
            openLevelsPopup();
        }
    }

    public void openLevelsPopup(){
        LevelsButton.SetActive(false);
        LevelsPopup.SetActive(true);
    }

    public void closeLevelsPopup(){
        LevelsPopup.SetActive(false);
        LevelsButton.SetActive(true);       
    }
    public void closeAll(){
        LevelsPopup.SetActive(false);
        LevelsButton.SetActive(false);       
    }
}
