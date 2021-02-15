using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Score : MonoBehaviour
{
    public GameObject FinishLevelDisplay, WinScreen;
    public Spawn spawnPoint;
    public Text TimeToCompleteText;
    public int Time_OneStar = 30, Time_TwoStars = 25,Time_ThreeStars = 20, Time_FourStars = 15, Time_FiveStars = 10;
    public Sprite Star,BlankStar;

    private void OnTriggerEnter(Collider other)
    {
        //When player enter score point
        if (other.tag == "Player")
        {
            Debug.Log(SceneManager.GetActiveScene().buildIndex + " " + SceneManager.sceneCountInBuildSettings);
            if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1)
            {
                WinScreen.SetActive(true);
            }
            else
            {
                FinishLevelDisplay.SetActive(true);
            }

            PlayerProgress.SaveProgress();
            FindObjectOfType<AudioManager>().Play("LevelComplete"); //level complete music

            Respawn Res = GameObject.Find("MenuLogic").GetComponent<Respawn>();

            if (Res.lives < 3)
            {
                TimeToCompleteText.text = "YOU COMPLETED IT IN " + Mathf.RoundToInt(spawnPoint.Timer) + " SECONDS!" + "\n" + "For each live lost your maximum score is reduced";
            }
            else
            {
                TimeToCompleteText.text = "YOU COMPLETED IT IN " + Mathf.RoundToInt(spawnPoint.Timer) + " SECONDS!";
            }

            //Display stars
            int bonus = 0;
            if (spawnPoint.Timer < Time_FiveStars && Res.lives>2)
            {
                bonus++;
            }
            if (spawnPoint.Timer < Time_FourStars && Res.lives>1)
            {
                bonus++;
            }
            if(spawnPoint.Timer < Time_ThreeStars && Res.lives>0)
            {
                bonus++;
            }
            if(spawnPoint.Timer <Time_TwoStars)
            {
                bonus++;
            }
            if(spawnPoint.Timer <Time_OneStar)
            {
                bonus++;
            }

            GameObject[] Stars = GameObject.FindGameObjectsWithTag("StarUI");
            int count = 0;
            foreach (GameObject h in Stars)
            {
                count++;
                if (count <= bonus)
                {
                    h.GetComponent<Image>().sprite = Star;
                }
                else
                {
                    h.GetComponent<Image>().sprite = BlankStar;
                }                
            }

            //Save highscores for this level
            int oldScore = PlayerPrefs.GetInt("HighScoreLevel" + SceneManager.GetActiveScene().buildIndex);
            //Debug.Log(Res.lives);
            if (bonus > oldScore)
            {
                PlayerPrefs.SetInt("HighScoreLevel" + SceneManager.GetActiveScene().buildIndex,(bonus));
            }

            GameObject.Find("MenuLogic").GetComponent<Respawn>().ResetLives();

            Destroy(spawnPoint.SpawnedPlayer);

            spawnPoint.Timer = 0.00f;

        }
    }

    public Text TimeRemainingText;
    public GameObject[] Stars;
    private void Update()
    {
        if (spawnPoint.Timer < Time_FiveStars)
        {
            TimeRemainingText.text = (Time_FiveStars - spawnPoint.Timer).ToString("F2") + "";
            SetStarCount(5);
        }
        else if (spawnPoint.Timer < Time_FourStars)
        {
            TimeRemainingText.text = (Time_FourStars - spawnPoint.Timer).ToString("F2") + "";
            SetStarCount(4);
        }
        else if (spawnPoint.Timer < Time_ThreeStars)
        {
            TimeRemainingText.text = (Time_ThreeStars - spawnPoint.Timer).ToString("F2") + "";
            SetStarCount(3);
        }
        else if (spawnPoint.Timer < Time_TwoStars)
        {
            TimeRemainingText.text = (Time_TwoStars - spawnPoint.Timer).ToString("F2") + "";
            SetStarCount(2);
        }
        else if(spawnPoint.Timer < Time_OneStar)
        {
            TimeRemainingText.text = (Time_OneStar - spawnPoint.Timer).ToString("F2") + "";
            SetStarCount(1);
        }
        else
        {
            TimeRemainingText.text = spawnPoint.Timer.ToString("F2") + "";
            SetStarCount(0);
        }
    }

    void SetStarCount(int newCount)
    {

        int count = 0;
        foreach(GameObject g in Stars)
        {
            count++;
            if(count<=newCount)
            {
                g.SetActive(true);
            }
            else
            {
                g.SetActive(false);
            }
        }
    }

}
