using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Respawn : MonoBehaviour
{
    //updates lives and resets them

    public int lives = 3;
    public Sprite EmptyHeart, FilledHeart;
    public GameObject[] Hearts;

    private void Start()
    {
        Debug.Log(lives);
        UpdateLives();
    }

    /// <summary>
    /// Takes a life
    /// </summary>
    public void TakeLive()
    {
        lives--;
        UpdateLives();
    }

    /// <summary>
    /// Sets the amount of active hearts to the current lives left
    /// </summary>
    public void UpdateLives()
    {
        int count = 1;
        foreach (GameObject h in Hearts)
        {
            if (count <= lives)
            {
                h.GetComponent<Image>().sprite = FilledHeart;
            }
            else
            {
                h.GetComponent<Image>().sprite = EmptyHeart;
            }
            count++;
        }
    }
    
    /// <summary>
    /// Resets lives to the default value
    /// </summary>
    public void ResetLives()
    {
        lives = 3;
        PlayerPrefs.SetInt(SceneManager.GetActiveScene().buildIndex + "Lvl", 3);
        UpdateLives();
    }
}
