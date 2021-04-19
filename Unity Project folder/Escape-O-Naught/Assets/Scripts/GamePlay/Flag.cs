using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flag : MonoBehaviour
{
    public Sprite[] sprites;
    public SpriteRenderer me;
    private int counter = 0;

    private void Start()
    {
        InvokeRepeating("flagit",0,0.4f);
    }

    /// <summary>
    /// Switches between flag sprites
    /// </summary>
    void flagit()
    {
        if (me)
        {
            counter++;
            if(counter == sprites.Length) { counter = 0; }
            me.sprite = sprites[counter];
        }
    }
}
