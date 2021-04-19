using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeQuality : MonoBehaviour
{
    public SaveGame saveScript;
    public Dropdown myDropDown;
    private int currentQuality = 2;

    /// <summary>
    /// sets new quality level, found in project settings tab, no longer used in project, used to be an option on the main menu
    /// </summary>
    public void ChangeLevel()
    {
        PlayerPrefs.SetInt("Saved", 1);
        currentQuality = myDropDown.value;
        QualitySettings.SetQualityLevel(currentQuality);
        PlayerPrefs.SetInt("Graphics", myDropDown.value);
    }

}
