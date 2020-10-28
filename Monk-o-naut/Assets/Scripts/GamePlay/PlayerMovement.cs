using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Transform footPosition;
    public string LastName = "";//last name of object it hit
    private ConstantForce myForce;
    private Rigidbody MyRigid;
    public int VerticalSpeed,HorizontalSpeed,FlyingSpeed,BounceForce = 20;

    private void Start()
    {
        MyRigid = GetComponent<Rigidbody>();
        myForce = GetComponent<ConstantForce>();
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.tag == "Slope")
        {
            if (LastName == collision.transform.parent.name) { return; }

            LastName = collision.transform.parent.name;

            if (collision.transform.position.y > footPosition.position.y)
            {
                //slide up
                Debug.Log("UP");
                myForce.relativeForce = new Vector3(HorizontalSpeed, VerticalSpeed,0);
            }
            else
            {
                //slide down
                Debug.Log("DOWN");
                myForce.relativeForce = new Vector3(HorizontalSpeed, -VerticalSpeed,0);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        
         if (collision.transform.tag == "Flip")
        {
            if (LastName == collision.transform.parent.name) { return; }
            Debug.Log("Flip");
            //set name of flip it it
            LastName = collision.transform.parent.name;
            float yDif = transform.position.y - collision.transform.position.y;

            //move the player to the other side of the flip and invert gravity
            transform.position = new Vector3(transform.position.x, collision.transform.position.y - yDif - 0.5f, transform.position.z);
            Physics.gravity = -Physics.gravity;
            transform.Rotate(Vector2.left * 180);
        }
        else if (collision.transform.tag == "Deflective")
        {
            Debug.Log("Deflect");
            myForce.relativeForce = -myForce.relativeForce;
            MyRigid.velocity = Vector2.zero;
        }
        else if (collision.transform.tag == "Bounce")
        {
            Debug.Log("Bounce");
            MyRigid.velocity = new Vector2(MyRigid.velocity.x,0);
            MyRigid.AddForce(transform.TransformDirection(Vector3.up)* BounceForce, ForceMode.Impulse);
            StartCoroutine("Spring", collision.transform);
            FindObjectOfType<AudioManager>().Play("BounceStraight"); // jump sound effect
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

        if (MyRigid.velocity.x > 0 || MyRigid.velocity.x < 0)
        {
            walkToggle = true;
            //walk
        }
        else
        {
            //IDLE ANIMATION
            walkToggle = false;
            //Stand
        }
        
        if (walkToggle)
        {
            //WALK ANMIATION
        }

        //jump
        if ((MyRigid.velocity.y > 2 || MyRigid.velocity.y < -1) && myForce.relativeForce.y == -1)
        {
            //JUMP ANIMATION
            if (MyRigid.velocity.x > FlyingSpeed)
            {
                MyRigid.velocity = new Vector3(FlyingSpeed, MyRigid.velocity.y,0);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "Slope")
        {
            if (LastName == collision.transform.parent.name)
            {
                myForce.relativeForce = new Vector3(HorizontalSpeed, -1,0);
                LastName = "";
            }
        }
    }

    private IEnumerator Spring(Transform springer)
    {
        Debug.Log(springer.name);
        yield break;
        //SPRING ANIMATION
    }
}
