using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeasonsButton : MonoBehaviour
{
    [Header("Halloween")]
    public SpriteState halloweenButton;
    [Header("Christmas")]
    public SpriteState xmasButton;

    void Awake()
    {
        if (DateTime.Now.Month == 10)
        {
            GetComponent<Button>().spriteState = halloweenButton;
        }
        else if (DateTime.Now.Month == 12)
        {
            GetComponent<Button>().spriteState = xmasButton;
        }
    }
}
