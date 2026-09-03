using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeasonsImageColor : MonoBehaviour
{
    [Header("Halloween")]
    public Color halloweenColor;
    [Header("Christmas")]
    public Color xmasColor;

    void Awake()
    {
        if (DateTime.Now.Month == 10)
        {
            GetComponent<Image>().color = halloweenColor;
        }
        else if (DateTime.Now.Month == 12)
        {
            GetComponent<Image>().color = xmasColor;
        }
    }
}
