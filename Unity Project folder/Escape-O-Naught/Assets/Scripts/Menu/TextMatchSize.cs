using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextMatchSize : MonoBehaviour
{   

    private Text[] texts;

    void Start()
    {
        StartCoroutine("DelayUI");
    }

    /// <summary>
    /// Sets a consistent text size to the smallest size on preffered fit
    /// </summary>
    /// <returns></returns>
    public IEnumerator DelayUI()
    {
        yield return new WaitForSeconds(0.1f);

        texts = GetComponentsInChildren<Text>();

        foreach (Text t in texts)
        {
            t.color = Color.clear;
        }

        int SmallestSize = 10000;
        foreach (Text t in texts)
        {            
            if (t.cachedTextGenerator.fontSizeUsedForBestFit < SmallestSize && t.cachedTextGenerator.fontSizeUsedForBestFit != 0)
            {
                SmallestSize = t.cachedTextGenerator.fontSizeUsedForBestFit;
            }
        }

        foreach (Text t in texts)
        {
            t.fontSize = SmallestSize/2;
            t.color = Color.white;
            t.resizeTextForBestFit = false;
        }
    }

}
