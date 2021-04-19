﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    //Speed at which camera tracks player
    public int LerpSpeed = 5;

    //Player prefab to be spawned, UI panel for startinh player
    public GameObject Player,startPanel;

    //Controls camera following player or not
    private bool Lerping = false;

    //Positions of camera and player
    private Transform camTransform, playerTransform;

    //Current player in scene, outofbounds object in scene, end point in scene, 
    public GameObject SpawnedPlayer,EndPoint;

    //Time from sim start
    public float Timer = 0.00f;
    private float ZoomInFov = 35, ZoomOutFov = 60f,ZoomSpeed = 15f;

    Vector3 OutPosition;

    private void Start()
    {
        camTransform = Camera.main.transform;
        OutPosition = camTransform.position;
        ZoomOutFov = Camera.main.fieldOfView;
    }

    private Vector3 StartRot = new Vector3(0, 90, 0);

    /// <summary>
    /// Spawns the player at the spawn position
    /// </summary>
    public void SpawnPlayer()
    {
        SpawnedPlayer = Instantiate(Player, transform.position, Quaternion.Euler(StartRot));

        playerTransform = SpawnedPlayer.transform;

        startPanel.SetActive(false);
        Lerping = true;

        EndPoint.SetActive(true);
    }

    private void Update()
    {
        Timer += Time.deltaTime;

        //Move fov out ot building value
        if (!Lerping)
        {
            if (Camera.main.fieldOfView < ZoomOutFov)
            {
                Camera.main.fieldOfView += ZoomSpeed * Time.deltaTime;
            }
        }

        //Move camera to building position (far out)
        if (SpawnedPlayer == null)
        {
            camTransform.position = Vector3.Lerp(camTransform.position, OutPosition, Time.deltaTime * LerpSpeed);
            if(Vector3.Distance(camTransform.position, OutPosition) < 1)
            {
                Lerping = false;
            }
            return;
        }

        //Move camera and fov to follow the player in play mode
        if (Lerping)
        {
            Vector3 adjustedPoisitionForDistance = new Vector3(playerTransform.position.x, Mathf.Clamp(playerTransform.position.y,-8,16), camTransform.position.z);
            camTransform.position = Vector3.Lerp(camTransform.position, adjustedPoisitionForDistance, Time.deltaTime * LerpSpeed);

            if (Camera.main.fieldOfView > ZoomInFov)
            {
                Camera.main.fieldOfView -= ZoomSpeed * Time.deltaTime;
            }
        }
    }


    public void ResetTimer() { Timer = 0f; }
}