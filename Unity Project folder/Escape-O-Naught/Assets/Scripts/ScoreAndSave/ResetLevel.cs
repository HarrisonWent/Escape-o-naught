using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetLevel : MonoBehaviour
{
    public Builder BuilderScript;
    public Spawn SpawnScript;
    public GameObject BuildMenu,EndMenu;

    /// <summary>
    /// Used by reset button, deletes all placed platforms
    /// </summary>
    public void Reset()
    {
        GameObject[] UserObjects = GameObject.FindGameObjectsWithTag("USEROBJECT");

        //Destroy all objects placed by user
        foreach (GameObject G in UserObjects)
        {
            Destroy(G);
        }

        //Reset amount of blocks left
        BuilderScript.ResetLimits();

        if (Physics.gravity.y > 0) { Physics.gravity = -Physics.gravity; }

        //Destroy player
        GameObject.Find("MenuLogic").GetComponent<PauseToggle>().SetToBuildMode();

        BuildMenu.SetActive(true);
        EndMenu.SetActive(false);

        Time.timeScale = 1;
    }

}
