using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleSource : MonoBehaviour
{
    public static SingleSource SS = null;
    //Ensures only one instance of this object is in the scene
    void Start()
    {
        if (SS != null)
        {
            Destroy(gameObject);
        }
        else
        {
            SS = this;
            DontDestroyOnLoad(gameObject);
        }
    }

}
