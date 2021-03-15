using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleSource : MonoBehaviour
{
    public static SingleSource SS = null;
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
