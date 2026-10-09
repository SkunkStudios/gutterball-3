using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeasonsOutline : MonoBehaviour
{
    [Header("Halloween")]
    public Color halloweenColor;
    [Header("Christmas")]
    public Color xmasColor;

    void Awake()
    {
        if (DateTime.Now.Month == 10)
        {
            GetComponent<Outline>().effectColor = halloweenColor;
        }
        else if (DateTime.Now.Month == 12)
        {
            GetComponent<Outline>().effectColor = xmasColor;
        }
    }
}
