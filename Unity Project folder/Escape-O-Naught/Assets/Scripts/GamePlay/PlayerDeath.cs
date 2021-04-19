using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDeath : MonoBehaviour
{
    private Spawn spawner;

    /// <summary>
    /// kills the player, Used on out of bounds objects (the floor and ceiling)
    /// </summary>
    /// <param name="other">Collider that hits this</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") { return; }

        spawner = FindObjectOfType<Spawn>();
        spawner.SpawnedPlayer.GetComponent<PlayerMovement>().dead = true;

        Invoke("Respawn", 0.5f);
    }

    /// <summary>
    /// Takes a life and switches to build mode
    /// </summary>
    private void Respawn()
    {
        FindObjectOfType<PauseToggle>().SetToBuildMode();
        FindObjectOfType<Respawn>().TakeLive();
    }
}
