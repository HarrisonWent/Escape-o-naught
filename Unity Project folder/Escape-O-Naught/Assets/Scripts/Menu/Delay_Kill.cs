using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delay_Kill : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 2);
    }
}
