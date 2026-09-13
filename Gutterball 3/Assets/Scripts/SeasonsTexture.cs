using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeasonsTexture : MonoBehaviour
{
    [Header("Halloween")]
    public Texture2D halloweenTexture;
    public bool isHalloween;
    [Header("Christmas")]
    public Texture2D xmasTexture;
    public bool isXmas;
    [Header("Renderers")]
    public int rendererIndex = 0;

    void Awake()
    {
        if (DateTime.Now.Month == 10 && isHalloween)
        {
            if (rendererIndex == 0)
            {
                GetComponent<Renderer>().material.mainTexture = halloweenTexture;
            }
            else
            {
                GetComponent<Renderer>().materials[rendererIndex].mainTexture = halloweenTexture;
            }
        }
        else if (DateTime.Now.Month == 12 && isXmas)
        {
            if (rendererIndex == 0)
            {
                GetComponent<Renderer>().material.mainTexture = xmasTexture;
            }
            else
            {
                GetComponent<Renderer>().materials[rendererIndex].mainTexture = xmasTexture;
            }
        }
    }
}
