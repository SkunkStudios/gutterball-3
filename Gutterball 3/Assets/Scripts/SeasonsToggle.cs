using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeasonsToggle : MonoBehaviour
{
    [Header("Halloween")]
    public SpriteState halloweenToggle;
    [Header("Christmas")]
    public SpriteState xmasToggle;

    void Awake()
    {
        if (DateTime.Now.Month == 10)
        {
            GetComponent<Toggle>().spriteState = halloweenToggle;
        }
        else if (DateTime.Now.Month == 12)
        {
            GetComponent<Toggle>().spriteState = xmasToggle;
        }
    }
}
