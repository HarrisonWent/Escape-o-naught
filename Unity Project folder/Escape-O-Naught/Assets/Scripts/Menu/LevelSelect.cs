﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    public Image BackGroundMain, BackGroundLoading;
    public ScrollRect Scroller;

    public GameObject LevelButton;
    public GameObject LevelViewScroll;

    /// <summary>
    /// Load level from build index, called from level select buttons
    /// </summary>
    /// <param name="number">the build index of the level to load "File->BuildSettings"</param>
    public void LoadLevel(int number)
    {
        StartCoroutine("OpenLoad", number);
    }

    /// <summary>
    /// load next level from current
    /// </summary>
    public void NextLevel()
    {
        int NextlevelInt = SceneManager.GetActiveScene().buildIndex + 1;

        if (Physics.gravity.y > 0f)
        {
            Physics.gravity *= -1;
        }

        //bool PreStarted = GameObject.Find("SaveGame").GetComponent<SaveGame>().LoadSavedLevel("/" + NextlevelInt);
        //if (!PreStarted)
        //{
        if (NextlevelInt > SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadSceneAsync(0);
            }
            else
            {
                SceneManager.LoadSceneAsync(NextlevelInt);
            }
        //}

    }

    //Spawn the buttons for each level in level select panel
    private void Start()
    {
        if(SceneManager.GetActiveScene().buildIndex != 0) { return; }

        if (PlayerPrefs.GetInt("PlayerLevel") == 0)
        {
            PlayerPrefs.SetInt("PlayerLevel", 1);
        }

        for (int c = 1; c< (SceneManager.sceneCountInBuildSettings); c++)
        {
            GameObject spawn =Instantiate(LevelButton, transform.position, transform.rotation);

            string sceneName = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(c));

            //LevelLocked
            if (PlayerPrefs.GetInt("PlayerLevel") < c)
            {
                spawn.GetComponentInChildren<Text>().text = sceneName + ": LOCKED";
            }
            //LevelUnlocked
            else
            {
                spawn.GetComponentInChildren<Text>().text = sceneName + ": " + PlayerPrefs.GetInt("HighScoreLevel" + c) + "/5 STARS";
            }

            ;

            spawn.transform.SetParent(LevelViewScroll.transform);
            spawn.GetComponent<ButtonLoadLevel>().LevelToLoad = c;
        }
    }

    //
    /// <summary>
    /// load the given level
    /// </summary>
    /// <param name="number">the build index of the level to load "File->BuildSettings"</param>
    /// <returns></returns>
    private IEnumerator OpenLoad(int number)
    {
        //Move the loading screen down

        RectTransform b = BackGroundMain.GetComponent<RectTransform>();
        RectTransform c = BackGroundLoading.GetComponent<RectTransform>();

        BackGroundLoading.transform.gameObject.SetActive(true);

        float height = Screen.height;
        float step = height / 16;

        float a = 0;
        do
        {
            a -= step;

            c.offsetMin = new Vector2(b.offsetMin.x, a+height);//LowerLeftCorner
            c.offsetMax = new Vector2(b.offsetMax.x, a+height);//UpperRightCorner

            yield return new WaitForSeconds(0.02f);
        } while (a != -height);

        yield return new WaitForSeconds(1f);

        //start loading the level
        //bool PreStarted = GameObject.Find("SaveGame").GetComponent<SaveGame>().LoadSavedLevel("/" + number);
        //if (!PreStarted)
        //{
            SceneManager.LoadSceneAsync(number);
        //}
    }
}