using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Transform footPosition;
    public string LastName = "";//last name of object it hit

    private ConstantForce myForce;
    private Rigidbody MyRigid;
    public Animator MyAnimator;

    //Slope speed only, -1 is default grav, these are used to travel up and down slopes
    public int SlopeSpeed,FallingSpeed,
        BounceForce = 20;

    private void Start()
    {
        MyRigid = GetComponent<Rigidbody>();
        myForce = GetComponent<ConstantForce>();
    }

    private void OnCollisionStay(Collision collision)
    {
        //On slopes, help player move up or down them
        if (collision.transform.tag == "Slope")
        {
            if (LastName == collision.transform.parent.name) { return; }

            LastName = collision.transform.parent.name;

            if (collision.transform.position.y > footPosition.position.y)
            {
                //slide up
                myForce.force = new Vector3(myForce.force.x, SlopeSpeed, 0);
            }
            else
            {
                //slide down
                myForce.force = new Vector3(myForce.force.x, -SlopeSpeed, 0);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {        
        //Hit flip platform, invert gravity
        if (collision.transform.tag == "Flip")
        {
            if (LastName == collision.transform.parent.name) { return; }

            Debug.Log("Flip");

            //set name of flip it it
            LastName = collision.transform.parent.name;
            
            Physics.gravity = -Physics.gravity;            
        }
        //Hit deflect platform invert move direction
        else if (collision.transform.tag == "Deflective")
        {
            Debug.Log("Deflect");
            MyRigid.velocity = new Vector3(MyRigid.velocity.x*0.3f,MyRigid.velocity.y, MyRigid.velocity.z);
            myForce.force = new Vector3(-myForce.force.x, myForce.force.y, 0);            
            transform.Rotate(Vector3.up * 180);
        }
        //Hit bounce platform, add upwards velocity
        else if (collision.transform.tag == "Bounce")
        {
            Debug.Log("Bounce");
            MyRigid.velocity = new Vector2(MyRigid.velocity.x,0);
            MyRigid.AddForce(transform.TransformDirection(Vector3.up)* BounceForce, ForceMode.Impulse);
            FindObjectOfType<AudioManager>().Play("BounceStraight"); // jump sound effect
            collision.gameObject.GetComponentInParent<Animation>().Play();
        }
    }

    bool walkToggle = true;
    public bool dead = false;

    private void Update()
    {
        if (dead)
        {
            //DEAD ANIMATION
            return;
        }
        //Debug.Log("Current speed: " + MyRigid.velocity.magnitude);
        MyAnimator.SetFloat("Speed", MyRigid.velocity.magnitude/myForce.force.magnitude);

        if (MyRigid.velocity.x > 0 || MyRigid.velocity.x < 0)
        {
            //WALK ANMIATION
            walkToggle = true;
        }
        else
        {
            //IDLE ANIMATION            
            walkToggle = false;
        }

        //jump
        if ((MyRigid.velocity.y > 2 || MyRigid.velocity.y < -1) && myForce.force.y == -1)
        {
            //JUMP/FALL ANIMATION 
            MyAnimator.SetBool("Falling", true);
            if (MyRigid.velocity.x > FallingSpeed)
            {
                MyRigid.velocity = new Vector3(FallingSpeed, MyRigid.velocity.y,0);
            }
        }
        else
        {
            //JUMP/FALL ANIMATION 
            if (Physics.gravity.y > 0f)
            {
                MyAnimator.SetBool("Falling", true);
            }
            else
            {
                if (MyAnimator.GetBool("Falling") == true)
                {
                    FindObjectOfType<AudioManager>().Play("FallLand");
                }
                MyAnimator.SetBool("Falling", false);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        //Stop slope movement assist when leaving a slope
        if (collision.transform.tag == "Slope")
        {
            if (LastName == collision.transform.parent.name)
            {
                myForce.force = new Vector3(myForce.force.x, -1f,0);
                LastName = "";
            }
        }
    }
}
