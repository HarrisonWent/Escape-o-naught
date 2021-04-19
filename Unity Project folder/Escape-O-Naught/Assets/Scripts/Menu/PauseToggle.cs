using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseToggle : MonoBehaviour
{
    //Toggles time between 0 and 1
    private bool PlayMode = false;
    public Sprite PauseIcon,PlayIcon;
    public Image PauseButtonImage;
    public Spawn PlayerSpawn;
    public GameObject BuildPanel, StartSign;

    private GameObject Grid;

    private void Start()
    {
        Grid = GameObject.Find("Grid");
    }

    //Toggles between build and play mode (called from the pause/play button)
    public void TogglePlay()
    {
        PlayMode = !PlayMode;

        if (PlayMode)
        {
            SetToPlayMode();
        }
        else
        {
            SetToBuildMode();
        }
    }

    /// <summary>
    /// Switches to build mode, also resets gravity to default
    /// </summary>
    public void SetToBuildMode()
    {
        if (Physics.gravity.y > 0f)
        {
            Physics.gravity *= -1;
        }
        PlayMode = false;
        PauseButtonImage.sprite = PlayIcon;
        Destroy(PlayerSpawn.SpawnedPlayer);
        Grid.SetActive(true);
        BuildPanel.SetActive(true);
        StartSign.SetActive(true);
    }

    /// <summary>
    /// Switches to play mode
    /// </summary>
    public void SetToPlayMode()
    {
        PlayMode = true;
        PauseButtonImage.sprite = PauseIcon;
        PlayerSpawn.SpawnPlayer();
        Grid.SetActive(false);
        StartSign.SetActive(false);
    }
}