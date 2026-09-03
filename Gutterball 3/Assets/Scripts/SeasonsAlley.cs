using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeasonsAlley : MonoBehaviour
{
    public GameObject halloweenAlley;
    public GameObject xmasAlley;

    void Awake()
    {
        if (DateTime.Now.Month == 10 && halloweenAlley != null)
        {
            halloweenAlley.SetActive(true);
        }
        else if (DateTime.Now.Month == 12 && xmasAlley != null)
        {
            xmasAlley.SetActive(true);
        }
    }
}
