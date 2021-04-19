using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnStart : MonoBehaviour
{
    //Disables on start
    void Start()
    {
        gameObject.SetActive(false);
    }

}
