using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject LevelsPopup, LevelsButton;

    public void openLevelsPopup(){
        LevelsButton.SetActive(false);
        LevelsPopup.SetActive(true);
    }

    public void closeLevelsPopup(){
        LevelsPopup.SetActive(false);
        LevelsButton.SetActive(true);       
    }
}
