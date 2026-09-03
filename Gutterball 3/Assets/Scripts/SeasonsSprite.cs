using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeasonsSprite : MonoBehaviour
{
    [Header("Halloween")]
    public Sprite halloweenSprite;
    [Header("Christmas")]
    public Sprite xmasSprite;

    void Awake()
    {
        if (DateTime.Now.Month == 10)
        {
            GetComponent<SpriteRenderer>().sprite = halloweenSprite;
        }
        else if (DateTime.Now.Month == 12)
        {
            GetComponent<SpriteRenderer>().sprite = xmasSprite;
        }
    }
}
