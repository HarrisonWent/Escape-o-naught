using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValidateBuild : MonoBehaviour
{
    public Collider myCollider;
    public Rigidbody myRigid;
    public Vector3 PlacingOffset;

    private void OnTriggerStay(Collider collider)
    {
        //Can't build here
        Builder.ValidateBuildCount = false;
    }

    private void OnTriggerExit(Collider collider)
    {
        //Can build here
        Builder.ValidateBuildCount = true;
    }

    //Turns on validation
    public void ActivateValidator()
    {        
        myRigid.WakeUp();
        if (myCollider)
        {
            myCollider.isTrigger = true;
        }
    }

    //Turns it off
    public void DisableValidator()
    {        
        if (myCollider)
        {
            myCollider.isTrigger = false;
        }
    }
}
