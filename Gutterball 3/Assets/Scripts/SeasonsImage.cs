using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeasonsImage : MonoBehaviour
{
    [Header("Halloween")]
    public Sprite halloweenSprite;
    [Header("Christmas")]
    public Sprite xmasSprite;

    void Awake()
    {
        if (DateTime.Now.Month == 10)
        {
            GetComponent<Image>().sprite = halloweenSprite;
        }
        else if (DateTime.Now.Month == 12)
        {
            GetComponent<Image>().sprite = xmasSprite;
        }
    }
}
