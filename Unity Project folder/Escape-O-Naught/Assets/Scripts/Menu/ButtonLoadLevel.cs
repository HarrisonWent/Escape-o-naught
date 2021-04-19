using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonLoadLevel : MonoBehaviour
{
    //Used to load specific levels
    public int LevelToLoad = 0;

    /// <summary>
    /// Loads a level by build index
    /// </summary>
    public void LoadLevel()
    {
        if(PlayerPrefs.GetInt("PlayerLevel") < LevelToLoad) { return; }

        FindObjectOfType< LevelSelect>().LoadLevel(LevelToLoad);
    }
}
