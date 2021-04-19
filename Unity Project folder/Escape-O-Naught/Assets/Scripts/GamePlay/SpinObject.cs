using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinObject : MonoBehaviour
{
    public Vector3 Direction;
    private void Update()
    {
        //Very complex, rotates an object... weeee
        transform.Rotate(Direction, Space.Self);
    }
}
