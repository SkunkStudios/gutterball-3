using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeasonsScrollbar : MonoBehaviour
{
    [Header("Halloween")]
    public ColorBlock halloweenScrollbar;
    [Header("Christmas")]
    public ColorBlock xmasScrollbar;

    void Awake()
    {
        if (DateTime.Now.Month == 10)
        {
            GetComponent<Scrollbar>().colors = halloweenScrollbar;
        }
        else if (DateTime.Now.Month == 12)
        {
            GetComponent<Scrollbar>().colors = xmasScrollbar;
        }
    }
}
