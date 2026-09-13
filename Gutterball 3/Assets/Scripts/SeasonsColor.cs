using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeasonsColor : MonoBehaviour
{
    [Header("Halloween/Christmas")]
    public Color halloweenXmasColor;
    public bool isHalloweenXmas;
    [Header("Renderers")]
    public int rendererIndex = 0;

    void Awake()
    {
        if (DateTime.Now.Month == 10 && isHalloweenXmas || DateTime.Now.Month == 12 && isHalloweenXmas)
        {
            GetComponent<Renderer>().materials[rendererIndex].color = halloweenXmasColor;
        }
    }
}
