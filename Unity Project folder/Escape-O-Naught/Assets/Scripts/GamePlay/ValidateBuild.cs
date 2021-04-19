using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValidateBuild : MonoBehaviour
{
    //Validator is whether a platform can be placed in the selected location or not

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

    public int ID = 0;

    /// <summary>
    /// Turns on validation, starts checking
    /// </summary>
    public void ActivateValidator()
    {        
        myRigid.WakeUp();
        if (myCollider)
        {
            myCollider.isTrigger = true;
        }
    }

    /// <summary>
    /// Turns it off, the object has been placed
    /// </summary>
    public void DisableValidator()
    {        
        if (myCollider)
        {
            myCollider.isTrigger = false;
        }
    }
}
