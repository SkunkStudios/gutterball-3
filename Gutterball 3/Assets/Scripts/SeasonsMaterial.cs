using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeasonsMaterial : MonoBehaviour
{
    [Header("Halloween")]
    public Material halloweenMaterial;
    public bool isHalloween;
    [Header("Christmas")]
    public Material xmasMaterial;
    public bool isXmas;

    void Awake()
    {
        if (DateTime.Now.Month == 10 && isHalloween)
        {
            GetComponent<Renderer>().material = halloweenMaterial;
        }
        else if (DateTime.Now.Month == 12 && isXmas)
        {
            GetComponent<Renderer>().material = xmasMaterial;
        }
    }
}
