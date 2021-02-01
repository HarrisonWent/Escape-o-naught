using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    public Animator MyAnim;
    public AudioClip[] FeetSounds;
    public AudioSource FeetSource;

    private float StepRate = 0.4f, Counter = 0f;
    private void OnCollisionStay()
    {
        // if (MyAnim.GetBool("Falling")) { return; }
        return;
        Counter += Time.deltaTime * MyAnim.GetFloat("Speed");

        if(Counter>=StepRate)
        {
            FeetSource.PlayOneShot(FeetSounds[Random.Range(0, FeetSounds.Length)]);
            Counter = 0f;
        }
    }//todo this
}
